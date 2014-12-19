namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompositeKeyBoilStep : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BoilStepFermentables", "StepId", "dbo.BoilSteps");
            DropForeignKey("dbo.BoilStepHops", "StepId", "dbo.BoilSteps");
            DropForeignKey("dbo.BoilStepOthers", "StepId", "dbo.BoilSteps");
            DropForeignKey("dbo.MashStepHops", "StepId", "dbo.MashSteps");
            DropForeignKey("dbo.MashStepFermentables", "StepId", "dbo.MashSteps");
            DropForeignKey("dbo.MashStepOthers", "StepId", "dbo.MashSteps");
            DropForeignKey("dbo.BoilStepFermentables", new[] { "StepNumber", "RecipeId" }, "dbo.BoilSteps");
            DropForeignKey("dbo.BoilStepHops", new[] { "StepNumber", "RecipeId" }, "dbo.BoilSteps");
            DropForeignKey("dbo.BoilStepOthers", new[] { "StepNumber", "RecipeId" }, "dbo.BoilSteps");
            DropForeignKey("dbo.MashStepFermentables", new[] { "StepNumber", "RecipeId" }, "dbo.MashSteps");
            DropForeignKey("dbo.MashStepHops", new[] { "StepNumber", "RecipeId" }, "dbo.MashSteps");
            DropForeignKey("dbo.MashStepOthers", new[] { "StepNumber", "RecipeId" }, "dbo.MashSteps");
            DropIndex("dbo.BoilStepFermentables", new[] { "StepId" });
            DropIndex("dbo.BoilStepHops", new[] { "StepId" });
            DropIndex("dbo.MashStepHops", new[] { "StepId" });
            DropIndex("dbo.MashStepFermentables", new[] { "StepId" });
            DropIndex("dbo.MashStepOthers", new[] { "StepId" });
            DropIndex("dbo.BoilStepOthers", new[] { "StepId" });
            RenameColumn(table: "dbo.BoilStepFermentables", name: "StepId", newName: "StepNumber");
            RenameColumn(table: "dbo.BoilStepHops", name: "StepId", newName: "StepNumber");
            RenameColumn(table: "dbo.BoilStepOthers", name: "StepId", newName: "StepNumber");
            RenameColumn(table: "dbo.MashStepHops", name: "StepId", newName: "StepNumber");
            RenameColumn(table: "dbo.MashStepFermentables", name: "StepId", newName: "StepNumber");
            RenameColumn(table: "dbo.MashStepOthers", name: "StepId", newName: "StepNumber");
            DropPrimaryKey("dbo.BoilSteps");
            DropPrimaryKey("dbo.BoilStepFermentables");
            DropPrimaryKey("dbo.BoilStepHops");
            DropPrimaryKey("dbo.MashStepHops");
            DropPrimaryKey("dbo.MashSteps");
            DropPrimaryKey("dbo.MashStepFermentables");
            DropPrimaryKey("dbo.MashStepOthers");
            DropPrimaryKey("dbo.BoilStepOthers");
            AddColumn("dbo.BoilSteps", "StepNumber", c => c.Int(nullable: false));
            AddColumn("dbo.BoilStepFermentables", "RecipeId", c => c.Int(nullable: false));
            AddColumn("dbo.BoilStepHops", "RecipeId", c => c.Int(nullable: false));
            AddColumn("dbo.MashStepHops", "RecipeId", c => c.Int(nullable: false));
            AddColumn("dbo.MashSteps", "StepNumber", c => c.Int(nullable: false));
            AddColumn("dbo.MashStepFermentables", "RecipeId", c => c.Int(nullable: false));
            AddColumn("dbo.MashStepOthers", "RecipeId", c => c.Int(nullable: false));
            AddColumn("dbo.BoilStepOthers", "RecipeId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.BoilSteps", new[] { "StepNumber", "RecipeId" });
            AddPrimaryKey("dbo.BoilStepFermentables", new[] { "FermentableId", "StepNumber", "RecipeId" });
            AddPrimaryKey("dbo.BoilStepHops", new[] { "StepNumber", "HopId", "RecipeId" });
            AddPrimaryKey("dbo.MashStepHops", new[] { "HopId", "StepNumber", "RecipeId" });
            AddPrimaryKey("dbo.MashSteps", new[] { "StepNumber", "RecipeId" });
            AddPrimaryKey("dbo.MashStepFermentables", new[] { "FermentableId", "StepNumber", "RecipeId" });
            AddPrimaryKey("dbo.MashStepOthers", new[] { "StepNumber", "OtherId", "RecipeId" });
            AddPrimaryKey("dbo.BoilStepOthers", new[] { "StepNumber", "OtherId", "RecipeId" });
            CreateIndex("dbo.BoilStepFermentables", new[] { "StepNumber", "RecipeId" });
            CreateIndex("dbo.BoilStepHops", new[] { "StepNumber", "RecipeId" });
            CreateIndex("dbo.MashStepHops", new[] { "StepNumber", "RecipeId" });
            CreateIndex("dbo.MashStepFermentables", new[] { "StepNumber", "RecipeId" });
            CreateIndex("dbo.MashStepOthers", new[] { "StepNumber", "RecipeId" });
            CreateIndex("dbo.BoilStepOthers", new[] { "StepNumber", "RecipeId" });
            AddForeignKey("dbo.BoilStepFermentables", new[] { "StepNumber", "RecipeId" }, "dbo.BoilSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.BoilStepHops", new[] { "StepNumber", "RecipeId" }, "dbo.BoilSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.BoilStepOthers", new[] { "StepNumber", "RecipeId" }, "dbo.BoilSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.MashStepHops", new[] { "StepNumber", "RecipeId" }, "dbo.MashSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.MashStepFermentables", new[] { "StepNumber", "RecipeId" }, "dbo.MashSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.MashStepOthers", new[] { "StepNumber", "RecipeId" }, "dbo.MashSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            DropColumn("dbo.BoilSteps", "BoilStepId");
            DropColumn("dbo.BoilSteps", "Number");
            DropColumn("dbo.MashSteps", "MashStepId");
            DropColumn("dbo.MashSteps", "Number");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MashSteps", "Number", c => c.Int(nullable: false));
            AddColumn("dbo.MashSteps", "MashStepId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.BoilSteps", "Number", c => c.Int(nullable: false));
            AddColumn("dbo.BoilSteps", "BoilStepId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.MashStepOthers", new[] { "StepNumber", "RecipeId" }, "dbo.MashSteps");
            DropForeignKey("dbo.MashStepFermentables", new[] { "StepNumber", "RecipeId" }, "dbo.MashSteps");
            DropForeignKey("dbo.MashStepHops", new[] { "StepNumber", "RecipeId" }, "dbo.MashSteps");
            DropForeignKey("dbo.BoilStepOthers", new[] { "StepNumber", "RecipeId" }, "dbo.BoilSteps");
            DropForeignKey("dbo.BoilStepHops", new[] { "StepNumber", "RecipeId" }, "dbo.BoilSteps");
            DropForeignKey("dbo.BoilStepFermentables", new[] { "StepNumber", "RecipeId" }, "dbo.BoilSteps");
            DropIndex("dbo.BoilStepOthers", new[] { "StepNumber", "RecipeId" });
            DropIndex("dbo.MashStepOthers", new[] { "StepNumber", "RecipeId" });
            DropIndex("dbo.MashStepFermentables", new[] { "StepNumber", "RecipeId" });
            DropIndex("dbo.MashStepHops", new[] { "StepNumber", "RecipeId" });
            DropIndex("dbo.BoilStepHops", new[] { "StepNumber", "RecipeId" });
            DropIndex("dbo.BoilStepFermentables", new[] { "StepNumber", "RecipeId" });
            DropPrimaryKey("dbo.BoilStepOthers");
            DropPrimaryKey("dbo.MashStepOthers");
            DropPrimaryKey("dbo.MashStepFermentables");
            DropPrimaryKey("dbo.MashSteps");
            DropPrimaryKey("dbo.MashStepHops");
            DropPrimaryKey("dbo.BoilStepHops");
            DropPrimaryKey("dbo.BoilStepFermentables");
            DropPrimaryKey("dbo.BoilSteps");
            DropColumn("dbo.BoilStepOthers", "RecipeId");
            DropColumn("dbo.MashStepOthers", "RecipeId");
            DropColumn("dbo.MashStepFermentables", "RecipeId");
            DropColumn("dbo.MashSteps", "StepNumber");
            DropColumn("dbo.MashStepHops", "RecipeId");
            DropColumn("dbo.BoilStepHops", "RecipeId");
            DropColumn("dbo.BoilStepFermentables", "RecipeId");
            DropColumn("dbo.BoilSteps", "StepNumber");
            AddPrimaryKey("dbo.BoilStepOthers", new[] { "StepId", "OtherId" });
            AddPrimaryKey("dbo.MashStepOthers", new[] { "StepId", "OtherId" });
            AddPrimaryKey("dbo.MashStepFermentables", new[] { "FermentableId", "StepId" });
            AddPrimaryKey("dbo.MashSteps", "MashStepId");
            AddPrimaryKey("dbo.MashStepHops", new[] { "HopId", "StepId" });
            AddPrimaryKey("dbo.BoilStepHops", new[] { "StepId", "HopId" });
            AddPrimaryKey("dbo.BoilStepFermentables", new[] { "FermentableId", "StepId" });
            AddPrimaryKey("dbo.BoilSteps", "BoilStepId");
            RenameColumn(table: "dbo.MashStepOthers", name: "StepNumber", newName: "StepId");
            RenameColumn(table: "dbo.MashStepFermentables", name: "StepNumber", newName: "StepId");
            RenameColumn(table: "dbo.MashStepHops", name: "StepNumber", newName: "StepId");
            RenameColumn(table: "dbo.BoilStepOthers", name: "StepNumber", newName: "StepId");
            RenameColumn(table: "dbo.BoilStepHops", name: "StepNumber", newName: "StepId");
            RenameColumn(table: "dbo.BoilStepFermentables", name: "StepNumber", newName: "StepId");
            CreateIndex("dbo.BoilStepOthers", "StepId");
            CreateIndex("dbo.MashStepOthers", "StepId");
            CreateIndex("dbo.MashStepFermentables", "StepId");
            CreateIndex("dbo.MashStepHops", "StepId");
            CreateIndex("dbo.BoilStepHops", "StepId");
            CreateIndex("dbo.BoilStepFermentables", "StepId");
            AddForeignKey("dbo.MashStepOthers", new[] { "StepNumber", "RecipeId" }, "dbo.MashSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.MashStepHops", new[] { "StepNumber", "RecipeId" }, "dbo.MashSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.MashStepFermentables", new[] { "StepNumber", "RecipeId" }, "dbo.MashSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.BoilStepOthers", new[] { "StepNumber", "RecipeId" }, "dbo.BoilSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.BoilStepHops", new[] { "StepNumber", "RecipeId" }, "dbo.BoilSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.BoilStepFermentables", new[] { "StepNumber", "RecipeId" }, "dbo.BoilSteps", new[] { "StepNumber", "RecipeId" }, cascadeDelete: true);
            AddForeignKey("dbo.MashStepOthers", "StepId", "dbo.MashSteps", "MashStepId", cascadeDelete: true);
            AddForeignKey("dbo.MashStepFermentables", "StepId", "dbo.MashSteps", "MashStepId", cascadeDelete: true);
            AddForeignKey("dbo.MashStepHops", "StepId", "dbo.MashSteps", "MashStepId", cascadeDelete: true);
            AddForeignKey("dbo.BoilStepOthers", "StepId", "dbo.BoilSteps", "BoilStepId", cascadeDelete: true);
            AddForeignKey("dbo.BoilStepHops", "StepId", "dbo.BoilSteps", "BoilStepId", cascadeDelete: true);
            AddForeignKey("dbo.BoilStepFermentables", "StepId", "dbo.BoilSteps", "BoilStepId", cascadeDelete: true);
        }
    }
}
