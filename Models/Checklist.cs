// Models/Checklist.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable enable

namespace geoback.Models
{
    [Table("Checklists")]
    public class Checklist
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string DclNo { get; set; } = string.Empty;

        // This should NOT be used for National ID - it's a separate identifier
        public string? CustomerId { get; set; }
        
        public string CustomerNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerEmail { get; set; }

        public string LoanType { get; set; } = string.Empty;

        public string? IbpsNo { get; set; }

        public string Status { get; set; } = "pending";

        public Guid? AssignedToRM { get; set; }

        [Column(TypeName = "longtext")]
        public string DocumentsJson { get; set; } = "[]";

        [Column(TypeName = "longtext")]
        public string? SiteVisitFormJson { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Map coordinate fields
        [Column(TypeName = "decimal(10,8)")]
        public decimal? VisitLatitude { get; set; }

        [Column(TypeName = "decimal(11,8)")]
        public decimal? VisitLongitude { get; set; }

        [MaxLength(500)]
        public string? LocationAddress { get; set; }

        public DateTime? VisitDate { get; set; }

        // Pinned Property Fields
        // FIX: Changed default from true to false for regular reports
        public bool IsPinnedOnly { get; set; } = false;
        
        [MaxLength(100)]
        public string? SitePin { get; set; }
        
        [MaxLength(100)]
        public string? PinnedBy { get; set; }
        
        public DateTime? PinnedAt { get; set; }

        // NEW: This is the National ID (matches CustomerId in Clients table)
        [MaxLength(50)]
        public string? NationalId { get; set; }
        
        // NEW: LR Number (Land Registry number)
        [MaxLength(100)]
        public string? LRNo { get; set; }

        // Lock fields
        public bool IsLocked { get; set; }
        public Guid? LockedByUserId { get; set; }
        public string? LockedByUserName { get; set; }
        public string? LockedByUserRole { get; set; }
        public DateTime? LockedAt { get; set; }

        public string? LockSessionId { get; set; }
        public DateTime? LockHeartbeat { get; set; }
        public DateTime? LockExpiresAt { get; set; }

        // QS fields
        public string? AssignedToQS { get; set; }
        public string? AssignedToQSName { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public string? Priority { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? ReviewedBy { get; set; }

        public DateTime? SiteVisitScheduledDate { get; set; }
        public string? SiteVisitNotes { get; set; }
        public string? SiteVisitFindings { get; set; }
        public DateTime? SiteVisitConfirmedAt { get; set; }
        public string? SiteVisitScheduledBy { get; set; }
        public string? SiteVisitScheduledByName { get; set; }

        // ========== NEW PROPERTIES FOR DRAWDOWNS ==========
        
        // Drawdown version (D1, D2, D3...)
        public int? DrawdownVersion { get; set; } = 1;
        
        // Reference to parent report for drawdown chains
        public Guid? ParentReportId { get; set; }
        
        // Reference to property (for linking to Properties table)
        public string? PropertyId { get; set; }
        
        // Drawdown amounts
        [Column(TypeName = "decimal(15,2)")]
        public decimal? DrawdownAmountRequested { get; set; }
        
        [Column(TypeName = "decimal(15,2)")]
        public decimal? DrawdownAmountApproved { get; set; }

        // Navigation properties
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        
        [ForeignKey("ParentReportId")]
        public virtual Checklist? ParentReport { get; set; }
        
        public virtual ICollection<Checklist>? ChildDrawdowns { get; set; }
    }
}