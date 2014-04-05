using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class Substitut
    {
        public int HopId { get; set; }
        public int SubstitutId { get; set; }

        public Hop Hop { get; set; }
        public Hop Substitute { get; set; }
    }
}
