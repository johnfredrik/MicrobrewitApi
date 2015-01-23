using Nest;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "fermentableStep")]
    public class FermentableStepDto
    {
        [Required]
        [JsonProperty(PropertyName = "fermentableId")]
        public int FermentableId { get; set; }
        [JsonProperty(PropertyName = "stepNumber")]
        public int StepNumber { get; set; }
        [JsonProperty(PropertyName = "recipeId")]
        public int RecipeId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "lovibond")]
        public double Lovibond { get; set; }
        [JsonProperty(PropertyName = "ppg")]
        public double PPG { get; set; }
        [JsonProperty(PropertyName = "supplier")]
        public SupplierDto Supplier { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [Required]
        [JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }
    }
}