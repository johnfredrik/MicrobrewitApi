using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microbrewit.Service.Elasticsearch;
using Microbrewit.Service.Interface;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("glasses")]
    public class GlassController : ApiController
    {
        private readonly IGlassService _glassService;

        public GlassController(IGlassService glassService)
        {
            _glassService = glassService;
        }

        /// <summary>
        /// Gets all glassware from the database
        /// </summary>
        /// <returns>200 ok</returns>
        [Route("")]
        public async Task<GlassCompleteDto> GetGlasses()
        {
            var glassesDto = await _glassService.GetAllAsync();
            var result = new GlassCompleteDto {Glasses = glassesDto.OrderBy(g => g.Name).ToList()};
            return result;
        }

        /// <summary>
        /// Get single glass
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="400">Not Found</response>
        /// <param name="id">Glass id</param>
        /// <returns>Glass</returns>
        [Route("{id:int}")]
        [ResponseType(typeof(GlassCompleteDto))]
        public async Task<IHttpActionResult> GetGlass(int id)
        {
            var glassDto = await _glassService.GetSingleAsync(id);
            var result = new GlassCompleteDto() { Glasses = new List<GlassDto>() };
            result.Glasses.Add(glassDto);
            return Ok(result);
        }

        /// <summary>
        /// Updates a glass.
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <param name="id"></param>
        /// <param name="glassDto"></param>
        /// <returns></returns>
        [ClaimsAuthorize("Put","Glass")]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutGlass(int id, GlassDto glassDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != glassDto.Id)
            {
                return BadRequest();
            }
            await _glassService.UpdateAsync(glassDto);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add new glass.
        /// </summary>
        /// <param name="glassDtos"></param>
        /// <returns></returns>
        [ClaimsAuthorize("Post", "Glass")]
        [Route("")]
        [ResponseType(typeof(GlassDto))]
        public async Task<IHttpActionResult> PostGlass(GlassDto glassDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _glassService.AddAsync(glassDto);
            return CreatedAtRoute("DefaultApi", new { controller = "others", }, glassDto);
        }

        /// <summary>
        /// Delets glass by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Glass id</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [ClaimsAuthorize("Delete", "Glass")]
        [Route("{id:int}")]
        [ResponseType(typeof(GlassDto))]
        public async Task<IHttpActionResult> DeleteGlass(int id)
        {
            var glass = await _glassService.DeleteAsync(id);
            if (glass == null)
            {
                return NotFound();
            }
            return Ok(glass);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [ClaimsAuthorize("Reindex","Glass")]
        [Route("es")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateGlassElasticSearch()
        {
            await _glassService.ReIndexElasticSearch();
            return Ok();
        }


        [HttpGet]
        [Route("")]
        public async Task<GlassCompleteDto> GetGlassBySearch(string query, int from = 0, int size = 20)
        {
            var glassesDto = await _glassService.SearchAsync(query, from, size);
            var result = new GlassCompleteDto {Glasses = glassesDto.ToList()};
            return result;
        }
    }
}
