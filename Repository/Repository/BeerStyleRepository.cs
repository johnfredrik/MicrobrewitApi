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
    public class BeerStyleRepository : IBeerStyleRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<BeerStyle> GetAll(params string[] navigationProperties)
        {
            List<BeerStyle> list;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<BeerStyle> dbQuery = context.Set<BeerStyle>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<BeerStyle>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<BeerStyle>();
            }
            return list;
        }

        public BeerStyle GetSingle(int id, params string[] navigationProperties)
        {
            BeerStyle item = null;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<BeerStyle> dbQuery = context.Set<BeerStyle>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<BeerStyle>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(s => s.BeerStyleId == id); //Apply where clause
            }
            return item;
        }

        public void Add(BeerStyle beerStyle)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(beerStyle).State = EntityState.Added;
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

        public virtual void Update(BeerStyle beerStyle)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(beerStyle).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public virtual void Remove(BeerStyle beerStyle)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(beerStyle).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public virtual async Task<IList<BeerStyle>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<BeerStyle> dbQuery = context.Set<BeerStyle>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include<BeerStyle>(navigationProperty);
                }
                return await dbQuery.ToListAsync();
            }
        }

        public virtual async Task<BeerStyle> GetSingleAsync(int id, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<BeerStyle> dbQuery = context.Set<BeerStyle>();

                //Apply eager loading
                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<BeerStyle>(navigationProperty));

                return await dbQuery.SingleOrDefaultAsync(s => s.BeerStyleId == id);
            }
        }

        public virtual async Task AddAsync(BeerStyle beerStyle)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(beerStyle).State = EntityState.Added;
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

        public virtual async Task<int> UpdateAsync(BeerStyle beerStyle)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(beerStyle).State = EntityState.Modified;
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

        public virtual async Task RemoveAsync(BeerStyle beerStyle)
        {
            using (var context = new MicrobrewitContext())
            {

                context.Entry(beerStyle).State = EntityState.Deleted;
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
