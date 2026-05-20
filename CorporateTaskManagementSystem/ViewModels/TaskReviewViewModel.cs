using System.ComponentModel.DataAnnotations;
using CorporateTaskManagementSystem.Common.Enums;

namespace CorporateTaskManagementSystem.Web.ViewModels
{
    public class TaskReviewViewModel
    {
        public int TaskAssignmentId { get; set; }

        public int TaskItemId { get; set; }

        public string Title { get; set; }

        public string AssignedTo { get; set; }

        public string CurrentStatus { get; set; }

        public string EmployeeRemarks { get; set; }

        [Required(ErrorMessage = "Please select review status")]
        public TaskStatus ReviewStatus { get; set; }

        public string LeaderRemarks { get; set; }
    }
}