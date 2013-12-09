namespace Pohui.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigrationsConfigurationAutomaticMigrationsEnabledtrue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "CreativeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.TagCreatives", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.TagCreatives", "Creative_Id", "dbo.Creatives");
            DropIndex("dbo.TagCreatives", new[] { "Tag_Id" });
            DropIndex("dbo.TagCreatives", new[] { "Creative_Id" });
            DropTable("dbo.TagCreatives");
            AddForeignKey("dbo.Tags", "CreativeId", "dbo.Creatives", "Id", cascadeDelete: true);
            CreateIndex("dbo.Tags", "CreativeId");

        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TagCreatives",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Creative_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Creative_Id });
            
            DropIndex("dbo.Tags", new[] { "CreativeId" });
            DropForeignKey("dbo.Tags", "CreativeId", "dbo.Creatives");
            DropColumn("dbo.Tags", "CreativeId");
            CreateIndex("dbo.TagCreatives", "Creative_Id");
            CreateIndex("dbo.TagCreatives", "Tag_Id");
            AddForeignKey("dbo.TagCreatives", "Creative_Id", "dbo.Creatives", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TagCreatives", "Tag_Id", "dbo.Tags", "Id", cascadeDelete: true);
        }
    }
}
