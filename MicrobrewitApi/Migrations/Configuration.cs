namespace MicrobrewitApi.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MicrobrewitApi.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MicrobrewitApi.Models.MicrobrewitApiContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MicrobrewitApi.Models.MicrobrewitApiContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Origins.AddOrUpdate(new Origin[] {
                new Origin() {OriginId = 1, Name ="United States"},
                new Origin() {OriginId = 2, Name ="United Kingdom"}
            });

            context.Hops.AddOrUpdate(new Hop[] {
               new Hop() { HopId = 1, Name="Admiral", AAHigh = 15, AALow = 9 , OriginId = 2},
               new Hop() { HopId = 2, Name="Challanger", AAHigh = 8.5, AALow = 6.5, OriginId = 2},
            });
        }
    }
}
