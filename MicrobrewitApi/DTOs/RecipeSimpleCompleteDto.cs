using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class RecipeSimpleCompleteDto
    {
        public Links Links { get; set; }
        public IList<RecipeSimpleDto> Recipes { get; set; }

        public RecipeSimpleCompleteDto()
        {
            Links = new Links()
            {
                Href = "http://api.microbrew.it/recipes/:id",
                Type = "recipes"
            };
        }
    }
}