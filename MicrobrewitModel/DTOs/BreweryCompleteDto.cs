using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class BreweryCompleteDto
    {
        private static readonly string apiPath = ConfigurationManager.AppSettings["api"];
        [JsonProperty(PropertyName = "links")]
        public LinksBrewery Links { get; set; }
        [JsonProperty(PropertyName = "breweries")]
        public IList<BreweryDto> Breweries { get; set; }

        public BreweryCompleteDto()
        {
            Links = new LinksBrewery()
            {
                Beer = new Links() 
                {
                    Href = apiPath + "/users/:username",
                    Type = "user"
                },
                User = new Links()
                {
                    Href = apiPath + "beers/:id",
                    Type = "beer"
                }

            };
        }
    }
}
