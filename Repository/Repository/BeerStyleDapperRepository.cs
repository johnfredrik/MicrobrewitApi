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
    public class BeerStyleDapperRepository : IBeerStyleRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<BeerStyle> GetAll(params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var beerStyles = context.Query<BeerStyle>("SELECT * FROM BeerStyles;");
                foreach (var beerStyle in beerStyles)
                {
                    if (beerStyle.SuperStyleId != null)
                    {
                        beerStyle.SuperStyle =
                            context.Query<BeerStyle>("SELECT * FROM BeerStyles WHERE BeerStyleId = @SuperStyleId;",
                                new { beerStyle.SuperStyleId }).SingleOrDefault();
                    }
                    beerStyle.SubStyles =
                        context.Query<BeerStyle>("SELECT * FROM BeerStyles WHERE SuperStyleId = @BeerStyleId;",
                            new { beerStyle.BeerStyleId }).ToList();
                }
                return beerStyles.ToList();
            }
        }

        public BeerStyle GetSingle(int id, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var beerStyles = context.Query<BeerStyle>("SELECT * FROM BeerStyles WHERE BeerStyleId = @BeerStyleId;", new { BeerStyleId = id });
                var beerStyle = beerStyles.SingleOrDefault();
                if (beerStyle == null) return null;
                if (beerStyle.SuperStyleId != null)
                {
                    beerStyle.SuperStyle =
                        context.Query<BeerStyle>("SELECT * FROM BeerStyles WHERE BeerStyleId = @SuperStyleId;",
                            new { beerStyle.SuperStyleId }).SingleOrDefault();
                }
                beerStyle.SubStyles =
                    context.Query<BeerStyle>("SELECT * FROM BeerStyles WHERE SuperStyleId = @BeerStyleId;",
                        new { beerStyle.BeerStyleId }).ToList();
                return beerStyle;
            }
        }

        public void Add(BeerStyle beerStyle)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var beerStyleId =
                            context.Query<int>("INSERT BeerStyles(Name,SuperStyleId,OGLow,OGHigh,FGLow,FGHigh,IBULow,IBUHigh,SRMLow,SRMHigh,ABVLow,ABVHigh,Comments) " +
                                               "VALUES(@Name,@SuperStyleId,@OGLow,@OGHigh,@FGLow,@FGHigh,@IBULow,@IBUHigh,@SRMLow,@SRMHigh,@ABVLow,@ABVHigh,@Comments);" +
                                               "SELECT CAST(SCOPE_IDENTITY() as int);",
                                               new
                                               {
                                                   beerStyle.Name,
                                                   beerStyle.SuperStyleId,
                                                   beerStyle.OGLow,
                                                   beerStyle.OGHigh,
                                                   beerStyle.FGLow,
                                                   beerStyle.FGHigh,
                                                   beerStyle.IBULow,
                                                   beerStyle.IBUHigh,
                                                   beerStyle.SRMLow,
                                                   beerStyle.SRMHigh,
                                                   beerStyle.ABVLow,
                                                   beerStyle.ABVHigh,
                                                   beerStyle.Comments,
                                               }, transaction);
                        beerStyle.BeerStyleId = beerStyleId.SingleOrDefault();
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

        public void Update(BeerStyle beerStyle)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        context.Execute("UPDATE BeerStyles set Name = @Name,SuperStyleId = @SuperStyleId,OGLow = @OGLow,OGHigh = @OGHigh," +
                                        "FGLow = @FGLow,FGHigh = @FGHigh,IBULow = @IBULow,IBUHigh = @IBUHigh, SRMLow = @SRMLow, SRMHigh = @SRMHigh," +
                                        "ABVLow = @ABVLow, ABVHigh = @ABVHigh, Comments = @Comments " +
                                        "WHERE BeerStyleId = @BeerStyleId",
                                        new
                                        {
                                            beerStyle.BeerStyleId,
                                            beerStyle.Name,
                                            beerStyle.SuperStyleId,
                                            beerStyle.OGLow,
                                            beerStyle.OGHigh,
                                            beerStyle.FGLow,
                                            beerStyle.FGHigh,
                                            beerStyle.IBULow,
                                            beerStyle.IBUHigh,
                                            beerStyle.SRMLow,
                                            beerStyle.SRMHigh,
                                            beerStyle.ABVLow,
                                            beerStyle.ABVHigh,
                                            beerStyle.Comments,
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

        public void Remove(BeerStyle beerStyle)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        context.Execute("DELETE FROM BeerStyles WHERE BeerStyleId = @BeerStyleId",
                            new { beerStyle.BeerStyleId }, transaction);
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

        public async Task<IList<BeerStyle>> GetAllAsync(int from, int size, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var beerStyles = await context.QueryAsync<BeerStyle>("SELECT * FROM BeerStyles ORDER BY BeerStyleId OFFSET @From ROWS FETCH NEXT @Size ROWS ONLY;", new {From = from, Size = size});
                foreach (var beerStyle in beerStyles)
                {
                    if (beerStyle.SuperStyleId != null)
                    {
                        var superStyle =
                            await context.QueryAsync<BeerStyle>("SELECT * FROM BeerStyles WHERE BeerStyleId = @SuperStyleId;",
                                new { beerStyle.SuperStyleId });
                        beerStyle.SuperStyle = superStyle.SingleOrDefault();
                    }
                    var subStyles =
                        await context.QueryAsync<BeerStyle>("SELECT * FROM BeerStyles WHERE SuperStyleId = @BeerStyleId;",
                            new { beerStyle.BeerStyleId });
                    beerStyle.SubStyles = subStyles.ToList();
                }
                return beerStyles.ToList();
            }
        }

        public async Task<BeerStyle> GetSingleAsync(int id, params string[] navigtionProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var beerStyles = await context.QueryAsync<BeerStyle>("SELECT * FROM BeerStyles WHERE BeerStyleId = @BeerStyleId;", new { BeerStyleId = id });
                var beerStyle = beerStyles.SingleOrDefault();
                if (beerStyle == null) return null;
                if (beerStyle.SuperStyleId != null)
                {
                    var superStyle =
                        await context.QueryAsync<BeerStyle>("SELECT * FROM BeerStyles WHERE BeerStyleId = @SuperStyleId;",
                            new { beerStyle.SuperStyleId });
                    beerStyle.SuperStyle = superStyle.SingleOrDefault();
                }
                var subStyles =
                    await context.QueryAsync<BeerStyle>("SELECT * FROM BeerStyles WHERE SuperStyleId = @BeerStyleId;",
                        new { beerStyle.BeerStyleId });
                beerStyle.SubStyles = subStyles.ToList();
                return beerStyle;
            }
        }

        public async Task AddAsync(BeerStyle beerStyle)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var beerStyleId =
                            await context.QueryAsync<int>("INSERT BeerStyles(Name,SuperStyleId,OGLow,OGHigh,FGLow,FGHigh,IBULow,IBUHigh,SRMLow,SRMHigh,ABVLow,ABVHigh,Comments) " +
                                               "VALUES(@Name,@SuperStyleId,@OGLow,@OGHigh,@FGLow,@FGHigh,@IBULow,@IBUHigh,@SRMLow,@SRMHigh,@ABVLow,@ABVHigh,@Comments);" +
                                               "SELECT CAST(SCOPE_IDENTITY() as int);",
                                               new
                                               {
                                                   beerStyle.Name,
                                                   beerStyle.SuperStyleId,
                                                   beerStyle.OGLow,
                                                   beerStyle.OGHigh,
                                                   beerStyle.FGLow,
                                                   beerStyle.FGHigh,
                                                   beerStyle.IBULow,
                                                   beerStyle.IBUHigh,
                                                   beerStyle.SRMLow,
                                                   beerStyle.SRMHigh,
                                                   beerStyle.ABVLow,
                                                   beerStyle.ABVHigh,
                                                   beerStyle.Comments,
                                               }, transaction);
                        beerStyle.BeerStyleId = beerStyleId.SingleOrDefault();
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

        public async Task<int> UpdateAsync(BeerStyle beerStyle)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var result = await context.ExecuteAsync("UPDATE BeerStyles set Name = @Name,SuperStyleId = @SuperStyleId,OGLow = @OGLow,OGHigh = @OGHigh," +
                                        "FGLow = @FGLow,FGHigh = @FGHigh,IBULow = @IBULow,IBUHigh = @IBUHigh, SRMLow = @SRMLow, SRMHigh = @SRMHigh," +
                                        "ABVLow = @ABVLow, ABVHigh = @ABVHigh, Comments = @Comments " +
                                        "WHERE BeerStyleId = @BeerStyleId",
                                        new
                                        {
                                            beerStyle.BeerStyleId,
                                            beerStyle.Name,
                                            beerStyle.SuperStyleId,
                                            beerStyle.OGLow,
                                            beerStyle.OGHigh,
                                            beerStyle.FGLow,
                                            beerStyle.FGHigh,
                                            beerStyle.IBULow,
                                            beerStyle.IBUHigh,
                                            beerStyle.SRMLow,
                                            beerStyle.SRMHigh,
                                            beerStyle.ABVLow,
                                            beerStyle.ABVHigh,
                                            beerStyle.Comments,
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

        public async Task RemoveAsync(BeerStyle beerStyle)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        await context.ExecuteAsync("DELETE FROM BeerStyles WHERE BeerStyleId = @BeerStyleId",
                            new { beerStyle.BeerStyleId }, transaction);
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
