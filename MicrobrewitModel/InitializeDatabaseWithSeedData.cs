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

            context.Origins.Add(new Origin() { Id = 1, Name ="United States"});
            context.Origins.Add(new Origin() { Id = 2, Name = "United Kingdom" });
            context.Origins.Add(new Origin() { Id = 3, Name = "Belgium" });

            context.FermentableTypes.Add(new FermentableType { Id = 1, Name = "Grain" });
            context.FermentableTypes.Add(new FermentableType { Id = 2, Name = "Liquid Extract"});
            context.FermentableTypes.Add(new FermentableType { Id = 3, Name = "Dry Extract"});

            context.Suppliers.Add(new Supplier { Id = 1, Name = "Boortmalt", OriginId = 3, });
            context.Suppliers.Add(new Supplier { Id = 2, Name = "White Labs", OriginId = 1, });
            context.Suppliers.Add(new Supplier { Id = 3, Name = "Fermentis", OriginId = 1, });

            context.Flavours.Add(new Flavour { Id = 1, Name = "Mild to moderate" });
            context.Flavours.Add(new Flavour { Id = 2, Name = "Quite spicy" });
            
            context.Hops.Add(new Hop() { Id = 2, Name="Challanger", AAHigh = 8.5, AALow = 6.5, OriginId = 2});
            context.Hops.Add(new Hop() { Id = 1, Name="Admiral", AAHigh = 15, AALow = 9 , OriginId = 2});

            context.HopFlavours.Add(new HopFlavour() { FlavourId = 1, HopId = 2 });
            context.HopFlavours.Add(new HopFlavour() { FlavourId = 2, HopId = 2 });

            context.HopForms.Add(new HopForm() { Id = 1, Name = "Pellet" });
            context.HopForms.Add(new HopForm() { Id = 2, Name = "Leaf" });
            context.HopForms.Add(new HopForm() { Id = 4, Name = "Plug" });

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

            var user = new User() { Username = "johnfredrik", Email = "john-f@online.no" };
            context.Users.Add(user);
            context.UserCredentials.Add(new UserCredentials()
            {
                Id = 1,
                Password = "EAAAAA2i7rB183t/vrZ62ahBVELmFmmO9B5Fzz4xz9F57tya",
                SharedSecret = "test",
                Username = "johnfredrik",
            });

            context.BeerStyles.Add(new BeerStyle() { Id = 1, Name = "Ale" });
            context.BeerStyles.Add(new BeerStyle() { Id = 2, Name = "Golden Ale", SuperStyleId = 1 });

           
            var recipe = new Recipe()
            {
                Name = "Beer Good",
                BeerStyleId = 1,
                Brewers = new List<User>{user},
                MashSteps = new List<MashStep>()
                    {
                        new MashStep()
                        {
                            Number = 1,
                            Temperature = 68,
                            Type = "Infusion",
                            Length = 68,
                            Volume = 28,
                            Fermentables = new List<MashStepFermentable>()
                            {
                               new MashStepFermentable()
                               {
                                   FermentableId = 1,
                                   Amount = 12

                               },
                               new MashStepFermentable()
                               {
                                   FermentableId = 2,
                                   Amount = 20
                               }

                            },
                            Others = new List<MashStepOther>()
                            {
                               new MashStepOther()
                               {
                                   OtherId = 1,
                                   Amount = 12
                               }

                            }
                        },
                        new MashStep()
                        {
                            Number = 2,
                            Temperature = 68,
                            Type = "Infusion",
                            Length = 60,
                            Volume = 28,
                        }

                    },
                BoilSteps = new List<BoilStep>()
                {
                    new BoilStep()
                    {
                        Number = 1,
                        Length = 60,
                        Volume = 28,
                        Hops = new List<BoilStepHop>()
                        {
                            new BoilStepHop()
                            {
                                HopId = 1,
                                HopFormId = 1,
                                AAValue = 13,
                                AAAmount = 70
                            }
                        }
                    }
                    
                },

                FermentationSteps = new List<FermentationStep>()
                {
                    new FermentationStep()
                    {
                        Number = 1,
                        Length = 14,
                        Temperature = 19,
                        Notes = "Something Cool",
                        Yeasts = new List<FermentationStepYeast>()
                        {
                            new FermentationStepYeast()
                            {
                                YeastId = 1,
                                Amount = 1
                            }
                        }                       
                    }
                }
            };

            context.Recipes.Add(recipe);
        }
    }
}
