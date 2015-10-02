using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using Microbrewit.Repository;
using Microbrewit.Repository.Repository;
using NUnit.Framework;

namespace Microbrewit.Test.Repository
{
    [TestFixture]
    public class BeerRepositoryTests
    {
        private IBeerRepository _beerRepository;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _beerRepository = new BeerDapperRepository();
        }

        [Test]
        public void GetAll_Return_Result()
        {
            var beers = _beerRepository.GetAll(0,20);
            Assert.NotNull(beers);
            Assert.True(beers.Any());
        }

        [Test]
        public void GetSingle_Return_Result()
        {
            var beer = _beerRepository.GetSingle(17);
            Assert.NotNull(beer);
            Assert.True(beer.Name.Any());
        }

        [Test]
        public async Task GetLastAsync_Returns_Value()
        {
            var beers = await _beerRepository.GetLastAsync(0,10);
            Assert.AreEqual(10,beers.Count);
        }

        [Test]
        public async Task GetAllUserBeersAsync_Returns_Value()
        {
            var beers = await _beerRepository.GetAllUserBeerAsync("johnfredrik");
            Assert.True(beers.Any());
            Assert.True(beers.All(b => b.Brewers.Any(u => u.Username == "johnfredrik")));
        }

        [Test]
        public void Add_Gets_Added()
        {
            var newBeer = new Beer
            {
                Name = "TestBeer" + DateTime.Now.Ticks,
                BeerStyleId = 1,
                ForkeOfId = 17,
                SRM = new SRM
                {
                    Daniels = 1,
                    Morey = 1,
                    Mosher = 1,
                    Standard = 1
                },
                ABV = new ABV
                {
                    Advanced = 2,
                    AdvancedAlternative = 2,
                    AlternativeSimple = 2,
                    Miller = 2,
                    Simple = 2,
                    Standard = 2,
                },
                IBU = new IBU
                {
                    Rager = 20,
                    Standard = 20,
                    Tinseth = 25
                },

            };
            var newRecipe = new Recipe
            {
                Volume = 20,
                Notes = "SOmething",
                Efficiency = 75,
                MashSteps = new List<MashStep>
                {
                    new MashStep
                    {
                        StepNumber = 1,
                        Temperature = 56,
                        Type = "Infusion",
                        Notes = "Notes",
                        Length = 10,
                        Volume = 20,
                        Fermentables = new List<MashStepFermentable>
                        {
                             new MashStepFermentable{Amount = 1000,Lovibond = 23,PPG = 12, FermentableId = 2},
                             new MashStepFermentable{Amount = 1000,Lovibond = 23,PPG = 12, FermentableId = 1},
                        },
                        Hops = new List<MashStepHop>
                        {
                            new MashStepHop
                            {
                                Amount = 30,
                                AAValue = 5,
                                HopFormId = 1,
                                HopId = 1,
                            }
                        },
                        Others = new List<MashStepOther>
                        {
                            new MashStepOther
                            {
                                Amount = 10,
                                OtherId = 1,
                            }
                        }

                    },
                     new MashStep
                    {
                        StepNumber = 2,
                        Temperature = 56,
                        Type = "Infusion",
                        Notes = "Notes",
                        Length = 10,
                        Volume = 20
                    }
                },
                BoilSteps = new List<BoilStep>
                {
                    new BoilStep
                    {
                        StepNumber = 3,
                        Volume = 21,
                        Notes = "Notes",
                        Length = 60,
                              Fermentables = new List<BoilStepFermentable>
                        {
                             new BoilStepFermentable{Amount = 1000,Lovibond = 23,PPG = 12, FermentableId = 2},
                             new BoilStepFermentable{Amount = 1000,Lovibond = 23,PPG = 12, FermentableId = 1},
                        },
                        Hops = new List<BoilStepHop>
                        {
                            new BoilStepHop
                            {
                                Amount = 30,
                                AAValue = 5,
                                HopFormId = 1,
                                HopId = 1,
                            }
                        },
                        Others = new List<BoilStepOther>
                        {
                            new BoilStepOther
                            {
                                Amount = 10,
                                OtherId = 1,
                            }
                        }
                    }
                },
                FermentationSteps = new List<FermentationStep>
                {
                    new FermentationStep
                    {
                        StepNumber = 4,
                        Length = 14,
                        Temperature = 21,
                        Notes = "Somehting more",
                        Volume = 19,
                               Fermentables = new List<FermentationStepFermentable>
                        {
                             new FermentationStepFermentable{Amount = 1000,Lovibond = 23,PPG = 12, FermentableId = 2},
                             new FermentationStepFermentable{Amount = 1000,Lovibond = 23,PPG = 12, FermentableId = 1},
                        },
                        Hops = new List<FermentationStepHop>
                        {
                            new FermentationStepHop
                            {
                                Amount = 30,
                                AAValue = 5,
                                HopFormId = 1,
                                HopId = 1,
                            }
                        },
                        Others = new List<FermentationStepOther>
                        {
                            new FermentationStepOther
                            {
                                Amount = 10,
                                OtherId = 1,
                            }
                        },
                        Yeasts = new List<FermentationStepYeast>
                        {
                            new FermentationStepYeast
                            {
                                Amount = 1,
                                YeastId = 1,
                            }
                        } 
                    }
                }

            };
            newBeer.Recipe = newRecipe;
            _beerRepository.Add(newBeer);
            var beer = _beerRepository.GetSingle(newBeer.BeerId);
            Assert.NotNull(beer);
            Assert.True(beer.Name.Any());
        }

