namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSuperSubFermentables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fermentables", "SuperFermentableId", c => c.Int());
            CreateIndex("dbo.Fermentables", "SuperFermentableId");
            AddForeignKey("dbo.Fermentables", "SuperFermentableId", "dbo.Fermentables", "FermentableId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fermentables", "SuperFermentableId", "dbo.Fermentables");
            DropIndex("dbo.Fermentables", new[] { "SuperFermentableId" });
            DropColumn("dbo.Fermentables", "SuperFermentableId");
        }
    }
}
