namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BreweryNameUniqueConstraint : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Breweries", "Name", unique: true, name: "IX_BreweryName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Breweries", "IX_BreweryName");
        }
    }
}
