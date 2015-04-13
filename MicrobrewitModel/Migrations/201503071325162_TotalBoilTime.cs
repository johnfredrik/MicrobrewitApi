namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TotalBoilTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipes", "TotalBoilTime", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipes", "TotalBoilTime");
        }
    }
}
