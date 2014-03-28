using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microbrewit.Model;
using Microbrewit.Repository;
using log4net;
using System.Linq.Expressions;
using Microbrewit.Api.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("api/recipes")]
    public class RecipeController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private IRecipeRepositoy recipeRepository = new RecipeRepository();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        

        // GET api/Recipe
        [Route("")]
        public RecipeSimpleCompleteDto GetRecipes()
        {
            

            //var recipes = db.Recipes.Include("RecipeHops.Hop").Project().To<RecipeDto>();
            var recipes = recipeRepository.GetAll("Beer.BeerStyle");
            var recipesDto =  Mapper.Map<IList<Recipe>, IList<RecipeSimpleDto>>(recipes);
            var result = new RecipeSimpleCompleteDto();
            result.Recipes = recipesDto;
            return result;

           
        }


        // GET api/Recipe/5
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(RecipeCompleteDto))]
        public IHttpActionResult GetRecipe(int id)
        {
            

           //Recipe recipe = await db.Recipes.Where(r => r.Id == id).FirstOrDefaultAsync();
            RecipeDto recipe = Mapper.Map<Recipe, RecipeDto>(recipeRepository.GetSingle(r => r.Id == id,
                "Beer.BeerStyle", "Mashsteps.Fermentables.Fermentable", "MashSteps.Others.Other", "Mashsteps.Hops.Hop", "BoilSteps.Hops.Hop.Origin",
                "BoilSteps.Fermentables.Fermentable", "BoilSteps.Others.Other", "FermentationSteps.Fermentables.Fermentable", "FermentationSteps.Hops.Hop", "FermentationSteps.Others.Other",
                "FermentationSteps.Yeasts.Yeast.Supplier"));
            //Recipe recipe = recipeRepository.GetRecipe(id);
            if (recipe == null)
            {
                return NotFound();
            }
            var result = new RecipeCompleteDto() { Recipes = new List<RecipeDto>() };
            result.Recipes.Add(recipe);
            return Ok(result);
        }

        // PUT api/Recipe/5
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutRecipe(int id, Recipe recipe)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != recipe.Id)
            {
                return BadRequest();
            }

            db.Entry(recipe).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Recipe
        [Route("")]
        [ResponseType(typeof(Recipe))]
        public async Task<IHttpActionResult> PostRecipe(Recipe recipe)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (!ModelState.IsValid)
            {
                Log.Debug("Modelstate failed");
                return BadRequest(ModelState);
            }
         
            db.Recipes.Add(recipe);
            await db.SaveChangesAsync();
            
            return CreatedAtRoute("DefaultApi", new { controller = "recipes",id = recipe.Id }, recipe);
        }

        // DELETE api/Recipe/5
        [Route("{id:int}")]
        [ResponseType(typeof(Recipe))]
        public async Task<IHttpActionResult> DeleteRecipe(int id)
        {
            Recipe recipe = await db.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            db.Recipes.Remove(recipe);
            await db.SaveChangesAsync();

            return Ok(recipe);
        }

        [Route("test")]
        public IList<Recipe> GetRecipesTest()
        {


            //var recipes = db.Recipes.Include("RecipeHops.Hop").Project().To<RecipeDto>();


            return recipeRepository.GetAll("Mashsteps.Hops.Hop.Origin", "Mashsteps.Fermentables.Fermentable", "MashSteps.Others.Other", "BoilSteps.Hops.Hop.Origin");


        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RecipeExists(int id)
        {
            return db.Recipes.Count(e => e.Id == id) > 0;
        }
    }
}