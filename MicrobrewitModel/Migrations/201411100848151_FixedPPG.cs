namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedPPG : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MashStepFermentables", "PPG", c => c.Int(nullable: false));
            DropColumn("dbo.MashStepFermentables", "PGG");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MashStepFermentables", "PGG", c => c.Int(nullable: false));
            DropColumn("dbo.MashStepFermentables", "PPG");
        }
    }
}
