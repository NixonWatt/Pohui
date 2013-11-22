namespace Pohui.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigrationsConfigurationAutomaticMigrationsEnabledtrue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Creatives", "User", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Creatives", "User");
        }
    }
}
