using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using log4net;


namespace Microbrewit.Repository
{
    public class FermentableRepository : IFermentableRepository
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<Fermentable> GetAll(params string[] navigationProperties)
        {
            List<Fermentable> list;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Fermentable> dbQuery = context.Set<Fermentable>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<Fermentable>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<Fermentable>();
            }
            return list;
        }

        public Fermentable GetSingle(int id, params string[] navigationProperties)
        {
            Fermentable item = null;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Fermentable> dbQuery = context.Set<Fermentable>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<Fermentable>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(s => s.FermentableId == id); //Apply where clause
            }
            return item;
        }

        public void Add(Fermentable fermentable)
        {
            using (var context = new MicrobrewitContext())
            {
                if (fermentable.Supplier != null)
                    fermentable.Supplier = null;
                context.Entry(fermentable).State = EntityState.Added;
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
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                                validationError.ErrorMessage);
                            Log.DebugFormat("Property: {0} Error: {1}", validationError.PropertyName,
                                validationError.ErrorMessage);
                        }
                    }
                }
            }
        }

        public virtual void Update(Fermentable fermentable)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(fermentable).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public virtual void Remove(Fermentable fermentable)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(fermentable).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public virtual async Task<IList<Fermentable>> GetAllAsync(int from, int size,params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Fermentable> dbQuery = context.Set<Fermentable>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include<Fermentable>(navigationProperty);
                }
                return await dbQuery.ToListAsync();
            }
        }

        public virtual async Task<Fermentable> GetSingleAsync(int id, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Fermentable> dbQuery = context.Set<Fermentable>();

                //Apply eager loading
                dbQuery = navigationProperties.Aggregate(dbQuery,
                    (current, navigationProperty) => current.Include<Fermentable>(navigationProperty));

                return await dbQuery.SingleOrDefaultAsync(s => s.FermentableId == id);
            }
        }

        public virtual async Task AddAsync(Fermentable fermentable)
        {
            using (var context = new MicrobrewitContext())
            {
                if (fermentable.Supplier != null)
                    fermentable.Supplier = null;
                context.Entry(fermentable).State = EntityState.Added;
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

        public virtual async Task<int> UpdateAsync(Fermentable fermentable)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(fermentable).State = EntityState.Modified;
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

        public virtual async Task RemoveAsync(Fermentable fermentable)
        {
            using (var context = new MicrobrewitContext())
            {

                context.Entry(fermentable).State = EntityState.Deleted;
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
