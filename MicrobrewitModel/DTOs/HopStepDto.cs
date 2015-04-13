using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "hopStep")]
    public class HopStepDto
    {
        
        [JsonProperty(PropertyName = "hopId")]
        [Required]
        public int HopId { get; set; }
        [JsonProperty(PropertyName = "stepNumber")]
        public int StepNumber { get; set; }
        [JsonProperty(PropertyName = "recipeId")]
        public int RecipeId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "amount")]
        [Required]
        public double Amount { get; set; }
        [JsonProperty(PropertyName = "aaValue")]
        [Required]
        public double AAValue { get; set; }
        [JsonProperty(PropertyName = "origin")]
        public DTO Origin { get; set; }
        [Required]
        [JsonProperty(PropertyName = "hopForm")]
        public DTO HopForm { get; set; }
        [JsonProperty(PropertyName = "flavourDescription")]
        public string FlavourDescription { get; set; }
        [JsonProperty(PropertyName = "flavours")]
        public IList<DTO> Flavours { get; set; }


    }
}