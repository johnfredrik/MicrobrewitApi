using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class SpargeStepDto
    {
        [Required]
        [JsonProperty(PropertyName = "stepNumber")]
        public int StepNumber { get; set; }
        [JsonProperty(PropertyName = "recipeId")]
        public int RecipeId { get; set; }
        [JsonProperty(PropertyName = "temperature")]
        public int Temperature { get; set; }
        [Required]
        [JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "hops")]
        public IList<HopStepDto> Hops { get; set; }
       
    }
}
