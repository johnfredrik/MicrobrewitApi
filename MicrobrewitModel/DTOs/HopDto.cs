using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "hop")]
    public class HopDto
    {
        [JsonProperty(PropertyName = "hopId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "aaLow")]
        public double AALow { get; set; }
        [JsonProperty(PropertyName = "aaHigh")]
        public double AAHigh { get; set; }
        [JsonProperty(PropertyName = "betaLow")]
        public double BetaLow { get; set; }
        [JsonProperty(PropertyName = "betaHigh")]
        public double BetaHigh { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
        [JsonProperty(PropertyName = "flavourDescription")]
        public String FlavourDescription { get; set; }
        [JsonProperty(PropertyName = "origin")]
        public DTO Origin { get; set; }
        [JsonProperty(PropertyName = "flavours")]
        public IList<DTO> Flavours { get; set; }
        [JsonProperty(PropertyName = "substituts")]
        public IList<DTO> Substituts { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "hop"; } }
        [JsonProperty(PropertyName = "custom")]
        public bool Custom { get; set; }
    }
}