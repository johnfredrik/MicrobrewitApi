using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microbrewit.Model;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class BeerStyleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
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
        public string Comments { get; set; }
        public IList<DTO> SubBeerStyles { get; set; }
        public string DataType { get { return "beerstyle"; } }
       
    }
}