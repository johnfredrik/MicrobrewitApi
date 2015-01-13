using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class BeerSimpleDto
    {
        [JsonProperty(PropertyName = "beerId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "beerStyle")]
        public DTO BeerStyle { get; set; }
        [JsonProperty(PropertyName = "abv")]
        public ABVDto ABV { get; set; }
        [JsonProperty(PropertyName = "ibu")]
        public IBUDto IBU { get; set; }
        [JsonProperty(PropertyName = "srm")]
        public SRMDto SRM { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get{return "beer";} }
    }
}