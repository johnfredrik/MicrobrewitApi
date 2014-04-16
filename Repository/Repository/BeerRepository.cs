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
                            var originalStep = context.FermentationSteps.SingleOrDefault(s => s.Id == step.Id);
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
                                        .SingleOrDefault(f => f.StepId == fermentableStep.StepId && f.FermentableId == fermentableStep.FermentableId);
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
                                        .SingleOrDefault(h => h.StepId == hopStep.StepId && h.HopId == hopStep.HopId);
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
                                        .SingleOrDefault(o => o.StepId == otherStep.StepId && o.OtherId == otherStep.OtherId);
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
                                        .SingleOrDefault(y => y.StepId == yeastStep.StepId && y.YeastId == yeastStep.YeastId);
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
                            var originalStep = context.BoilSteps.SingleOrDefault(s => s.Id == step.Id);
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
                                        .SingleOrDefault(f => f.StepId == fermentableStep.StepId && f.FermentableId == fermentableStep.FermentableId);
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
                                        .SingleOrDefault(h => h.StepId == hopStep.StepId && h.HopId == hopStep.HopId);
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
                                        .SingleOrDefault(o => o.StepId == otherStep.StepId && o.OtherId == otherStep.OtherId);
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
                            var originalStep = context.MashSteps.SingleOrDefault(s => s.Id == step.Id);
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
                                        .SingleOrDefault(f => f.StepId == fermentableStep.StepId && f.FermentableId == fermentableStep.FermentableId);
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
                                        .SingleOrDefault(h => h.StepId == hopStep.StepId && h.HopId == hopStep.HopId);
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
                                        .SingleOrDefault(o => o.StepId == otherStep.StepId && o.OtherId == otherStep.OtherId);
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

    }
}
