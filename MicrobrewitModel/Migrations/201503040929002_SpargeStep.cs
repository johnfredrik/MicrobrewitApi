namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpargeStep : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BoilSteps", "RecipeId", "dbo.Recipes");
            DropForeignKey("dbo.MashSteps", "RecipeId", "dbo.Recipes");
            CreateTable(
                "dbo.SpargeSteps",
                c => new
                    {
                        RecipeId = c.Int(nullable: false),
                        StepNumber = c.Int(nullable: false),
                        Temperature = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        Notes = c.String(),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.RecipeId)
                .ForeignKey("dbo.Recipes", t => t.RecipeId, cascadeDelete: true)
                .Index(t => t.RecipeId);
            
            AddColumn("dbo.BoilSteps", "Recipe_RecipeId", c => c.Int());
            AddColumn("dbo.MashSteps", "Recipe_RecipeId", c => c.Int());
            CreateIndex("dbo.BoilSteps", "Recipe_RecipeId");
            CreateIndex("dbo.MashSteps", "Recipe_RecipeId");
            AddForeignKey("dbo.BoilSteps", "Recipe_RecipeId", "dbo.Recipes", "RecipeId");
            AddForeignKey("dbo.MashSteps", "Recipe_RecipeId", "dbo.Recipes", "RecipeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MashSteps", "Recipe_RecipeId", "dbo.Recipes");
            DropForeignKey("dbo.BoilSteps", "Recipe_RecipeId", "dbo.Recipes");
            DropForeignKey("dbo.SpargeSteps", "RecipeId", "dbo.Recipes");
            DropIndex("dbo.SpargeSteps", new[] { "RecipeId" });
            DropIndex("dbo.MashSteps", new[] { "Recipe_RecipeId" });
            DropIndex("dbo.BoilSteps", new[] { "Recipe_RecipeId" });
            DropColumn("dbo.MashSteps", "Recipe_RecipeId");
            DropColumn("dbo.BoilSteps", "Recipe_RecipeId");
            DropTable("dbo.SpargeSteps");
            AddForeignKey("dbo.MashSteps", "RecipeId", "dbo.Recipes", "RecipeId", cascadeDelete: true);
            AddForeignKey("dbo.BoilSteps", "RecipeId", "dbo.Recipes", "RecipeId", cascadeDelete: true);
        }
    }
}
