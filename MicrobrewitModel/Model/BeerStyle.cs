using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class BeerStyle
    {
        public int BeerStyleId { get; set; }
        public string Name { get; set; }
        public double OGLow { get; set; }
        public double OGHigh { get; set; }
        public double FGLow { get; set; }
        public double FGHigh { get; set; }
        public double IBULow { get; set; }
        public double IBUHigh { get; set; }
        public double SRMLow { get; set; }
        public double SRMHigh { get; set; }
        public double ABVLow { get; set; }
        public double ABVHigh { get; set; }
        public string Comments { get; set; }


        public int? SuperStyleId { get; set; }
        public BeerStyle SuperStyle { get; set; }
        public ICollection<BeerStyle> SubStyles { get; set; }

        public ICollection<Beer> Beers { get; set; }
        public ICollection<BeerStyleGlass> Glasses { get; set; }

    }
}
