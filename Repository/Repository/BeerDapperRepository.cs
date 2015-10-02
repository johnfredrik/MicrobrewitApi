using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Dapper;
using log4net;
using log4net.Repository.Hierarchy;
using Microbrewit.Model;
using Microbrewit.Model.BeerXml;
using Microbrewit.Model.Migrations;
using Fermentable = Microbrewit.Model.Fermentable;
using Hop = Microbrewit.Model.Hop;
using MashStep = Microbrewit.Model.MashStep;
using Recipe = Microbrewit.Model.Recipe;
using SpargeStep = Microbrewit.Model.SpargeStep;
using Yeast = Microbrewit.Model.Yeast;

namespace Microbrewit.Repository
{
    public class BeerDapperRepository : IBeerRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<Beer> GetAll(int from, int size, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var beers = context.Query<Beer, BeerStyle, Recipe, SRM, ABV, IBU, Beer>(
                    //"SELECT Top " + size + " * FROM Beers b " +
                    "SELECT Top " + size +
                    " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY BeerId) as ROW_NUM FROM Beers) AS b " +
                    "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                    "LEFT JOIN Recipes r ON r.RecipeId = b.BeerId " +
                    "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                    "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                    "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                    "WHERE ROW_NUM >= @From"
                    , (beer, beerStyle, recipe, srm, abv, ibu) =>
                    {
                        if (beerStyle != null)
                            beer.BeerStyle = beerStyle;
                        if (recipe != null)
                            beer.Recipe = recipe;
                        if (srm != null)
                            beer.SRM = srm;
                        if (abv != null)
                            beer.ABV = abv;
                        if (ibu != null)
                            beer.IBU = ibu;
                        return beer;
                    }, new {From = from, To = size},
                    splitOn: "BeerStyleId,RecipeId,SrmId,AbvId,IbuId"
                    ).ToList();

                foreach (var beer in beers)
                {
                    GetForkOf(context, beer);
                    GetForks(context, beer);
                    GetBreweries(context, beer);
                    GetBrewers(context, beer);
                    if (beer.Recipe != null)
                    {
                        GetRecipeSteps(context, beer.Recipe);
                    }
                }

                return beers;
            }
        }

        public Beer GetSingle(int id, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var beers = context.Query<Beer, BeerStyle, Recipe, SRM, ABV, IBU, Beer>(
                    "SELECT * FROM Beers b " +
                    "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                    "LEFT JOIN Recipes r ON r.RecipeId = b.BeerId " +
                    "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                    "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                    "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                    "WHERE BeerId = @BeerId;"
                    , (b, beerStyle, recipe, srm, abv, ibu) =>
                    {
                        if (beerStyle != null)
                            b.BeerStyle = beerStyle;
                        if (recipe != null)
                            b.Recipe = recipe;
                        if (srm != null)
                            b.SRM = srm;
                        if (abv != null)
                            b.ABV = abv;
                        if (ibu != null)
                            b.IBU = ibu;
                        return b;
                    },
                    new { BeerId = id },
                    splitOn: "BeerStyleId,RecipeId,SrmId,AbvId,IbuId"
                    );
                var beer = beers.SingleOrDefault();
                if (beer == null) return null;
                GetForkOf(context, beer);
                GetForks(context, beer);
                GetBreweries(context, beer);
                GetBrewers(context, beer);
                if (beer.Recipe != null)
                {
                    GetRecipeSteps(context, beer.Recipe);
                }
                return beer;
            }
        }

        public void Add(Beer beer)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        beer.CreatedDate = DateTime.Now;
                        beer.UpdatedDate = DateTime.Now;
                        var beerId = context.Query<int>(
                            "INSERT Beers(Name,BeerStyleId,CreatedDate,UpdatedDate,ForkeOfId) " +
                            "VALUES(@Name,@BeerStyleId,@CreatedDate,@UpdatedDate,@ForkeOfId);" +
                            "SELECT CAST(Scope_Identity() as int);", beer, transaction);
                        beer.BeerId = beerId.SingleOrDefault();

                        beer.SRM.SrmId = beer.BeerId;
                        context.Execute("INSERT SRMs(SrmId,Standard,Mosher,Daniels,Morey) VALUES(@SrmId,@Standard,@Mosher,@Daniels,@Morey);",
                            new { beer.SRM.SrmId, beer.SRM.Standard, beer.SRM.Mosher, beer.SRM.Daniels, beer.SRM.Morey }, transaction);

                        beer.ABV.AbvId = beer.BeerId;
                        context.Execute("INSERT ABVs(AbvId,Standard,Miller,Advanced,AdvancedAlternative,Simple,AlternativeSimple) " +
                                        "VALUES(@AbvId,@Standard,@Miller,@Advanced,@AdvancedAlternative,@Simple,@AlternativeSimple);",
                            new { AbvId = beer.BeerId, beer.ABV.Standard, beer.ABV.Miller, beer.ABV.Advanced, beer.ABV.AdvancedAlternative, beer.ABV.Simple, beer.ABV.AlternativeSimple }, transaction);

                        beer.IBU.IbuId = beer.BeerId;
                        context.Execute("INSERT IBUs(IbuId,Standard,Tinseth,Rager) " +
                                        "VALUES(@IbuId,@Standard,@Tinseth,@Rager);",
                            new { IbuId = beer.BeerId, beer.IBU.Standard, beer.IBU.Tinseth, beer.IBU.Rager }, transaction);

                        if (beer.Recipe != null)
                        {
                            beer.Recipe.RecipeId = beer.BeerId;
                            AddRecipe(beer.Recipe, context, transaction);
                        }
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        transaction.Commit();
                        throw;
                    }
                }
            }
        }

        public void Update(Beer beer)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        context.Execute(
                            "UPDATE Beers set Name = @Name, BeerStyleId = @BeerStyleId, UpdatedDate = @UpdatedDate, ForkeOfId = @ForkeOfId WHERE BeerId = @BeerId;",
                            beer, transaction);

                        context.Execute("UPDATE SRMs set Standard = @Standard, Mosher = @Mosher, Daniels = @Daniels, Morey = @Morey " +
                                        "WHERE SrmId = @SrmId;",
                            new { SrmId = beer.BeerId, beer.SRM.Standard, beer.SRM.Mosher, beer.SRM.Daniels, beer.SRM.Morey }, transaction);

                        context.Execute("UPDATE ABVs set Standard = @Standard, Miller = @Miller, Advanced = @Advanced, AdvancedAlternative = @AdvancedAlternative, Simple = @Simple, AlternativeSimple = @AlternativeSimple " +
                                        "WHERE AbvId = @AbvId;",
                           new { AbvId = beer.BeerId, beer.ABV.Standard, beer.ABV.Miller, beer.ABV.Advanced, beer.ABV.AdvancedAlternative, beer.ABV.Simple, beer.ABV.AlternativeSimple }, transaction);

                        context.Execute("UPDATE IBUs set Standard = @Standard,Tinseth =@Tinseth,Rager =@Rager WHERE IbuId = @IbuId;",
                            new { IbuId = beer.BeerId, beer.IBU.Standard, beer.IBU.Tinseth, beer.IBU.Rager }, transaction);

                        beer.Recipe.RecipeId = beer.BeerId;
                        UpdateRecipe(context, transaction, beer.Recipe);
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        Log.Error(exception.ToString());
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Remove(Beer beer)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Beer>> GetAllAsync(int from, int size, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var beers = await context.QueryAsync<Beer, BeerStyle, Recipe, SRM, ABV, IBU, Beer>(
                    //"SELECT Top " + size + " * FROM Beers b " +
                    "SELECT Top " + size + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY BeerId) as ROW_NUM FROM Beers) AS b " +
                    "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                    "LEFT JOIN Recipes r ON r.RecipeId = b.BeerId " +
                    "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                    "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                    "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                    "WHERE ROW_NUM >= @From"
                    , (beer, beerStyle, recipe, srm, abv, ibu) =>
                    {
                        if (beerStyle != null)
                            beer.BeerStyle = beerStyle;
                        if (recipe != null)
                            beer.Recipe = recipe;
                        if (srm != null)
                            beer.SRM = srm;
                        if (abv != null)
                            beer.ABV = abv;
                        if (ibu != null)
                            beer.IBU = ibu;
                        return beer;
                    }, new { From = from, To = size },
                    splitOn: "BeerStyleId,RecipeId,SrmId,AbvId,IbuId"
                    );

                foreach (var beer in beers)
                {
                    GetForkOf(context, beer);
                    GetForks(context, beer);
                    GetBreweries(context, beer);
                    GetBrewers(context, beer);
                    if (beer.Recipe != null)
                    {
                        GetRecipeSteps(context, beer.Recipe);
                    }
                }

                return beers.ToList();
            }
        }

        public async Task<Beer> GetSingleAsync(int id, params string[] navigtionProperties)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var beers = await context.QueryAsync<Beer, BeerStyle, Recipe, SRM, ABV, IBU, Beer>(
                    "SELECT * FROM Beers b " +
                    "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                    "LEFT JOIN Recipes r ON r.RecipeId = b.BeerId " +
                    "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                    "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                    "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                    "WHERE BeerId = @BeerId;"
                    , (b, beerStyle, recipe, srm, abv, ibu) =>
                    {
                        if (beerStyle != null)
                            b.BeerStyle = beerStyle;
                        if (recipe != null)
                            b.Recipe = recipe;
                        if (srm != null)
                            b.SRM = srm;
                        if (abv != null)
                            b.ABV = abv;
                        if (ibu != null)
                            b.IBU = ibu;
                        return b;
                    },
                    new { BeerId = id },
                    splitOn: "BeerStyleId,RecipeId,SrmId,AbvId,IbuId"
                    );
                var beer = beers.SingleOrDefault();
                if (beer == null) return null;
                GetForkOf(context, beer);
                GetForks(context, beer);
                GetBreweries(context, beer);
                GetBrewers(context, beer);
                if (beer.Recipe != null)
                {
                    GetRecipeSteps(context, beer.Recipe);
                }
                return beer;
            }
        }

        public async Task AddAsync(Beer beer)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        beer.CreatedDate = DateTime.Now;
                        beer.UpdatedDate = DateTime.Now;
                        var beerId = await context.QueryAsync<int>(
                            "INSERT Beers(Name,BeerStyleId,CreatedDate,UpdatedDate,ForkeOfId) " +
                            "VALUES(@Name,@BeerStyleId,@CreatedDate,@UpdatedDate,@ForkeOfId);" +
                            "SELECT CAST(Scope_Identity() as int);", beer, transaction);
                        beer.BeerId = beerId.SingleOrDefault();

                        beer.SRM.SrmId = beer.BeerId;
                        await context.ExecuteAsync("INSERT SRMs(SrmId,Standard,Mosher,Daniels,Morey) VALUES(@SrmId,@Standard,@Mosher,@Daniels,@Morey);",
                            new { beer.SRM.SrmId, beer.SRM.Standard, beer.SRM.Mosher, beer.SRM.Daniels, beer.SRM.Morey }, transaction);

                        beer.ABV.AbvId = beer.BeerId;
                        await context.ExecuteAsync("INSERT ABVs(AbvId,Standard,Miller,Advanced,AdvancedAlternative,Simple,AlternativeSimple) " +
                                        "VALUES(@AbvId,@Standard,@Miller,@Advanced,@AdvancedAlternative,@Simple,@AlternativeSimple);",
                            new { AbvId = beer.BeerId, beer.ABV.Standard, beer.ABV.Miller, beer.ABV.Advanced, beer.ABV.AdvancedAlternative, beer.ABV.Simple, beer.ABV.AlternativeSimple }, transaction);

                        beer.IBU.IbuId = beer.BeerId;
                        await context.ExecuteAsync("INSERT IBUs(IbuId,Standard,Tinseth,Rager) " +
                                        "VALUES(@IbuId,@Standard,@Tinseth,@Rager);",
                            new { IbuId = beer.BeerId, beer.IBU.Standard, beer.IBU.Tinseth, beer.IBU.Rager }, transaction);

                        AddBrewers(beer, context, transaction);

                        if (beer.Recipe != null)
                        {
                            beer.Recipe.RecipeId = beer.BeerId;
                            AddRecipe(beer.Recipe, context, transaction);
                        }
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private void AddBrewers(Beer beer, DbConnection context, DbTransaction transaction)
        {
            //TODO: FIX THIS.
            if (beer.Brewers == null || !beer.Brewers.Any()) return;
            //foreach (var userBeer in beer.Brewers)
            //{
            //    userBeer.Username = userBeer.Username.ToLower();
            //}
            var brewers = beer.Brewers.DistinctBy(u => u.Username.ToLower());
            var distinct =  brewers.Select(b => new {beer.BeerId, b.Username, b.Confirmed});
            context.Execute("INSERT UserBeers(BeerId,Username,Confirmed) VALUES(@BeerId,@Username,@Confirmed);", distinct, transaction);
        }

        public async Task<int> UpdateAsync(Beer beer)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        beer.UpdatedDate = DateTime.Now;
                        var result = await context.ExecuteAsync(
                            "UPDATE Beers set Name = @Name, BeerStyleId = @BeerStyleId, UpdatedDate = @UpdatedDate, ForkeOfId = @ForkeOfId WHERE BeerId = @BeerId;",
                            beer, transaction);

                        await context.ExecuteAsync("UPDATE SRMs set Standard = @Standard, Mosher = @Mosher, Daniels = @Daniels, Morey = @Morey " +
                                        "WHERE SrmId = @SrmId;",
                            new { SrmId = beer.BeerId, beer.SRM.Standard, beer.SRM.Mosher, beer.SRM.Daniels, beer.SRM.Morey }, transaction);

                        await context.ExecuteAsync("UPDATE ABVs set Standard = @Standard, Miller = @Miller, Advanced = @Advanced, AdvancedAlternative = @AdvancedAlternative, Simple = @Simple, AlternativeSimple = @AlternativeSimple " +
                                        "WHERE AbvId = @AbvId;",
                           new { AbvId = beer.BeerId, beer.ABV.Standard, beer.ABV.Miller, beer.ABV.Advanced, beer.ABV.AdvancedAlternative, beer.ABV.Simple, beer.ABV.AlternativeSimple }, transaction);

                        await context.ExecuteAsync("UPDATE IBUs set Standard = @Standard,Tinseth =@Tinseth,Rager =@Rager WHERE IbuId = @IbuId;",
                            new { IbuId = beer.BeerId, beer.IBU.Standard, beer.IBU.Tinseth, beer.IBU.Rager }, transaction);

                        beer.Recipe.RecipeId = beer.BeerId;
                        UpdateRecipe(context, transaction, beer.Recipe);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception exception)
                    {
                        Log.Error(exception.ToString());
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public Task RemoveAsync(Beer beer)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Beer>> GetLastAsync(int @from, int size, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var beers =
                    await
                        context.QueryAsync<Beer, BeerStyle, Recipe, SRM, ABV, IBU, Beer>(
                            "SELECT Top "+ size +" * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY CreatedDate DESC) as ROW_NUM FROM Beers) AS b " +
                            "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                            "LEFT JOIN Recipes r ON r.RecipeId = b.BeerId " +
                            "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                            "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                            "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                            "WHERE ROW_NUM >= @From",
                             (beer, beerStyle, recipe, srm, abv, ibu) =>
                             {
                                 if (beerStyle != null)
                                     beer.BeerStyle = beerStyle;
                                 if (recipe != null)
                                     beer.Recipe = recipe;
                                 if (srm != null)
                                     beer.SRM = srm;
                                 if (abv != null)
                                     beer.ABV = abv;
                                 if (ibu != null)
                                     beer.IBU = ibu;
                                 return beer;
                             }, new { From = from, To = size }, splitOn: "BeerStyleId,RecipeId,SrmId,AbvId,IbuId");

                foreach (var beer in beers)
                {
                    GetForkOf(context, beer);
                    GetForks(context, beer);
                    GetBreweries(context, beer);
                    GetBrewers(context, beer);
                    if (beer.Recipe != null)
                    {
                        GetRecipeSteps(context, beer.Recipe);
                    }
                }

                return beers.ToList();
            }
        }

        public async Task<IList<Beer>> GetAllUserBeerAsync(string username, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var beers =
                    await
                        context.QueryAsync<Beer, BeerStyle, Recipe, SRM, ABV, IBU, Beer>(
                            "SELECT * FROM Beers b " +
                            "LEFT JOIN UserBeers ub ON ub.BeerId = b.BeerId " +
                            "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                            "LEFT JOIN Recipes r ON r.RecipeId = b.BeerId " +
                            "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                            "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                            "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                            "WHERE ub.Username = @Username;",
                             (beer, beerStyle, recipe, srm, abv, ibu) =>
                             {
                                 if (beerStyle != null)
                                     beer.BeerStyle = beerStyle;
                                 if (recipe != null)
                                     beer.Recipe = recipe;
                                 if (srm != null)
                                     beer.SRM = srm;
                                 if (abv != null)
                                     beer.ABV = abv;
                                 if (ibu != null)
                                     beer.IBU = ibu;
                                 return beer;
                             }, new { Username = username}, splitOn: "BeerStyleId,RecipeId,SrmId,AbvId,IbuId");

                foreach (var beer in beers)
                {
                    GetForkOf(context, beer);
                    GetForks(context, beer);
                    GetBreweries(context, beer);
                    GetBrewers(context, beer);
                    if (beer.Recipe != null)
                    {
                        GetRecipeSteps(context, beer.Recipe);
                    }
                }

                return beers.ToList();
            }
        }

        public IList<Beer> GetAllUserBeer(string username, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var beers =
                    context.Query<Beer, BeerStyle, Recipe, SRM, ABV, IBU, Beer>(
                        "SELECT * FROM Beers b " +
                        "LEFT JOIN UserBeers ub ON ub.BeerId = b.BeerId " +
                        "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                        "LEFT JOIN Recipes r ON r.RecipeId = b.BeerId " +
                        "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                        "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                        "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                        "WHERE ub.Username = @Username;",
                        (beer, beerStyle, recipe, srm, abv, ibu) =>
                        {
                            if (beerStyle != null)
                                beer.BeerStyle = beerStyle;
                            if (recipe != null)
                                beer.Recipe = recipe;
                            if (srm != null)
                                beer.SRM = srm;
                            if (abv != null)
                                beer.ABV = abv;
                            if (ibu != null)
                                beer.IBU = ibu;
                            return beer;
                        }, new {Username = username}, splitOn: "BeerStyleId,RecipeId,SrmId,AbvId,IbuId");

                foreach (var beer in beers)
                {
                    GetForkOf(context, beer);
                    GetForks(context, beer);
                    GetBreweries(context, beer);
                    GetBrewers(context, beer);
                    if (beer.Recipe != null)
                    {
                        GetRecipeSteps(context, beer.Recipe);
                    }
                }

                return beers.ToList();
            }
        }

        public IList<Beer> GetAllBreweryBeers(int breweryId, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var beers =
                    context.Query<Beer, BeerStyle, Recipe, SRM, ABV, IBU, Beer>(
                        "SELECT * FROM Beers b " +
                        "LEFT JOIN BreweryBeers bb ON ub.BeerId = b.BeerId " +
                        "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                        "LEFT JOIN Recipes r ON r.RecipeId = b.BeerId " +
                        "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                        "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                        "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                        "WHERE bb.BreweryId = @BreweryId;",
                        (beer, beerStyle, recipe, srm, abv, ibu) =>
                        {
                            if (beerStyle != null)
                                beer.BeerStyle = beerStyle;
                            if (recipe != null)
                                beer.Recipe = recipe;
                            if (srm != null)
                                beer.SRM = srm;
                            if (abv != null)
                                beer.ABV = abv;
                            if (ibu != null)
                                beer.IBU = ibu;
                            return beer;
                        }, new {BreweryId = breweryId}, splitOn: "BeerStyleId,RecipeId,SrmId,AbvId,IbuId");

                foreach (var beer in beers)
                {
                    GetForkOf(context, beer);
                    GetForks(context, beer);
                    GetBreweries(context, beer);
                    GetBrewers(context, beer);
                    if (beer.Recipe != null)
                    {
                        GetRecipeSteps(context, beer.Recipe);
                    }
                }

                return beers.ToList();
            }
        }

        private void GetRecipeSteps(DbConnection context, Recipe recipe)
        {
            GetMashSteps(context, recipe);
            GetBoilSteps(context, recipe);
            GetFermentationSteps(context, recipe);
            GetSpargeStep(context, recipe);
        }

        private void GetSpargeStep(DbConnection context, Recipe recipe)
        {
            var spargeSteps = context.Query<SpargeStep>(
                "SELECT * FROM SpargeSteps WHERE RecipeId = @RecipeId;", new { recipe.RecipeId });
            var spargeStep = spargeSteps.SingleOrDefault();
            if (spargeStep == null) return;
            var spargeStepHops = context.Query<SpargeStepHop, Hop, SpargeStepHop>(
                 "SELECT * FROM SpargeStepHops fs " +
                 "LEFT JOIN Hops h ON fs.HopId = h.HopId " +
                 "WHERE RecipeId = @RecipeId and StepNumber = @StepNumber;", (spargeStepHop, hop) =>
                 {
                     spargeStepHop.Hop = hop;
                     return spargeStepHop;
                 }, new { recipe.RecipeId, spargeStep.StepNumber }, splitOn: "HopId");
            spargeStep.Hops = spargeStepHops.ToList();
            recipe.SpargeStep = spargeStep;
        }

        private void GetFermentationSteps(DbConnection context, Recipe recipe)
        {
            var fermentationSteps = context.Query<FermentationStep>(
               "SELECT * FROM FermentationSteps WHERE RecipeId = @RecipeId;", new { recipe.RecipeId });
            foreach (var fermentationStep in fermentationSteps)
            {
                var fermentationStepFermentables = context.Query<FermentationStepFermentable, Fermentable, FermentationStepFermentable>(
                    "SELECT * FROM FermentationStepFermentables fs " +
                    "LEFT JOIN Fermentables f ON fs.FermentableId = f.FermentableId " +
                    "WHERE RecipeId = @RecipeId and StepNumber = @StepNumber;", (fermentationStepFermentable, fermentable) =>
                    {
                        fermentationStepFermentable.Fermentable = fermentable;
                        return fermentationStepFermentable;
                    }, new { recipe.RecipeId, fermentationStep.StepNumber }, splitOn: "FermentableId");
                fermentationStep.Fermentables = fermentationStepFermentables.ToList();

                var fermentationStepHops = context.Query<FermentationStepHop, Hop, FermentationStepHop>(
                   "SELECT * FROM FermentationStepHops fs " +
                   "LEFT JOIN Hops h ON fs.HopId = h.HopId " +
                   "WHERE RecipeId = @RecipeId and StepNumber = @StepNumber;", (fermentationStepHop, hop) =>
                   {
                       fermentationStepHop.Hop = hop;
                       return fermentationStepHop;
                   }, new { recipe.RecipeId, fermentationStep.StepNumber }, splitOn: "HopId");
                fermentationStep.Hops = fermentationStepHops.ToList();

                var fermentationStepOthers = context.Query<FermentationStepOther, Other, FermentationStepOther>(
                  "SELECT * FROM FermentationStepOthers fs " +
                  "LEFT JOIN Others o ON fs.OtherId = o.OtherId " +
                  "WHERE RecipeId = @RecipeId and StepNumber = @StepNumber;", (fermentationStepOther, other) =>
                  {
                      fermentationStepOther.Other = other;
                      return fermentationStepOther;
                  }, new { recipe.RecipeId, fermentationStep.StepNumber }, splitOn: "OtherId");
                fermentationStep.Others = fermentationStepOthers.ToList();

                var fermentationStepYeasts = context.Query<FermentationStepYeast, Yeast, FermentationStepYeast>(
                 "SELECT * FROM FermentationStepYeasts f " +
                 "LEFT JOIN Yeasts y ON f.YeastId = y.YeastId " +
                 "WHERE RecipeId = @RecipeId and StepNumber = @StepNumber;", (fermentationStepYeast, yeast) =>
                 {
                     fermentationStepYeast.Yeast = yeast;
                     return fermentationStepYeast;
                 }, new { recipe.RecipeId, fermentationStep.StepNumber }, splitOn: "YeastId");
                fermentationStep.Yeasts = fermentationStepYeasts.ToList();
            }
            recipe.FermentationSteps = fermentationSteps.ToList();
        }

        private void GetBoilSteps(DbConnection context, Recipe recipe)
        {
            var boilSteps = context.Query<BoilStep>(
                "SELECT * FROM BoilSteps WHERE RecipeId = @RecipeId;", new { recipe.RecipeId });
            foreach (var boilStep in boilSteps)
            {
                var boilStepFermentables = context.Query<BoilStepFermentable, Fermentable, BoilStepFermentable>(
                    "SELECT * FROM BoilStepFermentables b " +
                    "LEFT JOIN Fermentables f ON b.FermentableId = f.FermentableId " +
                    "WHERE RecipeId = @RecipeId and StepNumber = @StepNumber;", (boilStepFermentable, fermentable) =>
                    {
                        boilStepFermentable.Fermentable = fermentable;
                        return boilStepFermentable;
                    }, new { recipe.RecipeId, boilStep.StepNumber }, splitOn: "FermentableId");
                boilStep.Fermentables = boilStepFermentables.ToList();

                var boilStepHops = context.Query<BoilStepHop, Hop, BoilStepHop>(
                   "SELECT * FROM BoilStepHops b " +
                   "LEFT JOIN Hops h ON b.HopId = h.HopId " +
                   "WHERE RecipeId = @RecipeId and StepNumber = @StepNumber;", (boilStepHop, hop) =>
                   {
                       boilStepHop.Hop = hop;
                       return boilStepHop;
                   }, new { recipe.RecipeId, boilStep.StepNumber }, splitOn: "HopId");
                boilStep.Hops = boilStepHops.ToList();

                var boilStepOthers = context.Query<BoilStepOther, Other, BoilStepOther>(
                  "SELECT * FROM BoilStepOthers b " +
                  "LEFT JOIN Others o ON b.OtherId = o.OtherId " +
                  "WHERE RecipeId = @RecipeId and StepNumber = @StepNumber;", (boilStepOther, other) =>
                  {
                      boilStepOther.Other = other;
                      return boilStepOther;
                  }, new { recipe.RecipeId, boilStep.StepNumber }, splitOn: "OtherId");
                boilStep.Others = boilStepOthers.ToList();
            }
            recipe.BoilSteps = boilSteps.ToList();
        }

        private void GetMashSteps(DbConnection context, Recipe recipe)
        {
            var mashSteps = context.Query<MashStep>(
                "SELECT * FROM MashSteps WHERE RecipeId = @RecipeId;", new { recipe.RecipeId });
            foreach (var mashStep in mashSteps)
            {
                var mashStepFermentables = context.Query<MashStepFermentable, Fermentable, MashStepFermentable>(
                    "SELECT * FROM MashStepFermentables m " +
                    "LEFT JOIN Fermentables f ON m.FermentableId = f.FermentableId " +
                    "WHERE RecipeId = @RecipeId and StepNumber = @StepNumber;", (mashStepFermentable, fermentable) =>
                    {
                        mashStepFermentable.Fermentable = fermentable;
                        return mashStepFermentable;
                    }, new { recipe.RecipeId, mashStep.StepNumber }, splitOn: "FermentableId");
                mashStep.Fermentables = mashStepFermentables.ToList();

                var mashStepHops = context.Query<MashStepHop, Hop, MashStepHop>(
                   "SELECT * FROM MashStepHops m " +
                   "LEFT JOIN Hops h ON m.HopId = h.HopId " +
                   "WHERE RecipeId = @RecipeId and StepNumber = @StepNumber;", (mashStepHop, hop) =>
                   {
                       mashStepHop.Hop = hop;
                       return mashStepHop;
                   }, new { recipe.RecipeId, mashStep.StepNumber }, splitOn: "HopId");
                mashStep.Hops = mashStepHops.ToList();

                var mashStepOthers = context.Query<MashStepOther, Other, MashStepOther>(
                  "SELECT * FROM MashStepOthers m " +
                  "LEFT JOIN Others o ON m.OtherId = o.OtherId " +
                  "WHERE RecipeId = @RecipeId and StepNumber = @StepNumber;", (mashStepOther, other) =>
                  {
                      mashStepOther.Other = other;
                      return mashStepOther;
                  }, new { recipe.RecipeId, mashStep.StepNumber }, splitOn: "OtherId");
                mashStep.Others = mashStepOthers.ToList();
            }
            recipe.MashSteps = mashSteps.ToList();
        }

        private void GetForkOf(DbConnection context, Beer beer)
        {
            var forkOf = context.Query<Beer, BeerStyle, SRM, ABV, IBU, Beer>(
                    "SELECT * FROM Beers b " +
                    "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                    "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                    "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                    "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                    "WHERE BeerId = @BeerId"
                    , (b, beerStyle, srm, abv, ibu) =>
                    {
                        if (beerStyle != null)
                            b.BeerStyle = beerStyle;
                        if (srm != null)
                            b.SRM = srm;
                        if (abv != null)
                            b.ABV = abv;
                        if (ibu != null)
                            b.IBU = ibu;
                        return b;
                    },
                    new { beer.BeerId },
                    splitOn: "BeerStyleId,SrmId,AbvId,IbuId"
                    );
            beer.ForkeOf = forkOf.SingleOrDefault();
        }

        private void GetBrewers(DbConnection context, Beer beer)
        {
            var brewers = context.Query<UserBeer, User, UserBeer>(
                "SELECT * FROM UserBeers ub " +
                "LEFT JOIN Users u ON ub.Username = u.Username " +
                "WHERE ub.BeerId = @BeerId;",
                (userBeer, user) =>
                {
                    userBeer.User = user;
                    return userBeer;
                }, new { beer.BeerId }, splitOn: "Username");
            beer.Brewers = brewers.ToList();
        }

        private void GetBreweries(DbConnection context, Beer beer)
        {
            var breweries = context.Query<BreweryBeer, Brewery, BreweryBeer>(
                       "SELECT * " +
                       "FROM BreweryBeers bb " +
                       "LEFT JOIN Breweries b ON bb.BreweryId = b.BreweryId " +
                       "WHERE bb.BeerId = @BeerId", (breweryBeer, brewery) =>
                       {
                           breweryBeer.Brewery = brewery;
                           return breweryBeer;
                       }, new { beer.BeerId }, splitOn: "BreweryId");
            beer.Breweries = breweries.ToList();
        }

        private void GetForks(DbConnection context, Beer beer)
        {
            var forks = context.Query<Beer, SRM, ABV, IBU, BeerStyle, Beer>(
                    "SELECT * " +
                    "FROM Beers b " +
                    "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                    "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                    "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                    "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                    "WHERE b.ForkeOfId = @BeerId", (fork, srm, abv, ibu, beerStyle) =>
                    {
                        if (srm != null)
                            fork.SRM = srm;
                        if (abv != null)
                            fork.ABV = abv;
                        if (ibu != null)
                            fork.IBU = ibu;
                        if (beerStyle != null)
                            fork.BeerStyle = beerStyle;
                        return beer;
                    }, new { beer.BeerId }, splitOn: "SrmId,AbvId,IbuId,BeerStyleId");
            beer.Forks = forks.ToList();
        }

        private static void AddRecipe(Recipe recipe, DbConnection context, DbTransaction transaction)
        {

            context.Execute("INSERT Recipes(RecipeId,Volume,Notes,OG,FG,Efficiency,TotalBoilTime) " +
                            "VALUES(@RecipeId,@Volume,@Notes,@OG,@FG,@Efficiency,@TotalBoilTime);",
                new
                {
                    recipe.RecipeId,
                    recipe.Volume,
                    recipe.Notes,
                    recipe.OG,
                    recipe.FG,
                    recipe.Efficiency,
                    recipe.TotalBoilTime
                }, transaction);

            SetStepNumber(recipe);
            if (recipe.MashSteps != null)
            {
                foreach (var mashStep in recipe.MashSteps)
                {
                    mashStep.RecipeId = recipe.RecipeId;
                    AddMashStep(context, transaction, mashStep);
                }
            }
            if (recipe.BoilSteps != null)
            {
                foreach (var boilStep in recipe.BoilSteps)
                {
                    boilStep.RecipeId = recipe.RecipeId;
                    AddBoilStep(context, transaction, boilStep);
                }
            }

            if (recipe.FermentationSteps != null)
            {
                foreach (var fermentationStep in recipe.FermentationSteps)
                {
                    fermentationStep.RecipeId = recipe.RecipeId;
                    AddFermentationStep(context, transaction, fermentationStep);
                }
            }
        }

        private static void AddMashStep(DbConnection context, DbTransaction transaction, MashStep mashStep)
        {

            context.Execute(
                "INSERT MashSteps(Temperature,Type,Length,Volume,Notes,RecipeId,StepNumber)" +
                "VALUES(@Temperature,@Type,@Length,@Volume,@Notes,@RecipeId,@StepNumber)", mashStep, transaction);
            if (mashStep.Fermentables != null)
            {
                var duplicates = mashStep.Fermentables.GroupBy(f => f.FermentableId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    mashStep.Fermentables = mashStep.Fermentables.GroupBy(o => o.FermentableId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                context.Execute(
                    "INSERT MashStepFermentables(RecipeId,StepNumber,FermentableId,Amount,Lovibond,PPG) " +
                    "VALUES(@RecipeId,@StepNumber,@FermentableId,@Amount,@Lovibond,@PPG);",
                    mashStep.Fermentables.Select(f => new
                    {
                        mashStep.RecipeId,
                        mashStep.StepNumber,
                        f.FermentableId,
                        f.Amount,
                        f.Lovibond,
                        f.PPG,
                    }), transaction);
            }

            if (mashStep.Hops != null)
            {
                var duplicates = mashStep.Hops.GroupBy(f => f.HopId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    mashStep.Hops = mashStep.Hops.GroupBy(o => o.HopId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                context.Execute(
                    "INSERT MashStepHops(RecipeId,StepNumber,HopId,Amount,AAValue,HopFormId) " +
                    "VALUES(@RecipeId,@StepNumber,@HopId,@Amount,@AAValue,@HopFormId);",
                    mashStep.Hops.Select(h => new
                    {
                        mashStep.RecipeId,
                        mashStep.StepNumber,
                        h.HopId,
                        h.HopFormId,
                        h.AAValue,
                        h.Amount
                    }), transaction);
            }

            if (mashStep.Others != null)
            {
                var duplicates = mashStep.Others.GroupBy(f => f.OtherId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    mashStep.Others = mashStep.Others.GroupBy(o => o.OtherId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                context.Execute(
                    "INSERT MashStepOthers(RecipeId,StepNumber,OtherId,Amount) " +
                    "VALUES(@RecipeId,@StepNumber,@OtherId,@Amount);",
                    mashStep.Others.Select(o => new
                    {
                        mashStep.RecipeId,
                        mashStep.StepNumber,
                        o.Amount,
                        o.OtherId
                    }), transaction);
            }
        }

        private static void AddBoilStep(DbConnection context, DbTransaction transaction, BoilStep boilStep)
        {
            context.Execute(
                "INSERT BoilSteps(Length,Volume,Notes,RecipeId,StepNumber)" +
                "VALUES(@Length,@Volume,@Notes,@RecipeId,@StepNumber)", boilStep, transaction);


            if (boilStep.Fermentables != null)
            {
                var duplicates = boilStep.Fermentables.GroupBy(f => f.FermentableId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    boilStep.Fermentables = boilStep.Fermentables.GroupBy(o => o.FermentableId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                context.Execute(
                    "INSERT BoilStepFermentables(RecipeId,StepNumber,FermentableId,Amount,Lovibond,PPG) " +
                    "VALUES(@RecipeId,@StepNumber,@FermentableId,@Amount,@Lovibond,@PPG);",
                    boilStep.Fermentables.Select(f => new
                    {
                        boilStep.RecipeId,
                        boilStep.StepNumber,
                        f.FermentableId,
                        f.Amount,
                        f.Lovibond,
                        f.PPG,
                    }), transaction);
            }

            if (boilStep.Hops != null)
            {
                var duplicates = boilStep.Hops.GroupBy(f => f.HopId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    boilStep.Hops = boilStep.Hops.GroupBy(o => o.Hop)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                context.Execute(
                    "INSERT BoilStepHops(RecipeId,StepNumber,HopId,Amount,AAValue,HopFormId) " +
                    "VALUES(@RecipeId,@StepNumber,@HopId,@Amount,@AAValue,@HopFormId);",
                    boilStep.Hops.Select(h => new
                    {
                        boilStep.RecipeId,
                        boilStep.StepNumber,
                        h.HopId,
                        h.HopFormId,
                        h.AAValue,
                        h.Amount
                    }), transaction);
            }

            if (boilStep.Others != null)
            {
                var duplicates = boilStep.Others.GroupBy(f => f.OtherId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    boilStep.Others = boilStep.Others.GroupBy(o => o.OtherId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                context.Execute(
                    "INSERT BoilStepOthers(RecipeId,StepNumber,OtherId,Amount) " +
                    "VALUES(@RecipeId,@StepNumber,@OtherId,@Amount);",
                    boilStep.Others.Select(o => new
                    {
                        boilStep.RecipeId,
                        boilStep.StepNumber,
                        o.Amount,
                        o.OtherId
                    }), transaction);
            }
        }

        private static void AddFermentationStep(DbConnection context, DbTransaction transaction, FermentationStep fermentationStep)
        {
            context.Execute(
                "INSERT FermentationSteps(Temperature,Length,Volume,Notes,RecipeId,StepNumber)" +
                "VALUES(@Temperature,@Length,@Volume,@Notes,@RecipeId,@StepNumber)", fermentationStep, transaction);

            if (fermentationStep.Fermentables != null)
            {
                var duplicates = fermentationStep.Fermentables.GroupBy(f => f.FermentableId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    fermentationStep.Fermentables = fermentationStep.Fermentables.GroupBy(o => o.FermentableId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                context.Execute(
                    "INSERT FermentationStepFermentables(RecipeId,StepNumber,FermentableId,Amount,Lovibond,PPG) " +
                    "VALUES(@RecipeId,@StepNumber,@FermentableId,@Amount,@Lovibond,@PPG);",
                    fermentationStep.Fermentables.Select(f => new
                    {
                        fermentationStep.RecipeId,
                        fermentationStep.StepNumber,
                        f.FermentableId,
                        f.Amount,
                        f.Lovibond,
                        f.PPG,
                    }), transaction);
            }

            if (fermentationStep.Hops != null)
            {
                var duplicates = fermentationStep.Hops.GroupBy(f => f.HopId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    fermentationStep.Hops = fermentationStep.Hops.GroupBy(o => o.HopId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                context.Execute(
                    "INSERT FermentationStepHops(RecipeId,StepNumber,HopId,Amount,AAValue,HopFormId) " +
                    "VALUES(@RecipeId,@StepNumber,@HopId,@Amount,@AAValue,@HopFormId);",
                    fermentationStep.Hops.Select(h => new
                    {
                        fermentationStep.RecipeId,
                        fermentationStep.StepNumber,
                        h.HopId,
                        h.HopFormId,
                        h.AAValue,
                        h.Amount
                    }), transaction);
            }

            if (fermentationStep.Others != null)
            {
                var duplicates = fermentationStep.Others.GroupBy(f => f.OtherId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    fermentationStep.Others = fermentationStep.Others.GroupBy(o => o.OtherId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                context.Execute(
                    "INSERT FermentationStepOthers(RecipeId,StepNumber,OtherId,Amount) " +
                    "VALUES(@RecipeId,@StepNumber,@OtherId,@Amount);",
                    fermentationStep.Others.Select(o => new
                    {
                        fermentationStep.RecipeId,
                        fermentationStep.StepNumber,
                        o.Amount,
                        o.OtherId
                    }), transaction);
            }

            if (fermentationStep.Yeasts != null)
            {
                var duplicates = fermentationStep.Yeasts.GroupBy(f => f.YeastId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    fermentationStep.Yeasts = fermentationStep.Yeasts.GroupBy(o => o.YeastId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                context.Execute(
                    "INSERT FermentationStepYeasts(RecipeId,StepNumber,YeastId,Amount) " +
                    "VALUES(@RecipeId,@StepNumber,@YeastId,@Amount);",
                    fermentationStep.Yeasts.Select(o => new
                    {
                        fermentationStep.RecipeId,
                        fermentationStep.StepNumber,
                        o.Amount,
                        o.YeastId
                    }), transaction);
            }
        }

        private static void UpdateRecipe(DbConnection context, DbTransaction transaction, Recipe recipe)
        {
            context.Execute("UPDATE Recipes set Volume = @Volume, Notes = @Notes,OG = @OG,FG = @FG, Efficiency = @Efficiency, TotalBoilTime = @TotalBoilTime " +
                           "WHERE RecipeId = @RecipeId", recipe, transaction);
            SetStepNumber(recipe);
            UpdateMashSteps(context, transaction, recipe);
            UpdateBoilSteps(context, transaction, recipe);
            UpdateFermentationSteps(context, transaction, recipe);
        }

        private static void SetStepNumber(Recipe recipe)
        {
            var stepNumber = 1;
            foreach (var mashStep in recipe.MashSteps)
            {
                mashStep.StepNumber = stepNumber;
                stepNumber++;
            }

            //TODO: Validate that this is null on insert/update
            if (recipe.SpargeStep != null)
            {
                recipe.SpargeStep.StepNumber = stepNumber;
                stepNumber++;
            }

            foreach (var boilStep in recipe.BoilSteps.OrderByDescending(b => b.Length))
            {
                boilStep.StepNumber = stepNumber;
                stepNumber++;
            }
            foreach (var fermentationStep in recipe.FermentationSteps.OrderByDescending(f => f.Length))
            {
                fermentationStep.StepNumber = stepNumber;
                stepNumber++;
            }
        }

        private static void UpdateMashSteps(DbConnection context, DbTransaction transaction, Recipe recipe)
        {
            //Remove mash steps 
            context.Execute("DELETE FROM MashSteps WHERE RecipeId = @RecipeId;", new { recipe.RecipeId }, transaction);
            context.Execute("DELETE FROM MashStepFermentables WHERE RecipeId = @RecipeId;", new { recipe.RecipeId }, transaction);
            context.Execute("DELETE FROM MashStepHops WHERE RecipeId = @RecipeId;", new { recipe.RecipeId }, transaction);
            context.Execute("DELETE FROM MashStepOthers WHERE RecipeId = @RecipeId ;", new { recipe.RecipeId }, transaction);
            //Add mash steps
            foreach (var mashStep in recipe.MashSteps)
            {
                mashStep.RecipeId = recipe.RecipeId;
                AddMashStep(context, transaction, mashStep);
            }
        }

        private static void UpdateBoilSteps(DbConnection context, DbTransaction transaction, Recipe recipe)
        {
            //Remove mash steps 
            context.Execute("DELETE FROM BoilSteps WHERE RecipeId = @RecipeId;", new { recipe.RecipeId }, transaction);
            context.Execute("DELETE FROM BoilStepFermentables WHERE RecipeId = @RecipeId;", new { recipe.RecipeId }, transaction);
            context.Execute("DELETE FROM BoilStepHops WHERE RecipeId = @RecipeId;", new { recipe.RecipeId }, transaction);
            context.Execute("DELETE FROM BoilStepOthers WHERE RecipeId = @RecipeId ;", new { recipe.RecipeId }, transaction);
            //Add mash steps
            foreach (var boilStep in recipe.BoilSteps)
            {
                boilStep.RecipeId = recipe.RecipeId;
                AddBoilStep(context, transaction, boilStep);
            }
        }

        private static void UpdateFermentationSteps(DbConnection context, DbTransaction transaction, Recipe recipe)
        {
            //Remove mash steps 
            context.Execute("DELETE FROM FermentationSteps WHERE RecipeId = @RecipeId;", new { recipe.RecipeId }, transaction);
            context.Execute("DELETE FROM FermentationStepFermentables WHERE RecipeId = @RecipeId;", new { recipe.RecipeId }, transaction);
            context.Execute("DELETE FROM FermentationStepHops WHERE RecipeId = @RecipeId;", new { recipe.RecipeId }, transaction);
            context.Execute("DELETE FROM FermentationStepOthers WHERE RecipeId = @RecipeId ;", new { recipe.RecipeId }, transaction);
            //Add mash steps
            foreach (var fermentationStep in recipe.FermentationSteps)
            {
                fermentationStep.RecipeId = recipe.RecipeId;
                AddFermentationStep(context, transaction, fermentationStep);
            }
        }
    }
}
