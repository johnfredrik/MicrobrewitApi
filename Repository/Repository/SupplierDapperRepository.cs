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

    public class SupplierDapperRepository : ISupplierRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public IList<Supplier> GetAll(params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var sql = @"SELECT * FROM Suppliers s LEFT JOIN Origins o ON s.OriginId = o.OriginId";
                var suppliers = context.Query<Supplier, Origin, Supplier>(sql, (supplier, origin) =>
                {
                    supplier.Origin = origin;
                    return supplier;
                }, splitOn: "SupplierId,OriginId");
                return suppliers.ToList();
            }
        }

        public Supplier GetSingle(int id, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var sql = @"SELECT * FROM Suppliers s LEFT JOIN Origins o ON s.OriginId = o.OriginId WHERE SupplierId = @SupplierId;";
                var supplier = context.Query<Supplier, Origin, Supplier>(sql, (s, origin) =>
                {
                    s.Origin = origin;
                    return s;
                }, new {SupplierId = id}, splitOn: "SupplierId,OriginId");
                return supplier.SingleOrDefault();
            }
        }

        public void Add(Supplier supplier)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {

                    try
                    {
                       var supplierId = context.Query<int>("INSERT Suppliers(Name,OriginId) VALUES(@Name, @OriginId); SELECT CAST(SCOPE_IDENTITY() as int);",
                            new {supplier.Name,supplier.OriginId},transaction);
                        supplier.SupplierId = supplierId.SingleOrDefault();
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

        public void Update(Supplier supplier)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        context.Execute("UPDATE Suppliers set Name=@Name, OriginId=@OriginId WHERE SupplierId = @SupplierId;",
                            new {supplier.Name, supplier.OriginId,supplier.SupplierId},transaction);
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

        public void Remove(Supplier supplier)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        context.Execute("DELETE FROM Suppliers WHERE SupplierId = @SupplierId", new {supplier.SupplierId}, transaction);
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

        public async Task<IList<Supplier>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var sql = @"SELECT * FROM Suppliers s LEFT JOIN Origins o ON s.OriginId = o.OriginId";
                var suppliers = await context.QueryAsync<Supplier, Origin, Supplier>(sql, (supplier, origin) =>
                {
                    supplier.Origin = origin;
                    return supplier;
                }, splitOn: "SupplierId,OriginId");
                return suppliers.ToList();
            }
        }

        public async Task<Supplier> GetSingleAsync(int id, params string[] navigtionProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var sql = @"SELECT * FROM Suppliers s LEFT JOIN Origins o ON s.OriginId = o.OriginId WHERE SupplierId = @SupplierId;";
                var supplier = await context.QueryAsync<Supplier, Origin, Supplier>(sql, (s, origin) =>
                {
                    s.Origin = origin;
                    return s;
                }, new { SupplierId = id }, splitOn: "SupplierId,OriginId");
                return supplier.SingleOrDefault();
            };
        }

        public async Task AddAsync(Supplier supplier)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var supplierId = await context.QueryAsync<int>("INSERT Suppliers(Name,OriginId) VALUES(@Name, @OriginId); SELECT CAST(SCOPE_IDENTITY() as int);",
                             new { supplier.Name, supplier.OriginId }, transaction);
                        supplier.SupplierId = supplierId.SingleOrDefault();
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

        public async Task<int> UpdateAsync(Supplier supplier)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var result = await context.ExecuteAsync(
                                "UPDATE Suppliers set Name=@Name, OriginId=@OriginId WHERE SupplierId = @SupplierId;",
                                new {supplier.Name, supplier.OriginId, supplier.SupplierId}, transaction);
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

        public async Task RemoveAsync(Supplier supplier)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        await context.ExecuteAsync("DELETE FROM Suppliers WHERE SupplierId = @SupplierId", new { supplier.SupplierId }, transaction);
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
