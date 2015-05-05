using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

namespace Microbrewit.Repository
{
    public class HopRepository : IHopRepository
    {
        public IList<Hop> GetAll(params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Hop> dbQueryable = context.Set<Hop>();
                dbQueryable = navigationProperties.Aggregate(dbQueryable, (current, navigationProperty) => current.Include(navigationProperty));
                return dbQueryable.ToList();
            }
        }

        public IList<Hop> GetList(Expression<Func<Hop, bool>> @where, params string[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public Hop GetSingle(int id, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Hop> dbQueryable = context.Set<Hop>();
                dbQueryable = navigationProperties.Aggregate(dbQueryable, (current, navigationProperty) => current.Include(navigationProperty));
                return dbQueryable.SingleOrDefault(h => h.HopId == id);
            }
        }

        public void Add(Hop hop)
        {
            using (var context = new MicrobrewitContext())
            {
                    if (hop.OriginId > 0)
                    {
                        hop.Origin = null;
                    }
                    foreach (var subs in hop.Substituts)
                    {
                        context.Entry(subs).State = EntityState.Unchanged;
                    }
                 context.Hops.Add(hop);
                 context.SaveChanges();
            }
        }

        public void Update(Hop hop)
        {
            using (var context = new MicrobrewitContext())
            {
                var dbHop = context.Hops.SingleOrDefault(h => h.HopId == hop.HopId);
                if(dbHop == null) throw new DbUpdateException("Hop does not exist.");
                context.Entry(dbHop).CurrentValues.SetValues(hop);
                context.SaveChanges();
            }
        }

        public void Remove(Hop hop)
        {
            using (var context = new MicrobrewitContext())
            {
                var dbHop = context.Hops.SingleOrDefault(h => h.HopId == hop.HopId);
                if(dbHop == null) throw new DbUpdateException("Hop does not exist.");
                context.Hops.Remove(dbHop);
                context.SaveChanges();
            }
        }

        public async Task<IList<Hop>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Hop> dbQueryable = context.Set<Hop>();
                dbQueryable = navigationProperties.Aggregate(dbQueryable, (current, navigationProperty) => current.Include(navigationProperty));
                return await dbQueryable.ToListAsync();
            }
        }

        public async Task<Hop> GetSingleAsync(int id, params string[] navigtionProperties)
        {
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Hop> dbQueryable = context.Set<Hop>();
                dbQueryable = navigtionProperties.Aggregate(dbQueryable, (current, navigationProperty) => current.Include(navigationProperty));
                return await dbQueryable.SingleOrDefaultAsync(h => h.HopId == id);
            }
        }

        public async Task AddAsync(Hop hop)
        {
            using (var context = new MicrobrewitContext())
            {
                    if (hop.OriginId > 0)
                    {
                        hop.Origin = null;
                    }
                    foreach (var subs in hop.Substituts)
                    {
                        context.Entry(subs).State = EntityState.Unchanged;
                    }
                context.Hops.Add(hop);
                await context.SaveChangesAsync();
            }
        }

        public async Task<int> UpdateAsync(Hop hop)
        {
            using (var context = new MicrobrewitContext())
            {
                var dbHop = context.Hops.SingleOrDefault(h => h.HopId == hop.HopId);
                if (dbHop == null) throw new DbUpdateException("Hop does not exist.");
                context.Entry(dbHop).CurrentValues.SetValues(hop);
                return await context.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(Hop hop)
        {
            using (var context = new MicrobrewitContext())
            {
                var dbHop = context.Hops.SingleOrDefault(h => h.HopId == hop.HopId);
                if (dbHop == null) throw new DbUpdateException("Hop does not exist.");
                context.Entry(dbHop).CurrentValues.SetValues(hop);
                await context.SaveChangesAsync();
            }
        }

        public Flavour AddFlavour(string name)
        {
            using (var context = new MicrobrewitContext())
            {
                var flavourId = (context.Flavours.Max(f => (int?)f.FlavourId) ?? 0) + 1;
                var flavour = new Flavour() { 
                    FlavourId = flavourId, 
                    Name = name
                };
                context.Flavours.Add(flavour);
                context.SaveChanges();

                return flavour;
            }

        }

        public HopForm GetForm(int id)
        {
            using (var context = new MicrobrewitContext())
            {

                return context.HopForms.SingleOrDefault(h => h.Id == id);
            }
        }

        public async Task<IList<HopForm>> GetHopFormsAsync()
        {
            using(var context = new MicrobrewitContext())
            {
                return await context.HopForms.ToListAsync();
            }
        }

        public IList<HopForm> GetHopForms()
        {
            using (var context = new MicrobrewitContext())
            {
                return context.HopForms.ToList();
            }
        }
    }
}
