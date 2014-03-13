using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace MicrobrewitModel
{
    public class InitializeDatabaseWithSeedData : DropCreateDatabaseAlways<MicrobrewitContext>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void Seed(MicrobrewitContext context)
        {
            Log.Debug("Initilizing DataBase with Seed Data");

            context.Origins.Add(new Origin() { Id = 1, Name ="United States"});
            context.Origins.Add(new Origin() { Id = 2, Name = "United Kingdom" });
            context.Origins.Add(new Origin() { Id = 3, Name = "Belgium" });

            context.FermentableTypes.Add(new FermentableType { Id = 1, Name = "Grain" });
            context.FermentableTypes.Add(new FermentableType { Id = 2, Name = "Liquid Extract"});
            context.FermentableTypes.Add(new FermentableType { Id = 3, Name = "Dry Extract"});

            context.Suppliers.Add(new Supplier { Id = 1, Name = "Boortmalt", OriginId = 3, });
            context.Suppliers.Add(new Supplier { Id = 2, Name = "White Labs", OriginId = 1, });
            context.Suppliers.Add(new Supplier { Id = 3, Name = "Fermentis", OriginId = 1, });

            context.Hops.Add(new Hop() { Id = 1, Name="Admiral", AAHigh = 15, AALow = 9 , OriginId = 2});
            context.Hops.Add(new Hop() { Id = 2, Name="Challanger", AAHigh = 8.5, AALow = 6.5, OriginId = 2});

            context.Others.Add(new Fruit() { Id = 1, Name = "Strawberry" });
            context.Others.Add(new NoneFermentableSugar() { Id = 2, Name = "Honey" });
            context.Others.Add(new Spice() { Id = 3, Name = "Koriander" });

            context.Yeasts.Add(new LiquidYeast()
            {
                Id = 1,
                Name = "California Ale Yeast",
                TemperatureLow = 73,
                TemperatureHigh = 80,
                Comment = "This yeast is famous for its clean flavors, balance and ability to be used in almost any style ale. It accentuates the hop flavors and is extremely versatile",
                ProductCode = "WLP001",             
                SupplierId = 2,
            });
            context.Yeasts.Add(new LiquidYeast()
            {
                Id = 2,
                Name = "Safale US 05",                
                Comment = "Ready-to-pitch American ale yeast for well balanced beers with low diacetyl and a very crisp end palate.",                
                SupplierId = 3,
            });

            context.Fermentables.Add(new Grain() {Id = 1, Name = "Malt", Colour = 20, PPG = 34, });
            context.Fermentables.Add(new Grain() {Id = 2, Name = "Amber Malt", Colour = 20, PPG = 34, SupplierId = 1 });
            context.Fermentables.Add(new Grain() {Id = 3, Name = "Pale Ale Malt" , Colour = 2, PPG = 37, });
            context.Fermentables.Add(new DryExtract() {Id = 4, Name = "Plain Light DME" , Colour = 4, PPG = 43,});
            context.Fermentables.Add(new LiquidExtract() {Id = 5, Name = "Plain Light DME", Colour = 4, PPG = 43,});

            context.Users.Add(new User() { Username = "johnfredrik", Email = "john-f@online.no" });
            context.UserCredentials.Add(new UserCredentials()
            {
                Id = 1,
                Password = "EAAAAA2i7rB183t/vrZ62ahBVELmFmmO9B5Fzz4xz9F57tya",
                SharedSecret = "test",
                Username = "johnfredrik",
            });

            //context.Recipes.Add(new Recipe() { Id = 1, Name = "Beer Good", Hops = new List<Hop>() { new Hop() {Id = 1, Name = "Malt"}} });

            
        }
    }
}
