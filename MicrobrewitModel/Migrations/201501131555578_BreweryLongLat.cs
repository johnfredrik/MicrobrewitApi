namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BreweryLongLat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Breweries", "Longitude", c => c.Double(nullable: false));
            AddColumn("dbo.Breweries", "Latitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Breweries", "Latitude");
            DropColumn("dbo.Breweries", "Longitude");
        }
    }
}