        [Test]
        public void Update_Gets_Updated()
        {
            var beer = _beerRepository.GetSingle(70);
            beer.Name = "Updated" + DateTime.Now.Ticks;
            beer.Recipe.Volume = (int)DateTime.Now.Ticks;
            beer.SRM.Mosher = (int)DateTime.Now.Ticks;
            beer.ABV.AlternativeSimple = (int)DateTime.Now.Ticks;
            beer.IBU.Tinseth = (int)DateTime.Now.Ticks;
            _beerRepository.Update(beer);
            var updated = _beerRepository.GetSingle(beer.BeerId);
            Assert.AreEqual(beer.Name,updated.Name);
            Assert.AreEqual(beer.Recipe.Volume,updated.Recipe.Volume);
            Assert.AreEqual(beer.SRM.Mosher, updated.SRM.Mosher);
            Assert.AreEqual(beer.ABV.AlternativeSimple, updated.ABV.AlternativeSimple);
            Assert.AreEqual(beer.IBU.Tinseth, updated.IBU.Tinseth);
        }

        [Test]
        public void UpdateMashStep_Gets_Updated()
        {
            var beer = _beerRepository.GetSingle(70);
            beer.Name = "Updated" + DateTime.Now.Ticks;

            beer.Recipe.MashSteps = new List<MashStep>
            {
                new MashStep
                {
                    StepNumber = 1,
                    Temperature = 56,
                    Type = "Infusion",
                    Notes = "Notes",
                    Length = 20,
                    Volume = 20,
                    Fermentables = new List<MashStepFermentable>
                    {
                        new MashStepFermentable {Amount = 1000, Lovibond = 23, PPG = 12, FermentableId = 2},
                        new MashStepFermentable {Amount = 1000, Lovibond = 23, PPG = 12, FermentableId = 1},
                    },
                    Hops = new List<MashStepHop>
                    {
                        new MashStepHop
                        {
                            Amount = 30,
                            AAValue = 5,
                            HopFormId = 1,
                            HopId = 1,
                        }
                    },
                    Others = new List<MashStepOther>
                    {
                        new MashStepOther
                        {
                            Amount = 10,
                            OtherId = 1,
                        }
                    }

                },
                new MashStep
                {
                    StepNumber = 2,
                    Temperature = 68,
                    Type = "Infusion",
                    Notes = "Notes",
                    Length = 70,
                    Volume = 20
                }
            };
            beer.Recipe.BoilSteps = new List<BoilStep>
            {
                new BoilStep
                {
                    StepNumber = 3,
                    Volume = 21,
                    Notes = "Notes",
                    Length = 60,
                    Fermentables = new List<BoilStepFermentable>
                    {
                        new BoilStepFermentable {Amount = 1000, Lovibond = 23, PPG = 12, FermentableId = 2},
                        new BoilStepFermentable {Amount = 1000, Lovibond = 23, PPG = 12, FermentableId = 1},
                    },
                    Hops = new List<BoilStepHop>
                    {
                        new BoilStepHop
                        {
                            Amount = 30,
                            AAValue = 5,
                            HopFormId = 1,
                            HopId = 1,
                        }
                    },
                    Others = new List<BoilStepOther>
                    {
                        new BoilStepOther
                        {
                            Amount = 10,
                            OtherId = 1,
                        }
                    }
                }
            };
            beer.Recipe.FermentationSteps = new List<FermentationStep>
            {
                new FermentationStep
                {
                    StepNumber = 4,
                    Length = 14,
                    Temperature = 21,
                    Notes = "Somehting more",
                    Volume = 19,
                    Fermentables = new List<FermentationStepFermentable>
                    {
                        new FermentationStepFermentable {Amount = 1000, Lovibond = 23, PPG = 12, FermentableId = 2},
                        new FermentationStepFermentable {Amount = 1000, Lovibond = 23, PPG = 12, FermentableId = 1},
                    },
                    Hops = new List<FermentationStepHop>
                    {
                        new FermentationStepHop
                        {
                            Amount = 30,
                            AAValue = 5,
                            HopFormId = 1,
                            HopId = 1,
                        }
                    },
                    Others = new List<FermentationStepOther>
                    {
                        new FermentationStepOther
                        {
                            Amount = 10,
                            OtherId = 1,
                        }
                    },
                    Yeasts = new List<FermentationStepYeast>
                    {
                        new FermentationStepYeast
                        {
                            Amount = 1,
                            YeastId = 1,
                        }
                    }
                }
            };
            _beerRepository.Update(beer);
            var updated = _beerRepository.GetSingle(beer.BeerId);
            Assert.AreEqual(beer.Name, updated.Name);
           
        }

        [Test]
        public void UpdateMashStep_Gets_Removed()
        {
            var beer = _beerRepository.GetSingle(70);
            beer.Name = "Updated" + DateTime.Now.Ticks;
            beer.Recipe.MashSteps = new List<MashStep>();
            beer.Recipe.BoilSteps = new List<BoilStep>();
            beer.Recipe.FermentationSteps = new List<FermentationStep>();
            _beerRepository.Update(beer);
            var updated = _beerRepository.GetSingle(beer.BeerId);
            Assert.AreEqual(beer.Name, updated.Name);

        }
    }
}
