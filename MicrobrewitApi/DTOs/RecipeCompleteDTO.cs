using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class RecipeCompleteDto
    {
        public Links Links { get; set; }
        public IList<RecipeDto> Recipes { get; set; }

        public RecipeCompleteDto()
        {
            Links = new Links()
            {
                Href = "http://api.microbrew.it/recipes/:id",
                Type = "recipes"
            };
        }
    }
}