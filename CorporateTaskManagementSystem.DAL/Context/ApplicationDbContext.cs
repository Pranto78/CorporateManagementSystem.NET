using System.Data.Entity;
using CorporateTaskManagementSystem.Entities.Models;

namespace CorporateTaskManagementSystem.DAL.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("CorporateTaskManagementSystemDb")
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<TaskStatusHistory> TaskStatusHistories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskAssignment>()
                .HasRequired(x => x.TaskItem)
                .WithMany()
                .HasForeignKey(x => x.TaskItemId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaskAssignment>()
                .HasRequired(x => x.AssignedToUser)
                .WithMany()
                .HasForeignKey(x => x.AssignedToUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaskAssignment>()
                .HasRequired(x => x.AssignedByUser)
                .WithMany()
                .HasForeignKey(x => x.AssignedByUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaskItem>()
                .HasRequired(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaskComment>()
                .HasRequired(x => x.TaskItem)
                .WithMany()
                .HasForeignKey(x => x.TaskItemId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaskComment>()
                .HasRequired(x => x.CommentedByUser)
                .WithMany()
                .HasForeignKey(x => x.CommentedByUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaskStatusHistory>()
                .HasRequired(x => x.TaskItem)
                .WithMany()
                .HasForeignKey(x => x.TaskItemId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaskStatusHistory>()
                .HasRequired(x => x.ChangedByUser)
                .WithMany()
                .HasForeignKey(x => x.ChangedByUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Notification>()
                .HasRequired(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>()
    .HasOptional(t => t.DeletedByUser)
    .WithMany()
    .HasForeignKey(t => t.DeletedByUserId)
    .WillCascadeOnDelete(false);
        }
    }
}