using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Microbrewit.Repository
{
    public class HopRepository : GenericDataRepository<Hop>,IHopRepository
    {
        public override void Add(params Hop[] hops)
        {
            using (var context = new MicrobrewitContext())
            {
                foreach (Hop hop in hops)
                {
                    if (hop.OriginId > 0)
                    {
                        hop.Origin = null;
                    }
                    foreach (var subs in hop.Substituts)
                    {
                        context.Entry(subs).State = EntityState.Unchanged;
                    }
                    //context.Entry(hop).State = EntityState.Added;

                }
                base.Add(hops);
                //context.SaveChanges();
            }
        }

        public async override Task AddAsync(params Hop[] hops)
        {
            using (var context = new MicrobrewitContext())
            {
                foreach (Hop hop in hops)
                {
                    if (hop.OriginId > 0)
                    {
                        hop.Origin = null;
                    }
                    foreach (var subs in hop.Substituts)
                    {
                        context.Entry(subs).State = EntityState.Unchanged;
                    }
                    //context.Entry(hop).State = EntityState.Added;

                }
                await base.AddAsync(hops);
                //context.SaveChanges();
            }
        }
        public Flavour AddFlavour(string name)
        {
            using (var context = new MicrobrewitContext())
            {
                var flavourId = (context.Flavours.Max(f => (int?)f.Id) ?? 0) + 1;
                var flavour = new Flavour() { 
                    Id = flavourId, 
                    Name = name
                };
                context.Flavours.Add(flavour);
                context.SaveChanges();

                return flavour;
            }

        }

        public HopForm GetForm(Expression<Func<HopForm, bool>> where, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {
                IQueryable<HopForm> dbQuery = context.Set<HopForm>();
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<HopForm>(navigationProperty);

                return dbQuery.SingleOrDefault(where);
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
