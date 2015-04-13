using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microbrewit.Model;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Nest;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "recipe")]
    public class RecipeDto
    {
        [JsonProperty(PropertyName = "recipeId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string  Notes { get; set; }
        [Required]
        [JsonProperty(PropertyName = "volume")]
        public int Volume { get; set; }
        [JsonProperty(PropertyName = "og")]
        public double OG { get; set; }
        [JsonProperty(PropertyName = "fg")]
        public double FG { get; set; }
        [JsonProperty(PropertyName = "efficiency")]
        public double Efficiency { get; set; }
        [JsonProperty(PropertyName = "totalBoilTime")]
        public double TotalBoilTime { get; set; }
        [Required]
        [JsonProperty(PropertyName = "mashSteps")]
        public IList<MashStepDto> MashSteps { get; set; }
        [JsonProperty(PropertyName = "spargeStep")]
        public SpargeStepDto SpargeStep { get; set; }
        [JsonProperty(PropertyName = "boilSteps")]
        public IList<BoilStepDto> BoilSteps { get; set; }
        [Required]
        [JsonProperty(PropertyName = "fermentationSteps")]
        public IList<FermentationStepDto> FermentationSteps { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "recipe"; } }
    }
}