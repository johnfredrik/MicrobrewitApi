using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;

namespace Microbrewit.Repository
{
    public class OtherRepository : IOtherRepository
    {

        public IList<Other> GetOthers()
        {
            using (var context = new MicrobrewitContext())   
            {
                return context.Others.ToList();
            }
        }

        public Other GetOther(int id)
        {
            using (var context = new MicrobrewitContext())
            {
                return context.Others.Where(o => o.Id == id).SingleOrDefault();
            }
        }
    }
}
