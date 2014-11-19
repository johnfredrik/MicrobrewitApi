using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class BeerSimpleCompleteDto
    {
        private static readonly string apiPath = ConfigurationManager.AppSettings["api"];
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "beers")]
        public IList<BeerSimpleDto> Beers { get; set; }

        public BeerSimpleCompleteDto()
        {
            Links = new Links()
            {
                Href = apiPath + "/beers/:id",
                Type = "beer"
            };
        }
    }
}