namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfirmedUserBeerBreweryMember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BreweryMembers", "Confirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserBeers", "Confirmed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserBeers", "Confirmed");
            DropColumn("dbo.BreweryMembers", "Confirmed");
        }
    }
}
