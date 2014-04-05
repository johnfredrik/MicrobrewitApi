using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.DTOs
{
    public class BreweryCompleteDto
    {
        public LinksBrewery Links { get; set; }
        public IList<BreweryDto> Breweries { get; set; }

        public BreweryCompleteDto()
        {
            Links = new LinksBrewery()
            {
                Beer = new Links() 
                {
                    Href = "http://api.microbrew.it/users/:username",
                    Type = "user"
                },
                User = new Links()
                {
                    Href = "http://api.microbrew.it/beers/:id",
                    Type = "beer"
                }

            };
        }
    }
}
