using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;

namespace Microbrewit.Repository
{
    public class FermentableRepository : IFermentableRepository
    {

        public IList<Fermentable> GetFermentables()
        {
            using (var context = new MicrobrewitContext())
            {
                return context.Fermentables
                    .Include("Supplier.Origin")
                    .ToList();
            }
        }

        public Fermentable GetFermentable(int fermentableId)
        {
            using (var context = new MicrobrewitContext())
            {
                return context.Fermentables
                    .Include("Supplier.Origin")
                    .Where(f => f.Id == fermentableId)
                    .SingleOrDefault();
            }
        }


        public IList<Grain> GetGrains()
        {
            using (var context = new MicrobrewitContext())
            {
                return context.Fermentables
                    .Include("Supplier.Origin")
                    .OfType<Grain>()
                    .ToList();
            }
        }

        public IList<Sugar> GetSugars()
        {
            using (var context = new MicrobrewitContext())
            {
                return context.Fermentables
                                    .Include("Supplier.Origin")
                                    .OfType<Sugar>()
                                    .ToList();
            }
        }

        public IList<DryExtract> GetDryExtracts()
        {
            using (var context = new MicrobrewitContext())
            {
                return context.Fermentables
                    .Include("Supplier.Origin")
                    .OfType<DryExtract>()
                    .ToList();
            }
        }

        public IList<LiquidExtract> GetLiquidExtracts()
        {
           using(var context = new MicrobrewitContext())
	       {
		 
                return context.Fermentables
                        .Include("Supplier.Origin")
                        .OfType<LiquidExtract>()
                        .ToList();
	        }
        }

    }
}
