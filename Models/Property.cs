// Models/Property.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace geoback.Models
{
    public enum PropertyStatus
    {
        pending,
        verified
    }
    
    public enum PinnedByRole
    {
        valuer,
        qs
    }

    [Table("Properties")]
    public class Property
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [Column("customer_id")]
        public string CustomerId { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        [Column("lr_number")]
        public string LrNumber { get; set; } = string.Empty;
        
        [MaxLength(500)]
        [Column("location_address")]
        public string? LocationAddress { get; set; }
        
        [Column("latitude")]
        public decimal? Latitude { get; set; }
        
        [Column("longitude")]
        public decimal? Longitude { get; set; }
        
        [Column("status", TypeName = "varchar(20)")]
        public PropertyStatus Status { get; set; } = PropertyStatus.pending;
        
        [MaxLength(255)]
        [Column("pinned_by")]
        public string? PinnedBy { get; set; }
        
        [Column("pinned_by_role", TypeName = "varchar(10)")]
        public PinnedByRole PinnedByRole { get; set; } = PinnedByRole.valuer;
        
        [Column("pinned_at")]
        public DateTime PinnedAt { get; set; } = DateTime.UtcNow;
        
        [Column("verified_at")]
        public DateTime? VerifiedAt { get; set; }
        
        [MaxLength(255)]
        [Column("verified_by")]
        public string? VerifiedBy { get; set; }
        
        [Column("notes")]
        public string? Notes { get; set; }

[Column("photo_sections", TypeName = "json")]
public string? PhotoSectionsJson { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
        
        public virtual ICollection<PropertyPhoto> Photos { get; set; } = new List<PropertyPhoto>();
    }
}