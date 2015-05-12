using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using log4net;
using log4net.Repository.Hierarchy;
using Microbrewit.Model;

namespace Microbrewit.Repository.Repository
{

    public class YeastDapperRepository : IYeastRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public IList<Yeast> GetAll(params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var sql = @"SELECT * FROM Yeasts y LEFT JOIN Suppliers s ON y.SupplierId = s.SupplierId ORDER BY y.Name;";
                var yeasts = context.Query<Yeast, Supplier, Yeast>(sql, (yeast, supplier) =>
                {
                    yeast.Supplier = supplier;
                    return yeast;
                }, splitOn: "YeastId,SupplierId");
                return yeasts.ToList();
            }
        }

        public Yeast GetSingle(int id, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var sql = @"SELECT * FROM Yeasts y LEFT JOIN Suppliers s ON y.SupplierId = s.SupplierId WHERE YeastId = @YeastId ORDER BY y.Name;";
                var yeast = context.Query<Yeast, Supplier, Yeast>(sql, (s, supplier) =>
                {
                    s.Supplier = supplier;
                    return s;
                }, new {YeastId = id}, splitOn: "YeastId,SupplierId");
                return yeast.SingleOrDefault();
            }
        }

        public void Add(Yeast yeast)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                       var yeastId = context.Query<int>("INSERT Yeasts(Name,TemperatureHigh,TemperatureLow,Flocculation,AlcoholTolerance,ProductCode,Notes,Type,BrewerySource,Species,AttenutionRange,PitchingFermentationNotes,SupplierId,Custom) " +
                                                        "VALUES(@Name,@TemperatureHigh,@TemperatureLow,@Flocculation,@AlcoholTolerance,@ProductCode,@Notes,@Type,@BrewerySource,@Species,@AttenutionRange,@PitchingFermentationNotes,@SupplierId,@Custom); SELECT CAST(SCOPE_IDENTITY() as int);",
                            new
                            {
                               yeast.Name, 
                               yeast.TemperatureHigh, 
                               yeast.TemperatureLow, 
                               yeast.Flocculation, 
                               yeast.AlcoholTolerance, 
                               yeast.ProductCode, 
                               yeast.Notes, 
                               yeast.Type, 
                               yeast.BrewerySource, 
                               yeast.Species, 
                               yeast.AttenutionRange, 
                               yeast.PitchingFermentationNotes, 
                               yeast.SupplierId, 
                               yeast.Custom
                            }, transaction);
                        yeast.YeastId = yeastId.SingleOrDefault();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        transaction.Rollback();
                    }
                }
            }
        }

        public void Update(Yeast yeast)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        context.Execute(
                         "UPDATE Yeasts set Name = @Name,TemperatureHigh = @TemperatureHigh,TemperatureLow = @TemperatureLow,Flocculation = @Flocculation," +
                         "AlcoholTolerance = @AlcoholTolerance,ProductCode = @ProductCode, Notes = @Notes, Type = @Type, BrewerySource = @BrewerySource, Species = @Species," +
                         "AttenutionRange = @AttenutionRange, PitchingFermentationNotes = @PitchingFermentationNotes, SupplierId = @SupplierId, Custom = @Custom " +
                         "WHERE YeastId = @YeastId;",
                         new
                         {
                             yeast.YeastId,
                             yeast.Name,
                             yeast.TemperatureHigh,
                             yeast.TemperatureLow,
                             yeast.Flocculation,
                             yeast.AlcoholTolerance,
                             yeast.ProductCode,
                             yeast.Notes,
                             yeast.Type,
                             yeast.BrewerySource,
                             yeast.Species,
                             yeast.AttenutionRange,
                             yeast.PitchingFermentationNotes,
                             yeast.SupplierId,
                             yeast.Custom
                         }, transaction);
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

        public void Remove(Yeast yeast)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        context.Execute("DELETE FROM Yeasts WHERE YeastId = @YeastId", new {yeast.YeastId}, transaction);
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

        public async Task<IList<Yeast>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                const string sql = @"SELECT * FROM Yeasts y LEFT JOIN Suppliers s ON s.SupplierId = s.SupplierId ORDER BY y.Name;";
                var yeasts = await context.QueryAsync<Yeast, Supplier, Yeast>(sql, (yeast, supplier) =>
                {
                    yeast.Supplier = supplier;
                    return yeast;
                }, splitOn: "YeastId,SupplierId");
                return yeasts.ToList();
            }
        }

        public async Task<Yeast> GetSingleAsync(int id, params string[] navigtionProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                const string sql = @"SELECT * FROM Yeasts y LEFT JOIN Suppliers s ON y.SupplierId = s.SupplierId WHERE YeastId = @YeastId ORDER BY y.Name;";
                var yeast = await context.QueryAsync<Yeast, Supplier, Yeast>(sql, (s, supplier) =>
                {
                    s.Supplier = supplier;
                    return s;
                }, new { YeastId = id }, splitOn: "YeastId,SupplierId");
                return yeast.SingleOrDefault();
            };
        }

        public async Task AddAsync(Yeast yeast)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var yeastId = await context.QueryAsync<int>("INSERT Yeasts(Name,TemperatureHigh,TemperatureLow,Flocculation,AlcoholTolerance,ProductCode,Notes,Type,BrewerySource,Species,AttenutionRange,PitchingFermentationNotes,SupplierId,Custom) " +
                                                        "VALUES(@Name,@TemperatureHigh,@TemperatureLow,@Flocculation,@AlcoholTolerance,@ProductCode,@Notes,@Type,@BrewerySource,@Species,@AttenutionRange,@PitchingFermentationNotes,@SupplierId,@Custom); SELECT CAST(SCOPE_IDENTITY() as int);",
                            new
                            {
                                yeast.Name,
                                yeast.TemperatureHigh,
                                yeast.TemperatureLow,
                                yeast.Flocculation,
                                yeast.AlcoholTolerance,
                                yeast.ProductCode,
                                yeast.Notes,
                                yeast.Type,
                                yeast.BrewerySource,
                                yeast.Species,
                                yeast.AttenutionRange,
                                yeast.PitchingFermentationNotes,
                                yeast.SupplierId,
                                yeast.Custom
                            }, transaction);
                        yeast.YeastId = yeastId.SingleOrDefault();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task<int> UpdateAsync(Yeast yeast)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var result = await context.ExecuteAsync(
                                "UPDATE Yeasts set Name = @Name,TemperatureHigh = @TemperatureHigh,TemperatureLow = @TemperatureLow,Flocculation = @Flocculation," +
                                "AlcoholTolerance = @AlcoholTolerance,ProductCode = @ProductCode, Notes = @Notes, Type = @Type, BrewerySource = @BrewerySource, Species = @Species," +
                                "AttenutionRange = @AttenutionRange, PitchingFermentationNotes = @PitchingFermentationNotes, SupplierId = @SupplierId, Custom = @Custom " +
                                "WHERE YeastId = @YeastId;",
                                new
                                {
                                    yeast.YeastId,
                                    yeast.Name,
                                    yeast.TemperatureHigh,
                                    yeast.TemperatureLow,
                                    yeast.Flocculation,
                                    yeast.AlcoholTolerance,
                                    yeast.ProductCode,
                                    yeast.Notes,
                                    yeast.Type,
                                    yeast.BrewerySource,
                                    yeast.Species,
                                    yeast.AttenutionRange,
                                    yeast.PitchingFermentationNotes,
                                    yeast.SupplierId,
                                    yeast.Custom
                                }, transaction);
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

        public async Task RemoveAsync(Yeast yeast)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        await context.ExecuteAsync("DELETE FROM Yeasts WHERE YeastId = @YeastId", new { yeast.YeastId }, transaction);
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
    }
}
