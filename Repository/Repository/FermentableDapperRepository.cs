using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using log4net;
using Microbrewit.Model;

namespace Microbrewit.Repository.Repository
{
    public class FermentableDapperRepository : IFermentableRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<Fermentable> GetAll(params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var fermentables = context.Query<Fermentable, Supplier,Origin, Fermentable>(
                    "SELECT * FROM Fermentables f " +
                    "LEFT JOIN Suppliers s ON f.SupplierId = s.SupplierId " +
                    "LEFT JOIN Origins o ON s.OriginId = o.OriginId;",
                    (f, supplier, origin) =>
                    {
                        if(supplier != null)
                            supplier.Origin = origin;
                        f.Supplier = supplier;
                        return f;
                    }, splitOn: "SuperFermentableId,SupplierId,OriginId");

                foreach (var fermentable in fermentables)
                {
                    if (fermentable.SuperFermentableId != null)
                    {
                        fermentable.SuperFermentable =
                            context.Query<Fermentable>("SELECT * FROM Fermentables WHERE FermentableId = @SuperFermentableId;",
                                new { fermentable.SuperFermentableId }).SingleOrDefault();
                    }
                    fermentable.SubFermentables =
                        context.Query<Fermentable>("SELECT * FROM Fermentables WHERE SuperFermentableId = @FermentableId;",
                            new { fermentable.FermentableId }).ToList();
                }
                return fermentables.ToList();
            }
        }

        public Fermentable GetSingle(int id, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var fermentables = context.Query<Fermentable, Supplier, Origin, Fermentable>(
                    "SELECT * FROM Fermentables f " +
                    "LEFT JOIN Suppliers s ON f.SupplierId = s.SupplierId " +
                    "LEFT JOIN Origins o ON s.OriginId = o.OriginID " +
                    "WHERE f.FermentableId = @FermentableId;",
                    (f, supplier, origin) =>
                    {
                        if (supplier != null)
                            supplier.Origin = origin;
                        f.Supplier = supplier;
                        return f;
                    }, new { FermentableId = id }, splitOn:"SupplierId,OriginId");
                var fermentable = fermentables.SingleOrDefault();
                if (fermentable == null) return null;
                if (fermentable.SuperFermentableId != null)
                {
                    fermentable.SuperFermentable =
                        context.Query<Fermentable>("SELECT * FROM Fermentables WHERE FermentableId = @SuperFermentableId;",
                            new { fermentable.SuperFermentableId }).SingleOrDefault();
                }
                fermentable.SubFermentables =
                    context.Query<Fermentable>("SELECT * FROM Fermentables WHERE SuperFermentableId = @FermentableId;",
                        new { fermentable.FermentableId }).ToList();
                return fermentable;
            }
        }

        public void Add(Fermentable fermentable)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var fermentableId =
                            context.Query<int>("INSERT Fermentables(Name,SuperFermentableId,EBC,Lovibond,PPG,SupplierId,Type,Custom) " +
                                               "VALUES(@Name,@SuperFermentableId,@EBC,@Lovibond,@PPG,@SupplierId,@Type,@Custom);" +
                                               "SELECT CAST(SCOPE_IDENTITY() as int);",
                                               new
                                               {
                                                   fermentable.Name,
                                                   fermentable.SuperFermentableId,
                                                   fermentable.EBC,
                                                   fermentable.Lovibond,
                                                   fermentable.PPG,
                                                   fermentable.SupplierId,
                                                   fermentable.Type,
                                                   fermentable.Custom,
                                               }, transaction);
                        fermentable.FermentableId = fermentableId.SingleOrDefault();
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

        public void Update(Fermentable fermentable)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        context.Execute("Update Fermentables set Name = @Name,SuperFermentableId = @SuperFermentableId,EBC = @EBC,Lovibond = @Lovibond,PPG= @PPG,SupplierId = @SupplierId,Type = @Type, Custom = @Custom " +
                                        "WHERE FermentableId = @FermentableId;",
                                               new
                                               {
                                                   fermentable.FermentableId,
                                                   fermentable.Name,
                                                   fermentable.SuperFermentableId,
                                                   fermentable.EBC,
                                                   fermentable.Lovibond,
                                                   fermentable.PPG,
                                                   fermentable.SupplierId,
                                                   fermentable.Type,
                                                   fermentable.Custom,
                                               }, transaction);
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

        public void Remove(Fermentable fermentable)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        context.Execute("DELETE FROM Fermentables WHERE FermentableId = @FermentableId;",
                            new { fermentable.FermentableId }, transaction);
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

        public async Task<IList<Fermentable>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var fermentables = await context.QueryAsync<Fermentable, Supplier, Origin, Fermentable>(
                    "SELECT * FROM Fermentables f " +
                    "LEFT JOIN Suppliers s ON f.SupplierId = s.SupplierId " +
                    "LEFT JOIN Origins o ON s.OriginId = o.OriginId;",
                    (f, supplier, origin) =>
                    {
                        if (supplier != null)
                            supplier.Origin = origin;
                        f.Supplier = supplier;
                        return f;
                    }, splitOn: "SuperFermentableId,SupplierId,OriginId");

                foreach (var fermentable in fermentables)
                {
                    if (fermentable.SuperFermentableId != null)
                    {
                        var superFermentable = await context.QueryAsync<Fermentable>("SELECT * FROM Fermentables WHERE FermentableId = @SuperFermentableId;",
                                new { fermentable.SuperFermentableId });
                        fermentable.SuperFermentable = superFermentable.SingleOrDefault();
                    }
                    var subFermentables =
                        await context.QueryAsync<Fermentable>("SELECT * FROM Fermentables WHERE SuperFermentableId = @FermentableId;",
                            new { fermentable.FermentableId });
                    fermentable.SubFermentables = subFermentables.ToList();
                }
                return fermentables.ToList();
            }
        }

        public async Task<Fermentable> GetSingleAsync(int id, params string[] navigtionProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var fermentables = await context.QueryAsync<Fermentable, Supplier, Origin, Fermentable>(
                    "SELECT * FROM Fermentables f " +
                    "LEFT JOIN Suppliers s ON f.SupplierId = s.SupplierId " +
                    "LEFT JOIN Origins o ON s.OriginId = o.OriginID " +
                    "WHERE f.FermentableId = @FermentableId;",
                    (f, supplier, origin) =>
                    {
                        if (supplier != null)
                            supplier.Origin = origin;
                        f.Supplier = supplier;
                        return f;
                    }, new { FermentableId = id }, splitOn: "SupplierId,OriginId");
                var fermentable = fermentables.SingleOrDefault();
                if (fermentable == null) return null;
                if (fermentable.SuperFermentableId != null)
                {
                    var superStyle =
                        await context.QueryAsync<Fermentable>("SELECT * FROM Fermentables WHERE FermentableId = @SuperFermentableId;",
                            new { fermentable.SuperFermentableId });
                    fermentable.SuperFermentable = superStyle.SingleOrDefault();
                }
                var subStyles =
                    await context.QueryAsync<Fermentable>("SELECT * FROM Fermentables WHERE SuperFermentableId = @FermentableId;",
                        new { fermentable.FermentableId });
                fermentable.SubFermentables = subStyles.ToList();
                return fermentable;
            }
        }

        public async Task AddAsync(Fermentable fermentable)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var fermentableId =
                            await context.QueryAsync<int>("INSERT Fermentables(Name,SuperFermentableId,EBC,Lovibond,PPG,SupplierId,Type,Custom) " +
                                               "VALUES(@Name,@SuperFermentableId,@EBC,@Lovibond,@PPG,@SupplierId,@Type,@Custom);" +
                                               "SELECT CAST(SCOPE_IDENTITY() as int);",
                                               new
                                               {
                                                   fermentable.Name,
                                                   fermentable.SuperFermentableId,
                                                   fermentable.EBC,
                                                   fermentable.Lovibond,
                                                   fermentable.PPG,
                                                   fermentable.SupplierId,
                                                   fermentable.Type,
                                                   fermentable.Custom,
                                               }, transaction);
                        fermentable.FermentableId = fermentableId.SingleOrDefault();
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

        public async Task<int> UpdateAsync(Fermentable fermentable)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var result = await context.ExecuteAsync("Update Fermentables set Name = @Name,SuperFermentableId = @SuperFermentableId,EBC = @EBC,Lovibond = @Lovibond,PPG= @PPG,SupplierId = @SupplierId,Type = @Type, Custom = @Custom " +
                                        "WHERE FermentableId = @FermentableId;",
                                               new
                                               {
                                                   fermentable.FermentableId,
                                                   fermentable.Name,
                                                   fermentable.SuperFermentableId,
                                                   fermentable.EBC,
                                                   fermentable.Lovibond,
                                                   fermentable.PPG,
                                                   fermentable.SupplierId,
                                                   fermentable.Type,
                                                   fermentable.Custom,
                                               }, transaction);
                        transaction.Commit();
                        return result;
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

        public async Task RemoveAsync(Fermentable fermentable)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        await context.ExecuteAsync("DELETE FROM Fermentables WHERE FermentableId = @FermentableId",
                            new { fermentable.FermentableId }, transaction);
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
