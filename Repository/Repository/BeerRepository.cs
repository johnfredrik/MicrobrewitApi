using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

                    var originalABV = context.ABVs.SingleOrDefault(a => a.Id == beer.ABV.Id);
                    SetChanges(context, originalABV, beer.ABV);

                    var originalSRM = context.SRMs.SingleOrDefault(s => s.Id == beer.SRM.Id);
                    SetChanges(context, originalSRM, beer.SRM);

                    var originalIBU = context.IBUs.SingleOrDefault(i => i.Id == beer.IBU.Id);

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

                    var originalRecipe = context.Recipes.SingleOrDefault(r => r.Id == beer.Recipe.Id);
                    if (originalRecipe == null)
                    {
                        context.Recipes.Add(beer.Recipe);
                    }
                    else
                    {

                        SetChanges(context, originalRecipe, beer.Recipe);

                        // Updates changes to the fermentation steps
                        foreach (var step in beer.Recipe.FermentationSteps)
                        {
                            var originalStep = context.FermentationSteps.SingleOrDefault(s => s.StepNumber == step.StepNumber && s.RecipeId == step.RecipeId);
                            if (originalStep == null)
                            {
                                context.FermentationSteps.Add(step);
                            }
                            else
                            {
                                SetChanges(context, originalStep, step);

                                foreach (var fermentableStep in step.Fermentables)
                                {
                                    var originalFermentableStep = context.FermentationStepFermentables
                                        .SingleOrDefault(f => f.StepNumber == fermentableStep.StepNumber && f.FermentableId == fermentableStep.FermentableId && f.RecipeId == fermentableStep.RecipeId);
                                    if (originalFermentableStep != null)
                                    {
                                        SetChanges(context, originalFermentableStep, fermentableStep);
                                    }
                                    else
                                    {
                                        context.FermentationStepFermentables.Add(fermentableStep);
                                    }
                                }
                                foreach (var hopStep in step.Hops)
                                {
                                    var originalHopStep = context.FermentationStepHops
                                        .SingleOrDefault(h => h.StepNumber == hopStep.StepNumber && h.HopId == hopStep.HopId && h.RecipeId == hopStep.RecipeId);
                                    if (originalHopStep != null)
                                    {
                                        SetChanges(context, originalHopStep, hopStep);
                                    }
                                    else
                                    {
                                        context.FermentationStepHops.Add(hopStep);
                                    }
                                }
                                foreach (var otherStep in step.Others)
                                {
                                    var originalOtherStep = context.FermentationStepOthers
                                        .SingleOrDefault(o => o.StepNumber == otherStep.StepNumber && o.OtherId == otherStep.OtherId && o.RecipeId == otherStep.RecipeId);
                                    if (originalOtherStep != null)
                                    {
                                        SetChanges(context, originalOtherStep, otherStep);
                                    }
                                    else
                                    {
                                        context.FermentationStepOthers.Add(otherStep);
                                    }
                                }
                                foreach (var yeastStep in step.Yeasts)
                                {
                                    var originalYeastStep = context.FermentationStepYeasts
                                        .SingleOrDefault(y => y.StepNumber == yeastStep.StepNumber && y.YeastId == yeastStep.YeastId && y.RecipeId == yeastStep.RecipeId);
                                    if (originalYeastStep != null)
                                    {
                                        SetChanges(context, originalYeastStep, yeastStep);
                                    }
                                    else
                                    {
                                        context.FermentationStepYeasts.Add(yeastStep);
                                    }
                                }
                            }
                        }

                        // Updates changes to the boil steps
                        foreach (var step in beer.Recipe.BoilSteps)
                        {
                            var originalStep = context.BoilSteps.SingleOrDefault(s => s.RecipeId == step.RecipeId && s.StepNumber == step.StepNumber);
                            if (originalStep == null)
                            {
                                context.BoilSteps.Add(step);
                            }
                            else
                            {
                                SetChanges(context, originalStep, step);

                                foreach (var fermentableStep in step.Fermentables)
                                {
                                    var originalFermentableStep = context.BoilStepFermentables
                                        .SingleOrDefault(f => f.StepNumber == fermentableStep.StepNumber && f.FermentableId == fermentableStep.FermentableId);
                                    if (originalFermentableStep != null)
                                    {
                                        SetChanges(context, originalFermentableStep, fermentableStep);
                                    }
                                    else
                                    {
                                        context.BoilStepFermentables.Add(fermentableStep);
                                    }
                                }
                                foreach (var hopStep in step.Hops)
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
                                foreach (var otherStep in step.Others)
                                {
                                    var originalOtherStep = context.BoilStepOthers
                                        .SingleOrDefault(o => o.StepNumber == otherStep.StepNumber && o.OtherId == otherStep.OtherId);
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
                                foreach (var fermentableStep in step.Fermentables)
                                {
                                    var originalFermentableStep = context.MashStepFermentables
                                        .SingleOrDefault(f => f.StepNumber == fermentableStep.StepNumber && f.FermentableId == fermentableStep.FermentableId);
                                    if (originalFermentableStep != null)
                                    {
                                        SetChanges(context, originalFermentableStep, fermentableStep);
                                    }
                                    else
                                    {
                                        context.MashStepFermentables.Add(fermentableStep);
                                    }
                                }
                                foreach (var hopStep in step.Hops)
                                {
                                    var originalHopStep = context.MashStepHops
                                        .SingleOrDefault(h => h.StepNumber == hopStep.StepNumber && h.HopId == hopStep.HopId);
                                    if (originalHopStep != null)
                                    {
                                        SetChanges(context, originalHopStep, hopStep);
                                    }
                                    else
                                    {
                                        context.MashStepHops.Add(hopStep);
                                    }
                                }
                                foreach (var otherStep in step.Others)
                                {
                                    var originalOtherStep = context.MashStepOthers
                                        .SingleOrDefault(o => o.StepNumber == otherStep.StepNumber && o.OtherId == otherStep.OtherId);
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

                //base.Update(items);

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
                foreach (string navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include<Beer>(navigationProperty);
                }
                return await dbQuery.OrderByDescending(b => b.CreatedDate).Skip(from).Take(size).ToListAsync();
            }
        }


        public async Task<IList<Beer>> GetAllUserBeer(string username, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {
                var dbQuery = from beer in context.Beers
                             join userBeers in context.UserBeers on beer.Id equals userBeers.BeerId
                             where userBeers.Username.Equals(username)
                             select beer;

                foreach (string navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include<Beer>(navigationProperty);
                }
                return await dbQuery.ToListAsync();
            }
        }
    }
}
