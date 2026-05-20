using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateTaskManagementSystem.Entities.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        public int RoleId { get; set; }

        public int? DepartmentId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public User()
        {
            IsActive = true;
            CreatedAt = DateTime.Now;
        }
    }
}