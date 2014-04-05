using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class Flavour
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Hop> Hops { get; set; }
    }
}
