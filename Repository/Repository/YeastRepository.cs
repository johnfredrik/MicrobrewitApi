using Microbrewit.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;

namespace Microbrewit.Repository
{
    public class YeastRepository : GenericDataRepository<Yeast>, IYeastRepository 
    {
        public override void Add(params Yeast[] yeasts)
        {
            using (var context = new MicrobrewitContext())
            {
                foreach (var yeast in yeasts)
                {
                    if (yeast.Supplier != null)
                    {
                        yeast.Supplier = null;
                    }
                }
                base.Add(yeasts);
            }
        }
      
    }
}
