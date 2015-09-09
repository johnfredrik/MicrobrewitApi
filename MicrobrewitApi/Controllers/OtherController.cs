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
    
    [RoutePrefix("others")]
    public class OtherController : ApiController
    {
        private IOtherService _otherService;

        public OtherController(IOtherService otherService)
        {
            _otherService = otherService;
        }

        /// <summary>
        /// Gets a collection of others
        /// </summary>
        /// <returns>Ok 200 on success</returns>
        /// <errorCode code="400"></errorCode>
        [Route("")]
        public async Task<OtherCompleteDto> GetOthers(string custom = "false")
        {
            var other = await _otherService.GetAllAsync(custom);
            var result = new OtherCompleteDto {Others = other.ToList()};
            return result;
        }


        /// <summary>
        /// Get other.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="400">Not Found</response>
        /// <param name="id">Other id</param>
        /// <returns></returns>
        [Route("{id:int}")]
        [ResponseType(typeof(OtherCompleteDto))]
        public async Task<IHttpActionResult> GetOther(int id)
        {
            var otherDto = await _otherService.GetSingleAsync(id);
            if (otherDto == null)
            {
                return NotFound();
            }
            var response = new OtherCompleteDto() { Others = new List<OtherDto>() };
            response.Others.Add(otherDto);
            return Ok(response);
        }

        /// <summary>
        /// Updates a other.
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <param name="id"></param>
        /// <param name="otherDto"></param>
        /// <returns></returns>
        [ClaimsAuthorize("Put", "Other")]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutOther(int id, OtherDto otherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != otherDto.Id)
            {
                return BadRequest();
            }
            await _otherService.UpdateAsync(otherDto);
            return StatusCode(HttpStatusCode.NoContent);
        }
        /// <summary>
        /// Add new other.
        /// </summary>
        /// <param name="otherDto"></param>
        /// <returns></returns>
        [ClaimsAuthorize("Post", "Other")]
        [Route("")]
        [ResponseType(typeof(OtherDto))]
        public async Task<IHttpActionResult> PostOther(OtherDto otherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _otherService.AddAsync(otherDto);
            return CreatedAtRoute("DefaultApi", new { controller = "others", }, result);
        }

        
        /// <summary>
        /// Delets other by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Other id</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi=true)]
        [ClaimsAuthorize("Delete", "Other")]
        [Route("{id:int}")]
        [ResponseType(typeof(OtherCompleteDto))]
        public async Task<IHttpActionResult> DeleteOther(int id)
        {
            var other = await _otherService.GetSingleAsync(id);
            if (other == null)
            {
                return NotFound();
            }
            return Ok(other);
        }

        [Route("es")]
        [ClaimsAuthorize("Reindex","Other")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateOtherElasticSearch()
        {

            await _otherService.ReIndexElasticSearch();   
            return Ok();
        }

        [HttpGet]
        [Route("")]
        public async Task<OtherCompleteDto> GetOthersBySearch(string query, int from = 0, int size = 20)
        {
            var othersDto = await _otherService.SearchAsync(query, from, size);
            return new OtherCompleteDto {Others = othersDto.ToList()};
        }
    }
}