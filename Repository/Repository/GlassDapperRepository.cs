using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microbrewit.Model;

namespace Microbrewit.Repository.Repository
{
    public class GlassDapperRepository : IGlassRepository
    {
        public IList<Glass> GetAll(params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                return context.Query<Glass>(@"SELECT * FROM Glasses").ToList();
            }
        }

        public Glass GetSingle(int id, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                return context.Query<Glass>(@"SELECT * FROM Glasses WHERE GlassId = @GlassId", new { GlassId = id }).SingleOrDefault();
            }
        }

        public void Add(Glass glass)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    var glassId =  context.Query<int>("SELECT MAX(GlassId) FROM Glasses",transaction: transaction);
                    glass.GlassId = glassId.SingleOrDefault() + 1;
                    context.Query<int>(@"INSERT Glasses(GlassId,Name) VALUES(@GlassId,@Name);", new { glass.Name, glass.GlassId }, transaction);
                    transaction.Commit();
                }
            }
        }

        public void Update(Glass glass)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    context.Execute(@"UPDATE Glasses set Name = @Name WHERE GlassId = @GlassId", new { glass.Name, glass.GlassId }, transaction);
                    transaction.Commit();
                }
            }
        }

        public void Remove(Glass glass)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    context.Execute(@"DELETE FROM Glasses WHERE GlassId = @GlassId;", new { glass.GlassId },
                        transaction);
                    transaction.Commit();
                }
            }
        }

        public async Task<IList<Glass>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var glasss = await context.QueryAsync<Glass>(@"SELECT * FROM Glasses");
                return glasss.ToList();
            }
        }

        public async Task<Glass> GetSingleAsync(int id, params string[] navigtionProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var glass =
                    await
                        context.QueryAsync<Glass>(@"SELECT * FROM Glasses WHERE GlassId = @GlassId",
                            new { GlassId = id });
                return glass.SingleOrDefault();
            }
        }

        public async Task AddAsync(Glass glass)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    var glassId = await context.QueryAsync<int>("SELECT MAX(GlassId) FROM Glasses", transaction: transaction);
                    glass.GlassId = glassId.SingleOrDefault() + 1;
                    await context.QueryAsync<int>(@"INSERT Glasses(GlassId,Name) VALUES(@GlassId,@Name);", new { glass.Name, glass.GlassId }, transaction);
                    transaction.Commit();
                }
            }
        }

        public async Task<int> UpdateAsync(Glass glass)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    var result = await context.ExecuteAsync(@"UPDATE Glasses set Name = @Name WHERE GlassId = @GlassId", new { glass.Name, glass.GlassId }, transaction);
                    transaction.Commit();
                    return result;
                }
            }
        }

        public async Task RemoveAsync(Glass glass)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    await context.ExecuteAsync(@"DELETE FROM Glasses WHERE GlassId = @GlassId;", new { glass.GlassId },
                        transaction);
                    transaction.Commit();
                }
            }
        }
    }
}
