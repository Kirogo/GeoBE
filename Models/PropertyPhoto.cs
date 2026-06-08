using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace geoback.Models
{
    public enum PhotoUploadedByRole
    {
        valuer,
        qs
    }

    [Table("PropertyPhotos")]
    public class PropertyPhoto
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [Column("property_id")]
        public string PropertyId { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(500)]
        [Column("photo_url")]
        public string PhotoUrl { get; set; } = string.Empty;
        
        [MaxLength(50)]
        [Column("photo_type")]
        public string? PhotoType { get; set; } = "site_photo";
        
        [Column("caption")]
        public string? Caption { get; set; }
        
        [MaxLength(255)]
        [Column("uploaded_by")]
        public string? UploadedBy { get; set; }
        
        [Column("uploaded_by_role")]
        public PhotoUploadedByRole UploadedByRole { get; set; } = PhotoUploadedByRole.valuer;
        
        [Column("uploaded_at")]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        
        [ForeignKey("PropertyId")]
        public virtual Property? Property { get; set; }
    }
}