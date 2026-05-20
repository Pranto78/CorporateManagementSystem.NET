using System.ComponentModel.DataAnnotations;

namespace CorporateTaskManagementSystem.Web.ViewModels
{
    public class UserEditViewModel
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
    }
}