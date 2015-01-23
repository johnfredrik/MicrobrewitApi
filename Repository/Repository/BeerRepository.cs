using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace Microbrewit.Repository
{
    public class BeerRepository : GenericDataRepository<Beer>, IBeerRepository
    {
        public override void Update(params Beer[] items)
        {
            using (var context = new MicrobrewitContext())
            {
                foreach (var beer in items)
                {

                    var originalBeer = context.Beers.SingleOrDefault(b => b.Id == beer.Id);
                    SetChanges(context, originalBeer, beer);

                    var originalAbv = context.ABVs.SingleOrDefault(a => a.Id == beer.ABV.Id);
                    if(originalAbv != null)
                        SetChanges(context, originalAbv, beer.ABV);

                    var originalSrm = context.SRMs.SingleOrDefault(s => s.Id == beer.SRM.Id);
                    if(originalSrm != null)
                        SetChanges(context, originalSrm, beer.SRM);

                    var originalIbu = context.IBUs.SingleOrDefault(i => i.Id == beer.IBU.Id);
                    if (originalIbu == null)
                        SetChanges(context,originalIbu,beer.IBU);

                    // Brewers
                    foreach (var userBeer in beer.Brewers)
                    {
                        var originalUserBeer = context.UserBeers.SingleOrDefault(u => u.Username.Equals(userBeer.Username) && u.BeerId == beer.Id);
                        if (originalUserBeer != null)
                        {
                            SetChanges(context, originalUserBeer, userBeer);
                        }
                        else
                        {
                            context.UserBeers.Add(userBeer);
                        }
                    }
                    // Brewery
                    foreach (var breweryBeer in beer.Breweries)
                    {
                        var originalBreweryBeer = context.BreweryBeers.SingleOrDefault(b => b.BreweryId == breweryBeer.BreweryId && b.BeerId == beer.Id);
                        if (originalBreweryBeer != null)
                        {
                            SetChanges(context, originalBreweryBeer, breweryBeer);
                        }
                        else
                        {
                            context.BreweryBeers.Add(breweryBeer);
                        }
                    }
                    // Recipe 
                    var originalRecipe = context.Recipes.SingleOrDefault(r => r.Id == beer.Recipe.Id);
                    if (originalRecipe == null)
                    {
                        context.Recipes.Add(beer.Recipe);
                    }
                    else
                    {
                        SetChanges(context, originalRecipe, beer.Recipe);
                        // Fermentation Step
                        foreach (var fermentationStep in beer.Recipe.FermentationSteps)
                        {
                            var originalFermentationStep = context.FermentationSteps.SingleOrDefault(s =>
                                s.StepNumber == fermentationStep.StepNumber &&
                                s.RecipeId == fermentationStep.RecipeId);
                            if (originalFermentationStep == null)
                            {
                                context.FermentationSteps.Add(fermentationStep);
                            }
                            else
                            {
                                SetChanges(context, originalFermentationStep, fermentationStep);
                                //Fermentable
                                foreach (var fermentable in fermentationStep.Fermentables)
                                {
                                    var originalFermentable = context.FermentationStepFermentables
                                        .SingleOrDefault(f => f.StepNumber == fermentable.StepNumber && f.FermentableId == fermentable.FermentableId && f.RecipeId == fermentable.RecipeId);
                                    if (originalFermentable != null)
                                    {
                                        SetChanges(context, originalFermentable, fermentable);
                                    }
                                    else
                                    {
                                        context.FermentationStepFermentables.Add(fermentable);
                                    }
                                }
                                //Hop
                                foreach (var hop in fermentationStep.Hops)
                                {
                                    var originalHop = context.FermentationStepHops
                                        .SingleOrDefault(h => h.StepNumber == hop.StepNumber && h.HopId == hop.HopId && h.RecipeId == hop.RecipeId);
                                    if (originalHop != null)
                                    {
                                        SetChanges(context, originalHop, hop);
                                    }
                                    else
                                    {
                                        context.FermentationStepHops.Add(hop);
                                    }
                                }
                                //Other
                                foreach (var other in fermentationStep.Others)
                                {
                                    var originalOther = context.FermentationStepOthers
                                        .SingleOrDefault(o => o.StepNumber == other.StepNumber && o.OtherId == other.OtherId && o.RecipeId == other.RecipeId);
                                    if (originalOther != null)
                                    {
                                        SetChanges(context, originalOther, other);
                                    }
                                    else
                                    {
                                        context.FermentationStepOthers.Add(other);
                                    }
                                }
                                //Yeast
                                foreach (var yeast in fermentationStep.Yeasts)
                                {
                                    var originalYeast = context.FermentationStepYeasts
                                        .SingleOrDefault(y => y.StepNumber == yeast.StepNumber && y.YeastId == yeast.YeastId && y.RecipeId == yeast.RecipeId);
                                    if (originalYeast != null)
                                    {
                                        SetChanges(context, originalYeast, yeast);
                                    }
                                    else
                                    {
                                        context.FermentationStepYeasts.Add(yeast);
                                    }
                                }
                            }
                        }

                        // Boil step
                        foreach (var boilStep in beer.Recipe.BoilSteps)
                        {
                            var originalBoilStep = context.BoilSteps.SingleOrDefault(s => s.RecipeId == boilStep.RecipeId && s.StepNumber == boilStep.StepNumber);
                            if (originalBoilStep == null)
                            {
                                context.BoilSteps.Add(boilStep);
                            }
                            else
                            {
                                SetChanges(context, originalBoilStep, boilStep);
                                //Fermentable
                                foreach (var fermentable in boilStep.Fermentables)
                                {
                                    var originalFermentable = context.BoilStepFermentables
                                        .SingleOrDefault(f =>
                                            f.StepNumber == fermentable.StepNumber &&
                                            f.FermentableId == fermentable.FermentableId &&
                                            f.RecipeId == boilStep.RecipeId);

                                    if (originalFermentable != null)
                                    {
                                        SetChanges(context, originalFermentable, fermentable);
                                    }
                                    else
                                    {
                                        context.BoilStepFermentables.Add(fermentable);
                                    }
                                }
                                //Hop
                                foreach (var hopStep in boilStep.Hops)
                                {
                                    var originalHopStep = context.BoilStepHops
                                        .SingleOrDefault(h => h.StepNumber == hopStep.StepNumber && h.HopId == hopStep.HopId && h.RecipeId == hopStep.RecipeId);
                                    if (originalHopStep != null)
                                    {
                                        SetChanges(context, originalHopStep, hopStep);
                                    }
                                    else
                                    {
                                        context.BoilStepHops.Add(hopStep);
                                    }
                                }
                                //Other
                                foreach (var otherStep in boilStep.Others)
                                {
                                    var originalOtherStep = context.BoilStepOthers
                                        .SingleOrDefault(o => o.StepNumber == otherStep.StepNumber && o.OtherId == otherStep.OtherId && o.RecipeId == otherStep.RecipeId);
                                    if (originalOtherStep != null)
                                    {
                                        SetChanges(context, originalOtherStep, otherStep);
                                    }
                                    else
                                    {
                                        context.BoilStepOthers.Add(otherStep);
                                    }

                                }
                            }
                        }
                        // Updates changes to the mash steps
                        foreach (var step in beer.Recipe.MashSteps)
                        {
                            var originalStep = context.MashSteps.SingleOrDefault(s => s.StepNumber == step.StepNumber && s.RecipeId == step.RecipeId);
                            if (originalStep == null)
                            {
                                context.MashSteps.Add(step);
                            }
                            else
                            {
                                SetChanges(context, originalStep, step);
                                //Fermentable
                                foreach (var fermentableStep in step.Fermentables)
                                {
                                    var originalFermentableStep = context.MashStepFermentables
                                        .SingleOrDefault(f => f.StepNumber == fermentableStep.StepNumber && f.FermentableId == fermentableStep.FermentableId && f.RecipeId == step.RecipeId);
                                    if (originalFermentableStep != null)
                                    {
                                        SetChanges(context, originalFermentableStep, fermentableStep);
                                    }
                                    else
                                    {
                                        context.MashStepFermentables.Add(fermentableStep);
                                    }
                                }
                                //Hop
                                foreach (var hopStep in step.Hops)
                                {
                                    var originalHopStep = context.MashStepHops
                                        .SingleOrDefault(h => h.StepNumber == hopStep.StepNumber && h.HopId == hopStep.HopId && h.RecipeId == step.RecipeId);
                                    if (originalHopStep != null)
                                    {
                                        SetChanges(context, originalHopStep, hopStep);
                                    }
                                    else
                                    {
                                        context.MashStepHops.Add(hopStep);
                                    }
                                }
                                //Other
                                foreach (var otherStep in step.Others)
                                {
                                    var originalOtherStep = context.MashStepOthers
                                        .SingleOrDefault(o => o.StepNumber == otherStep.StepNumber && o.OtherId == otherStep.OtherId && o.RecipeId == step.RecipeId);
                                    if (originalOtherStep != null)
                                    {
                                        SetChanges(context, originalOtherStep, otherStep);
                                    }
                                    else
                                    {
                                        context.MashStepOthers.Add(otherStep);
                                    }
                                }
                            }
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        private static void SetChanges(MicrobrewitContext context, object original, object updated)
        {
            foreach (PropertyInfo propertyInfo in original.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(updated, null) == null)
                {
                    propertyInfo.SetValue(updated, propertyInfo.GetValue(original, null), null);
                }
            }
            context.Entry(original).CurrentValues.SetValues(updated);
        }


        public async Task<IList<Beer>> GetLastAsync(int from, int size, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Beer> dbQuery = context.Set<Beer>();

                //Apply eager loading
                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<Beer>(navigationProperty));
                return await dbQuery.OrderByDescending(b => b.CreatedDate).Skip(from).Take(size).ToListAsync();
            }
        }




        public async Task<IList<Beer>> GetAllUserBeerAsync(string username, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {
                var dbQuery = from beer in context.Beers
                              join userBeers in context.UserBeers on beer.Id equals userBeers.BeerId
                              where userBeers.Username.Equals(username)
                              select beer;

                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<Beer>(navigationProperty));
                return await dbQuery.ToListAsync();
            }
        }

        public IList<Beer> GetAllUserBeer(string username, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {
                var dbQuery = from beer in context.Beers
                              join userBeers in context.UserBeers on beer.Id equals userBeers.BeerId
                              where userBeers.Username.Equals(username)
                              select beer;

                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<Beer>(navigationProperty));
                return dbQuery.ToList();
            }
        }

        public IList<Beer> GetAllBreweryBeers(int breweryId, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {
                var dbQuery = from beer in context.Beers
                              join breweryBeer in context.BreweryBeers on beer.Id equals breweryBeer.BeerId
                              where breweryBeer.BreweryId.Equals(breweryId)
                              select beer;

                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<Beer>(navigationProperty));
                return dbQuery.ToList();
            }
        }
    }
}
