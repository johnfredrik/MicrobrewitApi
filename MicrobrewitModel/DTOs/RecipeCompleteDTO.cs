using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class RecipeCompleteDto
    {
        private static readonly string apiPath = ConfigurationManager.AppSettings["api"];
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "recipes")]
        public IList<RecipeDto> Recipes { get; set; }

        public RecipeCompleteDto()
        {
            Links = new Links()
            {
                Href = apiPath + "/recipes/:id",
                Type = "recipes"
            };
        }
    }
}