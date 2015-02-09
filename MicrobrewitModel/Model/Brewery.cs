using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class Brewery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Website { get; set; }
        public string Established { get; set; }
        public string HeaderImage { get; set; }
        public string Avatar { get; set; }

        public ICollection<BreweryMember> Members { get; set; }
        public ICollection<BreweryBeer> Beers { get; set; }
        public ICollection<BrewerySocial> Socials { get; set; }

    }
}
