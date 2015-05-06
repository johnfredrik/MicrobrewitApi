using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Microbrewit.Repository
{
    public class SupplierRepository :  ISupplierRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<Supplier> GetAll(params string[] navigationProperties)
        {
            List<Supplier> list;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Supplier> dbQuery = context.Set<Supplier>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<Supplier>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<Supplier>();
            }
            return list;
        }

        public Supplier GetSingle(int id, params string[] navigationProperties)
        {
            Supplier item = null;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Supplier> dbQuery = context.Set<Supplier>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<Supplier>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(s => s.SupplierId == id); //Apply where clause
            }
            return item;
        }

        public void Add(Supplier supplier)
        {
            using (var context = new MicrobrewitContext())
            {
                if (supplier.Origin != null)
                    supplier.Origin = null;

                context.Entry(supplier).State = EntityState.Added;
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

        public virtual void Update(Supplier supplier)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(supplier).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public virtual void Remove(Supplier supplier)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(supplier).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public virtual async Task<IList<Supplier>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Supplier> dbQuery = context.Set<Supplier>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include<Supplier>(navigationProperty);
                }
                return await dbQuery.ToListAsync();
            }
        }

        public virtual async Task<Supplier> GetSingleAsync(int id, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Supplier> dbQuery = context.Set<Supplier>();

                //Apply eager loading
                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<Supplier>(navigationProperty));

                return await dbQuery.SingleOrDefaultAsync(s => s.SupplierId == id);
            }
        }

        public virtual async Task AddAsync(Supplier supplier)
        {
            using (var context = new MicrobrewitContext())
            {
                if (supplier.Origin != null)
                    supplier.Origin = null;
                context.Entry(supplier).State = EntityState.Added;
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbEntityValidationException dbEx)
                {
                    //foreach (var validationErrors in dbEx.EntityValidationErrors)
                    //{
                    //    foreach (var validationError in validationErrors.ValidationErrors)
                    //    {
                    //        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    //        Log.DebugFormat("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    //        throw dbEx;
                    //    }
                    //}
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }

        public virtual async Task<int> UpdateAsync(Supplier supplier)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(supplier).State = EntityState.Modified;
                try
                {
                    return await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw;
                }

            }
        }

        public virtual async Task RemoveAsync(Supplier supplier)
        {
            using (var context = new MicrobrewitContext())
            {

                context.Entry(supplier).State = EntityState.Deleted;
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
    }
}
