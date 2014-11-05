namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedBeerStyles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BeerStyles", "OGLow", c => c.Double(nullable: false));
            AddColumn("dbo.BeerStyles", "OGHigh", c => c.Double(nullable: false));
            AddColumn("dbo.BeerStyles", "FGLow", c => c.Double(nullable: false));
            AddColumn("dbo.BeerStyles", "FGHigh", c => c.Double(nullable: false));
            AddColumn("dbo.BeerStyles", "IBULow", c => c.Double(nullable: false));
            AddColumn("dbo.BeerStyles", "IBUHigh", c => c.Double(nullable: false));
            AddColumn("dbo.BeerStyles", "SRMLow", c => c.Double(nullable: false));
            AddColumn("dbo.BeerStyles", "SRMHigh", c => c.Double(nullable: false));
            AddColumn("dbo.BeerStyles", "ABVLow", c => c.Double(nullable: false));
            AddColumn("dbo.BeerStyles", "ABVHigh", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BeerStyles", "ABVHigh");
            DropColumn("dbo.BeerStyles", "ABVLow");
            DropColumn("dbo.BeerStyles", "SRMHigh");
            DropColumn("dbo.BeerStyles", "SRMLow");
            DropColumn("dbo.BeerStyles", "IBUHigh");
            DropColumn("dbo.BeerStyles", "IBULow");
            DropColumn("dbo.BeerStyles", "FGHigh");
            DropColumn("dbo.BeerStyles", "FGLow");
            DropColumn("dbo.BeerStyles", "OGHigh");
            DropColumn("dbo.BeerStyles", "OGLow");
        }
    }
}
