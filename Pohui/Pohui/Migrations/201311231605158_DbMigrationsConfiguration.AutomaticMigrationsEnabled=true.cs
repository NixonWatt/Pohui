namespace Pohui.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigrationsConfigurationAutomaticMigrationsEnabledtrue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chapters", "Content", c => c.String());
            DropColumn("dbo.Chapters", "Path");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Chapters", "Path", c => c.String());
            DropColumn("dbo.Chapters", "Content");
        }
    }
}
