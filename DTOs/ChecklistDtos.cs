// DTOs/ChecklistDtos.cs
using System.Text.Json.Serialization;
using System.Text.Json;

#nullable enable

namespace geoback.DTOs
{
    public class ChecklistDocumentItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = "pendingrm";
        public string? Action { get; set; }
        public string? Comment { get; set; }
    }

    public class ChecklistDocumentCategoryDto
    {
        public string Category { get; set; } = string.Empty;
        public List<ChecklistDocumentItemDto> DocList { get; set; } = new();
    }

    public class CreateChecklistDto
    {
        public string? CustomerId { get; set; }
        public string CustomerNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerEmail { get; set; }
        
        public string LoanType { get; set; } = string.Empty;

        [JsonPropertyName("projectName")]
        public string ProjectName 
        { 
            get => LoanType; 
            set => LoanType = value; 
        }

        public string? IbpsNo { get; set; }
        
        [JsonConverter(typeof(GuidNullableConverter))]
        public Guid? AssignedToRM { get; set; }
        
        public List<ChecklistDocumentCategoryDto> Documents { get; set; } = new();
        
        public object? SiteVisitForm { get; set; }
        
        public string? Priority { get; set; }
        
        public double? VisitLatitude { get; set; }
        public double? VisitLongitude { get; set; }
        public string? LocationAddress { get; set; }
        public DateTime? VisitDate { get; set; }
        
        // DRAW DOWN FIELDS - CRITICAL FOR DRAW DOWN LINKING
        public int? DrawdownVersion { get; set; }
        
        [JsonConverter(typeof(GuidNullableConverter))]
        public Guid? ParentReportId { get; set; }
        
        public string? PropertyId { get; set; }
        public string? LrNo { get; set; }
        public string? SitePin { get; set; }
    }

    public class UpdateChecklistDto
    {
        public string? CustomerId { get; set; }
        public string CustomerNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerEmail { get; set; }
        
        public string LoanType { get; set; } = string.Empty;

        [JsonPropertyName("projectName")]
        public string ProjectName 
        { 
            get => LoanType; 
            set => LoanType = value; 
        }

        public string? IbpsNo { get; set; }
        public Guid? AssignedToRM { get; set; }
        public string? Status { get; set; }
        public List<ChecklistDocumentCategoryDto> Documents { get; set; } = new();
        
        public object? SiteVisitForm { get; set; }
        
        public string? Priority { get; set; }
        
        public double? VisitLatitude { get; set; }
        public double? VisitLongitude { get; set; }
        public string? LocationAddress { get; set; }
        public DateTime? VisitDate { get; set; }
        
        // DRAW DOWN FIELDS - CRITICAL FOR DRAW DOWN LINKING
        public int? DrawdownVersion { get; set; }
        
        [JsonConverter(typeof(GuidNullableConverter))]
        public Guid? ParentReportId { get; set; }
        
        public decimal? DrawdownAmountRequested { get; set; }
        public decimal? DrawdownAmountApproved { get; set; }
        
        public string? PropertyId { get; set; }
        public string? LrNo { get; set; }
        public string? SitePin { get; set; }
    }

    public class ChecklistUserRefDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
        
        [JsonPropertyName("role")]
        public string? Role { get; set; }
    }

    public class ChecklistResponseDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("_id")]
        public Guid MongoLikeId => Id;

        [JsonPropertyName("dclNo")]
        public string DclNo { get; set; } = string.Empty;
        
        [JsonPropertyName("callReportNo")]
        public string CallReportNo => DclNo;

        [JsonPropertyName("customerId")]
        public string? CustomerId { get; set; }
        
        [JsonPropertyName("customerNumber")]
        public string CustomerNumber { get; set; } = string.Empty;
        
        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; } = string.Empty;
        
        [JsonPropertyName("customerEmail")]
        public string? CustomerEmail { get; set; }
        
        [JsonPropertyName("loanType")]
        public string LoanType { get; set; } = string.Empty;
        
        [JsonPropertyName("projectName")]
        public string ProjectName 
        { 
            get => LoanType; 
            set => LoanType = value; 
        }

        [JsonPropertyName("ibpsNo")]
        public string? IbpsNo { get; set; }
        
        [JsonPropertyName("status")]
        public string Status { get; set; } = "pending";
        
        [JsonPropertyName("assignedToRM")]
        public ChecklistUserRefDto? AssignedToRM { get; set; }
        
        [JsonPropertyName("documents")]
        public List<ChecklistDocumentCategoryDto> Documents { get; set; } = new();
        
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        
        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }
        
        [JsonPropertyName("siteVisitForm")]
        public object? SiteVisitForm { get; set; }
        
        [JsonPropertyName("isLocked")]
        public bool IsLocked { get; set; }
        
        [JsonPropertyName("lockedBy")]
        public ChecklistUserRefDto? LockedBy { get; set; }
        
        [JsonPropertyName("lockedAt")]
        public DateTime? LockedAt { get; set; }
        
        [JsonPropertyName("assignedToQS")]
        public string? AssignedToQS { get; set; }
        
        [JsonPropertyName("assignedToQSName")]
        public string? AssignedToQSName { get; set; }
        
        [JsonPropertyName("submittedAt")]
        public DateTime? SubmittedAt { get; set; }
        
        [JsonPropertyName("priority")]
        public string? Priority { get; set; }
        
        [JsonPropertyName("reviewedAt")]
        public DateTime? ReviewedAt { get; set; }
        
        [JsonPropertyName("reviewedBy")]
        public string? ReviewedBy { get; set; }
        
        [JsonPropertyName("lockSessionId")]
        public string? LockSessionId { get; set; }
        
        [JsonPropertyName("lockHeartbeat")]
        public DateTime? LockHeartbeat { get; set; }
        
        [JsonPropertyName("lockExpiresAt")]
        public DateTime? LockExpiresAt { get; set; }
        
        [JsonPropertyName("visitLatitude")]
        public double? VisitLatitude { get; set; }
        
        [JsonPropertyName("visitLongitude")]
        public double? VisitLongitude { get; set; }
        
        [JsonPropertyName("locationAddress")]
        public string? LocationAddress { get; set; }
        
        [JsonPropertyName("visitDate")]
        public DateTime? VisitDate { get; set; }

        // DRAW DOWN FIELDS
        [JsonPropertyName("drawdownVersion")]
        public int? DrawdownVersion { get; set; }
        
        [JsonPropertyName("parentReportId")]
        public Guid? ParentReportId { get; set; }
        
        [JsonPropertyName("drawdownAmountRequested")]
        public decimal? DrawdownAmountRequested { get; set; }
        
        [JsonPropertyName("drawdownAmountApproved")]
        public decimal? DrawdownAmountApproved { get; set; }
        
        [JsonPropertyName("propertyId")]
        public string? PropertyId { get; set; }
        
        [JsonPropertyName("lrNo")]
        public string? LrNo { get; set; }
        
        [JsonPropertyName("sitePin")]
        public string? SitePin { get; set; }
    }

    public class GuidNullableConverter : JsonConverter<Guid?>
    {
        public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (Guid.TryParse(stringValue, out var guid))
                    return guid;
                if (string.IsNullOrEmpty(stringValue))
                    return null;
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString());
        }
    }
    
    // CommentDto is already defined elsewhere - do NOT redefine it here
    // If you need to use CommentDto, ensure it's imported from its existing location
}