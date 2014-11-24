using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "brewer")]
    public class BrewerDto
    {
        [JsonProperty(PropertyName = "brewerId")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string BrewerName { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "brewer"; } }
    }

}