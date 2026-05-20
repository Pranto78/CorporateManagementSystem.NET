using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateTaskManagementSystem.Entities.Models
{
    public class TaskAssignment
    {
        [Key]
        public int TaskAssignmentId { get; set; }

        public int TaskItemId { get; set; }

        public int AssignedToUserId { get; set; }

        public int AssignedByUserId { get; set; }

        public DateTime AssignedAt { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("TaskItemId")]
        public virtual TaskItem TaskItem { get; set; }

        [ForeignKey("AssignedToUserId")]
        public virtual User AssignedToUser { get; set; }

        [ForeignKey("AssignedByUserId")]
        public virtual User AssignedByUser { get; set; }

        public TaskAssignment()
        {
            AssignedAt = DateTime.Now;
            IsActive = true;
        }
    }
}