using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Interface;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("beerstyles")]
    public class BeerStyleController : ApiController
    {
        private IBeerStyleService _beerStyleService;

        public BeerStyleController(IBeerStyleService beerStyleService)
        {
            _beerStyleService = beerStyleService;
        }

        /// <summary>
        /// Get all beerstyles.
        /// <response code="200">OK</response>
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public async Task<BeerStyleCompleteDto> GetBeerStyles()
        {
            var beerStylesDto = await _beerStyleService.GetAllAsync();
            var response = new BeerStyleCompleteDto() { BeerStyles = beerStylesDto.ToList()};
            return response;
        }


        /// <summary>
        /// Get beerstyle by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Beerstyle id</param>
        /// <returns></returns>
        [Route("{id:int}")]
        [ResponseType(typeof(BeerStyleCompleteDto))]
        public async Task<IHttpActionResult> GetBeerStyle(int id)
        {
            var beerStyleDto = await _beerStyleService.GetSingleAsync(id);
            if (beerStyleDto == null)
            {
                return NotFound();
            }
            var response = new BeerStyleCompleteDto() { BeerStyles = new List<BeerStyleDto>() };
            response.BeerStyles.Add(beerStyleDto);
            return Ok(response);
        }

        /// <summary>
        /// Updates a beerstyle.
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Beerstyle id</param>
        /// <param name="beerstyle">BeerStyle object</param>
        /// <returns></returns>
        [ClaimsAuthorize("Put","BeerStyle")]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutBeerStyle(int id, BeerStyleDto beerstyle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != beerstyle.Id)
            {
                return BadRequest();
            }
            await _beerStyleService.UpdateAsync(beerstyle);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add beerstyle.
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <param name="beerStylesDto">Beerstyle object.</param>
        /// <returns></returns>
        [ClaimsAuthorize("Post", "BeerStyle")]
        [Route("")]
        [ResponseType(typeof(BeerStyleDto))]
        public async Task<IHttpActionResult> PostBeerStyle(BeerStyleDto beerStylesDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _beerStyleService.AddAsync(beerStylesDto);
            return CreatedAtRoute("DefaultApi", new { controller = "beerstyles" }, result);
        }

        /// <summary>
        /// Deletes beerstyle by id.
        /// </summary>
        /// <response code="404">Node Found</response>
        /// <param name="id">Beerstyle id.</param>
        /// <returns></returns>
        [ClaimsAuthorize("Delete", "BeerStyle")]
        [Route("{id}")]
        [ResponseType(typeof(BeerStyleDto))]
        public async Task<IHttpActionResult> DeleteBeerStyle(int id)
        {
            var beerstyle = await _beerStyleService.DeleteAsync(id);
            if (beerstyle == null)
            {
                return NotFound();
            }
            return Ok(beerstyle);
        }
        /// <summary>
        /// Searches in beer styles.
        /// </summary>
        /// <param name="query">Query phrase</param>
        /// <param name="from">From what object</param>
        /// <param name="size">Number of objects returned</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<BeerStyleCompleteDto> GetBeerBySearch(string query, int from = 0, int size = 20)
        {
            var result = await _beerStyleService.SearchAsync(query, from, size);
            return new BeerStyleCompleteDto {BeerStyles = result.ToList()};
        }


        [ClaimsAuthorize("Reindex", "BeerStyle")]
        [HttpGet]
        [Route("es")]
        public async Task<IHttpActionResult> UpdateBeerStyleElasticSearch()
        {
            await _beerStyleService.ReIndexElasticSearch();
            return Ok();
        }
    }
}