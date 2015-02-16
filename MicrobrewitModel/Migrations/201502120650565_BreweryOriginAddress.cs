namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BreweryOriginAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Breweries", "OriginId", c => c.Int(nullable: false));
            AddColumn("dbo.Breweries", "Address", c => c.String());
            CreateIndex("dbo.Breweries", "OriginId");
            AddForeignKey("dbo.Breweries", "OriginId", "dbo.Origins", "OriginId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Breweries", "OriginId", "dbo.Origins");
            DropIndex("dbo.Breweries", new[] { "OriginId" });
            DropColumn("dbo.Breweries", "Address");
            DropColumn("dbo.Breweries", "OriginId");
        }
    }
}
