using Microbrewit.Model;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "boilStep")]
    public class BoilStepDto
    {
        [Required]
        [JsonProperty(PropertyName = "number")]
        public int Number { get; set; }
        [JsonProperty(PropertyName = "recipeId")]
        public int RecipeId { get; set; }
        [Required]
        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }
        [JsonProperty(PropertyName = "volume")]
        public int Volume { get; set; }
        [JsonProperty(PropertyName = "hops")]
        public IList<HopStepDto> Hops { get; set; }
        [JsonProperty(PropertyName = "fermentables")]
        public IList<FermentableStepDto> Fermentables { get; set; }
        [JsonProperty(PropertyName = "others")]
        public IList<OtherStepDto> Others { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }

    }
}