using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "other")]
    public class OtherDto
    {
        [JsonProperty(PropertyName = "otherId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "other"; } }
        [JsonProperty(PropertyName = "custom")]
        public bool Custom { get; set; }
    }
}