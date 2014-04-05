using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Microbrewit.Model
{
    public class InitializeDatabaseWithSeedData : DropCreateDatabaseAlways<MicrobrewitContext>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void Seed(MicrobrewitContext context)
        {
            Log.Debug("Initilizing DataBase with Seed Data");

            //context.Origins.Add(new Origin() { Id = 1, Name = "United States" });
            //context.Origins.Add(new Origin() { Id = 2, Name = "United Kingdom" });
            //context.Origins.Add(new Origin() { Id = 3, Name = "Belgium" });

            //context.Suppliers.Add(new Supplier { Id = 1, Name = "Boortmalt", OriginId = 3, });
            //context.Suppliers.Add(new Supplier { Id = 2, Name = "White Labs", OriginId = 1, });
            //context.Suppliers.Add(new Supplier { Id = 3, Name = "Fermentis", OriginId = 1, });
            //context.Suppliers.Add(new Supplier { Id = 4, Name = "Thomas Fawcett", OriginId = 2 });
            //context.Suppliers.Add(new Supplier { Id = 5, Name = " De Wolf-Cosyns", OriginId = 3 });

            var mild = new Flavour { Id = 1, Name = "Mild to moderate" };
            var spicy = new Flavour { Id = 2, Name = "Quite spicy" };
            context.Flavours.Add(mild);
            context.Flavours.Add(spicy);
            
           // var target = new Hop() { Id = 1, Name ="Target", AALow = 9.5, AAHigh = 12.5, OriginId = 2, FlavourDescription = "Pleasant English hop aroma, quite intense." };
           // var challanger = new Hop() { Id = 2, Name="Challanger", AAHigh = 8.5, AALow = 6.5, OriginId = 2, Flavours = new List<HopFlavour>(){new HopFlavour(){HopId = 1, FlavourId = mild.Id},
           //     new HopFlavour(){HopId = 1, FlavourId = spicy.Id}}};
           // var admiral = new Hop() { Id = 3, Name = "Admiral", AAHigh = 15, AALow = 9, OriginId = 2, Substituts = new List<Hop> { target, challanger } };
           // var ekg = new Hop() { Id = 4,Name = "East Kent Goldings", AAHigh = 7, AALow = 4, OriginId = 2, BetaLow = 1.9, BetaHigh = 2.8 };

           // context.Hops.Add(target);
           // context.Hops.Add(challanger);
           // context.Hops.Add(admiral);
           // context.Hops.Add(ekg);

           // context.HopFlavours.Add(new HopFlavour() { FlavourId = 1, HopId = 2 });
           /// context.HopFlavours.Add(new HopFlavour() { FlavourId = 2, HopId = 2 });

            context.HopForms.Add(new HopForm() { Id = 1, Name = "Pellet" });
            context.HopForms.Add(new HopForm() { Id = 2, Name = "Leaf" });
            context.HopForms.Add(new HopForm() { Id = 4, Name = "Plug" });

            //context.Others.Add(new Fruit() { Id = 1, Name = "Strawberry" });
            //context.Others.Add(new NoneFermentableSugar() { Id = 2, Name = "Honey" });
            //context.Others.Add(new Spice() { Id = 3, Name = "Koriander" });

            //context.Yeasts.Add(new LiquidYeast()
            //{
            //    Id = 1,
            //    Name = "California Ale Yeast",
            //    TemperatureLow = 73,
            //    TemperatureHigh = 80,
            //    Comment = "This yeast is famous for its clean flavors, balance and ability to be used in almost any style ale. It accentuates the hop flavors and is extremely versatile",
            //    ProductCode = "WLP001",             
            //    SupplierId = 2,
            //});
            //context.Yeasts.Add(new LiquidYeast()
            //{
            //    Id = 2,
            //    Name = "Safale US 05",                
            //    Comment = "Ready-to-pitch American ale yeast for well balanced beers with low diacetyl and a very crisp end palate.",                
            //    SupplierId = 3,
            //});

            //context.Fermentables.Add(new Grain() {Id = 1, Name = "Malt", EBC = 20, PPG = 34, });
            //context.Fermentables.Add(new Grain() { Name = "Maris Otter Pale Malt", EBC = 7, PPG = 36, SupplierId = 4});
            //context.Fermentables.Add(new Grain() { Name = "Pale Crystal", EBC = 75, SupplierId = 4 });
            //context.Fermentables.Add(new Grain() { Name = "Biscute Malt", EBC = 50, SupplierId = 5 });
            //context.Fermentables.Add(new Grain() { Name = "Chocolate Malt", EBC = 1175, SupplierId = 4 });
            //context.Fermentables.Add(new Grain() {Id = 2, Name = "Amber Malt", EBC = 20, PPG = 34, SupplierId = 1 });
            //context.Fermentables.Add(new Grain() {Id = 3, Name = "Pale Ale Malt" , EBC = 2, PPG = 37, });
            //context.Fermentables.Add(new DryExtract() {Id = 4, Name = "Plain Light DME" , EBC = 4, PPG = 43,});
            //context.Fermentables.Add(new LiquidExtract() {Id = 5, Name = "Plain Light DME", EBC = 4, PPG = 43,});

            var user = new User() { Username = "johnfredrik", Email = "john-f@online.no" };
            context.Users.Add(user);
            context.UserCredentials.Add(new UserCredentials()
            {
                Id = 1,
                Password = "EAAAAA2i7rB183t/vrZ62ahBVELmFmmO9B5Fzz4xz9F57tya",
                SharedSecret = "test",
                Username = "johnfredrik",
            });



            context.Breweries.Add(new Brewery() { Name = "Asphaugs Hjemmebryggeri", Members = new List<BreweryMember>() { new BreweryMember() { MemberId = "johnfredrik"} } });

            context.BeerStyles.Add(new BeerStyle() { Id = 1, Name = "Ale" });
            context.BeerStyles.Add(new BeerStyle() { Id = 2, Name = "Golden Ale", SuperStyleId = 1 });

           
            //var recipe = new Recipe()
            //{
            //    Id = 1,
            //    BeerStyleId = 1,
            //    MashSteps = new List<MashStep>()
            //        {
            //            new MashStep()
            //            {
            //                Number = 1,
            //                Temperature = 68,
            //                Type = "Infusion",
            //                Length = 68,
            //                Volume = 28,
            //                Fermentables = new List<MashStepFermentable>()
            //                {
            //                   new MashStepFermentable()
            //                   {
            //                       FermentableId = 1,
            //                       Amount = 12

            //                   },
            //                   new MashStepFermentable()
            //                   {
            //                       FermentableId = 2,
            //                       Amount = 20
            //                   }

            //                },
            //                Others = new List<MashStepOther>()
            //                {
            //                   new MashStepOther()
            //                   {
            //                       OtherId = 1,
            //                       Amount = 12
            //                   }

            //                }
            //            },
            //            new MashStep()
            //            {
            //                Number = 2,
            //                Temperature = 68,
            //                Type = "Infusion",
            //                Length = 60,
            //                Volume = 28,
            //            }

            //        },
            //    BoilSteps = new List<BoilStep>()
            //    {
            //        new BoilStep()
            //        {
            //            Number = 1,
            //            Length = 60,
            //            Volume = 28,
            //            Hops = new List<BoilStepHop>()
            //            {
            //                new BoilStepHop()
            //                {
            //                    HopId = 1,
            //                    HopFormId = 1,
            //                    AAValue = 13,
            //                    AAAmount = 70
            //                }
            //            }
            //        }
                    
            //    },

            //    FermentationSteps = new List<FermentationStep>()
            //    {
            //        new FermentationStep()
            //        {
            //            Number = 1,
            //            Length = 14,
            //            Temperature = 19,
            //            Notes = "Something Cool",
            //            Yeasts = new List<FermentationStepYeast>()
            //            {
            //                new FermentationStepYeast()
            //                {
            //                    YeastId = 1,
            //                    Amount = 1
            //                }
            //            }                       
            //        }
            //    }
            //};
            //var brewers = new List<User>();
            //brewers.Add(user);
            //var beer = new Beer() { Id = 1, Name = "Good Beer", Recipe = recipe, Brewers = brewers, BeerStyleId = 1 };
            //beer.ABV = new ABV() { Id = beer.Id, Standard = 5 };
            //beer.IBU = new IBU() { Id = beer.Id, Standard = 16};
            //beer.SRM = new SRM() { Id = beer.Id, Standard = 20};
          //  context.Recipes.Add(recipe);
          //  context.Beers.Add(beer);


        }
    }
}
