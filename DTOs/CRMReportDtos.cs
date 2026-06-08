using System;
using System.Text.Json.Serialization;

namespace geoback.DTOs
{
    // Create CRM Report DTO
    public class CreateCRMReportDto
    {
        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        [JsonPropertyName("purpose")]
        public string Purpose { get; set; } = string.Empty;

        [JsonPropertyName("purposeOther")]
        public string PurposeOther { get; set; } = string.Empty;

        [JsonPropertyName("customerType")]
        public string CustomerType { get; set; } = string.Empty;

        [JsonPropertyName("engagementType")]
        public string EngagementType { get; set; } = string.Empty;

        [JsonPropertyName("potentialCustomer")]
        public string PotentialCustomer { get; set; } = string.Empty;

        [JsonPropertyName("classification")]
        public string Classification { get; set; } = string.Empty;

        [JsonPropertyName("callPlan")]
        public string CallPlan { get; set; } = string.Empty;

        [JsonPropertyName("minutes")]
        public string Minutes { get; set; } = string.Empty;

        [JsonPropertyName("callResults")]
        public string CallResults { get; set; } = string.Empty;

        [JsonPropertyName("callReportStatus")]
        public string CallReportStatus { get; set; } = string.Empty;

        [JsonPropertyName("customerNumber")]
        public string CustomerNumber { get; set; } = string.Empty;

        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; } = string.Empty;

        [JsonPropertyName("customerEmail")]
        public string CustomerEmail { get; set; } = string.Empty;

        [JsonPropertyName("locationAddress")]
        public string LocationAddress { get; set; } = string.Empty;

        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }

        [JsonPropertyName("lrNo")]
        public string LrNo { get; set; } = string.Empty;

        [JsonPropertyName("rmId")]
        public string RmId { get; set; } = string.Empty;

        [JsonPropertyName("rmName")]
        public string RmName { get; set; } = string.Empty;
    }

    // Update CRM Report DTO
    public class UpdateCRMReportDto
    {
        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        [JsonPropertyName("purpose")]
        public string Purpose { get; set; } = string.Empty;

        [JsonPropertyName("purposeOther")]
        public string PurposeOther { get; set; } = string.Empty;

        [JsonPropertyName("customerType")]
        public string CustomerType { get; set; } = string.Empty;

        [JsonPropertyName("engagementType")]
        public string EngagementType { get; set; } = string.Empty;

        [JsonPropertyName("potentialCustomer")]
        public string PotentialCustomer { get; set; } = string.Empty;

        [JsonPropertyName("classification")]
        public string Classification { get; set; } = string.Empty;

        [JsonPropertyName("callPlan")]
        public string CallPlan { get; set; } = string.Empty;

        [JsonPropertyName("minutes")]
        public string Minutes { get; set; } = string.Empty;

        [JsonPropertyName("callResults")]
        public string CallResults { get; set; } = string.Empty;

        [JsonPropertyName("callReportStatus")]
        public string CallReportStatus { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = "draft";
    }

    // CRM Report Response DTO
    public class CRMReportResponseDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("reportNumber")]
        public string ReportNumber { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        [JsonPropertyName("purpose")]
        public string Purpose { get; set; } = string.Empty;

        [JsonPropertyName("purposeOther")]
        public string PurposeOther { get; set; } = string.Empty;

        [JsonPropertyName("customerType")]
        public string CustomerType { get; set; } = string.Empty;

        [JsonPropertyName("engagementType")]
        public string EngagementType { get; set; } = string.Empty;

        [JsonPropertyName("potentialCustomer")]
        public string PotentialCustomer { get; set; } = string.Empty;

        [JsonPropertyName("classification")]
        public string Classification { get; set; } = string.Empty;

        [JsonPropertyName("callPlan")]
        public string CallPlan { get; set; } = string.Empty;

        [JsonPropertyName("minutes")]
        public string Minutes { get; set; } = string.Empty;

        [JsonPropertyName("callResults")]
        public string CallResults { get; set; } = string.Empty;

        [JsonPropertyName("callReportStatus")]
        public string CallReportStatus { get; set; } = string.Empty;

        [JsonPropertyName("customerNumber")]
        public string CustomerNumber { get; set; } = string.Empty;

        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; } = string.Empty;

        [JsonPropertyName("customerEmail")]
        public string CustomerEmail { get; set; } = string.Empty;

        [JsonPropertyName("locationAddress")]
        public string LocationAddress { get; set; } = string.Empty;

        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }

        [JsonPropertyName("lrNo")]
        public string LrNo { get; set; } = string.Empty;

        [JsonPropertyName("rmId")]
        public string RmId { get; set; } = string.Empty;

        [JsonPropertyName("rmName")]
        public string RmName { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}