namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ABVs",
                c => new
                    {
                        AbvId = c.Int(nullable: false),
                        Standard = c.Double(nullable: false),
                        Miller = c.Double(nullable: false),
                        Advanced = c.Double(nullable: false),
                        AdvancedAlternative = c.Double(nullable: false),
                        Simple = c.Double(nullable: false),
                        AlternativeSimple = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.AbvId)
                .ForeignKey("dbo.Beers", t => t.AbvId, cascadeDelete: true)
                .Index(t => t.AbvId);
            
            CreateTable(
                "dbo.Beers",
                c => new
                    {
                        BeerId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        BeerStyleId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BeerId)
                .ForeignKey("dbo.BeerStyles", t => t.BeerStyleId)
                .ForeignKey("dbo.SRMs", t => t.BeerId, cascadeDelete: true)
                .Index(t => t.BeerId)
                .Index(t => t.BeerStyleId);
            
            CreateTable(
                "dbo.BeerStyles",
                c => new
                    {
                        BeerStyleId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        SuperStyleId = c.Int(),
                    })
                .PrimaryKey(t => t.BeerStyleId)
                .ForeignKey("dbo.BeerStyles", t => t.SuperStyleId)
                .Index(t => t.SuperStyleId);
            
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
            
            CreateTable(
                "dbo.BreweryBeers",
                c => new
                    {
                        BeerId = c.Int(nullable: false),
                        BreweryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BeerId, t.BreweryId })
                .ForeignKey("dbo.Breweries", t => t.BreweryId, cascadeDelete: true)
                .ForeignKey("dbo.Beers", t => t.BeerId, cascadeDelete: true)
                .Index(t => t.BeerId)
                .Index(t => t.BreweryId);
            
            CreateTable(
                "dbo.Breweries",
                c => new
                    {
                        BreweryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(),
                        Type = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BreweryId);
            
            CreateTable(
                "dbo.BreweryMembers",
                c => new
                    {
                        BreweryId = c.Int(nullable: false),
                        MemberUsername = c.String(nullable: false, maxLength: 128),
                        Role = c.String(),
                    })
                .PrimaryKey(t => new { t.BreweryId, t.MemberUsername })
                .ForeignKey("dbo.Users", t => t.MemberUsername, cascadeDelete: true)
                .ForeignKey("dbo.Breweries", t => t.BreweryId, cascadeDelete: true)
                .Index(t => t.BreweryId)
                .Index(t => t.MemberUsername);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        Email = c.String(nullable: false),
                        Settings = c.String(),
                    })
                .PrimaryKey(t => t.Username);
            
            CreateTable(
                "dbo.UserBeers",
                c => new
                    {
                        BeerId = c.Int(nullable: false),
                        Username = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.BeerId, t.Username })
                .ForeignKey("dbo.Users", t => t.Username, cascadeDelete: true)
                .ForeignKey("dbo.Beers", t => t.BeerId, cascadeDelete: true)
                .Index(t => t.BeerId)
                .Index(t => t.Username);
            
            CreateTable(
                "dbo.IBUs",
                c => new
                    {
                        IbuId = c.Int(nullable: false),
                        Standard = c.Double(nullable: false),
                        Tinseth = c.Double(nullable: false),
                        Rager = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.IbuId)
                .ForeignKey("dbo.Beers", t => t.IbuId, cascadeDelete: true)
                .Index(t => t.IbuId);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        RecipeId = c.Int(nullable: false),
                        Volume = c.Int(nullable: false),
                        Notes = c.String(),
                        ForkeOfId = c.Int(),
                        OG = c.Double(nullable: false),
                        FG = c.Double(nullable: false),
                        Efficiency = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.RecipeId)
                .ForeignKey("dbo.Recipes", t => t.ForkeOfId)
                .ForeignKey("dbo.Beers", t => t.RecipeId, cascadeDelete: true)
                .Index(t => t.RecipeId)
                .Index(t => t.ForkeOfId);
            
            CreateTable(
                "dbo.BoilSteps",
                c => new
                    {
                        BoilStepId = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Length = c.Int(nullable: false),
                        Volume = c.Int(nullable: false),
                        Notes = c.String(),
                        RecipeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BoilStepId)
                .ForeignKey("dbo.Recipes", t => t.RecipeId, cascadeDelete: true)
                .Index(t => t.RecipeId);
            
            CreateTable(
                "dbo.BoilStepFermentables",
                c => new
                    {
                        FermentableId = c.Int(nullable: false),
                        StepId = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        Lovibond = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.FermentableId, t.StepId })
                .ForeignKey("dbo.Fermentables", t => t.FermentableId, cascadeDelete: true)
                .ForeignKey("dbo.BoilSteps", t => t.StepId, cascadeDelete: true)
                .Index(t => t.FermentableId)
                .Index(t => t.StepId);
            
            CreateTable(
                "dbo.Fermentables",
                c => new
                    {
                        FermentableId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        EBC = c.Double(nullable: false),
                        Lovibond = c.Double(nullable: false),
                        PPG = c.Int(),
                        SupplierId = c.Int(),
                        Type = c.String(nullable: false, maxLength: 60),
                        Custom = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FermentableId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "dbo.FermentationStepFermentables",
                c => new
                    {
                        FermentableId = c.Int(nullable: false),
                        StepId = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        Lovibond = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.FermentableId, t.StepId })
                .ForeignKey("dbo.FermentationSteps", t => t.StepId, cascadeDelete: true)
                .ForeignKey("dbo.Fermentables", t => t.FermentableId, cascadeDelete: true)
                .Index(t => t.FermentableId)
                .Index(t => t.StepId);
            
            CreateTable(
                "dbo.FermentationSteps",
                c => new
                    {
                        FermentationStepId = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Length = c.Int(nullable: false),
                        Temperature = c.Int(nullable: false),
                        Notes = c.String(),
                        RecipeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FermentationStepId)
                .ForeignKey("dbo.Recipes", t => t.RecipeId, cascadeDelete: true)
                .Index(t => t.RecipeId);
            
            CreateTable(
                "dbo.FermentationStepHops",
                c => new
                    {
                        StepId = c.Int(nullable: false),
                        HopFormId = c.Int(nullable: false),
                        HopId = c.Int(nullable: false),
                        AAValue = c.Int(nullable: false),
                        AAAmount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StepId, t.HopFormId })
                .ForeignKey("dbo.Hops", t => t.HopId, cascadeDelete: true)
                .ForeignKey("dbo.HopForms", t => t.HopFormId, cascadeDelete: true)
                .ForeignKey("dbo.FermentationSteps", t => t.StepId, cascadeDelete: true)
                .Index(t => t.StepId)
                .Index(t => t.HopFormId)
                .Index(t => t.HopId);
            
            CreateTable(
                "dbo.Hops",
                c => new
                    {
                        HopId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        AALow = c.Double(nullable: false),
                        AAHigh = c.Double(nullable: false),
                        BetaLow = c.Double(nullable: false),
                        BetaHigh = c.Double(nullable: false),
                        Notes = c.String(),
                        FlavourDescription = c.String(),
                        Custom = c.Boolean(nullable: false),
                        OriginId = c.Int(),
                    })
                .PrimaryKey(t => t.HopId)
                .ForeignKey("dbo.Origins", t => t.OriginId)
                .Index(t => t.OriginId);
            
            CreateTable(
                "dbo.BoilStepHops",
                c => new
                    {
                        StepId = c.Int(nullable: false),
                        HopId = c.Int(nullable: false),
                        AAValue = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        HopFormId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StepId, t.HopId })
                .ForeignKey("dbo.HopForms", t => t.HopFormId, cascadeDelete: true)
                .ForeignKey("dbo.Hops", t => t.HopId, cascadeDelete: true)
                .ForeignKey("dbo.BoilSteps", t => t.StepId, cascadeDelete: true)
                .Index(t => t.StepId)
                .Index(t => t.HopId)
                .Index(t => t.HopFormId);
            
            CreateTable(
                "dbo.HopForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 60),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HopFlavours",
                c => new
                    {
                        FlavourId = c.Int(nullable: false),
                        HopId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FlavourId, t.HopId })
                .ForeignKey("dbo.Flavours", t => t.FlavourId, cascadeDelete: true)
                .ForeignKey("dbo.Hops", t => t.HopId, cascadeDelete: true)
                .Index(t => t.FlavourId)
                .Index(t => t.HopId);
            
            CreateTable(
                "dbo.Flavours",
                c => new
                    {
                        FlavourId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.FlavourId);
            
            CreateTable(
                "dbo.MashStepHops",
                c => new
                    {
                        HopId = c.Int(nullable: false),
                        StepId = c.Int(nullable: false),
                        AAValue = c.Int(nullable: false),
                        AAAmount = c.Int(nullable: false),
                        HopFormId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HopId, t.StepId })
                .ForeignKey("dbo.HopForms", t => t.HopFormId, cascadeDelete: true)
                .ForeignKey("dbo.MashSteps", t => t.StepId, cascadeDelete: true)
                .ForeignKey("dbo.Hops", t => t.HopId, cascadeDelete: true)
                .Index(t => t.HopId)
                .Index(t => t.StepId)
                .Index(t => t.HopFormId);
            
            CreateTable(
                "dbo.MashSteps",
                c => new
                    {
                        MashStepId = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Temperature = c.Int(nullable: false),
                        Type = c.String(),
                        Length = c.Int(nullable: false),
                        Volume = c.Int(nullable: false),
                        Notes = c.String(),
                        RecipeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MashStepId)
                .ForeignKey("dbo.Recipes", t => t.RecipeId, cascadeDelete: true)
                .Index(t => t.RecipeId);
            
            CreateTable(
                "dbo.MashStepFermentables",
                c => new
                    {
                        FermentableId = c.Int(nullable: false),
                        StepId = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        Lovibond = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.FermentableId, t.StepId })
                .ForeignKey("dbo.Fermentables", t => t.FermentableId, cascadeDelete: true)
                .ForeignKey("dbo.MashSteps", t => t.StepId, cascadeDelete: true)
                .Index(t => t.FermentableId)
                .Index(t => t.StepId);
            
            CreateTable(
                "dbo.MashStepOthers",
                c => new
                    {
                        StepId = c.Int(nullable: false),
                        OtherId = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StepId, t.OtherId })
                .ForeignKey("dbo.Other", t => t.OtherId, cascadeDelete: true)
                .ForeignKey("dbo.MashSteps", t => t.StepId, cascadeDelete: true)
                .Index(t => t.StepId)
                .Index(t => t.OtherId);
            
            CreateTable(
                "dbo.Other",
                c => new
                    {
                        OtherId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Type = c.String(),
                        Custom = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OtherId);
            
            CreateTable(
                "dbo.BoilStepOthers",
                c => new
                    {
                        StepId = c.Int(nullable: false),
                        OtherId = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StepId, t.OtherId })
                .ForeignKey("dbo.Other", t => t.OtherId, cascadeDelete: true)
                .ForeignKey("dbo.BoilSteps", t => t.StepId, cascadeDelete: true)
                .Index(t => t.StepId)
                .Index(t => t.OtherId);
            
            CreateTable(
                "dbo.FermentationStepOthers",
                c => new
                    {
                        StepId = c.Int(nullable: false),
                        OtherId = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StepId, t.OtherId })
                .ForeignKey("dbo.Other", t => t.OtherId, cascadeDelete: true)
                .ForeignKey("dbo.FermentationSteps", t => t.StepId, cascadeDelete: true)
                .Index(t => t.StepId)
                .Index(t => t.OtherId);
            
            CreateTable(
                "dbo.Origins",
                c => new
                    {
                        OriginId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.OriginId);
            
            CreateTable(
                "dbo.FermentationStepYeasts",
                c => new
                    {
                        StepId = c.Int(nullable: false),
                        YeastId = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StepId, t.YeastId })
                .ForeignKey("dbo.Yeasts", t => t.YeastId, cascadeDelete: true)
                .ForeignKey("dbo.FermentationSteps", t => t.StepId, cascadeDelete: true)
                .Index(t => t.StepId)
                .Index(t => t.YeastId);
            
            CreateTable(
                "dbo.Yeasts",
                c => new
                    {
                        YeastId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        TemperatureHigh = c.Double(),
                        TemperatureLow = c.Double(),
                        Flocculation = c.String(),
                        AlcoholTolerance = c.String(),
                        ProductCode = c.String(),
                        Notes = c.String(),
                        Type = c.String(),
                        BrewerySource = c.String(),
                        Species = c.String(),
                        AttenutionRange = c.String(),
                        PitchingFermentationNotes = c.String(),
                        SupplierId = c.Int(),
                        Custom = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.YeastId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        SupplierId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        OriginId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SupplierId)
                .ForeignKey("dbo.Origins", t => t.OriginId, cascadeDelete: true)
                .Index(t => t.OriginId);
            
            CreateTable(
                "dbo.SRMs",
                c => new
                    {
                        SrmId = c.Int(nullable: false, identity: true),
                        Standard = c.Double(nullable: false),
                        Mosher = c.Double(nullable: false),
                        Daniels = c.Double(nullable: false),
                        Morey = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.SrmId);
            
            CreateTable(
                "dbo.UserCredentials",
                c => new
                    {
                        UserCredentialsId = c.Int(nullable: false, identity: true),
                        Password = c.Binary(nullable: false),
                        Salt = c.Binary(nullable: false),
                        Username = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserCredentialsId)
                .ForeignKey("dbo.Users", t => t.Username, cascadeDelete: true)
                .Index(t => t.Username);
            
            CreateTable(
                "dbo.Substitute",
                c => new
                    {
                        HopId = c.Int(nullable: false),
                        SubstitutHopId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HopId, t.SubstitutHopId })
                .ForeignKey("dbo.Hops", t => t.HopId)
                .ForeignKey("dbo.Hops", t => t.SubstitutHopId)
                .Index(t => t.HopId)
                .Index(t => t.SubstitutHopId);
            
            CreateTable(
                "dbo.ForkedRecipe",
                c => new
                    {
                        RecipeId = c.Int(nullable: false),
                        ForkedRecipeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RecipeId, t.ForkedRecipeId })
                .ForeignKey("dbo.Recipes", t => t.RecipeId)
                .ForeignKey("dbo.Recipes", t => t.ForkedRecipeId)
                .Index(t => t.RecipeId)
                .Index(t => t.ForkedRecipeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCredentials", "Username", "dbo.Users");
            DropForeignKey("dbo.ABVs", "AbvId", "dbo.Beers");
            DropForeignKey("dbo.Beers", "BeerId", "dbo.SRMs");
            DropForeignKey("dbo.Recipes", "RecipeId", "dbo.Beers");
            DropForeignKey("dbo.ForkedRecipe", "ForkedRecipeId", "dbo.Recipes");
            DropForeignKey("dbo.ForkedRecipe", "RecipeId", "dbo.Recipes");
            DropForeignKey("dbo.Recipes", "ForkeOfId", "dbo.Recipes");
            DropForeignKey("dbo.BoilSteps", "RecipeId", "dbo.Recipes");
            DropForeignKey("dbo.BoilStepOthers", "StepId", "dbo.BoilSteps");
            DropForeignKey("dbo.BoilStepHops", "StepId", "dbo.BoilSteps");
            DropForeignKey("dbo.BoilStepFermentables", "StepId", "dbo.BoilSteps");
            DropForeignKey("dbo.Fermentables", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.FermentationStepFermentables", "FermentableId", "dbo.Fermentables");
            DropForeignKey("dbo.FermentationStepYeasts", "StepId", "dbo.FermentationSteps");
            DropForeignKey("dbo.Yeasts", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.Suppliers", "OriginId", "dbo.Origins");
            DropForeignKey("dbo.FermentationStepYeasts", "YeastId", "dbo.Yeasts");
            DropForeignKey("dbo.FermentationSteps", "RecipeId", "dbo.Recipes");
            DropForeignKey("dbo.FermentationStepOthers", "StepId", "dbo.FermentationSteps");
            DropForeignKey("dbo.FermentationStepHops", "StepId", "dbo.FermentationSteps");
            DropForeignKey("dbo.FermentationStepHops", "HopFormId", "dbo.HopForms");
            DropForeignKey("dbo.Substitute", "SubstitutHopId", "dbo.Hops");
            DropForeignKey("dbo.Substitute", "HopId", "dbo.Hops");
            DropForeignKey("dbo.Hops", "OriginId", "dbo.Origins");
            DropForeignKey("dbo.MashStepHops", "HopId", "dbo.Hops");
            DropForeignKey("dbo.MashSteps", "RecipeId", "dbo.Recipes");
            DropForeignKey("dbo.MashStepOthers", "StepId", "dbo.MashSteps");
            DropForeignKey("dbo.MashStepOthers", "OtherId", "dbo.Other");
            DropForeignKey("dbo.FermentationStepOthers", "OtherId", "dbo.Other");
            DropForeignKey("dbo.BoilStepOthers", "OtherId", "dbo.Other");
            DropForeignKey("dbo.MashStepHops", "StepId", "dbo.MashSteps");
            DropForeignKey("dbo.MashStepFermentables", "StepId", "dbo.MashSteps");
            DropForeignKey("dbo.MashStepFermentables", "FermentableId", "dbo.Fermentables");
            DropForeignKey("dbo.MashStepHops", "HopFormId", "dbo.HopForms");
            DropForeignKey("dbo.HopFlavours", "HopId", "dbo.Hops");
            DropForeignKey("dbo.HopFlavours", "FlavourId", "dbo.Flavours");
            DropForeignKey("dbo.FermentationStepHops", "HopId", "dbo.Hops");
            DropForeignKey("dbo.BoilStepHops", "HopId", "dbo.Hops");
            DropForeignKey("dbo.BoilStepHops", "HopFormId", "dbo.HopForms");
            DropForeignKey("dbo.FermentationStepFermentables", "StepId", "dbo.FermentationSteps");
            DropForeignKey("dbo.BoilStepFermentables", "FermentableId", "dbo.Fermentables");
            DropForeignKey("dbo.IBUs", "IbuId", "dbo.Beers");
            DropForeignKey("dbo.UserBeers", "BeerId", "dbo.Beers");
            DropForeignKey("dbo.BreweryBeers", "BeerId", "dbo.Beers");
            DropForeignKey("dbo.BreweryMembers", "BreweryId", "dbo.Breweries");
            DropForeignKey("dbo.BreweryMembers", "MemberUsername", "dbo.Users");
            DropForeignKey("dbo.UserBeers", "Username", "dbo.Users");
            DropForeignKey("dbo.BreweryBeers", "BreweryId", "dbo.Breweries");
            DropForeignKey("dbo.BeerStyles", "SuperStyleId", "dbo.BeerStyles");
            DropForeignKey("dbo.BeerStyleGlasses", "BeerStyleId", "dbo.BeerStyles");
            DropForeignKey("dbo.BeerStyleGlasses", "GlassId", "dbo.Glasses");
            DropForeignKey("dbo.Beers", "BeerStyleId", "dbo.BeerStyles");
            DropIndex("dbo.ForkedRecipe", new[] { "ForkedRecipeId" });
            DropIndex("dbo.ForkedRecipe", new[] { "RecipeId" });
            DropIndex("dbo.Substitute", new[] { "SubstitutHopId" });
            DropIndex("dbo.Substitute", new[] { "HopId" });
            DropIndex("dbo.UserCredentials", new[] { "Username" });
            DropIndex("dbo.Suppliers", new[] { "OriginId" });
            DropIndex("dbo.Yeasts", new[] { "SupplierId" });
            DropIndex("dbo.FermentationStepYeasts", new[] { "YeastId" });
            DropIndex("dbo.FermentationStepYeasts", new[] { "StepId" });
            DropIndex("dbo.FermentationStepOthers", new[] { "OtherId" });
            DropIndex("dbo.FermentationStepOthers", new[] { "StepId" });
            DropIndex("dbo.BoilStepOthers", new[] { "OtherId" });
            DropIndex("dbo.BoilStepOthers", new[] { "StepId" });
            DropIndex("dbo.MashStepOthers", new[] { "OtherId" });
            DropIndex("dbo.MashStepOthers", new[] { "StepId" });
            DropIndex("dbo.MashStepFermentables", new[] { "StepId" });
            DropIndex("dbo.MashStepFermentables", new[] { "FermentableId" });
            DropIndex("dbo.MashSteps", new[] { "RecipeId" });
            DropIndex("dbo.MashStepHops", new[] { "HopFormId" });
            DropIndex("dbo.MashStepHops", new[] { "StepId" });
            DropIndex("dbo.MashStepHops", new[] { "HopId" });
            DropIndex("dbo.HopFlavours", new[] { "HopId" });
            DropIndex("dbo.HopFlavours", new[] { "FlavourId" });
            DropIndex("dbo.BoilStepHops", new[] { "HopFormId" });
            DropIndex("dbo.BoilStepHops", new[] { "HopId" });
            DropIndex("dbo.BoilStepHops", new[] { "StepId" });
            DropIndex("dbo.Hops", new[] { "OriginId" });
            DropIndex("dbo.FermentationStepHops", new[] { "HopId" });
            DropIndex("dbo.FermentationStepHops", new[] { "HopFormId" });
            DropIndex("dbo.FermentationStepHops", new[] { "StepId" });
            DropIndex("dbo.FermentationSteps", new[] { "RecipeId" });
            DropIndex("dbo.FermentationStepFermentables", new[] { "StepId" });
            DropIndex("dbo.FermentationStepFermentables", new[] { "FermentableId" });
            DropIndex("dbo.Fermentables", new[] { "SupplierId" });
            DropIndex("dbo.BoilStepFermentables", new[] { "StepId" });
            DropIndex("dbo.BoilStepFermentables", new[] { "FermentableId" });
            DropIndex("dbo.BoilSteps", new[] { "RecipeId" });
            DropIndex("dbo.Recipes", new[] { "ForkeOfId" });
            DropIndex("dbo.Recipes", new[] { "RecipeId" });
            DropIndex("dbo.IBUs", new[] { "IbuId" });
            DropIndex("dbo.UserBeers", new[] { "Username" });
            DropIndex("dbo.UserBeers", new[] { "BeerId" });
            DropIndex("dbo.BreweryMembers", new[] { "MemberUsername" });
            DropIndex("dbo.BreweryMembers", new[] { "BreweryId" });
            DropIndex("dbo.BreweryBeers", new[] { "BreweryId" });
            DropIndex("dbo.BreweryBeers", new[] { "BeerId" });
            DropIndex("dbo.BeerStyleGlasses", new[] { "GlassId" });
            DropIndex("dbo.BeerStyleGlasses", new[] { "BeerStyleId" });
            DropIndex("dbo.BeerStyles", new[] { "SuperStyleId" });
            DropIndex("dbo.Beers", new[] { "BeerStyleId" });
            DropIndex("dbo.Beers", new[] { "BeerId" });
            DropIndex("dbo.ABVs", new[] { "AbvId" });
            DropTable("dbo.ForkedRecipe");
            DropTable("dbo.Substitute");
            DropTable("dbo.UserCredentials");
            DropTable("dbo.SRMs");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Yeasts");
            DropTable("dbo.FermentationStepYeasts");
            DropTable("dbo.Origins");
            DropTable("dbo.FermentationStepOthers");
            DropTable("dbo.BoilStepOthers");
            DropTable("dbo.Other");
            DropTable("dbo.MashStepOthers");
            DropTable("dbo.MashStepFermentables");
            DropTable("dbo.MashSteps");
            DropTable("dbo.MashStepHops");
            DropTable("dbo.Flavours");
            DropTable("dbo.HopFlavours");
            DropTable("dbo.HopForms");
            DropTable("dbo.BoilStepHops");
            DropTable("dbo.Hops");
            DropTable("dbo.FermentationStepHops");
            DropTable("dbo.FermentationSteps");
            DropTable("dbo.FermentationStepFermentables");
            DropTable("dbo.Fermentables");
            DropTable("dbo.BoilStepFermentables");
            DropTable("dbo.BoilSteps");
            DropTable("dbo.Recipes");
            DropTable("dbo.IBUs");
            DropTable("dbo.UserBeers");
            DropTable("dbo.Users");
            DropTable("dbo.BreweryMembers");
            DropTable("dbo.Breweries");
            DropTable("dbo.BreweryBeers");
            DropTable("dbo.Glasses");
            DropTable("dbo.BeerStyleGlasses");
            DropTable("dbo.BeerStyles");
            DropTable("dbo.Beers");
            DropTable("dbo.ABVs");
        }
    }
}
