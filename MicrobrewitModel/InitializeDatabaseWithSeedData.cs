using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobrewitModel
{
    public class InitializeDatabaseWithSeedData : DropCreateDatabaseAlways<MicrobrewitApiContext>
    {
        protected override void Seed(MicrobrewitApiContext context)
        {
            context.Origins.Add(new Origin() {OriginId = 1, Name ="United States"});
            context.Origins.Add(new Origin() { OriginId = 2, Name = "United Kingdom" });
           

            context.Hops.Add(new Hop() { HopId = 1, Name="Admiral", AAHigh = 15, AALow = 9 , OriginId = 2});
            context.Hops.Add(new Hop() { HopId = 2, Name="Challanger", AAHigh = 8.5, AALow = 6.5, OriginId = 2});
           

            context.Fermentables.Add(new Grain() {FermentableId = 1, Name = "Malt", Colour = 20, PPG = 34});
            context.Fermentables.Add(new Grain() {FermentableId = 2, Name = "Pale Ale Malt" , Colour = 2, PPG = 37});
            context.Fermentables.Add(new LiquidExtract() {FermentableId = 3, Name = "Plain Light DME" , Colour = 4, PPG = 43});
            
        }
    }
}
