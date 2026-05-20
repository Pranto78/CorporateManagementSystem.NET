using System.ComponentModel.DataAnnotations;
using CorporateTaskManagementSystem.Common.Enums;

namespace CorporateTaskManagementSystem.Web.ViewModels
{
    public class TaskStatusUpdateViewModel
    {
        public int TaskAssignmentId { get; set; }

        public int TaskItemId { get; set; }

        public string Title { get; set; }

        public string CurrentStatus { get; set; }

        [Required(ErrorMessage = "Please select a status")]
        public TaskStatus NewStatus { get; set; }

        public string Remarks { get; set; }
    }
}