using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class Substitute
    {
        public int HopId { get; set; }
        public int SubstituteId { get; set; }

        public Hop Hop { get; set; }
        public Hop Sub { get; set; }
    }
}
