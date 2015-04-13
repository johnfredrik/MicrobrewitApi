using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class BreweryMember
    {
        //public int Id { get; set; }
        public string Role { get; set; }
        public string MemberUsername { get; set; }
        public int BreweryId { get; set; }
        public bool Confirmed { get; set; }

        public User Member { get; set; }
        public Brewery Brewery { get; set; }
    }
}
