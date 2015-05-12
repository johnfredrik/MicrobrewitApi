using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Microbrewit.Repository
{
    public class OriginRepository : IOriginRespository
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<Origin> GetAll(params string[] navigationProperties)
        {
            List<Origin> list;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Origin> dbQuery = context.Set<Origin>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<Origin>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<Origin>();
            }
            return list;
        }

        public Origin GetSingle(int id, params string[] navigationProperties)
        {
            Origin item = null;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Origin> dbQuery = context.Set<Origin>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<Origin>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(o => o.OriginId == id); //Apply where clause
            }
            return item;
        }

        public void Add(Origin origin)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(origin).State = EntityState.Added;
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

        public virtual void Update(Origin origin)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(origin).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public virtual void Remove(Origin origin)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(origin).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public virtual async Task<IList<Origin>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Origin> dbQuery = context.Set<Origin>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include<Origin>(navigationProperty);
                }
                return await dbQuery.ToListAsync();
            }
        }

        public virtual async Task<Origin> GetSingleAsync(int id, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Origin> dbQuery = context.Set<Origin>();

                //Apply eager loading
                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<Origin>(navigationProperty));

                return await dbQuery.SingleOrDefaultAsync(o => o.OriginId == id);
            }
        }

        public virtual async Task AddAsync(Origin origin)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(origin).State = EntityState.Added;
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

        public virtual async Task<int> UpdateAsync(Origin origin)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(origin).State = EntityState.Modified;
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

        public virtual async Task RemoveAsync(Origin origin)
        {
            using (var context = new MicrobrewitContext())
            {

                context.Entry(origin).State = EntityState.Deleted;
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
