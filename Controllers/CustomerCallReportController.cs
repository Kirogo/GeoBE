// Controllers/CustomerCallReportController.cs 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using geoback.Models;
using geoback.Data;
using System.Security.Claims;

namespace geoback.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerCallReportController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomerCallReportController> _logger;

        public CustomerCallReportController(ApplicationDbContext context, ILogger<CustomerCallReportController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/customercallreport
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerCallReport>>> GetAll()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                var query = _context.CustomerCallReports.AsQueryable();

                if (userRole == "RM")
                {
                    query = query.Where(r => r.RmId == userId);
                }

                var reports = await query
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync();

                _logger.LogInformation($"Retrieved {reports.Count} customer call reports for user {userId}");
                
                return Ok(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer call reports");
                return StatusCode(500, new { message = $"An error occurred while retrieving reports: {ex.Message}" });
            }
        }

        // GET: api/customercallreport/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerCallReport>> GetById(string id)
        {
            try
            {
                var report = await _context.CustomerCallReports.FindAsync(id);
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
                _logger.LogError(ex, "Error getting customer call report {Id}", id);
                return StatusCode(500, new { message = $"An error occurred while retrieving the report: {ex.Message}" });
            }
        }

        // POST: api/customercallreport
        [HttpPost]
        public async Task<ActionResult<object>> Create([FromBody] CustomerCallReport request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userRole == "RM" && request.RmId != userId)
                {
                    return BadRequest(new { message = "Cannot create report for another user" });
                }

                // Generate report number if not provided
                if (string.IsNullOrEmpty(request.ReportNumber))
                {
                    request.ReportNumber = GenerateReportNumber();
                }

                request.Id = Guid.NewGuid().ToString();
                request.CreatedAt = DateTime.UtcNow;
                request.UpdatedAt = DateTime.UtcNow;

                // Ensure JSON fields are not null and truncate if needed
                request.StakeholdersJson = TruncateString(request.StakeholdersJson, 16777215);
                request.FacilitiesJson = TruncateString(request.FacilitiesJson, 16777215);
                request.ConnectedExposuresJson = TruncateString(request.ConnectedExposuresJson, 16777215);
                request.OtherBankFacilitiesJson = TruncateString(request.OtherBankFacilitiesJson, 16777215);
                request.TotalNCBAExposureJson = TruncateString(request.TotalNCBAExposureJson, 16777215);
                request.TotalConnectedExposureJson = TruncateString(request.TotalConnectedExposureJson, 16777215);
                request.TotalOtherBanksExposureJson = TruncateString(request.TotalOtherBanksExposureJson, 16777215);
                request.TotalGroupExposureJson = TruncateString(request.TotalGroupExposureJson, 16777215);
                request.SecurityItemsJson = TruncateString(request.SecurityItemsJson, 16777215);
                request.TotalSecurityJson = TruncateString(request.TotalSecurityJson, 16777215);
                request.SecurityDescriptionsJson = TruncateString(request.SecurityDescriptionsJson, 16777215);
                request.SecuritiesJson = TruncateString(request.SecuritiesJson, 16777215);
                request.UnsecuredParametersJson = TruncateString(request.UnsecuredParametersJson, 16777215);
                request.SecurityPhotosJson = TruncateString(request.SecurityPhotosJson, 16777215);
                request.SummaryOfRequestsJson = TruncateString(request.SummaryOfRequestsJson, 16777215);
                request.WhatDidClientSayPhotosJson = TruncateString(request.WhatDidClientSayPhotosJson, 16777215);
                request.JustificationPhotosJson = TruncateString(request.JustificationPhotosJson, 16777215);
                request.CrbCheckResultsJson = TruncateString(request.CrbCheckResultsJson, 16777215);
                request.PolicyComplianceJson = TruncateString(request.PolicyComplianceJson, 16777215);
                request.GeneralConditionsJson = TruncateString(request.GeneralConditionsJson, 16777215);
                request.OtherConditionsJson = TruncateString(request.OtherConditionsJson, 16777215);
                request.CovenantsJson = TruncateString(request.CovenantsJson, 16777215);
                request.BankAccountsJson = TruncateString(request.BankAccountsJson, 16777215);
                request.AttachmentsJson = TruncateString(request.AttachmentsJson, 16777215);
                request.PhotoSectionsJson = TruncateString(request.PhotoSectionsJson, 16777215);
                request.PinnedLocationJson = TruncateString(request.PinnedLocationJson, 16777215);
                
                // Truncate text fields that might be too long
                request.UnsecuredAmount = TruncateString(request.UnsecuredAmount, 65535);
                request.AmountRequested = TruncateString(request.AmountRequested, 65535);
                request.YearsInBusiness = TruncateString(request.YearsInBusiness, 65535);
                request.Branch = TruncateString(request.Branch, 65535);
                request.TypeOfCompany = TruncateString(request.TypeOfCompany, 65535);
                request.BankOfficial = TruncateString(request.BankOfficial, 65535);
                request.TotalLoanExposureStamped = TruncateString(request.TotalLoanExposureStamped, 65535);
                request.TotalLoanExposureExtended = TruncateString(request.TotalLoanExposureExtended, 65535);
                request.SecurityCoverageStamped = TruncateString(request.SecurityCoverageStamped, 65535);
                request.SecurityCoverageExtended = TruncateString(request.SecurityCoverageExtended, 65535);
                request.TenorAndMoratorium = TruncateString(request.TenorAndMoratorium, 65535);
                request.AverageMonthlyTurnover = TruncateString(request.AverageMonthlyTurnover, 65535);
                request.AverageBalance = TruncateString(request.AverageBalance, 65535);
                request.SelectedProduct = TruncateString(request.SelectedProduct, 65535);

                _context.CustomerCallReports.Add(request);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Customer call report created: {request.ReportNumber} by user {userId}");

                return Ok(new { message = "Report created successfully", report = request });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error creating customer call report");
                return StatusCode(500, new { message = $"Database error: {ex.InnerException?.Message ?? ex.Message}" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer call report");
                return StatusCode(500, new { message = $"An error occurred while creating the report: {ex.Message}" });
            }
        }

        // PUT: api/customercallreport/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<object>> Update(string id, [FromBody] CustomerCallReport request)
        {
            try
            {
                var report = await _context.CustomerCallReports.FindAsync(id);
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

                // Update all fields from request with truncation
                report.Status = request.Status;
                report.ClientName = request.ClientName ?? "";
                report.YearsInBusiness = TruncateString(request.YearsInBusiness, 65535);
                report.NatureOfBusiness = request.NatureOfBusiness ?? "";
                report.LocationOfBusiness = TruncateString(request.LocationOfBusiness, 65535);
                report.Branch = TruncateString(request.Branch, 65535);
                report.TypeOfCompany = TruncateString(request.TypeOfCompany, 65535);
                report.BankOfficial = TruncateString(request.BankOfficial, 65535);
                
                // JSON fields
                report.StakeholdersJson = TruncateString(request.StakeholdersJson, 16777215);
                report.FacilitiesJson = TruncateString(request.FacilitiesJson, 16777215);
                report.ConnectedExposuresJson = TruncateString(request.ConnectedExposuresJson, 16777215);
                report.OtherBankFacilitiesJson = TruncateString(request.OtherBankFacilitiesJson, 16777215);
                report.TotalNCBAExposureJson = TruncateString(request.TotalNCBAExposureJson, 16777215);
                report.TotalConnectedExposureJson = TruncateString(request.TotalConnectedExposureJson, 16777215);
                report.TotalOtherBanksExposureJson = TruncateString(request.TotalOtherBanksExposureJson, 16777215);
                report.TotalGroupExposureJson = TruncateString(request.TotalGroupExposureJson, 16777215);
                report.SecurityHeldForConnected = TruncateString(request.SecurityHeldForConnected, 65535);
                report.SecurityHeldAtOtherBanks = TruncateString(request.SecurityHeldAtOtherBanks, 65535);
                report.SblInsiderNotes = TruncateString(request.SblInsiderNotes, 65535);
                
                report.SecurityItemsJson = TruncateString(request.SecurityItemsJson, 16777215);
                report.TotalSecurityJson = TruncateString(request.TotalSecurityJson, 16777215);
                report.TotalLoanExposureStamped = TruncateString(request.TotalLoanExposureStamped, 65535);
                report.TotalLoanExposureExtended = TruncateString(request.TotalLoanExposureExtended, 65535);
                report.SecurityCoverageStamped = TruncateString(request.SecurityCoverageStamped, 65535);
                report.SecurityCoverageExtended = TruncateString(request.SecurityCoverageExtended, 65535);
                report.SecurityNotes = request.SecurityNotes ?? "";
                report.SecurityDescriptionsJson = TruncateString(request.SecurityDescriptionsJson, 16777215);
                report.SecuritiesJson = TruncateString(request.SecuritiesJson, 16777215);
                report.UnsecuredAmount = TruncateString(request.UnsecuredAmount, 65535);
                report.UnsecuredParametersJson = TruncateString(request.UnsecuredParametersJson, 16777215);
                report.SecurityPhotosJson = TruncateString(request.SecurityPhotosJson, 16777215);
                report.SecurityPhotosNotes = TruncateString(request.SecurityPhotosNotes, 65535);
                
                report.SummaryOfRequestsJson = TruncateString(request.SummaryOfRequestsJson, 16777215);
                report.WhatDidClientSay = request.WhatDidClientSay ?? "";
                report.WhatDidClientSayPhotosJson = TruncateString(request.WhatDidClientSayPhotosJson, 16777215);
                report.AmountRequested = TruncateString(request.AmountRequested, 65535);
                report.HowWillBeDrawn = request.HowWillBeDrawn ?? "";
                report.Justification = request.Justification ?? "";
                report.JustificationPhotosJson = TruncateString(request.JustificationPhotosJson, 16777215);
                report.RepaymentSource = request.RepaymentSource ?? "";
                report.CertaintyOfRepayment = request.CertaintyOfRepayment ?? "";
                report.TenorAndMoratorium = TruncateString(request.TenorAndMoratorium, 65535);
                
                report.BorrowerBackground = request.BorrowerBackground ?? "";
                report.BorrowerHistory = request.BorrowerHistory ?? "";
                report.BorrowerDirectorsProfile = request.BorrowerDirectorsProfile ?? "";
                report.BorrowerEmployees = request.BorrowerEmployees ?? "";
                report.BorrowerMainBankers = TruncateString(request.BorrowerMainBankers, 65535);
                report.BorrowerCoreBusiness = TruncateString(request.BorrowerCoreBusiness, 65535);
                report.BorrowerProductsServices = TruncateString(request.BorrowerProductsServices, 65535);
                report.BorrowerCertifications = request.BorrowerCertifications ?? "";
                report.BorrowerModusOperandi = request.BorrowerModusOperandi ?? "";
                report.BorrowerMainSuppliers = request.BorrowerMainSuppliers ?? "";
                report.BorrowerMainCustomers = request.BorrowerMainCustomers ?? "";
                report.BorrowerOtherBusiness = request.BorrowerOtherBusiness ?? "";
                report.BorrowerCreditorsTerms = request.BorrowerCreditorsTerms ?? "";
                report.BorrowerContractsInfo = request.BorrowerContractsInfo ?? "";
                report.BorrowerRelatedBusiness = request.BorrowerRelatedBusiness ?? "";
                
                report.CrbCheckResultsJson = TruncateString(request.CrbCheckResultsJson, 16777215);
                report.PolicyComplianceJson = TruncateString(request.PolicyComplianceJson, 16777215);
                report.GeneralConditionsJson = TruncateString(request.GeneralConditionsJson, 16777215);
                report.OtherConditionsJson = TruncateString(request.OtherConditionsJson, 16777215);
                report.CovenantsJson = TruncateString(request.CovenantsJson, 16777215);
                report.SelectedProduct = TruncateString(request.SelectedProduct, 65535);
                
                report.BankAccountsJson = TruncateString(request.BankAccountsJson, 16777215);
                report.AverageMonthlyTurnover = TruncateString(request.AverageMonthlyTurnover, 65535);
                report.AverageBalance = TruncateString(request.AverageBalance, 65535);
                report.AccountPerformanceNotes = request.AccountPerformanceNotes ?? "";
                
                report.AttachmentsJson = TruncateString(request.AttachmentsJson, 16777215);
                report.PhotoSectionsJson = TruncateString(request.PhotoSectionsJson, 16777215);
                report.PinnedLocationJson = TruncateString(request.PinnedLocationJson, 16777215);
                report.BusinessLatitude = request.BusinessLatitude;
                report.BusinessLongitude = request.BusinessLongitude;
                
                report.UpdatedAt = DateTime.UtcNow;

                _context.Entry(report).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Customer call report updated: {report.ReportNumber} by user {userId}");

                return Ok(new { message = "Report updated successfully", report = report });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating customer call report {Id}", id);
                return StatusCode(500, new { message = $"Database error: {ex.InnerException?.Message ?? ex.Message}" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer call report {Id}", id);
                return StatusCode(500, new { message = $"An error occurred while updating the report: {ex.Message}" });
            }
        }

        // DELETE: api/customercallreport/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var report = await _context.CustomerCallReports.FindAsync(id);
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

                _context.CustomerCallReports.Remove(report);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Customer call report deleted: {report.ReportNumber} by user {userId}");

                return Ok(new { message = "Report deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer call report {Id}", id);
                return StatusCode(500, new { message = $"An error occurred while deleting the report: {ex.Message}" });
            }
        }

        #region Private Helper Methods

        private string GenerateReportNumber()
        {
            var year = DateTime.UtcNow.ToString("yyyy");
            var month = DateTime.UtcNow.ToString("MM");
            var random = new Random().Next(1, 9999).ToString("D4");
            return $"CCR-{year}{month}-{random}";
        }

        private string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (value.Length <= maxLength) return value;
            return value.Substring(0, maxLength);
        }

        #endregion
    }
}