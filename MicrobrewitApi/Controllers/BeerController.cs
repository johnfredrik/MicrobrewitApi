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
using System.Diagnostics;
using log4net;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("beers")]
    public class BeerController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IBeerRepository _beerRepository;

        public BeerController(IBeerRepository beerRepository)
        {
            this._beerRepository = beerRepository;
        }

        // GET api/Beer
        [Route("")]
        public BeerSimpleCompleteDto GetBeers()
        {
            var beers = Mapper.Map<IList<Beer>, IList<BeerSimpleDto>>(_beerRepository.GetAll("Recipe", "SRM", "ABV", "IBU", "Brewers", "Breweries"));
            var result = new BeerSimpleCompleteDto();
            result.Beers = beers;
            return result;
        }

        // GET api/Beer/5
        [Route("{id}")]
        [ResponseType(typeof(Beer))]
        public IHttpActionResult GetBeer(int id)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var beer = _beerRepository.GetSingle(b => b.Id == id,
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
                "Recipe.FermentationSteps.Yeasts",
                "ABV", "IBU", "SRM", "Brewers", "Breweries");
            Log.Debug("EF call time elapsed: " + stopwatch.Elapsed);

            if (beer == null)
            {
                return NotFound();
            }
            stopwatch.Restart();
            var result = new BeerCompleteDto() { Beers = new List<BeerDto>() };
            result.Beers.Add(Mapper.Map<Beer, BeerDto>(beer));
            Log.Debug("Mapper call time elapsed: " + stopwatch.Elapsed);
            return Ok(result);
        }

        // GET api/Beer/5
        [Route("redis/{id}")]
        [ResponseType(typeof(Beer))]
        public IHttpActionResult GetBeerRedis(int id)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var beer = _beerRepository.GetSingle(b => b.Id == id,
                "Recipe", "Brewers", "ABV", "IBU", "SRM", "Breweries");
            Log.Debug("EF call time elapsed: " + stopwatch.Elapsed);
            if (beer == null)
            {
                return NotFound();
            }
            stopwatch.Restart();
            var result = new BeerCompleteDto() { Beers = new List<BeerDto>() };
            result.Beers.Add(Mapper.Map<Beer, BeerDto>(beer));
            Log.Debug("Mapper call time elapsed: " + stopwatch.Elapsed);
            return Ok(result);
        }

        // PUT api/Beer/5
        [Route("{id}")]
        public IHttpActionResult PutBeer(int id, BeerDto beerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beerDto.Id)
            {
                return BadRequest();
            }
            var beer = Mapper.Map<BeerDto, Beer>(beerDto);
            BeerCalculations(beer);
            beer.BeerStyle = null;
            //beer.Recipe.BoilSteps = null;
            //beer.Recipe.FermentationSteps = null;
            //foreach (var item in beer.Recipe.FermentationSteps)
            //{
            //    item.Others = null;
            //    item.Hops = null;
            //    item.Yeasts = null;

            //}
            //beer.Recipe.MashSteps = null;
            //beer.IBU = null;
            ///beer.ABV = null;
            //beer.SRM = null;
            //beer.Brewers = null;

            _beerRepository.Update(beer);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Beer
        [Route("")]
        [ResponseType(typeof(BeerDto))]
        public IHttpActionResult PostBeer(BeerDto beerPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var beer = Mapper.Map<BeerDto, Beer>(beerPost);
            BeerCalculations(beer);
            beer.BeerStyle = null;


            try
            {
                _beerRepository.Add(beer);
            } 
            catch (DbUpdateException dbUpdateException)
            {
                //Log.Error(dbUpdateException);
                return BadRequest(dbUpdateException.ToString());
            }
                
               
            var result = new BeerCompleteDto() { Beers = new List<BeerDto>() };

            result.Beers.Add(Mapper.Map<Beer, BeerDto>(_beerRepository.GetSingle(b => b.Id == beer.Id,
                "Recipe.MashSteps.Hops",
                "Recipe.MashSteps.Fermentables",
                "Recipe.MashSteps.Others",
                "Recipe.BoilSteps.Hops",
                "Recipe.BoilSteps.Fermentables",
                "Recipe.BoilSteps.Others",
                "Recipe.FermentationSteps.Hops",
                "Recipe.FermentationSteps.Fermentables",
                "Recipe.FermentationSteps.Others",
                "Recipe.FermentationSteps.Yeasts",
                 "ABV", "IBU", "SRM", "Brewers", "Breweries")));

            return CreatedAtRoute("DefaultApi", new { controller = "beers" }, result);
        }

        private static void BeerCalculations(Beer beer)
        {
            beer.Recipe.OG = Calculation.CalculateOG(beer.Recipe);
            var abv = Calculation.CalculateABV(beer.Recipe);
            if (beer.ABV != null)
            {
                abv.Id = beer.ABV.Id;
            }
            beer.ABV = abv;
            var srm = Calculation.CalculateSRM(beer.Recipe);
            if (beer.SRM!= null)
            {
                srm.Id = beer.SRM.Id;
            }
            beer.SRM = srm;
            var ibu = Calculation.CalculateIBU(beer.Recipe);
            if (beer.IBU != null)
            {
                ibu.Id = beer.IBU.Id;
            }
            beer.IBU = ibu;
        }

        // DELETE api/Beer/5
        [Route("{id:int}")]
        [ResponseType(typeof(BeerDto))]
        public IHttpActionResult DeleteBeer(int id)
        {
            Beer beer = _beerRepository.GetSingle(b => b.Id == id);
            if (beer == null)
            {
                return NotFound();
            }

            _beerRepository.Remove(beer);
            var beerDto = Mapper.Map<Beer, BeerDto>(beer);
            return Ok(beerDto);
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