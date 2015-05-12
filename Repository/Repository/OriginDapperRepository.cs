using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microbrewit.Model;

namespace Microbrewit.Repository
{
    public class OriginDapperRepository : IOriginRespository
    {
        public IList<Origin> GetAll(params string[] navigationProperties)
        {
            using (var context =  DapperHelper.GetConnection())
            {
                return context.Query<Origin>(@"SELECT * FROM Origins").ToList();
            }
        }

        public Origin GetSingle(int id, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                return context.Query<Origin>(@"SELECT * FROM Origins WHERE OriginId = @OriginId", new {OriginId = id}).SingleOrDefault();
            }
        }

        public void Add(Origin origin)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    var originId = context.Query<int>(@"INSERT Origins(Name) VALUES(@Name); SELECT CAST(SCOPE_IDENTITY() as int)",new {origin.Name},transaction);
                    origin.OriginId = originId.SingleOrDefault();
                    transaction.Commit();
                }
            }
        }

        public void Update(Origin origin)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    context.Execute(@"UPDATE Origins set Name = @Name WHERE OriginId = @OriginId", new {origin.Name, origin.OriginId},transaction);
                    transaction.Commit();
                }
            }
        }

        public void Remove(Origin origin)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    context.Execute(@"DELETE FROM Origins WHERE OriginId = @OriginId;", new {origin.OriginId},
                        transaction);
                    transaction.Commit();
                }
            }
        }

        public async Task<IList<Origin>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var origins = await context.QueryAsync<Origin>(@"SELECT * FROM Origins");
                return origins.ToList();
            }
        }

        public async Task<Origin> GetSingleAsync(int id, params string[] navigtionProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var origin =
                    await
                        context.QueryAsync<Origin>(@"SELECT * FROM Origins WHERE OriginId = @OriginId",
                            new {OriginId = id});
                return origin.SingleOrDefault();
            }
        }

        public async Task AddAsync(Origin origin)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    var originId = await context.QueryAsync<int>(@"INSERT Origins(Name) VALUES(@Name); SELECT CAST(SCOPE_IDENTITY() as int)", new { origin.Name }, transaction);
                    origin.OriginId = originId.SingleOrDefault();
                    transaction.Commit();
                }
            }
        }

        public async Task<int> UpdateAsync(Origin origin)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    var result = await context.ExecuteAsync(@"UPDATE Origins set Name = @Name WHERE OriginId = @OriginId", new { origin.Name, origin.OriginId }, transaction);
                    transaction.Commit();
                    return result;
                }
            }
        }

        public async Task RemoveAsync(Origin origin)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    await context.ExecuteAsync(@"DELETE FROM Origins WHERE OriginId = @OriginId;", new { origin.OriginId },
                        transaction);
                    transaction.Commit();
                }
            }
        }
    }
}
