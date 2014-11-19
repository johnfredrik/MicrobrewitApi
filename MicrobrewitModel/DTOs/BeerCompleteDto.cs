using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "beerComplete")]
    public class BeerCompleteDto
    {
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "beers")]
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
