// Controllers/PropertiesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using geoback.Data;
using geoback.Models;
using System.Security.Claims;
using System.Text.Json;

#nullable enable

namespace geoback.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PropertiesController> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public PropertiesController(ApplicationDbContext context, ILogger<PropertiesController> logger)
        {
            _context = context;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        // GET: api/properties/pending
        [HttpGet("pending")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> GetPendingProperties()
        {
            try
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                
                if (userRole != "QS" && userRole != "Admin" && userRole != "SuperAdmin")
                {
                    return Forbid("Only QS, Admin, or SuperAdmin can access pending properties");
                }

                var pendingProperties = await _context.Properties
                    .Include(p => p.Customer)
                    .Include(p => p.Photos)
                    .Where(p => p.Status == PropertyStatus.pending)
                    .OrderByDescending(p => p.PinnedAt)
                    .Select(p => new
                    {
                        id = p.Id,
                        lrNo = p.LrNumber ?? string.Empty,
                        locationAddress = p.LocationAddress ?? string.Empty,
                        latitude = p.Latitude,
                        longitude = p.Longitude,
                        pinnedAt = p.PinnedAt,
                        customerName = p.Customer != null ? p.Customer.CustomerName : string.Empty,
                        customerEmail = p.Customer != null ? p.Customer.Email : string.Empty,
                        customerNumber = p.Customer != null ? p.Customer.CustomerNumber : string.Empty,
                        phone = p.Customer != null ? p.Customer.Phone : string.Empty,
                        pinnedBy = p.PinnedBy ?? string.Empty,
                        pinnedByRole = p.PinnedByRole.ToString(),
                        status = p.Status.ToString(),
                        photoSections = !string.IsNullOrWhiteSpace(p.PhotoSectionsJson) ? JsonSerializer.Deserialize<object>(p.PhotoSectionsJson, _jsonOptions) : null
                    })
                    .ToListAsync();

                return Ok(pendingProperties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending properties: {Message}", ex.Message);
                return StatusCode(500, new { message = "Error fetching properties", error = ex.Message });
            }
        }

        // GET: api/properties/verified
        [HttpGet("verified")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> GetVerifiedProperties()
        {
            try
            {
                var verifiedProperties = await _context.Properties
                    .Include(p => p.Customer)
                    .Where(p => p.Status == PropertyStatus.verified)
                    .OrderByDescending(p => p.VerifiedAt)
                    .Select(p => new
                    {
                        id = p.Id,
                        lrNo = p.LrNumber ?? string.Empty,
                        locationAddress = p.LocationAddress ?? string.Empty,
                        latitude = p.Latitude,
                        longitude = p.Longitude,
                        verifiedAt = p.VerifiedAt,
                        verifiedBy = p.VerifiedBy ?? string.Empty,
                        customerName = p.Customer != null ? p.Customer.CustomerName : string.Empty,
                        customerNumber = p.Customer != null ? p.Customer.CustomerNumber : string.Empty,
                        customerEmail = p.Customer != null ? p.Customer.Email : string.Empty,
                        phone = p.Customer != null ? p.Customer.Phone : string.Empty,
                        status = p.Status.ToString()
                    })
                    .ToListAsync();

                return Ok(verifiedProperties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting verified properties: {Message}", ex.Message);
                return StatusCode(500, new { message = "Error fetching properties", error = ex.Message });
            }
        }

        // GET: api/properties/pinned/all
        [HttpGet("pinned/all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> GetAllPinnedProperties()
        {
            try
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                
                if (userRole != "QS" && userRole != "Admin" && userRole != "SuperAdmin")
                {
                    return Forbid("Only QS, Admin, or SuperAdmin can access all pinned properties");
                }

                var properties = await _context.Properties
                    .Include(p => p.Customer)
                    .OrderByDescending(p => p.PinnedAt)
                    .Select(p => new
                    {
                        id = p.Id,
                        lrNo = p.LrNumber ?? string.Empty,
                        locationAddress = p.LocationAddress ?? string.Empty,
                        latitude = p.Latitude,
                        longitude = p.Longitude,
                        pinnedAt = p.PinnedAt,
                        verifiedAt = p.VerifiedAt,
                        customerName = p.Customer != null ? p.Customer.CustomerName : string.Empty,
                        customerEmail = p.Customer != null ? p.Customer.Email : string.Empty,
                        customerNumber = p.Customer != null ? p.Customer.CustomerNumber : string.Empty,
                        phone = p.Customer != null ? p.Customer.Phone : string.Empty,
                        pinnedBy = p.PinnedBy ?? string.Empty,
                        pinnedByRole = p.PinnedByRole.ToString(),
                        status = p.Status.ToString()
                    })
                    .ToListAsync();

                return Ok(properties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pinned properties: {Message}", ex.Message);
                return StatusCode(500, new { message = "Error fetching properties", error = ex.Message });
            }
        }

        // GET: api/properties/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<object>> GetPropertyById(string id)
        {
            try
            {
                var property = await _context.Properties
                    .Include(p => p.Customer)
                    .Include(p => p.Photos)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (property == null)
                {
                    return NotFound(new { message = "Property not found" });
                }

                object? photoSections = null;
                if (!string.IsNullOrWhiteSpace(property.PhotoSectionsJson))
                {
                    photoSections = JsonSerializer.Deserialize<object>(property.PhotoSectionsJson, _jsonOptions);
                }

                var result = new
                {
                    id = property.Id,
                    lrNo = property.LrNumber ?? string.Empty,
                    locationAddress = property.LocationAddress ?? string.Empty,
                    latitude = property.Latitude,
                    longitude = property.Longitude,
                    pinnedAt = property.PinnedAt,
                    verifiedAt = property.VerifiedAt,
                    verifiedBy = property.VerifiedBy ?? string.Empty,
                    customerName = property.Customer != null ? property.Customer.CustomerName : string.Empty,
                    customerNumber = property.Customer != null ? property.Customer.CustomerNumber : string.Empty,
                    customerEmail = property.Customer != null ? property.Customer.Email : string.Empty,
                    phone = property.Customer != null ? property.Customer.Phone : string.Empty,
                    pinnedBy = property.PinnedBy ?? string.Empty,
                    pinnedByRole = property.PinnedByRole.ToString(),
                    status = property.Status.ToString(),
                    photoSections = photoSections
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting property by ID: {Message}", ex.Message);
                return StatusCode(500, new { message = "Error fetching property", error = ex.Message });
            }
        }

        // GET: api/properties/auto-fill/{customerNumber}
        [HttpGet("auto-fill/{customerNumber}")]
        [Authorize]
        public async Task<ActionResult<object>> AutoFillCustomerDetails(string customerNumber)
        {
            try
            {
                var customerNumberDecoded = Uri.UnescapeDataString(customerNumber);
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerNumber == customerNumberDecoded);

                if (customer == null)
                {
                    return Ok(new { found = false, message = "Customer not found" });
                }

                return Ok(new
                {
                    found = true,
                    customerNumber = customer.CustomerNumber ?? string.Empty,
                    customerName = customer.CustomerName ?? string.Empty,
                    email = customer.Email ?? string.Empty,
                    phone = customer.Phone ?? string.Empty
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error auto-filling customer details: {Message}", ex.Message);
                return StatusCode(500, new { message = "Error fetching customer details", error = ex.Message });
            }
        }

        // GET: api/properties/{propertyId}/drawdowns
[HttpGet("{propertyId}/drawdowns")]
[Authorize]
public async Task<ActionResult<object>> GetDrawdownsByPropertyId(string propertyId)
{
    try
    {
        _logger.LogInformation($"Fetching drawdowns for property ID: '{propertyId}'");

        if (string.IsNullOrWhiteSpace(propertyId))
        {
            return BadRequest(new { message = "Property ID is required" });
        }

        // Get all drawdowns linked to this property
        var propertyDrawdowns = await _context.Checklists
            .Where(c => c.PropertyId == propertyId && !c.IsPinnedOnly)
            .OrderBy(c => c.DrawdownVersion)
            .Select(c => new
            {
                id = c.Id,
                version = c.DrawdownVersion ?? 1,
                status = c.Status,
                submittedAt = c.SubmittedAt,
                drawdownAmountRequested = c.DrawdownAmountRequested,
                drawdownAmountApproved = c.DrawdownAmountApproved,
                createdAt = c.CreatedAt,
                parentReportId = c.ParentReportId
            })
            .ToListAsync();

        // Check for blocking drawdowns (submitted or rework)
        var blockingDrawdown = propertyDrawdowns
            .FirstOrDefault(d => d.status == "submitted" || 
                                  d.status == "pending_qs_review" || 
                                  d.status == "pendingqsreview" || 
                                  d.status == "under_review" || 
                                  d.status == "rework");

        // Get the latest approved drawdown version
        var latestApproved = propertyDrawdowns
            .Where(d => d.status == "approved")
            .OrderByDescending(d => d.version)
            .FirstOrDefault();

        // Determine next drawdown version
        int nextVersion = 1;
        if (propertyDrawdowns.Any())
        {
            nextVersion = propertyDrawdowns.Max(d => d.version) + 1;
        }

        // Get first drawdown data for pre-population (if any approved drawdown exists)
        object? firstDrawdownData = null;
        var firstApproved = propertyDrawdowns.FirstOrDefault(d => d.status == "approved");
        if (firstApproved != null)
        {
            var report = await _context.Checklists.FindAsync(firstApproved.id);
            if (report != null && !string.IsNullOrWhiteSpace(report.SiteVisitFormJson))
            {
                try
                {
                    firstDrawdownData = JsonSerializer.Deserialize<object>(report.SiteVisitFormJson, _jsonOptions);
                }
                catch { }
            }
        }

        return Ok(new
        {
            propertyId = propertyId,
            hasDrawdowns = propertyDrawdowns.Any(),
            drawdowns = propertyDrawdowns,
            blockingDrawdown = blockingDrawdown != null ? new
            {
                version = blockingDrawdown.version,
                status = blockingDrawdown.status
            } : null,
            hasBlockingDrawdown = blockingDrawdown != null,
            canCreateNewDrawdown = blockingDrawdown == null,
            latestApprovedVersion = latestApproved?.version ?? 0,
            nextDrawdownVersion = nextVersion,
            totalDrawnFunds = propertyDrawdowns
                .Where(d => d.status == "approved")
                .Sum(d => d.drawdownAmountApproved ?? 0),
            firstDrawdownData = firstDrawdownData,
            parentReportId = propertyDrawdowns.FirstOrDefault()?.parentReportId
        });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error getting drawdowns by property ID: {Message}", ex.Message);
        return StatusCode(500, new { message = "Error fetching drawdowns", error = ex.Message });
    }
}

        // GET: api/properties/customer/{customerNumber} - For RM to search with drawdown history
        [HttpGet("customer/{customerNumber}")]
        [Authorize]
        public async Task<ActionResult<object>> GetCustomerWithVerifiedProperties(string customerNumber)
        {
            try
            {
                var customerNumberDecoded = Uri.UnescapeDataString(customerNumber);
                _logger.LogInformation($"Fetching customer with verified properties: '{customerNumberDecoded}'");

                if (string.IsNullOrWhiteSpace(customerNumberDecoded))
                {
                    return BadRequest(new { message = "Customer number is required" });
                }

                // Find customer by customer number
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerNumber == customerNumberDecoded);

                if (customer == null)
                {
                    _logger.LogWarning($"Customer '{customerNumberDecoded}' not found");
                    return NotFound(new { message = "Customer not found", hasProperties = false });
                }

                _logger.LogInformation($"Found customer: {customer.CustomerName} (ID: {customer.Id})");

                // Get verified properties from Properties table
                var verifiedProperties = await _context.Properties
                    .Where(p => p.CustomerId == customer.Id && p.Status == PropertyStatus.verified)
                    .Select(p => new
                    {
                        id = p.Id,
                        lrNo = p.LrNumber ?? string.Empty,
                        locationAddress = p.LocationAddress ?? string.Empty,
                        latitude = p.Latitude,
                        longitude = p.Longitude,
                        pinnedAt = p.PinnedAt,
                        status = p.Status.ToString()
                    })
                    .ToListAsync();

                _logger.LogInformation($"Found {verifiedProperties.Count} verified properties for customer {customerNumberDecoded}");

                // Get all drawdowns for this customer
                var allDrawdowns = await _context.Checklists
                    .Where(c => c.CustomerNumber == customer.CustomerNumber && !c.IsPinnedOnly && c.DrawdownVersion != null)
                    .OrderBy(c => c.DrawdownVersion)
                    .Select(c => new
                    {
                        version = c.DrawdownVersion ?? 1,
                        reportId = c.Id,
                        status = c.Status,
                        submittedAt = c.SubmittedAt,
                        drawdownAmountRequested = c.DrawdownAmountRequested,
                        drawdownAmountApproved = c.DrawdownAmountApproved,
                        createdAt = c.CreatedAt
                    })
                    .ToListAsync();

                // Get the first drawdown ID (D1) regardless of status for linking
                var firstDrawdown = allDrawdowns.OrderBy(d => d.version).FirstOrDefault();
                Guid? firstDrawdownId = firstDrawdown?.reportId;

                // Calculate drawdown summary
                var approvedDrawdowns = allDrawdowns.Where(d => d.status == "approved").ToList();
                var pendingDrawdowns = allDrawdowns.Where(d => d.status == "submitted" || d.status == "pending_qs_review" || d.status == "under_review").ToList();
                
                var totalDrawnFunds = approvedDrawdowns.Sum(d => d.drawdownAmountApproved ?? 0);
                var totalRequestedPending = pendingDrawdowns.Sum(d => d.drawdownAmountRequested ?? 0);
                
                // Get the first approved drawdown to get base data (Construction Loan, BQ Amount, etc.)
                var firstApprovedDrawdown = approvedDrawdowns.OrderBy(d => d.version).FirstOrDefault();
                object? firstDrawdownData = null;
                
                if (firstApprovedDrawdown != null)
                {
                    var firstReport = await _context.Checklists.FindAsync(firstApprovedDrawdown.reportId);
                    if (firstReport != null && !string.IsNullOrWhiteSpace(firstReport.SiteVisitFormJson))
                    {
                        try
                        {
                            firstDrawdownData = JsonSerializer.Deserialize<object>(firstReport.SiteVisitFormJson, _jsonOptions);
                        }
                        catch { }
                    }
                }

                var hasPendingDrawdown = pendingDrawdowns.Any();
                var nextDrawdownVersion = allDrawdowns.Any() ? allDrawdowns.Max(d => d.version) + 1 : 1;

                // Check if there's a pending drawdown that would block new creation
                var hasActivePendingDrawdown = pendingDrawdowns.Any(d => d.status != "rework");

                return Ok(new
                {
                    customerNumber = customer.CustomerNumber,
                    customerName = customer.CustomerName,
                    customerEmail = customer.Email ?? string.Empty,
                    nationalId = customer.NationalId ?? string.Empty,
                    phone = customer.Phone ?? string.Empty,
                    hasProperties = verifiedProperties.Any(),
                    properties = verifiedProperties,
                    drawdowns = allDrawdowns,
                    firstDrawdownId = firstDrawdownId, // ADD THIS LINE
                    drawdownSummary = new
                    {
                        totalDrawnFunds,
                        totalRequestedPending,
                        approvedCount = approvedDrawdowns.Count,
                        pendingCount = pendingDrawdowns.Count,
                        hasPendingDrawdown = hasPendingDrawdown,
                        hasActivePendingDrawdown = hasActivePendingDrawdown,
                        canCreateNewDrawdown = !hasActivePendingDrawdown,
                        lastApprovedVersion = approvedDrawdowns.Any() ? approvedDrawdowns.Max(d => d.version) : 0,
                        nextDrawdownVersion = nextDrawdownVersion,
                        firstDrawdownData = firstDrawdownData
                    },
                    existingDrawdowns = allDrawdowns,
                    nextDrawdownVersion = nextDrawdownVersion
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer with properties: {Message}", ex.Message);
                return StatusCode(500, new { message = "Error fetching customer data", error = ex.Message });
            }
        }

        // GET: api/properties/drawdowns/{parentReportId}
        [HttpGet("drawdowns/{parentReportId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> GetDrawdownsByParentReport(Guid parentReportId)
        {
            try
            {
                var drawdowns = await _context.Checklists
                    .Where(c => c.ParentReportId == parentReportId || c.Id == parentReportId)
                    .OrderBy(c => c.DrawdownVersion)
                    .Select(c => new
                    {
                        id = c.Id,
                        version = c.DrawdownVersion ?? 1,
                        status = c.Status,
                        submittedAt = c.SubmittedAt,
                        drawdownAmountRequested = c.DrawdownAmountRequested,
                        drawdownAmountApproved = c.DrawdownAmountApproved,
                        createdAt = c.CreatedAt
                    })
                    .ToListAsync();

                return Ok(drawdowns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drawdowns: {Message}", ex.Message);
                return StatusCode(500, new { message = "Error fetching drawdowns", error = ex.Message });
            }
        }

        // POST: api/properties/pin - Valuer pins property
        [HttpPost("pin")]
        [AllowAnonymous]
        public async Task<ActionResult> PinProperty([FromBody] PinPropertyDto dto)
        {
            try
            {
                _logger.LogInformation($"PinProperty attempt - Customer: {dto.CustomerName}");

                if (string.IsNullOrWhiteSpace(dto.CustomerName))
                {
                    return BadRequest(new { message = "Customer name is required" });
                }
                if (string.IsNullOrWhiteSpace(dto.LocationAddress))
                {
                    return BadRequest(new { message = "Location address is required" });
                }
                if (dto.Latitude == 0 || dto.Longitude == 0)
                {
                    return BadRequest(new { message = "Valid GPS coordinates are required" });
                }

                // Create a temporary customer record without customer number
                var tempCustomerNumber = $"TEMP_{Guid.NewGuid().ToString().Substring(0, 8)}";
                
                var customer = new Customer
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerNumber = tempCustomerNumber,
                    NationalId = null,
                    CustomerName = dto.CustomerName ?? string.Empty,
                    Email = dto.CustomerEmail ?? $"{tempCustomerNumber}@temp.com",
                    Phone = dto.Phone,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Created temporary customer: {customer.CustomerNumber}");

                string? photoSectionsJson = null;
                if (dto.PhotoSections != null && dto.PhotoSections.Any())
                {
                    photoSectionsJson = JsonSerializer.Serialize(dto.PhotoSections, _jsonOptions);
                }

                var property = new Property
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId = customer.Id,
                    LrNumber = dto.LRNo ?? string.Empty,
                    LocationAddress = dto.LocationAddress ?? string.Empty,
                    Latitude = (decimal?)dto.Latitude,
                    Longitude = (decimal?)dto.Longitude,
                    Status = PropertyStatus.pending,
                    PinnedBy = "Valuer",
                    PinnedByRole = PinnedByRole.valuer,
                    PinnedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PhotoSectionsJson = photoSectionsJson
                };

                _context.Properties.Add(property);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Property pinned successfully. ID: {property.Id}");
                return Ok(new
                {
                    message = "Property pinned successfully",
                    propertyId = property.Id,
                    status = property.Status.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error pinning property: {Message}", ex.Message);
                return StatusCode(500, new { message = "Error pinning property", error = ex.Message });
            }
        }

        // PUT: api/properties/{id} - QS updates/verifies property
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdatePinnedProperty(string id, [FromBody] UpdatePinnedPropertyDto dto)
        {
            try
            {
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var currentUserName = User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";

                var allowedRoles = new[] { "QS", "Admin", "SuperAdmin" };
                if (!allowedRoles.Contains(currentUserRole, StringComparer.OrdinalIgnoreCase))
                {
                    return Forbid("Only QS or Admin can update pinned properties");
                }

                var property = await _context.Properties
                    .Include(p => p.Customer)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (property == null)
                {
                    return NotFound(new { message = "Property not found" });
                }

                // Handle verification (linking to real customer)
                if (dto.Verify == true && property.Status == PropertyStatus.pending)
                {
                    if (!string.IsNullOrWhiteSpace(dto.CustomerNumber))
                    {
                        // Find the real customer by customer number
                        var realCustomer = await _context.Customers
                            .FirstOrDefaultAsync(c => c.CustomerNumber == dto.CustomerNumber);

                        if (realCustomer != null)
                        {
                            // Get the old customer ID to check if it's temporary
                            var oldCustomerId = property.CustomerId;
                            
                            // Update the property to link to the real customer
                            property.CustomerId = realCustomer.Id;
                            property.Status = PropertyStatus.verified;
                            property.VerifiedAt = DateTime.UtcNow;
                            property.VerifiedBy = currentUserName;
                            
                            // Update customer details if provided
                            if (!string.IsNullOrWhiteSpace(dto.CustomerEmail))
                            {
                                realCustomer.Email = dto.CustomerEmail;
                            }
                            if (!string.IsNullOrWhiteSpace(dto.Phone))
                            {
                                realCustomer.Phone = dto.Phone;
                            }
                            realCustomer.UpdatedAt = DateTime.UtcNow;
                            
                            // Delete the temporary customer if no other properties link to it
                            var tempCustomer = await _context.Customers
                                .FirstOrDefaultAsync(c => c.Id == oldCustomerId && c.CustomerNumber != null && c.CustomerNumber.StartsWith("TEMP_"));
                            if (tempCustomer != null)
                            {
                                var otherProperties = await _context.Properties
                                    .AnyAsync(p => p.CustomerId == tempCustomer.Id && p.Id != id);
                                if (!otherProperties)
                                {
                                    _context.Customers.Remove(tempCustomer);
                                }
                            }
                            
                            await _context.SaveChangesAsync();
                            _logger.LogInformation($"Property {id} verified by {currentUserName} and linked to customer {realCustomer.CustomerNumber}");
                        }
                        else
                        {
                            return BadRequest(new { message = $"Customer number {dto.CustomerNumber} not found" });
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = "Customer number is required for verification" });
                    }
                }
                else if (dto.IsEditing == true && property.Status == PropertyStatus.verified)
                {
                    // When editing a verified property, update fields but require re-verification
                    if (!string.IsNullOrWhiteSpace(dto.LRNo))
                        property.LrNumber = dto.LRNo;
                    
                    if (!string.IsNullOrWhiteSpace(dto.LocationAddress))
                        property.LocationAddress = dto.LocationAddress;
                    
                    if (dto.Latitude.HasValue)
                        property.Latitude = (decimal?)dto.Latitude.Value;
                    
                    if (dto.Longitude.HasValue)
                        property.Longitude = (decimal?)dto.Longitude.Value;
                    
                    if (!string.IsNullOrWhiteSpace(dto.Notes))
                        property.Notes = dto.Notes;
                    
                    // Set status back to pending for re-verification
                    property.Status = PropertyStatus.pending;
                    property.VerifiedAt = null;
                    property.VerifiedBy = null;
                    
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Property {id} edited by {currentUserName} and set back to pending for re-verification");
                }
                else if (dto.Verify == false && dto.IsEditing == false)
                {
                    // Regular update (draft save) - only update certain fields
                    if (!string.IsNullOrWhiteSpace(dto.LRNo))
                        property.LrNumber = dto.LRNo;
                    
                    if (!string.IsNullOrWhiteSpace(dto.LocationAddress))
                        property.LocationAddress = dto.LocationAddress;
                    
                    if (dto.Latitude.HasValue)
                        property.Latitude = (decimal?)dto.Latitude.Value;
                    
                    if (dto.Longitude.HasValue)
                        property.Longitude = (decimal?)dto.Longitude.Value;
                    
                    if (!string.IsNullOrWhiteSpace(dto.CustomerEmail) && property.Customer != null)
                        property.Customer.Email = dto.CustomerEmail;
                    
                    if (!string.IsNullOrWhiteSpace(dto.Phone) && property.Customer != null)
                        property.Customer.Phone = dto.Phone;
                    
                    property.UpdatedAt = DateTime.UtcNow;
                    
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Property {id} updated as draft by {currentUserName}");
                }

                return Ok(new
                {
                    message = dto.Verify == true ? "Property verified successfully" : "Property updated successfully",
                    status = property.Status.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating property: {Message}", ex.Message);
                return StatusCode(500, new { message = "Error updating property", error = ex.Message });
            }
        }

        // DELETE: api/properties/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeletePinnedProperty(string id)
        {
            try
            {
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                var allowedRoles = new[] { "QS", "Admin", "SuperAdmin" };
                if (!allowedRoles.Contains(currentUserRole, StringComparer.OrdinalIgnoreCase))
                {
                    return Forbid("Only QS or Admin can delete pinned properties");
                }

                var property = await _context.Properties.FindAsync(id);
                
                if (property != null)
                {
                    _context.Properties.Remove(property);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Property deleted successfully" });
                }

                return NotFound(new { message = "Property not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting property: {Message}", ex.Message);
                return StatusCode(500, new { message = "Error deleting property", error = ex.Message });
            }
        }
    }

    public class PinPropertyDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerEmail { get; set; }
        public string? Phone { get; set; }
        public string LocationAddress { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? LRNo { get; set; }
        public List<PhotoSectionDto>? PhotoSections { get; set; }
    }

    public class PhotoSectionDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public List<string> Photos { get; set; } = new List<string>();
    }

    public class UpdatePinnedPropertyDto
    {
        public string? CustomerName { get; set; }
        public string? CustomerNumber { get; set; }
        public string? CustomerEmail { get; set; }
        public string? Phone { get; set; }
        public string? LocationAddress { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? LRNo { get; set; }
        public string? Notes { get; set; }
        public bool? Verify { get; set; } = false;
        public bool? IsEditing { get; set; } = false;
    }
}