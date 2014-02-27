namespace MicrobrewitApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hops",
                c => new
                    {
                        HopId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        AALow = c.Double(nullable: false),
                        AAHigh = c.Double(nullable: false),
                        OriginId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HopId)
                .ForeignKey("dbo.Origins", t => t.OriginId, cascadeDelete: true)
                .Index(t => t.OriginId);
            
            CreateTable(
                "dbo.Origins",
                c => new
                    {
                        OriginId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.OriginId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Hops", "OriginId", "dbo.Origins");
            DropIndex("dbo.Hops", new[] { "OriginId" });
            DropTable("dbo.Origins");
            DropTable("dbo.Hops");
        }
    }
}
