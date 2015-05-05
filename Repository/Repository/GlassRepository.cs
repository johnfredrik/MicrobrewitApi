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
    public class GlassRepository :IGlassRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<Glass> GetAll(params string[] navigationProperties)
        {
            List<Glass> list;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Glass> dbQuery = context.Set<Glass>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<Glass>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<Glass>();
            }
            return list;
        }

        public Glass GetSingle(int id, params string[] navigationProperties)
        {
            Glass item = null;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Glass> dbQuery = context.Set<Glass>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<Glass>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(o => o.GlassId == id); //Apply where clause
            }
            return item;
        }

        public void Add(Glass glass)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(glass).State = EntityState.Added;
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

        public virtual void Update(Glass glass)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(glass).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public virtual void Remove(Glass glass)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(glass).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public virtual async Task<IList<Glass>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Glass> dbQuery = context.Set<Glass>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include<Glass>(navigationProperty);
                }
                return await dbQuery.ToListAsync();
            }
        }

        public virtual async Task<Glass> GetSingleAsync(int id, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Glass> dbQuery = context.Set<Glass>();

                //Apply eager loading
                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<Glass>(navigationProperty));

                return await dbQuery.SingleOrDefaultAsync(o => o.GlassId == id);
            }
        }

        public virtual async Task AddAsync(Glass glass)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(glass).State = EntityState.Added;
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

        public virtual async Task<int> UpdateAsync(Glass glass)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(glass).State = EntityState.Modified;
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

        public virtual async Task RemoveAsync(Glass glass)
        {
            using (var context = new MicrobrewitContext())
            {

                context.Entry(glass).State = EntityState.Deleted;
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
