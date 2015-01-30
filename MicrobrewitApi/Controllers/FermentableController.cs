using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using log4net;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Interface;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Microbrewit.Api.Controllers
{
    /// <summary>
    /// Fermentable Controller
    /// </summary>
    [RoutePrefix("fermentables")]
    public class FermentableController : ApiController
    {
        private IFermentableService _fermentableService;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public FermentableController(IFermentableService fermentableService)
        {
            _fermentableService = fermentableService;
        }

        /// <summary>
        /// Gets all fermentables.
        /// </summary>
        /// <response code="200">OK</response>
        /// <returns></returns>
        [Route("")]
        public async Task<FermentablesCompleteDto> GetFermentables(string custom = "false")
        {
            var fermentables = await _fermentableService.GetAllAsync(custom);
            return new FermentablesCompleteDto { Fermentables = fermentables.ToList() };
        }

        /// <summary>
        /// Get a fermentable by its id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Fermentable </param>
        /// <returns></returns>
        [Route("{id:int}")]
        [ResponseType(typeof(FermentablesCompleteDto))]
        public async Task<IHttpActionResult> GetFermentable(int id)
        {

            var fermentableDto = await _fermentableService.GetSingleAsync(id);
            if (fermentableDto == null)
            {
                return NotFound();
            }
            var result = new FermentablesCompleteDto() { Fermentables = new List<FermentableDto>() };
            result.Fermentables.Add(fermentableDto);
            return Ok(result);
        }

        /// <summary>
        /// Updates a fermentable.
        /// </summary>
        /// <param name="id">Fermentable id</param>
        /// <param name="fermentableDto">Fermentable data transfer object</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <returns></returns>
        [ClaimsAuthorize("Put","Fermentable")]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutFermentable(int id, FermentableDto fermentableDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fermentableDto.Id)
            {
                return BadRequest();
            }
            await _fermentableService.UpdateAsync(fermentableDto);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds fermentables.
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <param name="fermentableDtos">List of fermentable transfer objects</param>
        /// <returns></returns>
        [ClaimsAuthorize("Post", "Fermentable")]
        [Route("")]
        [ResponseType(typeof(FermentableDto))]
        public async Task<IHttpActionResult> PostFermentable(FermentableDto fermentableDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _fermentableService.AddAsync(fermentableDto);
            return CreatedAtRoute("DefaultApi", new { controller = "fermetables", }, result);
        }

        /// <summary>
        /// Deletes a fermentable by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Fermentable id</param>
        /// <returns></returns>
        [ClaimsAuthorize("Delete", "Fermentable")]
        [Route("{id:int}")]
        [ResponseType(typeof(Fermentable))]
        public async Task<IHttpActionResult> DeleteFermentable(int id)
        {
            var fermentable = await _fermentableService.DeleteAsync(id);
            if (fermentable == null)
            {
                return NotFound();
            }
            return Ok(fermentable);
        }

        [ClaimsAuthorize("Reindex", "Fermentable")]
        [HttpGet]
        [Route("es")]
        public async Task<IHttpActionResult> UpdateFermentableElasticSearch()
        {
            await _fermentableService.ReIndexElasticsearch();
            return Ok();
        }

        [HttpGet]
        [Route("")]
        public async Task<FermentablesCompleteDto> GetFermentablesBySearch(string query, int from = 0, int size = 20)
        {
            var fermentablesDto = await _fermentableService.SearchAsync(query, from, size);
            return new FermentablesCompleteDto { Fermentables = fermentablesDto.ToList() };
        }
    }
}