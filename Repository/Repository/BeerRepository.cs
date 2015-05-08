using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Win32.SafeHandles;

namespace Microbrewit.Repository
{
    public class BeerRepository : IBeerRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IList<Beer> GetAll(params string[] navigationProperties)
        {
            List<Beer> list;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Beer> dbQuery = context.Set<Beer>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<Beer>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<Beer>();
            }
            return list;
        }

        public Beer GetSingle(int id, params string[] navigationProperties)
        {
            Beer item = null;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Beer> dbQuery = context.Set<Beer>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<Beer>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(s => s.BeerId == id); //Apply where clause
            }
            return item;
        }

        public void Add(Beer beer)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(beer).State = EntityState.Added;
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            Log.DebugFormat("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
        }

        public void Update(Beer beer)
        {
            using (var context = new MicrobrewitContext())
            {
                var originalBeer = context.Beers.SingleOrDefault(b => b.BeerId == beer.BeerId);
                SetChanges(context, originalBeer, beer);

                var originalAbv = context.ABVs.SingleOrDefault(a => a.Id == beer.ABV.Id);
                if (originalAbv != null)
                    SetChanges(context, originalAbv, beer.ABV);

                var originalSrm = context.SRMs.SingleOrDefault(s => s.Id == beer.SRM.Id);
                if (originalSrm != null)
                    SetChanges(context, originalSrm, beer.SRM);

                var originalIbu = context.IBUs.SingleOrDefault(i => i.Id == beer.IBU.Id);
                if (originalIbu == null)
                    SetChanges(context, originalIbu, beer.IBU);

                // Brewers
                foreach (var userBeer in beer.Brewers)
                {
                    var originalUserBeer = context.UserBeers.SingleOrDefault(u => u.Username.Equals(userBeer.Username) && u.BeerId == beer.BeerId);
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
                    var originalBreweryBeer = context.BreweryBeers.SingleOrDefault(b => b.BreweryId == breweryBeer.BreweryId && b.BeerId == beer.BeerId);
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
                var originalRecipe = context.Recipes.SingleOrDefault(r => r.RecipeId == beer.Recipe.RecipeId);
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

                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {

                    throw;

                }
            }
        }

        public void Remove(Beer beer)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(beer).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public async Task<IList<Beer>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Beer> dbQuery = context.Set<Beer>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include<Beer>(navigationProperty);
                }
                return await dbQuery.ToListAsync();
            }
        }

        public async Task<Beer> GetSingleAsync(int id, params string[] navigtionProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Beer> dbQuery = context.Set<Beer>();

                //Apply eager loading
                dbQuery = navigtionProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<Beer>(navigationProperty));

                return await dbQuery.SingleOrDefaultAsync(s => s.BeerId == id);
            }
        }

        public async Task AddAsync(Beer beer)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(beer).State = EntityState.Added;
                await context.SaveChangesAsync();

            }
        }

        public async Task<int> UpdateAsync(Beer newBeer)
        {
            using (var context = new MicrobrewitContext())
            {
                var dbBeer = context.Beers
                    .Include(b => b.ABV)
                    .Include(b => b.SRM)
                    .Include(b => b.IBU)
                    .Include(b => b.Brewers)
                    .Include(b => b.Breweries)
                    .SingleOrDefault(b => b.BeerId == newBeer.BeerId);

                newBeer.BeerStyle = null;
                newBeer.CreatedDate = dbBeer.CreatedDate;
                newBeer.UpdatedDate = DateTime.Now;
                context.Entry(dbBeer).CurrentValues.SetValues(newBeer);

                if (newBeer.ABV != null)
                {
                    var dbAbv = context.ABVs.SingleOrDefault(a => a.Id == newBeer.ABV.Id);
                    if (dbAbv != null)
                    {
                        context.Entry(dbBeer.ABV).CurrentValues.SetValues(newBeer.ABV);
                    }
                    else
                    {
                        dbBeer.ABV = newBeer.ABV;
                    }
                }
                if (newBeer.SRM != null)
                {
                    var dbSrm = context.SRMs.SingleOrDefault(s => s.Id == newBeer.SRM.Id);
                    if (dbSrm != null)
                    {
                        context.Entry(dbBeer.SRM).CurrentValues.SetValues(newBeer.SRM);
                    }
                    else
                    {
                        dbBeer.SRM = newBeer.SRM;
                    }
                }

                if (newBeer.IBU != null)
                {
                    var dbIbu = context.IBUs.SingleOrDefault(i => i.Id == newBeer.IBU.Id);
                    if (dbIbu != null)
                    {
                        context.Entry(dbBeer.IBU).CurrentValues.SetValues(newBeer.IBU);
                    }
                    else
                    {
                        dbBeer.IBU = newBeer.IBU;
                    }
                }

                // Brewers
                foreach (var brewers in dbBeer.Brewers.ToList())
                {
                    if (newBeer.Brewers.All(b => b.Username != brewers.Username))
                        context.UserBeers.Remove(brewers);
                }

                foreach (var newUserBeer in newBeer.Brewers)
                {
                    var dbUserBeer = context.UserBeers.SingleOrDefault(u => u.Username.Equals(newUserBeer.Username) && u.BeerId == newBeer.BeerId);
                    if (dbUserBeer != null)
                    {
                        context.Entry(dbUserBeer).CurrentValues.SetValues(newUserBeer);
                    }
                    else
                    {
                        context.UserBeers.Add(newUserBeer);
                    }
                }
                // Brewery
                foreach (var dbBreweryBeers in dbBeer.Breweries.ToList())
                {
                    if (newBeer.Breweries.All(b => b.BreweryId != dbBreweryBeers.BreweryId))
                        context.BreweryBeers.Remove(dbBreweryBeers);
                }

                foreach (var newBreweryBeer in newBeer.Breweries)
                {
                    var dbBreweryBeer = context.BreweryBeers.SingleOrDefault(b => b.BreweryId == newBreweryBeer.BreweryId && b.BeerId == newBeer.BeerId);
                    if (dbBreweryBeer != null)
                    {
                        context.Entry(dbBreweryBeer).CurrentValues.SetValues(newBreweryBeer);
                    }
                    else
                    {
                        context.BreweryBeers.Add(newBreweryBeer);
                    }
                }
                // Recipe 
                if (newBeer.Recipe != null)
                {
                    var dbRecipe = context.Recipes
                        .Include(r => r.FermentationSteps)
                        .Include(r => r.MashSteps)
                        .Include(r => r.BoilSteps)
                        //   .Include(r => r.SpargeStep)
                        .SingleOrDefault(r => r.RecipeId == newBeer.Recipe.RecipeId);
                    if (dbRecipe != null)
                    {
                        context.Entry(dbRecipe).CurrentValues.SetValues(newBeer.Recipe);
                        //Sparge step
                        //if (newBeer.Recipe.SpargeStep != null)
                        //{
                        //    var newSpargeStep = newBeer.Recipe.SpargeStep;
                        //    var dbSpargeStep = context.SpargeSteps.SingleOrDefault(i => i.StepNumber == newSpargeStep.StepNumber && i.RecipeId == newSpargeStep.RecipeId);
                        //    if (dbSpargeStep != null)
                        //    {
                        //        context.Entry(dbSpargeStep).CurrentValues.SetValues(newSpargeStep);
                        //    }
                        //    else
                        //    {
                        //        dbRecipe.SpargeStep = newSpargeStep;
                        //    }
                        //}

                        // Fermentation Step
                        foreach (var dbFermentationStep in dbRecipe.FermentationSteps)
                        {
                            if (
                                newBeer.Recipe.FermentationSteps.All(
                                    f => f.StepNumber != dbFermentationStep.StepNumber))
                                context.FermentationSteps.Remove(dbFermentationStep);
                        }
                        foreach (var newFermentationStep in newBeer.Recipe.FermentationSteps)
                        {
                            var dbFermentationStep = context.FermentationSteps.SingleOrDefault(s =>
                                s.StepNumber == newFermentationStep.StepNumber &&
                                s.RecipeId == newFermentationStep.RecipeId);
                            if (dbFermentationStep != null)
                            {
                                context.Entry(dbFermentationStep).CurrentValues.SetValues(newFermentationStep);

                                //Fermentable
                                if (dbFermentationStep.Fermentables != null)
                                {
                                    foreach (var dbFermentable in dbFermentationStep.Fermentables)
                                    {
                                        if (
                                            newFermentationStep.Fermentables.All(
                                                f => f.FermentableId != dbFermentable.FermentableId))
                                            context.FermentationStepFermentables.Remove(dbFermentable);
                                    }
                                }

                                if (newFermentationStep.Fermentables != null)
                                {
                                    foreach (var newFermentable in newFermentationStep.Fermentables)
                                    {
                                        var dbFermentable = context.FermentationStepFermentables
                                            .SingleOrDefault(
                                                f =>
                                                    f.StepNumber == newFermentable.StepNumber &&
                                                    f.FermentableId == newFermentable.FermentableId &&
                                                    f.RecipeId == newFermentable.RecipeId);
                                        if (dbFermentable != null)
                                        {
                                            context.Entry(dbFermentable).CurrentValues.SetValues(newFermentable);
                                        }
                                        else
                                        {
                                            context.FermentationStepFermentables.Add(newFermentable);
                                        }
                                    }
                                }
                                //Hop
                                if (dbFermentationStep.Hops != null)
                                {
                                    foreach (var dbFermentationStepHop in dbFermentationStep.Hops)
                                    {
                                        if (newFermentationStep.Hops.All(h => h.HopId != dbFermentationStepHop.HopId))
                                            context.FermentationStepHops.Remove(dbFermentationStepHop);
                                    }
                                }
                                if (newFermentationStep.Hops != null)
                                {
                                    foreach (var newHop in newFermentationStep.Hops)
                                    {
                                        var dbHop = context.FermentationStepHops
                                            .SingleOrDefault(
                                                h =>
                                                    h.StepNumber == newHop.StepNumber && h.HopId == newHop.HopId &&
                                                    h.RecipeId == newHop.RecipeId);
                                        if (dbHop != null)
                                        {
                                            context.Entry(dbHop).CurrentValues.SetValues(newHop);
                                        }
                                        else
                                        {
                                            context.FermentationStepHops.Add(newHop);
                                        }
                                    }
                                }
                                //Other
                                if (dbFermentationStep.Others != null)
                                {
                                    foreach (var dbFermentationStepOther in dbFermentationStep.Others)
                                    {
                                        if (
                                            newFermentationStep.Others.All(
                                                o => o.OtherId != dbFermentationStepOther.OtherId))
                                            context.FermentationStepOthers.Remove(dbFermentationStepOther);
                                    }
                                }
                                if (newFermentationStep.Others != null)
                                {
                                    foreach (var newOther in newFermentationStep.Others)
                                    {
                                        var dbOther = context.FermentationStepOthers
                                            .SingleOrDefault(
                                                o =>
                                                    o.StepNumber == newOther.StepNumber &&
                                                    o.OtherId == newOther.OtherId &&
                                                    o.RecipeId == newOther.RecipeId);
                                        if (dbOther != null)
                                        {
                                            context.Entry(dbOther).CurrentValues.SetValues(newOther);
                                        }
                                        else
                                        {
                                            context.FermentationStepOthers.Add(newOther);
                                        }
                                    }
                                }
                                //Yeast
                                if (dbFermentationStep.Yeasts != null)
                                {
                                    foreach (var dbFermentationStepYeast in dbFermentationStep.Yeasts)
                                    {
                                        if (
                                            newFermentationStep.Yeasts.All(
                                                y => y.YeastId != dbFermentationStepYeast.YeastId))
                                            context.FermentationStepYeasts.Remove(dbFermentationStepYeast);
                                    }
                                }
                                if (newFermentationStep.Yeasts != null)
                                {
                                    foreach (var newFermentationStepYeast in newFermentationStep.Yeasts)
                                    {
                                        var dbYeast = context.FermentationStepYeasts
                                            .SingleOrDefault(
                                                y =>
                                                    y.StepNumber == newFermentationStepYeast.StepNumber &&
                                                    y.YeastId == newFermentationStepYeast.YeastId &&
                                                    y.RecipeId == newFermentationStepYeast.RecipeId);
                                        if (dbYeast != null)
                                        {
                                            context.Entry(dbYeast).CurrentValues.SetValues(newFermentationStepYeast);
                                        }
                                        else
                                        {
                                            context.FermentationStepYeasts.Add(newFermentationStepYeast);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                dbRecipe.FermentationSteps.Add(newFermentationStep);
                            }
                        }
                        // Boil step
                        foreach (var dbBoilStep in dbRecipe.BoilSteps)
                        {
                            if (newBeer.Recipe.BoilSteps.All(f => f.StepNumber != dbBoilStep.StepNumber))
                                context.BoilSteps.Remove(dbBoilStep);
                        }
                        foreach (var newBoilStep in newBeer.Recipe.BoilSteps)
                        {
                            var dbBoilStep = context.BoilSteps.SingleOrDefault(s =>
                                s.StepNumber == newBoilStep.StepNumber &&
                                s.RecipeId == newBoilStep.RecipeId);
                            if (dbBoilStep != null)
                            {
                                context.Entry(dbBoilStep).CurrentValues.SetValues(newBoilStep);

                                //Fermentable
                                if (dbBoilStep.Fermentables != null)
                                {
                                    foreach (var dbFermentable in dbBoilStep.Fermentables)
                                    {
                                        if (
                                            newBoilStep.Fermentables.All(
                                                f => f.FermentableId != dbFermentable.FermentableId))
                                            context.BoilStepFermentables.Remove(dbFermentable);
                                    }
                                }
                                if (newBoilStep.Fermentables != null)
                                {
                                    foreach (var newFermentable in newBoilStep.Fermentables)
                                    {
                                        var dbFermentable = context.BoilStepFermentables
                                            .SingleOrDefault(
                                                f =>
                                                    f.StepNumber == newFermentable.StepNumber &&
                                                    f.FermentableId == newFermentable.FermentableId &&
                                                    f.RecipeId == newFermentable.RecipeId);
                                        if (dbFermentable != null)
                                        {
                                            context.Entry(dbFermentable).CurrentValues.SetValues(newFermentable);
                                        }
                                        else
                                        {
                                            context.BoilStepFermentables.Add(newFermentable);
                                        }
                                    }
                                }
                                //Hop
                                if (dbBoilStep.Hops != null)
                                {
                                    foreach (var dbBoilStepHop in dbBoilStep.Hops)
                                    {
                                        if (newBoilStep.Hops.All(h => h.HopId != dbBoilStepHop.HopId))
                                            context.BoilStepHops.Remove(dbBoilStepHop);
                                    }
                                }
                                if (newBoilStep.Hops != null)
                                {
                                    foreach (var newHop in newBoilStep.Hops)
                                    {
                                        var dbHop = context.BoilStepHops
                                            .SingleOrDefault(
                                                h =>
                                                    h.StepNumber == newHop.StepNumber && h.HopId == newHop.HopId &&
                                                    h.RecipeId == newHop.RecipeId);
                                        if (dbHop != null)
                                        {
                                            context.Entry(dbHop).CurrentValues.SetValues(newHop);
                                        }
                                        else
                                        {
                                            context.BoilStepHops.Add(newHop);
                                        }
                                    }
                                }
                                //Other
                                if (dbBoilStep.Others != null)
                                {
                                    foreach (var dbBoilStepOther in dbBoilStep.Others)
                                    {
                                        if (
                                            newBoilStep.Others.All(
                                                o => o.OtherId != dbBoilStepOther.OtherId))
                                            context.BoilStepOthers.Remove(dbBoilStepOther);
                                    }
                                }
                                if (newBoilStep.Others != null)
                                {
                                    foreach (var newOther in newBoilStep.Others)
                                    {
                                        var dbOther = context.BoilStepOthers
                                            .SingleOrDefault(
                                                o =>
                                                    o.StepNumber == newOther.StepNumber &&
                                                    o.OtherId == newOther.OtherId &&
                                                    o.RecipeId == newOther.RecipeId);
                                        if (dbOther != null)
                                        {
                                            context.Entry(dbOther).CurrentValues.SetValues(newOther);
                                        }
                                        else
                                        {
                                            context.BoilStepOthers.Add(newOther);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                dbRecipe.BoilSteps.Add(newBoilStep);
                            }
                        }
                        // Updates changes to the mash steps
                        foreach (var dbMashStep in dbRecipe.MashSteps)
                        {
                            if (
                                newBeer.Recipe.MashSteps.All(
                                    f => f.StepNumber != dbMashStep.StepNumber))
                                context.MashSteps.Remove(dbMashStep);
                        }
                        foreach (var newMashStep in newBeer.Recipe.MashSteps)
                        {
                            var dbMashStep = context.MashSteps.SingleOrDefault(s =>
                                s.StepNumber == newMashStep.StepNumber &&
                                s.RecipeId == newMashStep.RecipeId);
                            if (dbMashStep != null)
                            {
                                context.Entry(dbMashStep).CurrentValues.SetValues(newMashStep);

                                //Fermentable
                                if (dbMashStep.Fermentables != null)
                                {
                                    foreach (var dbFermentable in dbMashStep.Fermentables)
                                    {
                                        if (
                                            newMashStep.Fermentables.All(
                                                f => f.FermentableId != dbFermentable.FermentableId))
                                            context.MashStepFermentables.Remove(dbFermentable);
                                    }
                                }
                                if (newMashStep.Fermentables != null)
                                {
                                    foreach (var newFermentable in newMashStep.Fermentables)
                                    {
                                        var dbFermentable = context.MashStepFermentables
                                            .SingleOrDefault(
                                                f =>
                                                    f.StepNumber == newFermentable.StepNumber &&
                                                    f.FermentableId == newFermentable.FermentableId &&
                                                    f.RecipeId == newFermentable.RecipeId);
                                        if (dbFermentable != null)
                                        {
                                            context.Entry(dbFermentable).CurrentValues.SetValues(newFermentable);
                                        }
                                        else
                                        {
                                            context.MashStepFermentables.Add(newFermentable);
                                        }
                                    }
                                }
                                //Hop
                                if (dbMashStep.Hops != null)
                                {
                                    foreach (var dbMashStepHop in dbMashStep.Hops)
                                    {
                                        if (newMashStep.Hops.All(h => h.HopId != dbMashStepHop.HopId))
                                            context.MashStepHops.Remove(dbMashStepHop);
                                    }
                                }
                                if (newMashStep.Hops != null)
                                {
                                    foreach (var newHop in newMashStep.Hops)
                                    {
                                        var dbHop = context.MashStepHops
                                            .SingleOrDefault(
                                                h =>
                                                    h.StepNumber == newHop.StepNumber && h.HopId == newHop.HopId &&
                                                    h.RecipeId == newHop.RecipeId);
                                        if (dbHop != null)
                                        {
                                            context.Entry(dbHop).CurrentValues.SetValues(newHop);
                                        }
                                        else
                                        {
                                            context.MashStepHops.Add(newHop);
                                        }
                                    }
                                }
                                //Other
                                if (dbMashStep.Others != null)
                                {
                                    foreach (var dbMashStepOther in dbMashStep.Others)
                                    {
                                        if (
                                            newMashStep.Others.All(
                                                o => o.OtherId != dbMashStepOther.OtherId))
                                            context.MashStepOthers.Remove(dbMashStepOther);
                                    }
                                }
                                if (newMashStep.Others != null)
                                {
                                    foreach (var newOther in newMashStep.Others)
                                    {
                                        var dbOther = context.MashStepOthers
                                            .SingleOrDefault(
                                                o =>
                                                    o.StepNumber == newOther.StepNumber &&
                                                    o.OtherId == newOther.OtherId &&
                                                    o.RecipeId == newOther.RecipeId);
                                        if (dbOther != null)
                                        {
                                            context.Entry(dbOther).CurrentValues.SetValues(newOther);
                                        }
                                        else
                                        {
                                            context.MashStepOthers.Add(newOther);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                dbRecipe.MashSteps.Add(newMashStep);
                            }
                        }
                    }
                    else
                    {
                        context.Recipes.Add(newBeer.Recipe);
                    }
                }
                try
                {
                    return await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public async Task RemoveAsync(Beer beer)
        {
            using (var context = new MicrobrewitContext())
            {

                context.Entry(beer).State = EntityState.Deleted;
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Log.Debug(e);
                    throw;
                }
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
                              join userBeers in context.UserBeers on beer.BeerId equals userBeers.BeerId
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
                              join userBeers in context.UserBeers on beer.BeerId equals userBeers.BeerId
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
                              join breweryBeer in context.BreweryBeers on beer.BeerId equals breweryBeer.BeerId
                              where breweryBeer.BreweryId.Equals(breweryId)
                              select beer;

                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<Beer>(navigationProperty));
                return dbQuery.ToList();
            }
        }
    }
}
