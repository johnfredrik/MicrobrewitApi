using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class RecipeCompleteDto
    {
        private static readonly string apiPath = ConfigurationManager.AppSettings["api"];
        public Links Links { get; set; }
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