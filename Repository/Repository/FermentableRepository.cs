using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using System.Data.Entity;


namespace Microbrewit.Repository
{
    public class FermentableRepository : GenericDataRepository<Fermentable>, IFermentableRepository
    {
        public override void Add(params Fermentable[] fermentables)
        {
            using (var context = new MicrobrewitContext())
            {
                foreach (Fermentable fermentable in fermentables)
                {
                    if (fermentable.Supplier != null)
                    {
                        fermentable.Supplier = null;
                    }
                }
                base.Add(fermentables);
            }
        }

        public async override Task AddAsync(params Fermentable[] fermentables)
        {
            using (var context = new MicrobrewitContext())
            {
                foreach (Fermentable fermentable in fermentables)
                {
                    if (fermentable.Supplier != null)
                    {
                        fermentable.Supplier = null;
                    }
                }
                await base.AddAsync(fermentables);
            }
        }
    }
}
