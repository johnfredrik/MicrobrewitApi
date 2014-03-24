using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class HopLinks
    {
        [JsonProperty(PropertyName = "originId")]
        public int? OriginId { get; set; }
        [JsonProperty(PropertyName = "flavourIds")]
        public IList<int> FlavourIds { get; set; }
        [JsonProperty(PropertyName = "substitutesIds")]
        public IList<int> SubstitutesIds { get; set; }
    }
}