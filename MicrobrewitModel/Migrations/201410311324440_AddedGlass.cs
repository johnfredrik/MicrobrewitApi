namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGlass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BeerStyleGlasses",
                c => new
                    {
                        BeerStyleId = c.Int(nullable: false),
                        GlassId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BeerStyleId, t.GlassId })
                .ForeignKey("dbo.Glasses", t => t.GlassId, cascadeDelete: true)
                .ForeignKey("dbo.BeerStyles", t => t.BeerStyleId, cascadeDelete: true)
                .Index(t => t.BeerStyleId)
                .Index(t => t.GlassId);
            
            CreateTable(
                "dbo.Glasses",
                c => new
                    {
                        GlassId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.GlassId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BeerStyleGlasses", "BeerStyleId", "dbo.BeerStyles");
            DropForeignKey("dbo.BeerStyleGlasses", "GlassId", "dbo.Glasses");
            DropIndex("dbo.BeerStyleGlasses", new[] { "GlassId" });
            DropIndex("dbo.BeerStyleGlasses", new[] { "BeerStyleId" });
            DropTable("dbo.Glasses");
            DropTable("dbo.BeerStyleGlasses");
        }
    }
}
