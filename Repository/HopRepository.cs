using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;

namespace Microbrewit.Repository
{
    public class HopRepository : IHopRepository
    {
        public IList<Model.Hop> GetHops()
        {
            using (var context = new MicrobrewitContext())
            {
                return context.Hops.Include("Origin").Include("HopFlavours.Flavour").Include("MashSteps").Include("FermentationSteps").ToList();
            }
        }

        public Model.Hop GetHop(int hopId)
        {
            using (var context = new MicrobrewitContext())
            {
                return context.Hops.Include("Origin").Include("HopFlavours.Flavour").Where(h => h.Id == hopId).SingleOrDefault();
            }
        }
    }
}
