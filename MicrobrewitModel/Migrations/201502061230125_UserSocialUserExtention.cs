namespace Microbrewit.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserSocialUserExtention : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSocials",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        SocialId = c.Int(nullable: false, identity: true),
                        Site = c.String(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => new { t.Username, t.SocialId })
                .ForeignKey("dbo.Users", t => t.Username, cascadeDelete: true)
                .Index(t => t.Username);
            
            AddColumn("dbo.Users", "HeaderImage", c => c.String());
            AddColumn("dbo.Users", "Avatar", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSocials", "Username", "dbo.Users");
            DropIndex("dbo.UserSocials", new[] { "Username" });
            DropColumn("dbo.Users", "Avatar");
            DropColumn("dbo.Users", "HeaderImage");
            DropTable("dbo.UserSocials");
        }
    }
}
