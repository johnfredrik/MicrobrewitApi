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
        public IList<RecipeDto> GetRecipes()
        {
            

            //var recipes = db.Recipes.Include("RecipeHops.Hop").Project().To<RecipeDto>();
            var recipes = recipeRepository.GetRecipes();
            var recipesDto =  Mapper.Map<IList<Recipe>, IList<RecipeDto>>(recipes);
            return recipesDto;

           
        }

        // GET api/Recipe/5
        [Route("{id:int}")]
        [ResponseType(typeof(Recipe))]
        public async Task<IHttpActionResult> GetRecipe(int id)
        {
            

           //Recipe recipe = await db.Recipes.Where(r => r.Id == id).FirstOrDefaultAsync();
           RecipeDto recipe =  Mapper.Map<Recipe,RecipeDto>(recipeRepository.GetRecipe(id));
            //Recipe recipe = recipeRepository.GetRecipe(id);
            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
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