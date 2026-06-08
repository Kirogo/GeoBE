// Controllers/CRMReportController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using geoback.Models;
using geoback.Data;
using System.Security.Claims;
using System.Text.Json;

namespace geoback.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CRMReportController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CRMReportController> _logger;

        public CRMReportController(ApplicationDbContext context, ILogger<CRMReportController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                var query = _context.Set<CRMReport>().AsQueryable();

                if (userRole == "RM")
                {
                    query = query.Where(r => r.RmId == userId);
                }

                var reports = await query
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync();

                return Ok(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting CRM reports");
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var report = await _context.Set<CRMReport>().FindAsync(id);
                if (report == null)
                {
                    return NotFound(new { message = "Report not found" });
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userRole == "RM" && report.RmId != userId)
                {
                    return Forbid();
                }

                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting CRM report {Id}", id);
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JsonElement request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                // Parse all fields safely
                var subject = GetStringProperty(request, "subject");
                var purpose = GetStringProperty(request, "purpose", "Client visit");
                var purposeOther = GetStringProperty(request, "purposeOther");
                var customerType = GetStringProperty(request, "customerType", "Existing customer");
                var engagementType = GetStringProperty(request, "engagementType", "Appointment - Physical visit");
                var potentialCustomer = GetStringProperty(request, "potentialCustomer");
                var classification = GetStringProperty(request, "classification", "Service");
                var callPlan = GetStringProperty(request, "callPlan");
                var minutes = GetStringProperty(request, "minutes");
                var callResults = GetStringProperty(request, "callResults");
                var callReportStatus = GetStringProperty(request, "callReportStatus", "Customer contacted");
                
                // Customer info
                var customerNumber = GetStringProperty(request, "customerNumber");
                var customerName = GetStringProperty(request, "customerName");
                var customerEmail = GetStringProperty(request, "customerEmail");
                var locationAddress = GetStringProperty(request, "locationAddress");
                var lrNo = GetStringProperty(request, "lrNo");
                
                double? latitude = GetDoubleProperty(request, "latitude");
                double? longitude = GetDoubleProperty(request, "longitude");
                
                var rmName = GetStringProperty(request, "rmName", User.FindFirst(ClaimTypes.Name)?.Value ?? "");

                var report = new CRMReport
                {
                    Id = Guid.NewGuid().ToString(),
                    ReportNumber = GenerateReportNumber(),
                    Status = "draft",
                    Subject = subject,
                    Purpose = purpose,
                    PurposeOther = purposeOther,
                    CustomerType = customerType,
                    EngagementType = engagementType,
                    PotentialCustomer = potentialCustomer,
                    Classification = classification,
                    CallPlan = callPlan,
                    Minutes = minutes,
                    CallResults = callResults,
                    CallReportStatus = callReportStatus,
                    CustomerNumber = customerNumber,
                    CustomerName = string.IsNullOrEmpty(customerName) ? potentialCustomer : customerName,
                    CustomerEmail = customerEmail,
                    LocationAddress = locationAddress,
                    Latitude = latitude.HasValue ? (decimal)latitude.Value : (decimal?)null,
                    Longitude = longitude.HasValue ? (decimal)longitude.Value : (decimal?)null,
                    LrNo = lrNo,
                    RmId = userId,
                    RmName = rmName,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Set<CRMReport>().Add(report);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"CRM report created: {report.ReportNumber} by user {userId}");

                return Ok(new { message = "Report created successfully", report });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating CRM report");
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] JsonElement request)
        {
            try
            {
                var report = await _context.Set<CRMReport>().FindAsync(id);
                if (report == null)
                {
                    return NotFound(new { message = "Report not found" });
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userRole == "RM" && report.RmId != userId)
                {
                    return Forbid();
                }

                // Update fields
                if (request.TryGetProperty("subject", out var sub)) report.Subject = sub.GetString() ?? report.Subject;
                if (request.TryGetProperty("purpose", out var pur)) report.Purpose = pur.GetString() ?? report.Purpose;
                if (request.TryGetProperty("purposeOther", out var purOther)) report.PurposeOther = purOther.GetString() ?? report.PurposeOther;
                if (request.TryGetProperty("customerType", out var custType)) report.CustomerType = custType.GetString() ?? report.CustomerType;
                if (request.TryGetProperty("engagementType", out var engType)) report.EngagementType = engType.GetString() ?? report.EngagementType;
                if (request.TryGetProperty("potentialCustomer", out var potCust)) report.PotentialCustomer = potCust.GetString() ?? report.PotentialCustomer;
                if (request.TryGetProperty("classification", out var classif)) report.Classification = classif.GetString() ?? report.Classification;
                if (request.TryGetProperty("callPlan", out var plan)) report.CallPlan = plan.GetString() ?? report.CallPlan;
                if (request.TryGetProperty("minutes", out var mins)) report.Minutes = mins.GetString() ?? report.Minutes;
                if (request.TryGetProperty("callResults", out var results)) report.CallResults = results.GetString() ?? report.CallResults;
                if (request.TryGetProperty("callReportStatus", out var callStatus)) report.CallReportStatus = callStatus.GetString() ?? report.CallReportStatus;
                if (request.TryGetProperty("status", out var stat)) report.Status = stat.GetString() ?? report.Status;
                
                report.UpdatedAt = DateTime.UtcNow;

                _context.Entry(report).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"CRM report updated: {report.ReportNumber} by user {userId}");

                return Ok(new { message = "Report updated successfully", report });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating CRM report {Id}", id);
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var report = await _context.Set<CRMReport>().FindAsync(id);
                if (report == null)
                {
                    return NotFound(new { message = "Report not found" });
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userRole == "RM" && report.RmId != userId)
                {
                    return Forbid();
                }

                _context.Set<CRMReport>().Remove(report);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"CRM report deleted: {report.ReportNumber} by user {userId}");

                return Ok(new { message = "Report deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting CRM report {Id}", id);
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }

        #region Private Helper Methods

        private string GenerateReportNumber()
        {
            var year = DateTime.UtcNow.ToString("yyyy");
            var month = DateTime.UtcNow.ToString("MM");
            var random = new Random().Next(1, 9999).ToString("D4");
            return $"CRM-{year}{month}-{random}";
        }

        private string GetStringProperty(JsonElement element, string propertyName, string defaultValue = "")
        {
            if (element.TryGetProperty(propertyName, out var value) && value.ValueKind != JsonValueKind.Null)
            {
                return value.GetString() ?? defaultValue;
            }
            return defaultValue;
        }

        private double? GetDoubleProperty(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var value) && value.ValueKind != JsonValueKind.Null)
            {
                if (value.ValueKind == JsonValueKind.Number && value.TryGetDouble(out var doubleValue))
                {
                    return doubleValue;
                }
            }
            return null;
        }

        #endregion
    }
}