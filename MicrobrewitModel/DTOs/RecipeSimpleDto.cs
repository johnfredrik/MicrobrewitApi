using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class RecipeSimpleDto
    {
        [JsonProperty(PropertyName = "recipeId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
        [JsonProperty(PropertyName = "volume")]
        public int Volume { get; set; }
        [JsonProperty(PropertyName = "beerStyle")]
        public DTO BeerStyle { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "recipe"; } }

        //public IList<DTO> MashSteps { get; set; }
        //public IList<DTO> BoilSteps { get; set; }
        //public IList<FermentationStepDto> FermentationSteps { get; set; }
    }
}