using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.DTOs
{
    public class BeerCompleteDto
    {
        public Links Links { get; set; }
        public IList<BeerDto> Beers { get; set; }

        public BeerCompleteDto()
        {
            Links = new Links()
            {
                Href = "http://api.microbrew.it/beers/:id",
                Type = "beer"
            };
        }
    }
}
