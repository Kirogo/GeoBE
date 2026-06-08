using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace geoback.Models
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [MaxLength(100)]
        [Column("customer_number")]
        public string CustomerNumber { get; set; } = string.Empty;
        
        [MaxLength(50)]
        [Column("national_id")]
        public string? NationalId { get; set; }  // Made nullable - remove Required
        
        [Required]
        [MaxLength(255)]
        [Column("customer_name")]
        public string CustomerName { get; set; } = string.Empty;
        
        [MaxLength(255)]
        [Column("email")]
        public string? Email { get; set; }
        
        [MaxLength(50)]
        [Column("phone")]
        public string? Phone { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}