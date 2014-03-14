using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public class RecipeRepository : IRecipeRepositoy
    {
        public IList<Recipe> GetRecipes()
        {
            using (var context = new MicrobrewitContext())
            {
                return context.Recipes.Include("RecipeHops.Hop").ToList();
            }
        }

        public Recipe GetRecipe(int recipeId)
        {
            using (var context = new MicrobrewitContext())
            {
                return context.Recipes.Include("RecipeHops.Hop.Origin").Where(r => r.Id == recipeId).SingleOrDefault();
            }
        }
    }
}
