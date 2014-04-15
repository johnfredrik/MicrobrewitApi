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
                    //var originalBeer = context.Beers
                    //    .Include("Recipe.MashSteps.Hops")
                    //    .Include("Recipe.MashSteps.Fermentables")
                    //    .Include("Recipe.MashSteps.Others")
                    //    .Include("Recipe.BoilSteps.Hops")
                    //    .Include("Recipe.BoilSteps.Fermentables")
                    //    .Include("Recipe.BoilSteps.Others")
                    //    .Include("Recipe.FermentationSteps.Hops")
                    //    .Include("Recipe.FermentationSteps.Fermentables")
                    //    .Include("Recipe.FermentationSteps.Others")
                    //    .Include("Recipe.FermentationSteps.Yeasts")
                    //    .Include("ABV")
                    //    .Include("IBU")
                    //    .Include("SRM")
                    //    .Include("Brewers")
                    //    .Include("Breweries")
                    //    .SingleOrDefault(b => b.Id == item.Id);

                   
                    var originalBeer = context.Beers.SingleOrDefault(b => b.Id == beer.Id);
                    SetChanges(context, originalBeer, beer);

                    var originalABV = context.ABVs.SingleOrDefault(a => a.Id == beer.ABV.Id);
                    SetChanges(context, originalABV, beer.ABV);

                    var originalSRM = context.SRMs.SingleOrDefault(s => s.Id == beer.SRM.Id);
                    SetChanges(context, originalSRM, beer.SRM);

                    var originalIBU = context.IBUs.SingleOrDefault(i => i.Id == beer.IBU.Id);
                        

                    var originalRecipe = context.Recipes.SingleOrDefault(r => r.Id == beer.Recipe.Id);
                    SetChanges(context, originalRecipe, beer.Recipe);
                    
                    // Updates changes to the fermentation steps
                    foreach (var step in beer.Recipe.FermentationSteps)
                    {
                        var originalStep = context.FermentationSteps.SingleOrDefault(s => s.Id == step.Id);
                        SetChanges(context, originalStep, step);
                        foreach (var fermentables in step.Fermentables)
                        {
                            var originalFermentables = context.FermentationStepFermentables
                                .SingleOrDefault(f => f.StepId == fermentables.StepId && f.FermentableId == fermentables.FermentableId);
                            SetChanges(context, originalFermentables, fermentables);
                        }
                        foreach (var hop in step.Hops)
                        {
                            var originalHops = context.FermentationStepHops
                                .SingleOrDefault(h => h.StepId == hop.StepId && h.HopId == hop.HopId);
                            SetChanges(context, originalHops, hop);
                        }
                        foreach (var other in step.Others)
                        {
                            var originalOther = context.FermentationStepOthers
                                .SingleOrDefault(o => o.StepId == other.StepId && o.OtherId == other.OtherId);
                            SetChanges(context, originalOther, other);
                        }
                        foreach (var yeast in step.Yeasts)
                        {
                            var originalYeast = context.FermentationStepYeasts
                                .SingleOrDefault(y => y.StepId == yeast.StepId && y.YeastId == yeast.YeastId);
                            SetChanges(context, originalYeast, yeast);
                        }
                    }

                    // Updates changes to the boil steps
                    foreach (var step in beer.Recipe.BoilSteps)
                    {
                        var originalStep = context.BoilSteps.SingleOrDefault(s => s.Id == step.Id);
                        SetChanges(context, originalStep, step);
                        
                        foreach (var fermentables in step.Fermentables)
                        {
                            var originalFermentables = context.BoilStepFermentables
                                .SingleOrDefault(f => f.StepId == fermentables.StepId && f.FermentableId == fermentables.FermentableId);
                            SetChanges(context, originalFermentables, fermentables);
                        }
                        foreach (var hop in step.Hops)
                        {
                            var originalHops = context.BoilStepHops
                                .SingleOrDefault(h => h.StepId == hop.StepId && h.HopId == hop.HopId);
                            SetChanges(context, originalHops, hop);
                        }
                        foreach (var other in step.Others)
                        {
                            var originalOther = context.BoilStepOthers
                                .SingleOrDefault(o => o.StepId == other.StepId && o.OtherId == other.OtherId);
                            SetChanges(context, originalOther, other);
                        }
                    }
                    // Updates changes to the mash steps
                    foreach (var step in beer.Recipe.MashSteps)
                    {
                        var originalStep = context.MashSteps.SingleOrDefault(s => s.Id == step.Id);
                        SetChanges(context, originalStep, step);
                        foreach (var fermentables in step.Fermentables)
                        {
                            var originalFermentables = context.MashStepFermentables
                                .SingleOrDefault(f => f.StepId == fermentables.StepId && f.FermentableId == fermentables.FermentableId);
                            SetChanges(context, originalFermentables, fermentables);
                        }
                        foreach (var hop in step.Hops)
                        {
                            var originalHop = context.MashStepHops
                                .SingleOrDefault(h => h.StepId == hop.StepId && h.HopId == hop.HopId);
                            SetChanges(context, originalHop, hop);
                        }
                        foreach (var other in step.Others)
                        {
                            var originalOther = context.MashStepOthers
                                .SingleOrDefault(o => o.StepId == other.StepId && o.OtherId == other.OtherId);
                            SetChanges(context, originalOther, other);
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

    }
}
