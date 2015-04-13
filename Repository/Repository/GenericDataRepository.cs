using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microbrewit.Model;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Threading;
using System.Data.Entity.Infrastructure;
using log4net;
using Newtonsoft.Json;

namespace Microbrewit.Repository
{
    public class GenericDataRepository<T> : IGenericDataRepository<T> where T : class
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public virtual IList<T> GetAll(params string[] navigationProperties)
        {
            List<T> list;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<T>();
            }
            return list;
        }

        public virtual IList<T> GetList(Expression<Func<T, bool>> where, params string[] navigationProperties)
        {
            List<T> list;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .ToList<T>();
            }
            return list;
        }

        public virtual T GetSingle(Func<T, bool> where, params string[] navigationProperties)
        {
            T item = null;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(where); //Apply where clause
            }
            return item;
        }

        public virtual void Add(params T[] items)
        {
            using (var context = new MicrobrewitContext())
            {
                foreach (T item in items)
                {
                    context.Entry(item).State = EntityState.Added;
                }

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

        public virtual void Update(params T[] items)
        {
            using (var context = new MicrobrewitContext())
            {
                foreach (T item in items)
                {
                    context.Entry(item).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        public virtual void Remove(params T[] items)
        {
            using (var context = new MicrobrewitContext())
            {
                foreach (T item in items)
                {
                    context.Entry(item).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }

        public virtual async Task<IList<T>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {
                
                IQueryable<T> dbQuery = context.Set<T>();

            //Apply eager loading
            foreach (string navigationProperty in navigationProperties)
            {
                dbQuery = dbQuery.Include<T>(navigationProperty);
            }
            return await dbQuery.ToListAsync();
            }
        }

        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> where, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

            IQueryable<T> dbQuery = context.Set<T>();

            //Apply eager loading
            dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<T>(navigationProperty));

                return await dbQuery.SingleOrDefaultAsync(where);
            }
        }

        public virtual async Task AddAsync(params T[] items)
        {
            using (var context = new MicrobrewitContext())
            {

            foreach (T item in items)
            {
                context.Entry(item).State = EntityState.Added;
            }

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

        public virtual async Task<int> UpdateAsync(params T[] items)
        {
            using (var context = new MicrobrewitContext())
            {

            foreach (T item in items)
            {
               
                context.Entry(item).State = EntityState.Modified;
                
            }
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

        public virtual async Task RemoveAsync(params T[] items)
        {
            using (var context = new MicrobrewitContext())
            {

            foreach (T item in items)
            {
                context.Entry(item).State = EntityState.Deleted;
               
            }
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
