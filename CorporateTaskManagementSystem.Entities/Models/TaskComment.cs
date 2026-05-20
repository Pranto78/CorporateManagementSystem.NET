using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateTaskManagementSystem.Entities.Models
{
    public class TaskComment
    {
        [Key]
        public int TaskCommentId { get; set; }

        public int TaskItemId { get; set; }

        public int CommentedByUserId { get; set; }

        [Required]
        [StringLength(1000)]
        public string CommentText { get; set; }

        public DateTime CommentedAt { get; set; }

        [ForeignKey("TaskItemId")]
        public virtual TaskItem TaskItem { get; set; }

        [ForeignKey("CommentedByUserId")]
        public virtual User CommentedByUser { get; set; }

        public TaskComment()
        {
            CommentedAt = DateTime.Now;
        }
    }
}