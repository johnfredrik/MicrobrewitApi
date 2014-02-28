namespace MicrobrewitApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Fermentables", newName: "Fermentable");
            RenameColumn(table: "dbo.Fermentable", name: "Discriminator", newName: "Type");
            AlterColumn("dbo.Fermentable", "Name", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Fermentable", "Type", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Hops", "Name", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Origins", "Name", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Origins", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Hops", "Name", c => c.String());
            AlterColumn("dbo.Fermentable", "Type", c => c.String());
            AlterColumn("dbo.Fermentable", "Name", c => c.String());
            RenameColumn(table: "dbo.Fermentable", name: "Type", newName: "Discriminator");
            RenameTable(name: "dbo.Fermentable", newName: "Fermentables");
        }
    }
}
