namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGravatarToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Gravatar", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Gravatar");
        }
    }
}
