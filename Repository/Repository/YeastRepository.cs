using Microbrewit.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;

namespace Microbrewit.Repository
{
    public class YeastRepository : IYeastRepository
    {
        public IList<Model.Yeast> GetYeasts()
        {
            using (var context = new MicrobrewitContext())
            {
                return context.Yeasts.Include("Supplier").ToList();
            }
        }

        public Model.Yeast GetYeast(int id)
        {
            using (var context = new MicrobrewitContext())
            {
                return context.Yeasts.Include("Supplier").Where(y => y.Id == id).SingleOrDefault();
            }
        }
    }
}
