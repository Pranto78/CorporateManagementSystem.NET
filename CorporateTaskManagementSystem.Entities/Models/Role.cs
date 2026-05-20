using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CorporateTaskManagementSystem.Entities.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public Role()
        {
            Users = new HashSet<User>();
        }
    }
}