namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpargeStepHopFirstWort : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SpargeStepHops",
                c => new
                    {
                        HopId = c.Int(nullable: false),
                        StepNumber = c.Int(nullable: false),
                        RecipeId = c.Int(nullable: false),
                        AaValue = c.Int(nullable: false),
                        AaAmount = c.Int(nullable: false),
                        HopFormId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HopId, t.StepNumber, t.RecipeId })
                .ForeignKey("dbo.Hops", t => t.HopId, cascadeDelete: true)
                .ForeignKey("dbo.HopForms", t => t.HopFormId, cascadeDelete: true)
                .ForeignKey("dbo.SpargeSteps", t => t.RecipeId, cascadeDelete: true)
                .Index(t => t.HopId)
                .Index(t => t.RecipeId)
                .Index(t => t.HopFormId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SpargeStepHops", "RecipeId", "dbo.SpargeSteps");
            DropForeignKey("dbo.SpargeStepHops", "HopFormId", "dbo.HopForms");
            DropForeignKey("dbo.SpargeStepHops", "HopId", "dbo.Hops");
            DropIndex("dbo.SpargeStepHops", new[] { "HopFormId" });
            DropIndex("dbo.SpargeStepHops", new[] { "RecipeId" });
            DropIndex("dbo.SpargeStepHops", new[] { "HopId" });
            DropTable("dbo.SpargeStepHops");
        }
    }
}
