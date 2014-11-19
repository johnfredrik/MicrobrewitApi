using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "hopStep")]
    public class HopStepDto
    {
        [JsonProperty(PropertyName = "hopId")]
        public int HopId { get; set; }
        [JsonProperty(PropertyName = "stepId")]
        public int StepId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "amount")]
        public double Amount { get; set; }
        [JsonProperty(PropertyName = "aaValue")]
        public double AAValue { get; set; }
        [JsonProperty(PropertyName = "origin")]
        public DTO Origin { get; set; }
        [JsonProperty(PropertyName = "hopForm")]
        public DTO HopForm { get; set; }
        [JsonProperty(PropertyName = "flavourDescription")]
        public string FlavourDescription { get; set; }
        [JsonProperty(PropertyName = "flavours")]
        public IList<DTO> Flavours { get; set; }


    }
}