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
        public int? BeerStyleId { get; set; }
        
        public BeerStyle BeerStyle { get; set; }
        public SRM SRM { get; set; }
        public ABV ABV { get; set; }
        public IBU IBU { get; set; }
        public Recipe Recipe { get; set; }
        public int? ForkeOfId { get; set; }
        public Beer ForkeOf { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ICollection<Beer> Forks { get; set; }
      
        public ICollection<BreweryBeer> Breweries { get; set; }
        public ICollection<UserBeer> Brewers { get; set; }

    }
}
