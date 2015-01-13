namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedTableForkedBeer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ForkedBeer", "BeerId", "dbo.Beers");
            DropForeignKey("dbo.ForkedBeer", "ForkedBeerId", "dbo.Beers");
            DropIndex("dbo.ForkedBeer", new[] { "BeerId" });
            DropIndex("dbo.ForkedBeer", new[] { "ForkedBeerId" });
            DropTable("dbo.ForkedBeer");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ForkedBeer",
                c => new
                    {
                        BeerId = c.Int(nullable: false),
                        ForkedBeerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BeerId, t.ForkedBeerId });
            
            CreateIndex("dbo.ForkedBeer", "ForkedBeerId");
            CreateIndex("dbo.ForkedBeer", "BeerId");
            AddForeignKey("dbo.ForkedBeer", "ForkedBeerId", "dbo.Beers", "BeerId");
            AddForeignKey("dbo.ForkedBeer", "BeerId", "dbo.Beers", "BeerId");
        }
    }
}
