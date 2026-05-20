using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CorporateTaskManagementSystem.Common.Enums;

namespace CorporateTaskManagementSystem.Entities.Models
{
    public class TaskStatusHistory
    {
        [Key]
        public int TaskStatusHistoryId { get; set; }

        public int TaskItemId { get; set; }

        public TaskStatus OldStatus { get; set; }

        public TaskStatus NewStatus { get; set; }

        public int ChangedByUserId { get; set; }

        public DateTime ChangedAt { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        [ForeignKey("TaskItemId")]
        public virtual TaskItem TaskItem { get; set; }

        [ForeignKey("ChangedByUserId")]
        public virtual User ChangedByUser { get; set; }

        public TaskStatusHistory()
        {
            ChangedAt = DateTime.Now;
        }

        public string SubmittedFilePath { get; set; }
        public string SubmittedFileName { get; set; }
    }
}