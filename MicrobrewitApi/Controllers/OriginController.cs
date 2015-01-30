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
using log4net;
using Microbrewit.Repository;
using AutoMapper;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Elasticsearch;
using Microbrewit.Service.Interface;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("origins")]
    public class OriginController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IOriginService _originService;

        public OriginController(IOriginService originService)
        {
            _originService = originService;
        }


        /// <summary>
        /// Get all origins.
        /// </summary>
        /// <response code="200">OK</response>
        /// <returns></returns>
        [Route("")]
        public async Task<OriginCompleteDto> GetOrigins(string custom = "false")
        {
            var originDto = await _originService.GetAllAsync(custom);
            var originsComplete = new OriginCompleteDto { Origins = originDto.ToList()};
            return originsComplete;
        }

        /// <summary>
        /// Get origin by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code=""404>Not Found</response>
        /// <param name="id">Origin id</param>
        /// <returns></returns>
        [Route("{id:int}")]
        [ResponseType(typeof(Origin))]
        public async Task<IHttpActionResult> GetOrigin(int id)
        {
            var originDto = await _originService.GetSingleAsync(id);
            if (originDto == null)
            {
                return NotFound();
            }
            var originsComplete = new OriginCompleteDto { Origins = new List<OriginDto>() };
            originsComplete.Origins.Add(originDto);
            return  Ok(originsComplete);
        }

        /// <summary>
        /// Updates a origin
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Origin id</param>
        /// <param name="origin"></param>
        /// <returns></returns>
        [ClaimsAuthorize("Put","Origin")]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutOrigin(int id, OriginDto origin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != origin.Id)
            {
                return BadRequest();
            }
            await _originService.UpdateAsync(origin);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds origins.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <param name="origin"></param>
        /// <returns></returns>
        [ClaimsAuthorize("Post", "Origin")]
        [Route("")]
        [ResponseType(typeof(Origin))]
        public async Task<IHttpActionResult> PostOrigin(OriginDto origin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _originService.AddAsync(origin);
            return CreatedAtRoute("DefaultApi", new { controller= "origins", }, result);
        }

        /// <summary>
        /// Deletes a origin.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Origin id</param>
        /// <returns></returns>
        [ClaimsAuthorize("Delete","Origin")]
        [Route("{id:int}")]
        [ResponseType(typeof(OriginDto))]
        public async Task<IHttpActionResult> DeleteOrigin(int id)
        {

            var origin = await _originService.DeleteAsync(id);
            if (origin == null)
            {
                return NotFound();
            }
            return Ok(origin);
        }
        /// <summary>
        /// Updates elasticsearch with data from the database.
        /// </summary>
        /// <returns>200 OK</returns>
        [Route("es")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateOriginElasticSearch()
        {
             await _originService.ReIndexElasticSearch();
             return Ok();
        }
        /// <summary>
        /// Searches in origin.
        /// </summary>
        /// <param name="query">Query phrase</param>
        /// <param name="from">From what object</param>
        /// <param name="size">Number of objects returned</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IList<OriginDto>> GetOriginBySearch(string query, int from = 0, int size = 20)
        {
            var result = await _originService.SearchAsync(query, from, size);
            return result.ToList();
        }
     
    }
}