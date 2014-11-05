namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedBeerStylesWithComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BeerStyles", "Comments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BeerStyles", "Comments");
        }
    }
}
