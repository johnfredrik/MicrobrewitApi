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

    public class OtherDapperRepository : IOtherRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public IList<Other> GetAll(params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                return context.Query<Other>(@"SELECT * FROM Others").ToList();
            }
        }

        public Other GetSingle(int id, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                return context.Query<Other>("SELECT * FROM Others WHERE OtherId = @OtherId", new {OtherId = id}).SingleOrDefault();
            }
        }

        public void Add(Other other)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {

                    try
                    {
                       var otherId = context.Query<int>("INSERT Others(Name,Type,Custom) VALUES(@Name, @Type, @Custom); SELECT CAST(SCOPE_IDENTITY() as int)",
                            new {other.Name, other.Type, other.Custom},transaction);
                        other.OtherId = otherId.SingleOrDefault();
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

        public void Update(Other other)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        context.Execute("UPDATE Others set Name=@Name, Type=@Type, Custom=@Custom WHERE OtherId = @Id;",
                            new {other.Name, other.Type, other.Custom,Id = other.OtherId },transaction);
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

        public void Remove(Other other)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        context.Execute("DELETE FROM Others WHERE OtherId = @OtherId", new {other.OtherId}, transaction);
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

        public async Task<IList<Other>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var others = await context.QueryAsync<Other>(@"SELECT * FROM Others");
                return others.ToList();
            }
        }

        public async Task<Other> GetSingleAsync(int id, params string[] navigtionProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var other = await  context.QueryAsync<Other>("SELECT * FROM Others WHERE OtherId = @OtherId", new { OtherId = id });
                return other.SingleOrDefault();
            };
        }

        public async Task AddAsync(Other other)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var otherId = await context.QueryAsync<int>("INSERT Others(Name,Type,Custom) VALUES(@Name, @Type, @Custom); SELECT CAST(SCOPE_IDENTITY() as int)",
                             new { other.Name, other.Type, other.Custom }, transaction);
                        other.OtherId = otherId.SingleOrDefault();
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

        public async Task<int> UpdateAsync(Other other)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var result = await context.ExecuteAsync(
                                "UPDATE Others set Name=@Name, Type=@Type, Custom=@Custom WHERE OtherId = @Id;",
                                new {other.Name, other.Type, other.Custom, Id = other.OtherId}, transaction);
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

        public async Task RemoveAsync(Other other)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        await context.ExecuteAsync("DELETE FROM Others WHERE OtherId = @OtherId", new { other.OtherId }, transaction);
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
