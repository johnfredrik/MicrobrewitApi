using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using System.Data.Entity;

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
                    context.Entry(hop).State = EntityState.Added;

                }
                context.SaveChanges();
            }
        }


        public Flavour AddFlavour(string name)
        {
            using (var context = new MicrobrewitContext())
            {
                var flavourId = context.Flavours.Max(f => f.Id) + 1;
                var flavour = new Flavour() { 
                    Id = flavourId, 
                    Name = name
                };
                context.Flavours.Add(flavour);
                context.SaveChanges();

                return flavour;
            }

        }

    }
}
