namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveForedOfFromRecipeToBeer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Recipes", "ForkeOfId", "dbo.Recipes");
            DropForeignKey("dbo.ForkedRecipe", "RecipeId", "dbo.Recipes");
            DropForeignKey("dbo.ForkedRecipe", "ForkedRecipeId", "dbo.Recipes");
            DropIndex("dbo.Recipes", new[] { "ForkeOfId" });
            DropIndex("dbo.ForkedRecipe", new[] { "RecipeId" });
            DropIndex("dbo.ForkedRecipe", new[] { "ForkedRecipeId" });
            CreateTable(
                "dbo.ForkedBeer",
                c => new
                    {
                        BeerId = c.Int(nullable: false),
                        ForkedBeerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BeerId, t.ForkedBeerId })
                .ForeignKey("dbo.Beers", t => t.BeerId)
                .ForeignKey("dbo.Beers", t => t.ForkedBeerId)
                .Index(t => t.BeerId)
                .Index(t => t.ForkedBeerId);
            
            AddColumn("dbo.Beers", "ForkeOfId", c => c.Int());
            CreateIndex("dbo.Beers", "ForkeOfId");
            AddForeignKey("dbo.Beers", "ForkeOfId", "dbo.Beers", "BeerId");
            DropTable("dbo.ForkedRecipe");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ForkedRecipe",
                c => new
                    {
                        RecipeId = c.Int(nullable: false),
                        ForkedRecipeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RecipeId, t.ForkedRecipeId });
            
            DropForeignKey("dbo.ForkedBeer", "ForkedBeerId", "dbo.Beers");
            DropForeignKey("dbo.ForkedBeer", "BeerId", "dbo.Beers");
            DropForeignKey("dbo.Beers", "ForkeOfId", "dbo.Beers");
            DropIndex("dbo.ForkedBeer", new[] { "ForkedBeerId" });
            DropIndex("dbo.ForkedBeer", new[] { "BeerId" });
            DropIndex("dbo.Beers", new[] { "ForkeOfId" });
            DropColumn("dbo.Beers", "ForkeOfId");
            DropTable("dbo.ForkedBeer");
            CreateIndex("dbo.ForkedRecipe", "ForkedRecipeId");
            CreateIndex("dbo.ForkedRecipe", "RecipeId");
            CreateIndex("dbo.Recipes", "ForkeOfId");
            AddForeignKey("dbo.ForkedRecipe", "ForkedRecipeId", "dbo.Recipes", "RecipeId");
            AddForeignKey("dbo.ForkedRecipe", "RecipeId", "dbo.Recipes", "RecipeId");
            AddForeignKey("dbo.Recipes", "ForkeOfId", "dbo.Recipes", "RecipeId");
        }
    }
}
