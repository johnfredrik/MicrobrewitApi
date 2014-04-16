using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class BreweryBeer
    {
        public int BreweryId { get; set; }
        public int BeerId { get; set; }

        public Beer Beer { get; set; }
        public Brewery Brewery { get; set; }
    }
}
