using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using geoback.Data;
using geoback.Models;
using System.Security.Claims;
using System.Text.Json;

namespace geoback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MapController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MapController> _logger;

        public MapController(ApplicationDbContext context, ILogger<MapController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/map/properties
        [HttpGet("properties")]
        public async Task<ActionResult<IEnumerable<object>>> GetPropertiesForMap()
        {
            try
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Guid? userGuid = userId != null ? Guid.Parse(userId) : (Guid?)null;

                _logger.LogInformation($"Fetching properties for user {userId} with role {userRole}");

                // Base query - get all checklists (reports) with coordinates
                // Exclude pending status reports - they don't have location captured yet
                var query = _context.Checklists
                    .Where(r => r.VisitLatitude.HasValue && r.VisitLatitude != 0 
                             && r.VisitLongitude.HasValue && r.VisitLongitude != 0
                             && r.Status != "pending");

                // Don't filter by RM for map view - show all properties to all authorized users
                // QS role can see submitted or assigned reports
                if (userRole == "QS")
                {
                    query = query.Where(r => r.AssignedToQS == userId || r.Status == "submitted");
                    _logger.LogInformation($"Filtering by QS: {userId}");
                }
                // RM role sees all reports on map (collaborative view)

                // First, get the raw data from database
                var rawProperties = await query
                    .OrderByDescending(r => r.UpdatedAt)
                    .Select(r => new
                    {
                        r.Id,
                        r.LoanType,
                        r.Status,
                        r.VisitLatitude,
                        r.VisitLongitude,
                        r.LocationAddress,
                        r.IbpsNo,
                        r.VisitDate,
                        r.CustomerName,
                        r.CustomerNumber,
                        r.AssignedToRM,
                        r.AssignedToQSName,
                        r.UpdatedAt,
                        r.SiteVisitFormJson
                    })
                    .ToListAsync();

                // Transform and calculate progress in memory (after database query)
                var properties = rawProperties.Select(r => new
                {
                    r.Id,
                    ProjectName = r.LoanType ?? "Unnamed Project",
                    Status = r.Status,
                    Latitude = r.VisitLatitude,
                    Longitude = r.VisitLongitude,
                    SiteAddress = r.LocationAddress ?? "",
                    r.IbpsNo,
                    VisitDate = r.VisitDate,
                    CustomerName = r.CustomerName,
                    CustomerNumber = r.CustomerNumber,
                    RMName = _context.Users
                        .Where(u => u.Id == r.AssignedToRM)
                        .Select(u => u.FirstName + " " + u.LastName)
                        .FirstOrDefault(),
                    QSName = r.AssignedToQSName ?? "Not Assigned",
                    LastUpdated = r.UpdatedAt,
                    Progress = CalculateProgressStatic(r.SiteVisitFormJson, r.Status),
                    Photos = new List<object>()
                }).ToList();

                _logger.LogInformation($"Returning {properties.Count} properties for map");
                return Ok(properties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching properties for map");
                return StatusCode(500, new { message = "Error fetching properties", error = ex.Message });
            }
        }

        // Static helper to calculate progress (can be used in memory)
        private static int CalculateProgressStatic(string? siteVisitFormJson, string? status)
        {
            // Parse SiteVisitFormJson to get progress if available
            if (!string.IsNullOrEmpty(siteVisitFormJson) && siteVisitFormJson != "null")
            {
                try
                {
                    var formData = JsonSerializer.Deserialize<JsonElement>(siteVisitFormJson);
                    
                    // Check for drawn funds vs loan amount
                    if (formData.TryGetProperty("constructionLoanAmount", out var loanAmount) &&
                        formData.TryGetProperty("drawnFundsSubtotal", out var drawnFunds))
                    {
                        if (loanAmount.TryGetDecimal(out var loan) && loan > 0 &&
                            drawnFunds.TryGetDecimal(out var drawn))
                        {
                            return Math.Min(100, (int)(drawn / loan * 100));
                        }
                    }
                    
                    // Check for works complete
                    if (formData.TryGetProperty("worksComplete", out var worksComplete))
                    {
                        var worksText = worksComplete.GetString() ?? "";
                        if (worksText.ToLower().Contains("complete") || worksText.ToLower().Contains("done"))
                            return 75;
                    }
                }
                catch (Exception ex)
                {
                    // Log error but don't fail
                    Console.WriteLine($"Error parsing SiteVisitFormJson: {ex.Message}");
                }
            }
            
            // Default progress based on status
            return status?.ToLower() switch
            {
                "approved" => 100,
                "submitted" => 75,
                "rework" => 50,
                "draft" => 25,
                "pending" => 10,
                _ => 0
            };
        }

        // GET: api/map/properties/all - Includes all properties including demo/pending ones
        [HttpGet("properties/all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllPropertiesForMap()
        {
            try
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Guid? userGuid = userId != null ? Guid.Parse(userId) : (Guid?)null;

                _logger.LogInformation($"Fetching all properties (including demo) for user {userId} with role {userRole}");

                // Base query - get all checklists with or without coordinates
                var query = _context.Checklists.AsQueryable();

                // Filter by role
                // QS role can see submitted or assigned reports
                if (userRole == "QS")
                {
                    query = query.Where(r => r.AssignedToQS == userId || r.Status == "submitted");
                    _logger.LogInformation($"Filtering by QS: {userId}");
                }
                // RM and other roles see all reports on map (collaborative view)

                // First, get the raw data from database
                var rawProperties = await query
                    .OrderByDescending(r => r.UpdatedAt)
                    .Select(r => new
                    {
                        r.Id,
                        r.LoanType,
                        r.Status,
                        r.VisitLatitude,
                        r.VisitLongitude,
                        r.LocationAddress,
                        r.IbpsNo,
                        r.VisitDate,
                        r.CustomerName,
                        r.CustomerNumber,
                        r.AssignedToRM,
                        r.AssignedToQSName,
                        r.UpdatedAt,
                        r.SiteVisitFormJson
                    })
                    .ToListAsync();

                // Transform all properties (including those without coordinates marked as demo)
                var properties = rawProperties.Select(r => new
                {
                    r.Id,
                    ProjectName = r.LoanType ?? "Unnamed Project",
                    Status = r.Status,
                    Latitude = r.VisitLatitude ?? 0,
                    Longitude = r.VisitLongitude ?? 0,
                    SiteAddress = r.LocationAddress ?? "",
                    r.IbpsNo,
                    VisitDate = r.VisitDate,
                    CustomerName = r.CustomerName,
                    CustomerNumber = r.CustomerNumber,
                    RMName = _context.Users
                        .Where(u => u.Id == r.AssignedToRM)
                        .Select(u => u.FirstName + " " + u.LastName)
                        .FirstOrDefault(),
                    QSName = r.AssignedToQSName ?? "Not Assigned",
                    LastUpdated = r.UpdatedAt,
                    Progress = CalculateProgressStatic(r.SiteVisitFormJson, r.Status),
                    IsDemo = (r.VisitLatitude == null || r.VisitLatitude == 0) || (r.VisitLongitude == null || r.VisitLongitude == 0),
                    Photos = new List<object>()
                }).ToList();

                _logger.LogInformation($"Returning {properties.Count} properties (including demo) for map");
                return Ok(properties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all properties for map");
                return StatusCode(500, new { message = "Error fetching properties", error = ex.Message });
            }
        }

        // GET: api/map/properties/stats
        [HttpGet("properties/stats")]
        public async Task<ActionResult<object>> GetPropertyStats()
        {
            try
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Guid? userGuid = userId != null ? Guid.Parse(userId) : (Guid?)null;

                // Start with all checklists that have coordinates
                var query = _context.Checklists
                    .Where(r => r.VisitLatitude.HasValue && r.VisitLatitude != 0 
                             && r.VisitLongitude.HasValue && r.VisitLongitude != 0
                             && r.Status != "pending");

                // QS role can see submitted or assigned reports
                if (userRole == "QS")
                {
                    query = query.Where(r => r.AssignedToQS == userId || r.Status == "submitted");
                }
                // RM role sees all reports on map (collaborative view)

                var total = await query.CountAsync();
                var withCoordinates = await query.CountAsync(r => r.VisitLatitude.HasValue && r.VisitLatitude != 0 
                                                               && r.VisitLongitude.HasValue && r.VisitLongitude != 0);
                var onTrack = await query.CountAsync(r => r.Status == "approved");
                var behind = await query.CountAsync(r => r.Status == "rework");
                var paused = await query.CountAsync(r => r.Status == "draft");
                var newCount = await query.CountAsync(r => r.Status == "pending" || r.Status == "submitted");
                
                var regionList = await query
                    .Where(r => r.LocationAddress != null)
                    .Select(r => r.LocationAddress)
                    .ToListAsync();
                
                var regions = regionList
                    .Select(addr => addr?.Split(',').Last().Trim())
                    .Where(addr => addr != null)
                    .Distinct()
                    .Count();
                
                var stats = new
                {
                    Total = total,
                    OnTrack = onTrack,
                    BehindSchedule = behind,
                    Paused = paused,
                    New = newCount,
                    Regions = regions,
                    WithCoordinates = withCoordinates
                };

                _logger.LogInformation($"Stats: Total={total}, WithCoordinates={withCoordinates}");
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching property stats");
                return StatusCode(500, new { message = "Error fetching stats", error = ex.Message });
            }
        }

        // GET: api/map/properties/{id}
        [HttpGet("properties/{id}")]
        public async Task<ActionResult<object>> GetPropertyById(Guid id)
        {
            try
            {
                var property = await _context.Checklists
                    .Where(r => r.Id == id)
                    .Select(r => new
                    {
                        r.Id,
                        ProjectName = r.LoanType ?? "Unnamed Project",
                        Status = r.Status,
                        Latitude = r.VisitLatitude,
                        Longitude = r.VisitLongitude,
                        SiteAddress = r.LocationAddress,
                        r.IbpsNo,
                        VisitDate = r.VisitDate,
                        CustomerName = r.CustomerName,
                        CustomerNumber = r.CustomerNumber,
                        RMName = _context.Users
                            .Where(u => u.Id == r.AssignedToRM)
                            .Select(u => u.FirstName + " " + u.LastName)
                            .FirstOrDefault(),
                        QSName = r.AssignedToQSName,
                        r.UpdatedAt,
                        r.SiteVisitFormJson,
                        r.DocumentsJson
                    })
                    .FirstOrDefaultAsync();

                if (property == null)
                {
                    return NotFound(new { message = "Property not found" });
                }

                // Calculate progress after retrieving data
                var result = new
                {
                    property.Id,
                    property.ProjectName,
                    property.Status,
                    property.Latitude,
                    property.Longitude,
                    property.SiteAddress,
                    property.IbpsNo,
                    property.VisitDate,
                    property.CustomerName,
                    property.CustomerNumber,
                    property.RMName,
                    property.QSName,
                    property.UpdatedAt,
                    property.SiteVisitFormJson,
                    property.DocumentsJson,
                    Progress = CalculateProgressStatic(property.SiteVisitFormJson, property.Status)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching property {id}");
                return StatusCode(500, new { message = "Error fetching property", error = ex.Message });
            }
        }

        // GET: api/map/properties/rm/{rmId}
        [HttpGet("properties/rm/{rmId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPropertiesByRM(Guid rmId)
        {
            try
            {
                var properties = await _context.Checklists
                    .Where(r => r.AssignedToRM == rmId)
                    .Where(r => r.VisitLatitude.HasValue && r.VisitLatitude != 0 
                             && r.VisitLongitude.HasValue && r.VisitLongitude != 0)
                    .Select(r => new
                    {
                        r.Id,
                        ProjectName = r.LoanType ?? "Unnamed Project",
                        Status = r.Status,
                        Latitude = r.VisitLatitude,
                        Longitude = r.VisitLongitude,
                        r.LocationAddress,
                        r.IbpsNo
                    })
                    .ToListAsync();

                return Ok(properties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching properties for RM {rmId}");
                return StatusCode(500, new { message = "Error fetching properties", error = ex.Message });
            }
        }

        // GET: api/map/properties/status/{status}
        [HttpGet("properties/status/{status}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPropertiesByStatus(string status)
        {
            try
            {
                var properties = await _context.Checklists
                    .Where(r => r.Status == status)
                    .Where(r => r.VisitLatitude.HasValue && r.VisitLatitude != 0 
                             && r.VisitLongitude.HasValue && r.VisitLongitude != 0)
                    .Select(r => new
                    {
                        r.Id,
                        ProjectName = r.LoanType ?? "Unnamed Project",
                        Status = r.Status,
                        Latitude = r.VisitLatitude,
                        Longitude = r.VisitLongitude,
                        r.LocationAddress,
                        r.IbpsNo
                    })
                    .ToListAsync();

                return Ok(properties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching properties with status {status}");
                return StatusCode(500, new { message = "Error fetching properties", error = ex.Message });
            }
        }

        // GET: api/map/properties/region/{region}
        [HttpGet("properties/region/{region}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPropertiesByRegion(string region)
        {
            try
            {
                var properties = await _context.Checklists
                    .Where(r => r.LocationAddress != null && r.LocationAddress.Contains(region))
                    .Where(r => r.VisitLatitude.HasValue && r.VisitLatitude != 0 
                             && r.VisitLongitude.HasValue && r.VisitLongitude != 0)
                    .Select(r => new
                    {
                        r.Id,
                        ProjectName = r.LoanType ?? "Unnamed Project",
                        Status = r.Status,
                        Latitude = r.VisitLatitude,
                        Longitude = r.VisitLongitude,
                        r.LocationAddress,
                        r.IbpsNo
                    })
                    .ToListAsync();

                return Ok(properties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching properties for region {region}");
                return StatusCode(500, new { message = "Error fetching properties", error = ex.Message });
            }
        }

        // POST: api/map/routes
        [HttpPost("routes")]
        [Authorize(Roles = "QS")]
        public async Task<ActionResult<object>> SaveRoute([FromBody] SaveRouteDto routeDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? "QS";

                var savedRoute = new
                {
                    id = Guid.NewGuid().ToString(),
                    qsId = userId,
                    qsName = userName,
                    name = routeDto.Name,
                    scheduledDate = routeDto.ScheduledDate,
                    waypoints = routeDto.Waypoints,
                    totalDistance = routeDto.TotalDistance,
                    totalDuration = routeDto.TotalDuration,
                    status = "planned",
                    notes = routeDto.Notes,
                    createdAt = DateTime.UtcNow
                };

                return Ok(savedRoute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving route");
                return StatusCode(500, new { message = "Error saving route", error = ex.Message });
            }
        }

        // GET: api/map/routes
        [HttpGet("routes")]
        [Authorize(Roles = "QS")]
        public async Task<ActionResult<IEnumerable<object>>> GetMyRoutes()
        {
            return Ok(new List<object>());
        }
    }

    public class SaveRouteDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public List<RouteWaypointDto> Waypoints { get; set; } = new List<RouteWaypointDto>();
        public double TotalDistance { get; set; }
        public int TotalDuration { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    public class RouteWaypointDto
    {
        public string PropertyId { get; set; } = string.Empty;
        public int Order { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}