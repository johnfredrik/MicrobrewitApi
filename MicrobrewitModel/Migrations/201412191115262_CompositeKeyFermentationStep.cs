namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompositeKeyFermentationStep : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FermentationStepFermentables", "StepId", "dbo.FermentationSteps");
            DropForeignKey("dbo.FermentationStepHops", "StepId", "dbo.FermentationSteps");
            DropForeignKey("dbo.FermentationStepOthers", "StepId", "dbo.FermentationSteps");
            DropForeignKey("dbo.FermentationStepYeasts", "StepId", "dbo.FermentationSteps");
            DropForeignKey("dbo.FermentationStepFermentables", new[] { "StepNumber", "RecipeId" }, "dbo.FermentationSteps");
            DropForeignKey("dbo.FermentationStepHops", new[] { "StepNumber", "RecipeId" }, "dbo.FermentationSteps");
            DropForeignKey("dbo.FermentationStepOthers", new[] { "StepNumber", "RecipeId" }, "dbo.FermentationSteps");
            DropForeignKey("dbo.FermentationStepYeasts", new[] { "StepNumber", "RecipeId" }, "dbo.FermentationSteps");
            DropIndex("dbo.FermentationStepFermentables", new[] { "StepId" });
            DropIndex("dbo.FermentationStepHops", new[] { "StepId" });
            DropIndex("dbo.FermentationStepOthers", new[] { "StepId" });
            DropIndex("dbo.FermentationStepYeasts", new[] { "StepId" });
            RenameColumn(table: "dbo.FermentationStepFermentables", name: "StepId", newName: "StepNumber");
            RenameColumn(table: "dbo.FermentationStepHops", name: "StepId", newName: "StepNumber");
            RenameColumn(table: "dbo.FermentationStepOthers", name: "StepId", newName: "StepNumber");
            RenameColumn(table: "dbo.FermentationStepYeasts", name: "StepId", newName: "StepNumber");
            DropPrimaryKey("dbo.FermentationStepFermentables");
            DropPrimaryKey("dbo.FermentationSteps");
            DropPrimaryKey("dbo.FermentationStepHops");
            DropPrimaryKey("dbo.FermentationStepOthers");
            DropPrimaryKey("dbo.FermentationStepYeasts");
            AddColumn("dbo.FermentationStepFermentables", "RecipeId", c => c.Int(nullable: false));
            AddColumn("dbo.FermentationSteps", "StepNumber", c => c.Int(nullable: false));
            AddColumn("dbo.FermentationStepHops", "RecipeId", c => c.Int(nullable: false));
            AddColumn("dbo.FermentationStepOthers", "RecipeId", c => c.Int(nullable: false));
            AddColumn("dbo.FermentationStepYeasts", "RecipeId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.FermentationStepFermentables", new[] { "FermentableId", "StepNumber", "RecipeId" });
            AddPrimaryKey("dbo.FermentationSteps", new[] { "StepNumber", "RecipeId" });
            AddPrimaryKey("dbo.FermentationStepHops", new[] { "StepNumber", "HopId", "RecipeId" });
            AddPrimaryKey("dbo.FermentationStepOthers", new[] { "StepNumber", "OtherId", "RecipeId" });
            AddPrimaryKey("dbo.FermentationStepYeasts", new[] { "StepNumber", "YeastId", "RecipeId" });
            CreateIndex("dbo.FermentationStepFermentables", new[] { "StepNumber", "RecipeId" });
            CreateIndex("dbo.FermentationStepHops", new[] { "StepNumber", "RecipeId" });
            CreateIndex("dbo.FermentationStepOthers", new[] { "StepNumber", "RecipeId" });
            CreateIndex("dbo.FermentationStepYeasts", new[] { "StepNumber", "RecipeId" });
            AddForeignKey("dbo.FermentationStepFermentables", new[] { "StepNumber", "RecipeId" }, "dbo.FermentationSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.FermentationStepHops", new[] { "StepNumber", "RecipeId" }, "dbo.FermentationSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.FermentationStepOthers", new[] { "StepNumber", "RecipeId" }, "dbo.FermentationSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.FermentationStepYeasts", new[] { "StepNumber", "RecipeId" }, "dbo.FermentationSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            DropColumn("dbo.FermentationSteps", "FermentationStepId");
            DropColumn("dbo.FermentationSteps", "Number");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FermentationSteps", "Number", c => c.Int(nullable: false));
            AddColumn("dbo.FermentationSteps", "FermentationStepId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.FermentationStepYeasts", new[] { "StepNumber", "RecipeId" }, "dbo.FermentationSteps");
            DropForeignKey("dbo.FermentationStepOthers", new[] { "StepNumber", "RecipeId" }, "dbo.FermentationSteps");
            DropForeignKey("dbo.FermentationStepHops", new[] { "StepNumber", "RecipeId" }, "dbo.FermentationSteps");
            DropForeignKey("dbo.FermentationStepFermentables", new[] { "StepNumber", "RecipeId" }, "dbo.FermentationSteps");
            DropIndex("dbo.FermentationStepYeasts", new[] { "StepNumber", "RecipeId" });
            DropIndex("dbo.FermentationStepOthers", new[] { "StepNumber", "RecipeId" });
            DropIndex("dbo.FermentationStepHops", new[] { "StepNumber", "RecipeId" });
            DropIndex("dbo.FermentationStepFermentables", new[] { "StepNumber", "RecipeId" });
            DropPrimaryKey("dbo.FermentationStepYeasts");
            DropPrimaryKey("dbo.FermentationStepOthers");
            DropPrimaryKey("dbo.FermentationStepHops");
            DropPrimaryKey("dbo.FermentationSteps");
            DropPrimaryKey("dbo.FermentationStepFermentables");
            DropColumn("dbo.FermentationStepYeasts", "RecipeId");
            DropColumn("dbo.FermentationStepOthers", "RecipeId");
            DropColumn("dbo.FermentationStepHops", "RecipeId");
            DropColumn("dbo.FermentationSteps", "StepNumber");
            DropColumn("dbo.FermentationStepFermentables", "RecipeId");
            AddPrimaryKey("dbo.FermentationStepYeasts", new[] { "StepId", "YeastId" });
            AddPrimaryKey("dbo.FermentationStepOthers", new[] { "StepId", "OtherId" });
            AddPrimaryKey("dbo.FermentationStepHops", new[] { "StepId", "HopFormId" });
            AddPrimaryKey("dbo.FermentationSteps", "FermentationStepId");
            AddPrimaryKey("dbo.FermentationStepFermentables", new[] { "FermentableId", "StepId" });
            RenameColumn(table: "dbo.FermentationStepYeasts", name: "StepNumber", newName: "StepId");
            RenameColumn(table: "dbo.FermentationStepOthers", name: "StepNumber", newName: "StepId");
            RenameColumn(table: "dbo.FermentationStepHops", name: "StepNumber", newName: "StepId");
            RenameColumn(table: "dbo.FermentationStepFermentables", name: "StepNumber", newName: "StepId");
            CreateIndex("dbo.FermentationStepYeasts", "StepId");
            CreateIndex("dbo.FermentationStepOthers", "StepId");
            CreateIndex("dbo.FermentationStepHops", "StepId");
            CreateIndex("dbo.FermentationStepFermentables", "StepId");
            AddForeignKey("dbo.FermentationStepYeasts", new[] { "StepNumber", "RecipeId" }, "dbo.FermentationSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.FermentationStepOthers", new[] { "StepNumber", "RecipeId" }, "dbo.FermentationSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.FermentationStepHops", new[] { "StepNumber", "RecipeId" }, "dbo.FermentationSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.FermentationStepFermentables", new[] { "StepNumber", "RecipeId" }, "dbo.FermentationSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.FermentationStepYeasts", "StepId", "dbo.FermentationSteps", "FermentationStepId", cascadeDelete: true);
            AddForeignKey("dbo.FermentationStepOthers", "StepId", "dbo.FermentationSteps", "FermentationStepId", cascadeDelete: true);
            AddForeignKey("dbo.FermentationStepHops", "StepId", "dbo.FermentationSteps", "FermentationStepId", cascadeDelete: true);
            AddForeignKey("dbo.FermentationStepFermentables", "StepId", "dbo.FermentationSteps", "FermentationStepId", cascadeDelete: true);
        }
    }
}
