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
        public string Name { get; set; }
        [JsonProperty(PropertyName = "aaLow")]
        public double AALow { get; set; }
        [JsonProperty(PropertyName = "aaHigh")]
        public double AAHigh { get; set; }
        public double BetaLow { get; set; }
        public double BetaHigh { get; set; }
        public string Notes { get; set; }
        public String FlavourDescription { get; set; }
        public DTO Origin { get; set; }
        public IList<DTO> Flavours { get; set; }
        public IList<DTO> Substituts { get; set; }
        public string DataType { get { return "hop"; } }
        public bool Custom { get; set; }
    }
}