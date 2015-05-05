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
    public class OtherRepository : IOtherRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<Other> GetAll(params string[] navigationProperties)
        {
            List<Other> list;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Other> dbQuery = context.Set<Other>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<Other>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<Other>();
            }
            return list;
        }

        public Other GetSingle(int id, params string[] navigationProperties)
        {
            Other item = null;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Other> dbQuery = context.Set<Other>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<Other>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(o => o.OtherId == id); //Apply where clause
            }
            return item;
        }

        public void Add(Other other)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(other).State = EntityState.Added;
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

        public virtual void Update(Other other)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(other).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public virtual void Remove(Other other)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(other).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public virtual async Task<IList<Other>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Other> dbQuery = context.Set<Other>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include<Other>(navigationProperty);
                }
                return await dbQuery.ToListAsync();
            }
        }

        public virtual async Task<Other> GetSingleAsync(int id, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Other> dbQuery = context.Set<Other>();

                //Apply eager loading
                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<Other>(navigationProperty));

                return await dbQuery.SingleOrDefaultAsync(o => o.OtherId == id);
            }
        }

        public virtual async Task AddAsync(Other origin)
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

        public virtual async Task<int> UpdateAsync(Other origin)
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

        public virtual async Task RemoveAsync(Other origin)
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
