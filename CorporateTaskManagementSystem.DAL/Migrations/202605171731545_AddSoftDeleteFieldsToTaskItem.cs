namespace CorporateTaskManagementSystem.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSoftDeleteFieldsToTaskItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskItems", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaskItems", "DeletedAt", c => c.DateTime());
            AddColumn("dbo.TaskItems", "DeletedByUserId", c => c.Int());
            CreateIndex("dbo.TaskItems", "DeletedByUserId");
            AddForeignKey("dbo.TaskItems", "DeletedByUserId", "dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskItems", "DeletedByUserId", "dbo.Users");
            DropIndex("dbo.TaskItems", new[] { "DeletedByUserId" });
            DropColumn("dbo.TaskItems", "DeletedByUserId");
            DropColumn("dbo.TaskItems", "DeletedAt");
            DropColumn("dbo.TaskItems", "IsDeleted");
        }
    }
}
