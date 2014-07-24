using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class BeerStyleCompleteDto
    {
        private static readonly string apiPath = ConfigurationManager.AppSettings["api"];
        //public Meta Meta { get; set; }
        public Links Links { get; set; }
        public IList<BeerStyleDto> BeerStyles { get; set; }

        public BeerStyleCompleteDto()
        {
            Links = new Links()
            {
                Href = apiPath + "/beerstyles/:id",
                Type = "beerstyle"
            };
        }
    }
}