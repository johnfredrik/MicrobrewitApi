using Microbrewit.Model;
using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "mashStep")]
    public class MashStepDto
    {
        [Required]
        [JsonProperty(PropertyName = "stepNumber")]
        public int StepNumber { get; set; }
        [JsonProperty(PropertyName = "recipeId")]
        public int RecipeId { get; set; }
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
    }
}