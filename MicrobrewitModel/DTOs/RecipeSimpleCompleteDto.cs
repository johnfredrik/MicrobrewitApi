using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class RecipeSimpleCompleteDto
    {
        private static readonly string apiPath = ConfigurationManager.AppSettings["api"];
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "recipes")]
        public IList<RecipeSimpleDto> Recipes { get; set; }

        public RecipeSimpleCompleteDto()
        {
            Links = new Links()
            {
                Href = apiPath + "/recipes/:id",
                Type = "recipes"
            };
        }
    }
}