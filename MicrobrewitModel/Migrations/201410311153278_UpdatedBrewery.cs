namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedBrewery : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Breweries", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Breweries", "UpdatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Breweries", "UpdatedDate");
            DropColumn("dbo.Breweries", "CreatedDate");
        }
    }
}
