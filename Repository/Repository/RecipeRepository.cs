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
                return context.Recipes.ToList();
                    
            }
        }

        public Recipe GetRecipe(int recipeId)
        {
            using (var context = new MicrobrewitContext())
            {
                return context.Recipes
                    .Include("MashSteps.Hops.Hop.Origin")
                    .Include("MashSteps.Fermentables.Fermentable.Supplier.Origin")
                    .Include("MashSteps.Others.Other")
                    .Include("BoilSteps.Hops.Hop.Origin")
                    .Include("BoilSteps.Fermentables.Fermentable.Supplier.Origin")
                    .Include("BoilSteps.Others.Other")
                    .Include("FermentationSteps.Hops.Hop.Origin")
                    .Include("FermentationSteps.Fermentables.Fermentable.Supplier.Origin")
                    .Include("FermentationSteps.Others.Other")
                    .Include("BeerStyle")
                    .Include("Brewers")
                    .Where(r => r.Id == recipeId).SingleOrDefault();
            }
        }
    }
}
