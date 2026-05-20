using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CorporateTaskManagementSystem.Entities.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public Department()
        {
            IsActive = true;
            Users = new HashSet<User>();
        }
    }
}