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
using System.Drawing;
using log4net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microbrewit.Service.Elasticsearch;
using Microbrewit.Service.Interface;
using Thinktecture.IdentityModel.Authorization;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityModel.Tokens.Http;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("beers")]
    public class BeerController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IBeerService _beerService;

        public BeerController(IBeerService beerService)
        {
            _beerService = beerService;
        }

        /// <summary>
        /// Gets all beer
        /// </summary>
        /// <response code="200">It's all good!</response>
        /// <returns>Returns collection of all beers</returns>
        [Route("")]
        public async Task<BeerCompleteDto> GetBeers(int from = 0, int size = 1000)
        {
            var beers = await _beerService.GetAllAsync(from, size);
            var result = new BeerCompleteDto { Beers = beers.ToList() };
            return result;
        }

        /// <summary>
        /// Gets all beer
        /// </summary>
        /// <response code="200">It's all good!</response>
        /// <returns>Returns collection of all beers</returns>
        [Route("user/{username}")]
        public async Task<BeerCompleteDto> GetUserBeers(string username)
        {
            var beersDto = await _beerService.GetUserBeersAsync(username);
            var result = new BeerCompleteDto { Beers = beersDto.ToList() };
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
        [ResponseType(typeof(BeerCompleteDto))]
        public async Task<IHttpActionResult> GetBeer(int id)
        {
            var beer = await _beerService.GetSingleAsync(id);
            if (beer == null)
            {
                return NotFound();
            }
            var result = new BeerCompleteDto() { Beers = new List<BeerDto>() };
            result.Beers.Add(beer);
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
        public async Task<IHttpActionResult> PutBeer(int id, BeerDto beerDto)
        {
            var isAllowed = ClaimsAuthorization.CheckAccess("Put", "BeerId", id.ToString());
            if (!isAllowed)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beerDto.Id)
            {
                return BadRequest();
            }
            await _beerService.UpdateAsync(beerDto);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a beer
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <param name="beerDto"></param>
        /// <returns></returns>
        [ClaimsAuthorize("Post", "Beer")]
        [Route("")]
        [ResponseType(typeof(BeerDto))]
        public async Task<IHttpActionResult> PostBeer(BeerDto beerDto)
        {
            if (beerDto == null) return BadRequest("Missing data");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var beer = await _beerService.AddAsync(beerDto);
            return CreatedAtRoute("DefaultApi", new { controller = "beers" }, beer);
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
        public async Task<IHttpActionResult> DeleteBeer(int id)
        {
            var isAllowed = ClaimsAuthorization.CheckAccess("Delete", "BeerId", id.ToString());
            if (!isAllowed)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
            var beerDto = await _beerService.DeleteAsync(id);
            if (beerDto == null)
            {
                return NotFound();
            }
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
            await _beerService.ReIndexElasticSearch();
            return Ok();
        }
        /// <summary>
        /// Searches in beers.
        /// </summary>
        /// <param name="query">Query phrase</param>
        /// <param name="from">From what object</param>
        /// <param name="size">StepNumber of objects returned</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IList<BeerDto>> GetBeerBySearch(string query, int from = 0, int size = 20)
        {
            var result = await _beerService.SearchAsync(query, from, size);
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
        public async Task<IEnumerable<BeerDto>> GetLastAddedBeers(int from = 0, int size = 20)
        {
            var beerDto = await _beerService.GetLastAsync(from, size);
            return beerDto;
        }
    }
}