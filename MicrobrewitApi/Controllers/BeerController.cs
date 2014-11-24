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
using System.Threading.Tasks;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("beers")]
    public class BeerController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IBeerRepository _beerRepository;
        private Elasticsearch.ElasticSearch _elasticsearch;

        public BeerController(IBeerRepository beerRepository)
        {
            this._beerRepository = beerRepository;
            this._elasticsearch = new Elasticsearch.ElasticSearch();
        }

        /// <summary>
        /// Gets all beer
        /// </summary>
        /// <response code="200">It's all good!</response>
        /// <returns>Returns collection of all beers</returns>
        [Route("")]
        public BeerSimpleCompleteDto GetBeers()
        {
            var beers = Mapper.Map<IList<Beer>, IList<BeerSimpleDto>>(_beerRepository.GetAll("Recipe", "SRM", "ABV", "IBU", "Brewers","Brewers.User", "Breweries"));
            var result = new BeerSimpleCompleteDto();
            result.Beers = beers;
            return result;
        }

        /// <summary>
        /// Gets all beer
        /// </summary>
        /// <response code="200">It's all good!</response>
        /// <returns>Returns collection of all beers</returns>
        [Route("user/{username}")]
        public async Task<BeerSimpleCompleteDto> GetUserBeers(string username)
        {
            var beers = await _beerRepository.GetAllUserBeer(username, "Recipe", "SRM", "ABV", "IBU", "Brewers","Brewers.User", "Breweries");
            var beersDto = Mapper.Map<IList<Beer>, IList<BeerSimpleDto>>(beers);
            var result = new BeerSimpleCompleteDto();
            result.Beers = beersDto;
            return result;
        }

        /// <summary>
        /// Gets beer by id
        /// </summary>
        /// <response code="200">Beer found and returned</response>
        /// <response code="404">Beer with that id not found</response>
        ///<param name="id">Id of the beer</param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(Beer))]
        public async Task<IHttpActionResult> GetBeer(int id)
        {
            var beer = await _beerRepository.GetSingleAsync(b => b.Id == id,
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
                "ABV", "IBU", "SRM", "Brewers.User", "Breweries");

            if (beer == null)
            {
                return NotFound();
            }
            var result = new BeerCompleteDto() { Beers = new List<BeerDto>() };
            result.Beers.Add(Mapper.Map<Beer, BeerDto>(beer));
            return Ok(result);
        }

        /// <summary>
        /// Updates a beer
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Id of the beer</param>
        /// <param name="beerDto"></param>
        /// <returns></returns>
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
            beer.UpdatedDate = DateTime.Now;
            _beerRepository.Update(beer);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a beer
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <param name="beerPost"></param>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(BeerDto))]
        public async Task<IHttpActionResult> PostBeer(BeerDto beerPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var beer = Mapper.Map<BeerDto, Beer>(beerPost);
            if (beerPost.Recipe != null)
            {
                BeerCalculations(beer);
            }
            beer.CreatedDate = DateTime.Now;
            beer.UpdatedDate = DateTime.Now;
            try
            {
                beer.BeerStyle = null;
                await _beerRepository.AddAsync(beer);
            }
            catch (DbUpdateException dbUpdateException)
            {
                //Log.Error(dbUpdateException);
                return BadRequest(dbUpdateException.ToString());
            }


            var result = new BeerCompleteDto() { Beers = new List<BeerDto>() };
            await UpdateSingleBeerES(beer, result);
            

            return CreatedAtRoute("DefaultApi", new { controller = "beers" }, result);
        }

        private async Task UpdateSingleBeerES(Beer beer, BeerCompleteDto result)
        {
            var singleBeer = await _beerRepository.GetSingleAsync(b => b.Id == beer.Id,
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
                 "ABV", "IBU", "SRM", "Brewers", "Breweries");

            result.Beers.Add(Mapper.Map<Beer, BeerDto>(singleBeer));
            var beerdto = Mapper.Map<Beer, BeerDto>(singleBeer);
            var beerES = new List<BeerDto>();
            beerES.Add(beerdto);
            await _elasticsearch.UpdateBeerElasticSearch(beerES);
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
            if (beer.SRM != null)
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

        /// <summary>
        /// Deletes a beer
        /// </summary>
        /// <response code="200">Ok</response>
        /// <resppmse code="404">Not Found</resppmse>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates elasticsearch with data from the database.
        /// </summary>
        /// <returns>200 OK</returns>
        [Route("es")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateBeersElasticSearch()
        {
            var beers = await _beerRepository.GetAllAsync("Recipe.MashSteps.Hops",
                "Recipe.MashSteps.Fermentables",
                "Recipe.MashSteps.Others",
                "Recipe.BoilSteps.Hops",
                "Recipe.BoilSteps.Fermentables",
                "Recipe.BoilSteps.Others",
                "Recipe.FermentationSteps.Hops",
                "Recipe.FermentationSteps.Fermentables",
                "Recipe.FermentationSteps.Others",
                "Recipe.FermentationSteps.Yeasts",
                 "ABV", "IBU", "SRM", "Brewers", "Breweries");
            var beersDto = Mapper.Map<IList<Beer>, IList<BeerDto>>(beers);
            // updated elasticsearch.
            await _elasticsearch.UpdateBeerElasticSearch(beersDto);

            return Ok();
        }
        /// <summary>
        /// Searches in beers.
        /// </summary>
        /// <param name="query">Query phrase</param>
        /// <param name="from">From what object</param>
        /// <param name="size">Number of objects returned</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IList<BeerDto>> GetBeerBySearch(string query, int from = 0, int size = 20)
        {
            var result = await _elasticsearch.SearchBeers(query, from, size);
            return result.ToList();
        }

        /// <summary>
        /// Get the last beers added.
        /// </summary>
        /// <param name="from">from beer</param>
        /// <param name="size">number of beer returned</param>
        /// <returns></returns>
        [HttpGet]
        [Route("last")]
        public async Task<IList<BeerDto>> GetLastAddedBeers(int from = 0, int size = 20)
        {
            var beerDto = await _elasticsearch.GetLastBeers(from, size);
            if (beerDto == null)
            {
                var beers = await _beerRepository.GetLastAsync(from, size, "Recipe", "SRM", "ABV", "IBU", "Brewers", "Breweries");
                beerDto = Mapper.Map<IList<Beer>, IList<BeerDto>>(beers);
            }
            //var result = await _elasticsearch.GetLastBeers(from,size);
            return beerDto;
        }
    }
}