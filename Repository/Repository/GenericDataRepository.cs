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

namespace Microbrewit.Repository
{
    public class GenericDataRepository<T> : IGenericDataRepository<T> where T : class
    {
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

        public virtual IList<T> GetList(Func<T, bool> where,
             params string[] navigationProperties)
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

        public virtual T GetSingle(Func<T, bool> where,
             params string[] navigationProperties)
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

    }
}
