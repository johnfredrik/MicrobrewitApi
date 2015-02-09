namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BrewerySocialBreweryExtention : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BrewerySocials",
                c => new
                    {
                        BreweryId = c.Int(nullable: false),
                        SocialId = c.Int(nullable: false, identity: true),
                        Site = c.String(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => new { t.BreweryId, t.SocialId })
                .ForeignKey("dbo.Breweries", t => t.BreweryId, cascadeDelete: true)
                .Index(t => t.BreweryId);
            
            AddColumn("dbo.Breweries", "Website", c => c.String());
            AddColumn("dbo.Breweries", "Established", c => c.String());
            AddColumn("dbo.Breweries", "HeaderImage", c => c.String());
            AddColumn("dbo.Breweries", "Avatar", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BrewerySocials", "BreweryId", "dbo.Breweries");
            DropIndex("dbo.BrewerySocials", new[] { "BreweryId" });
            DropColumn("dbo.Breweries", "Avatar");
            DropColumn("dbo.Breweries", "HeaderImage");
            DropColumn("dbo.Breweries", "Established");
            DropColumn("dbo.Breweries", "Website");
            DropTable("dbo.BrewerySocials");
        }
    }
}
