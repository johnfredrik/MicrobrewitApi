using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace MicrobrewitModel
{
    public class InitializeDatabaseWithSeedData : DropCreateDatabaseAlways<MicrobrewitApiContext>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void Seed(MicrobrewitApiContext context)
        {
            Log.Debug("Initilizing DataBase with Seed Data");

            context.Origins.Add(new Origin() {OriginId = 1, Name ="United States"});
            context.Origins.Add(new Origin() { OriginId = 2, Name = "United Kingdom" });

            context.FermentableTypes.Add(new FermentableType { Id = 1, Name = "Grain" });
            context.FermentableTypes.Add(new FermentableType { Id = 2, Name = "Liquid Extract"});
            context.FermentableTypes.Add(new FermentableType { Id = 3, Name = "Dry Extract"});


            context.Hops.Add(new Hop() { Id = 1, Name="Admiral", AAHigh = 15, AALow = 9 , OriginId = 2});
            context.Hops.Add(new Hop() { Id = 2, Name="Challanger", AAHigh = 8.5, AALow = 6.5, OriginId = 2});
           

            context.Fermentables.Add(new Fermentable() {Id = 1, Name = "Malt", Colour = 20, PPG = 34, TypeId = 1});
            context.Fermentables.Add(new Fermentable() {Id = 2, Name = "Pale Ale Malt" , Colour = 2, PPG = 37, TypeId = 1});
            context.Fermentables.Add(new Fermentable() {Id = 3, Name = "Plain Light DME" , Colour = 4, PPG = 43, TypeId = 2});
            context.Fermentables.Add(new Fermentable() {Id = 4, Name = "Plain Light DME", Colour = 4, PPG = 43, TypeId = 3});
            
        }
    }
}
