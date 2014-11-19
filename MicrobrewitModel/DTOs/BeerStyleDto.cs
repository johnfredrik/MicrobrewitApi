using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microbrewit.Model;
using Newtonsoft.Json;
using Nest;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "beerStyle")]
    public class BeerStyleDto
    {
        [JsonProperty(PropertyName = "beerStyleId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "superBeerStyle")]
        public DTO SuperBeerStyle { get; set; }
        [JsonProperty(PropertyName = "ogLow")]
        public double OGLow { get; set; }
        [JsonProperty(PropertyName = "ogHigh")]
        public double OGHigh { get; set; }
        [JsonProperty(PropertyName = "fgLow")]
        public double FGLow { get; set; }
        [JsonProperty(PropertyName = "fgHigh")]
        public double FGHigh { get; set; }
        [JsonProperty(PropertyName = "ibuLow")]
        public double IBULow { get; set; }
        [JsonProperty(PropertyName = "ibuHigh")]
        public double IBUHigh { get; set; }
        [JsonProperty(PropertyName = "srmLow")]
        public double SRMLow { get; set; }
        [JsonProperty(PropertyName = "srmHigh")]
        public double SRMHigh { get; set; }
        [JsonProperty(PropertyName = "abvLow")]
        public double ABVLow { get; set; }
        [JsonProperty(PropertyName = "abvHigh")]
        public double ABVHigh { get; set; }
        [JsonProperty(PropertyName = "comments")]
        public string Comments { get; set; }
        [JsonProperty(PropertyName = "subBeerStyles")]
        public IList<DTO> SubBeerStyles { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "beerstyle"; } }
       
    }
}