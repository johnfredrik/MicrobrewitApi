using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "srm")]
    public class SRMDto
    {
        [JsonProperty(PropertyName = "srmId")]
        public int Id { get; set; }
        // Malt Calculate Units
        [JsonProperty(PropertyName = "standard")]
        public int Standard { get; set; }
        [JsonProperty(PropertyName = "mosher")]
        public double Mosher { get; set; }
        [JsonProperty(PropertyName = "daniels")]
        public double Daniels { get; set; }
        [JsonProperty(PropertyName = "morey")]
        public double Morey { get; set; }
    }
}