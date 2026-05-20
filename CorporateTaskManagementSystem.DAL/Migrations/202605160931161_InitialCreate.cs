namespace CorporateTaskManagementSystem.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 250),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        PasswordHash = c.String(nullable: false, maxLength: 255),
                        RoleId = c.Int(nullable: false),
                        DepartmentId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 200),
                        Message = c.String(nullable: false, maxLength: 1000),
                        IsRead = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TaskAssignments",
                c => new
                    {
                        TaskAssignmentId = c.Int(nullable: false, identity: true),
                        TaskItemId = c.Int(nullable: false),
                        AssignedToUserId = c.Int(nullable: false),
                        AssignedByUserId = c.Int(nullable: false),
                        AssignedAt = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TaskAssignmentId)
                .ForeignKey("dbo.Users", t => t.AssignedByUserId)
                .ForeignKey("dbo.Users", t => t.AssignedToUserId)
                .ForeignKey("dbo.TaskItems", t => t.TaskItemId)
                .Index(t => t.TaskItemId)
                .Index(t => t.AssignedToUserId)
                .Index(t => t.AssignedByUserId);
            
            CreateTable(
                "dbo.TaskItems",
                c => new
                    {
                        TaskItemId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Description = c.String(maxLength: 1000),
                        Priority = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        DueDate = c.DateTime(),
                        CreatedByUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TaskItemId)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .Index(t => t.CreatedByUserId);
            
            CreateTable(
                "dbo.TaskComments",
                c => new
                    {
                        TaskCommentId = c.Int(nullable: false, identity: true),
                        TaskItemId = c.Int(nullable: false),
                        CommentedByUserId = c.Int(nullable: false),
                        CommentText = c.String(nullable: false, maxLength: 1000),
                        CommentedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TaskCommentId)
                .ForeignKey("dbo.Users", t => t.CommentedByUserId)
                .ForeignKey("dbo.TaskItems", t => t.TaskItemId)
                .Index(t => t.TaskItemId)
                .Index(t => t.CommentedByUserId);
            
            CreateTable(
                "dbo.TaskStatusHistories",
                c => new
                    {
                        TaskStatusHistoryId = c.Int(nullable: false, identity: true),
                        TaskItemId = c.Int(nullable: false),
                        OldStatus = c.Int(nullable: false),
                        NewStatus = c.Int(nullable: false),
                        ChangedByUserId = c.Int(nullable: false),
                        ChangedAt = c.DateTime(nullable: false),
                        Remarks = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.TaskStatusHistoryId)
                .ForeignKey("dbo.Users", t => t.ChangedByUserId)
                .ForeignKey("dbo.TaskItems", t => t.TaskItemId)
                .Index(t => t.TaskItemId)
                .Index(t => t.ChangedByUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskStatusHistories", "TaskItemId", "dbo.TaskItems");
            DropForeignKey("dbo.TaskStatusHistories", "ChangedByUserId", "dbo.Users");
            DropForeignKey("dbo.TaskComments", "TaskItemId", "dbo.TaskItems");
            DropForeignKey("dbo.TaskComments", "CommentedByUserId", "dbo.Users");
            DropForeignKey("dbo.TaskAssignments", "TaskItemId", "dbo.TaskItems");
            DropForeignKey("dbo.TaskItems", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.TaskAssignments", "AssignedToUserId", "dbo.Users");
            DropForeignKey("dbo.TaskAssignments", "AssignedByUserId", "dbo.Users");
            DropForeignKey("dbo.Notifications", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Users", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.TaskStatusHistories", new[] { "ChangedByUserId" });
            DropIndex("dbo.TaskStatusHistories", new[] { "TaskItemId" });
            DropIndex("dbo.TaskComments", new[] { "CommentedByUserId" });
            DropIndex("dbo.TaskComments", new[] { "TaskItemId" });
            DropIndex("dbo.TaskItems", new[] { "CreatedByUserId" });
            DropIndex("dbo.TaskAssignments", new[] { "AssignedByUserId" });
            DropIndex("dbo.TaskAssignments", new[] { "AssignedToUserId" });
            DropIndex("dbo.TaskAssignments", new[] { "TaskItemId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "DepartmentId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropTable("dbo.TaskStatusHistories");
            DropTable("dbo.TaskComments");
            DropTable("dbo.TaskItems");
            DropTable("dbo.TaskAssignments");
            DropTable("dbo.Notifications");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Departments");
        }
    }
}
