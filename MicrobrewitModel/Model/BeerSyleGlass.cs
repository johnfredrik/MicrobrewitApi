using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class BeerStyleGlass
    {
        public int BeerStyleId { get; set; }
        public int GlassId { get; set; }

        public BeerStyle BeerStyle { get; set; }
        public Glass Glass { get; set; }

    }
}
