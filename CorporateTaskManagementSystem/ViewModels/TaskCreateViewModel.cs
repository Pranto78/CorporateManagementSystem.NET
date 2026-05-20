using System;
using System.ComponentModel.DataAnnotations;

namespace CorporateTaskManagementSystem.Web.ViewModels
{
    public class TaskCreateViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Priority")]
        public int Priority { get; set; }

        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }

        [Required]
        [Display(Name = "Assign To Employee")]
        public int AssignedToUserId { get; set; }
    }
}