using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class BeerSimpleCompleteDto
    {
        public Links Links { get; set; }
        public IList<BeerSimpleDto> Beers { get; set; }

        public BeerSimpleCompleteDto()
        {
            Links = new Links()
            {
                Href = "http://api.microbrew.it/beers/:id",
                Type = "beer"
            };
        }
    }
}