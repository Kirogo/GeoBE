using geoback.Data;
using geoback.Services;
using geoback.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "GeoBuild API", 
        Version = "v1" 
    });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// MySQL Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database connection string 'DefaultConnection' not found.");
}

var maskedConnectionString = connectionString.Replace(
    connectionString.Split("Password=").Last().Split(';').First(), 
    "*****");
Console.WriteLine($"Attempting to connect with: {maskedConnectionString}");

var serverVersion = new MySqlServerVersion(new Version(9, 6, 0));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, serverVersion, mysqlOptions =>
    {
        mysqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null);
        
        mysqlOptions.CommandTimeout(60);
        mysqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    }));

builder.Services.AddScoped<IFacilityService, FacilityService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddHttpClient("CoreBanking", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["CoreBanking:BaseUrl"] ?? "https://core-banking.ncba.co.ke/api");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"] ?? 
                 builder.Configuration["Jwt:Key"] ?? 
                 "ThisIsASecretKeyForDevelopmentOnly12345!MakeSureItIsLongEnough";

var issuer = "geoback";
var audience = "GeoBuildClient";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"❌ Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine($"✅ Token validated for: {context.Principal?.Identity?.Name}");
            return Task.CompletedTask;
        }
    };
});

// Authorization policies - UPDATED to include Valuer
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RMOnly", policy => policy.RequireRole("RM"));
    options.AddPolicy("QSOnly", policy => policy.RequireRole("QS"));
    options.AddPolicy("ValuerOnly", policy => policy.RequireRole("Valuer"));
    options.AddPolicy("RMOrQS", policy => policy.RequireRole("RM", "QS"));
    options.AddPolicy("QSOrValuer", policy => policy.RequireRole("QS", "Valuer", "Admin"));
});

// CORS for React frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173", 
                "http://localhost:3000", 
                "http://localhost:5000",
                "https://localhost:5001"
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddSignalR();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeoBuild API V1");
    });
}
else
{
    app.UseHsts();
}

app.UseStaticFiles();

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/api/rmChecklist/photos") ||
        context.Request.Path.StartsWithSegments("/api/rmChecklist/documents"))
    {
        await next();
    }
    else
    {
        await next();
    }
});

app.UseCors("ReactApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();
app.MapHub<NotificationHub>("/hub/notificationHub");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        if (dbContext.Database.CanConnect())
        {
            logger.LogInformation("✓ Successfully connected to MySQL database");
            
            try
            {
                using var command = dbContext.Database.GetDbConnection().CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name IN ('ReportLocks', 'UserActiveLocks')";
                await dbContext.Database.OpenConnectionAsync();
                var count = Convert.ToInt32(await command.ExecuteScalarAsync());
                
                if (count == 2)
                {
                    logger.LogInformation("✓ Locking system tables detected");
                }
                else
                {
                    logger.LogWarning("⚠ Locking system tables not found. Run the migration script.");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning($"⚠ Could not check lock tables: {ex.Message}");
            }
            
            if (app.Environment.IsDevelopment())
            {
                logger.LogInformation("Database is ready");
            }
        }
        else
        {
            logger.LogError("✗ Failed to connect to MySQL database");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "✗ Database connection error: {Message}", ex.Message);
    }
}

var logger2 = app.Services.GetRequiredService<ILogger<Program>>();
logger2.LogInformation("✅ Application started successfully");
logger2.LogInformation("🌍 Environment: {Environment}", app.Environment.EnvironmentName);
logger2.LogInformation("🔗 API URLs: {Urls}", string.Join(", ", app.Urls));
logger2.LogInformation("🔑 JWT Audience: {Audience}", audience);
logger2.LogInformation("🔑 JWT Issuer: {Issuer}", issuer);
logger2.LogInformation("🔒 Locking System: Enabled (Session-based with heartbeat)");

app.Run();