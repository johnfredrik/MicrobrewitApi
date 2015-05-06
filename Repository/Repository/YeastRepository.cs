using Microbrewit.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microbrewit.Model;

namespace Microbrewit.Repository
{
    public class YeastRepository : IYeastRepository 
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<Yeast> GetAll(params string[] navigationProperties)
        {
            List<Yeast> list;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Yeast> dbQuery = context.Set<Yeast>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<Yeast>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<Yeast>();
            }
            return list;
        }

        public Yeast GetSingle(int id, params string[] navigationProperties)
        {
            Yeast item = null;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Yeast> dbQuery = context.Set<Yeast>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<Yeast>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(s => s.YeastId == id); //Apply where clause
            }
            return item;
        }

        public void Add(Yeast yeast)
        {
            using (var context = new MicrobrewitContext())
            {
                if (yeast.Supplier != null)
                    yeast.Supplier = null;

                context.Entry(yeast).State = EntityState.Added;
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

        public virtual void Update(Yeast yeast)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(yeast).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public virtual void Remove(Yeast yeast)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(yeast).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public virtual async Task<IList<Yeast>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Yeast> dbQuery = context.Set<Yeast>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include<Yeast>(navigationProperty);
                }
                return await dbQuery.ToListAsync();
            }
        }

        public virtual async Task<Yeast> GetSingleAsync(int id, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Yeast> dbQuery = context.Set<Yeast>();

                //Apply eager loading
                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<Yeast>(navigationProperty));

                return await dbQuery.SingleOrDefaultAsync(s => s.YeastId == id);
            }
        }

        public virtual async Task AddAsync(Yeast yeast)
        {
            using (var context = new MicrobrewitContext())
            {
                if (yeast.Supplier != null)
                    yeast.Supplier = null;
                context.Entry(yeast).State = EntityState.Added;
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

        public virtual async Task<int> UpdateAsync(Yeast yeast)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(yeast).State = EntityState.Modified;
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

        public virtual async Task RemoveAsync(Yeast yeast)
        {
            using (var context = new MicrobrewitContext())
            {

                context.Entry(yeast).State = EntityState.Deleted;
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
