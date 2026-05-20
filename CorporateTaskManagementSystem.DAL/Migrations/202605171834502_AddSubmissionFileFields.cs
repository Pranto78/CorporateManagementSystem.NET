namespace CorporateTaskManagementSystem.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubmissionFileFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskStatusHistories", "SubmittedFilePath", c => c.String());
            AddColumn("dbo.TaskStatusHistories", "SubmittedFileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskStatusHistories", "SubmittedFileName");
            DropColumn("dbo.TaskStatusHistories", "SubmittedFilePath");
        }
    }
}
