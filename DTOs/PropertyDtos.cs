// DTOs/PropertyDtos.cs
using geoback.Models;

namespace geoback.DTOs
{
    public class CreatePropertyDto
    {
        public string CustomerNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string LrNo { get; set; } = string.Empty;
        public string LocationAddress { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public List<string>? Photos { get; set; }
    }
    
    public class UpdatePropertyDto
    {
        public string? LrNo { get; set; }
        public string? LocationAddress { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? CustomerNumber { get; set; }  // For QS verification
        public string? Notes { get; set; }
        public bool Verify { get; set; }  // Set to true when QS verifies
    }
    
    public class PropertyResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string CustomerNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string LrNo { get; set; } = string.Empty;
        public string LocationAddress { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Status { get; set; } = "pending";
        public string PinnedBy { get; set; } = string.Empty;
        public string PinnedByRole { get; set; } = string.Empty;
        public DateTime PinnedAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? VerifiedBy { get; set; }
        public List<PropertyPhotoResponseDto> Photos { get; set; } = new();
    }
    
    public class PropertyPhotoResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string? PhotoType { get; set; }
        public string? Caption { get; set; }
        public string UploadedBy { get; set; } = string.Empty;
        public string UploadedByRole { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }
    
    public class CustomerAutoFillResponseDto
    {
        public string CustomerNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public bool Found { get; set; }
        public string? Message { get; set; }
    }
}