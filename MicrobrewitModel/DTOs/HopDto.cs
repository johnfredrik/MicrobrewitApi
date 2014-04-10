using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class HopDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "aalow")]
        public double AALow { get; set; }
        [JsonProperty(PropertyName = "aahigh")]
        public double AAHigh { get; set; }
        [JsonProperty(PropertyName = "betalow")]
        public double BetaLow { get; set; }
        [JsonProperty(PropertyName = "betahigh")]
        public double BetaHigh { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
        [JsonProperty(PropertyName = "flavourdescription")]
        public String FlavourDescription { get; set; }
        [JsonProperty(PropertyName = "origin")]
        public DTO Origin { get; set; }
        [JsonProperty(PropertyName = "flavours")]
        public IList<DTO> Flavours { get; set; }
        [JsonProperty(PropertyName = "substitutions")]
        public IList<DTO> Substituts { get; set; }
    }
}