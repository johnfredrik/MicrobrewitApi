using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class BrewerySocial
    {
        public int SocialId { get; set; }
        public int BreweryId { get; set; }
        public string Site { get; set; }
        public string Url { get; set; }
        public Brewery Brewery { get; set; }
    }
}
