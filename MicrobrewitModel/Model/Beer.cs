using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class Beer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public int BreweryId { get; set; }
        public int BeerStyleId { get; set; }
        public int AbvMin { get; set; }
        public int AbvMax { get; set; }
        public int IbuMin { get; set; }
        public int IbuMax { get; set; }
        public int SrmMin { get; set; }
        public int SrmMax { get; set; }

        //public Brewery Brewery { get; set; }
        public BeerStyle BeerStyle { get; set; }

    }
}
