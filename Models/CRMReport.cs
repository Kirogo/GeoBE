// Models/CRMReport.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace geoback.Models
{
    [Table("crm_reports")]
    public class CRMReport
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("report_number")]
        public string ReportNumber { get; set; } = string.Empty;

        [Column("status")]
        public string Status { get; set; } = "draft";

        [Column("subject")]
        public string Subject { get; set; } = string.Empty;

        [Column("purpose")]
        public string Purpose { get; set; } = string.Empty;

        [Column("purpose_other", TypeName = "LONGTEXT")]
        public string PurposeOther { get; set; } = string.Empty;

        [Column("customer_type")]
        public string CustomerType { get; set; } = string.Empty;

        [Column("engagement_type")]
        public string EngagementType { get; set; } = string.Empty;

        [Column("potential_customer")]
        public string PotentialCustomer { get; set; } = string.Empty;

        [Column("classification")]
        public string Classification { get; set; } = string.Empty;

        [Column("call_plan", TypeName = "LONGTEXT")]
        public string CallPlan { get; set; } = string.Empty;

        [Column("minutes", TypeName = "LONGTEXT")]
        public string Minutes { get; set; } = string.Empty;

        [Column("call_results", TypeName = "LONGTEXT")]
        public string CallResults { get; set; } = string.Empty;

        [Column("call_report_status")]
        public string CallReportStatus { get; set; } = string.Empty;

        // Customer Information - Note the exact column names
        [Column("CustomerNumber")]
        public string CustomerNumber { get; set; } = string.Empty;

        [Column("CustomerName")]
        public string CustomerName { get; set; } = string.Empty;

        [Column("CustomerEmail")]
        public string CustomerEmail { get; set; } = string.Empty;

        [Column("LocationAddress", TypeName = "TEXT")]
        public string LocationAddress { get; set; } = string.Empty;

        [Column("Latitude")]
        public decimal? Latitude { get; set; }

        [Column("Longitude")]
        public decimal? Longitude { get; set; }

        [Column("LrNo")]
        public string LrNo { get; set; } = string.Empty;

        [Column("rm_id")]
        public string RmId { get; set; } = string.Empty;

        [Column("rm_name")]
        public string RmName { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}