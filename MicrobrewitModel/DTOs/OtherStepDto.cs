using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "otherStep")]
    public class OtherStepDto
    {
        [JsonProperty(PropertyName = "otherId")]
        public int OtherId { get; set; }
        [JsonProperty(PropertyName = "stepId")]
        public int StepId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }
    }
}