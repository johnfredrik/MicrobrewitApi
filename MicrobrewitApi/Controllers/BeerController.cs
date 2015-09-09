using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using log4net;
using Microbrewit.Api.ErrorHandler;
using Microbrewit.Model.BeerXml;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Interface;
using Newtonsoft.Json;
using Thinktecture.IdentityModel.Authorization;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Thinktecture.IdentityModel.Extensions;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("beers")]
    public class BeerController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
            Log.Debug(JsonConvert.SerializeObject(beerDto));
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
            var username = HttpContext.Current.User.Identity.Name;
            if (username == null) return BadRequest("Missing user");
            var beer = await _beerService.AddAsync(beerDto, username);
            if (beer == null) return BadRequest();
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
        [ClaimsAuthorize("Reindex","Beer")]
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
        public async Task<BeerCompleteDto> GetBeerBySearch(string query, int from = 0, int size = 20)
        {
            var beerDtos = await _beerService.SearchAsync(query, from, size);
            return new BeerCompleteDto {Beers = beerDtos.ToList()};
        }

        /// <summary>
        /// Get the last beers added.
        /// </summary>
        /// <param name="from">from beer</param>
        /// <param name="size">number of beer returned</param>
        /// <returns></returns>
        [HttpGet]
        [Route("last")]
        public async Task<BeerCompleteDto> GetLastAddedBeers(int from = 0, int size = 20)
        {
            var beerDto = await _beerService.GetLastAsync(from, size);
            return new BeerCompleteDto{ Beers = beerDto.ToList()};
        }

        //[HttpPost]
        //[Route("beerxml")]
        //public async Task<Recipe> PostBeerXml([FromBody] Recipe recipe)
        //{
        //    var context = HttpContext.Current;
        //     return recipe;
        //}

        [HttpPost]
        [Route("beerxml")]
        [DbUpdateExceptionFilter]
        public async Task<IHttpActionResult> PostBeerXml([FromBody] RecipesComplete value)
        {
            if (value == null) return BadRequest();
            var beersDto = Mapper.Map<IList<Recipe>, IList<BeerDto>>(value.Recipes);
            var context = HttpContext.Current;
        
            //return Ok(new BeerCompleteDto{Beers = beersDto});
            return Ok(beersDto.FirstOrDefault());
        }
    }
}
