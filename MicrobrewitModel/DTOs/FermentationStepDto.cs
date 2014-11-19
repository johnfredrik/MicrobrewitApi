using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "fermentationStep")]
    public class FermentationStepDto
    {
        [JsonProperty(PropertyName = "fermentationStepId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "number")]
        public int Number { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }
        [JsonProperty(PropertyName = "volume")]
        public int Volume { get; set; }
        [JsonProperty(PropertyName = "temperature")]
        public int Temperature { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
        [JsonProperty(PropertyName = "hops")]
        public IList<HopStepDto> Hops { get; set; }
        [JsonProperty(PropertyName = "fermentables")]
        public IList<FermentableStepDto> Fermentables { get; set; }
        [JsonProperty(PropertyName = "others")]
        public IList<OtherStepDto> Others { get; set; }
        [JsonProperty(PropertyName = "yeasts")]
        public IList<YeastStepDto> Yeasts { get; set; }
    }
}