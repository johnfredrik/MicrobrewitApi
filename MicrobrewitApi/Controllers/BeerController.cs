using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Microbrewit.Model;
using Microbrewit.Api.Util;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using AutoMapper;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("beers")]
    public class BeerController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private readonly IBeerRepository beerRepository = new BeerRepository();

        // GET api/Beer
        [Route("")]
        public BeerSimpleCompleteDto GetBeers()
        {
            var beers = Mapper.Map<IList<Beer>,IList<BeerSimpleDto>>(beerRepository.GetAll("Recipe","SRM"));
            var result = new BeerSimpleCompleteDto();
            result.Beers = beers;
            return result;
        }

        // GET api/Beer/5
        [Route("{id}")]
        [ResponseType(typeof(Beer))]
        public IHttpActionResult GetBeer(int id)
        {
            var beer = beerRepository.GetSingle(b => b.Id == id,
                //"Recipe.MashSteps",
                "Recipe.MashSteps.Hops",
                "Recipe.MashSteps.Fermentables",
                "Recipe.MashSteps.Others",
               // "Recipe.BoilSteps",
                "Recipe.BoilSteps.Hops",
                "Recipe.BoilSteps.Fermentables",
                "Recipe.BoilSteps.Others",
               // "Recipe.FermentationSteps",
                "Recipe.FermentationSteps.Hops",
                "Recipe.FermentationSteps.Fermentables",
                "Recipe.FermentationSteps.Others",
                "Brewers", "ABV", "IBU", "SRM", "Brewers", "Breweries");
            if (beer == null)
            {
                return NotFound();
            }
            var result = new BeerCompleteDto() {Beers = new List<BeerDto>()};
            result.Beers.Add(Mapper.Map<Beer, BeerDto>(beer));
            return Ok(result);
        }

        // GET api/Beer/5
        [Route("redis/{id}")]
        [ResponseType(typeof(Beer))]
        public IHttpActionResult GetBeerRedis(int id)
        {
            var beer = beerRepository.GetSingle(b => b.Id == id,
                "Recipe.MashSteps",
                "Recipe.BoilSteps",
                "Recipe.FermentationSteps",
                "Brewers", "ABV", "IBU", "SRM", "Brewers", "Breweries");

            if (beer == null)
            {
                return NotFound();
            }
            var result = new BeerCompleteDto() { Beers = new List<BeerDto>() };
            result.Beers.Add(Mapper.Map<Beer, BeerDto>(beer));
            return Ok(result);
        }

        // PUT api/Beer/5
        public IHttpActionResult PutBeer(int id, Beer beer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beer.Id)
            {
                return BadRequest();
            }

            db.Entry(beer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeerExists(id))
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

        // POST api/Beer
        [Route("")]
        [ResponseType(typeof(BeerPostDto))]
        public IHttpActionResult PostBeer(BeerPostDto beerPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var beer = Mapper.Map<BeerPostDto, Beer>(beerPost);
            beer.SRM = Calculation.CalculateSRM(beer.Recipe);
            beer.BeerStyle = null;
            

            beerRepository.Add(beer);
            
           
            return CreatedAtRoute("DefaultApi", new { controller = "beers" }, beer);
        }

        // DELETE api/Beer/5
        [ResponseType(typeof(Beer))]
        public IHttpActionResult DeleteBeer(int id)
        {
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return NotFound();
            }

            db.Beers.Remove(beer);
            db.SaveChanges();

            return Ok(beer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BeerExists(int id)
        {
            return db.Beers.Count(e => e.Id == id) > 0;
        }
    }
}