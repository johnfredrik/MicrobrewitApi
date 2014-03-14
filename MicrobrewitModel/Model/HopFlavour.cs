using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class HopFlavour
    {
        public int FlavourId { get; set; }
        public int HopId { get; set; }

        public Hop Hop { get; set; }
        public Flavour Flavour { get; set; }
    }
}
