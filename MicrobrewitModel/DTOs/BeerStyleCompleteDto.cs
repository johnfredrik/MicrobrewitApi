using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class BeerStyleCompleteDto
    {
        //public Meta Meta { get; set; }
        public Links Links { get; set; }
        public IList<BeerStyleDto> BeerStyles { get; set; }

        public BeerStyleCompleteDto()
        {
            Links = new Links()
            {
                Href = "http://api.microbrew.it/beerstyles/:id",
                Type = "beerstyle"
            };
        }
    }
}