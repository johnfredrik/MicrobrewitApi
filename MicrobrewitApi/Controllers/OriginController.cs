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

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("origins")]
    public class OriginController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IOriginRespository _originRepository;
        private Elasticsearch.ElasticSearch _elasticsearch;

        public OriginController(IOriginRespository originRepository)
        {
            this._originRepository = originRepository;
            this._elasticsearch = new Elasticsearch.ElasticSearch();
        }


        /// <summary>
        /// Get all origins.
        /// </summary>
        /// <response code="200">OK</response>
        /// <returns></returns>
        [Route("")]
        public async Task<IList<Origin>> GetOrigins()
        {
            var origins = await Redis.OriginRedis.GetOriginsAsync();
            if (origins.Count <= 0)
            {

                origins = await _originRepository.GetAllAsync();
            }

            return origins;
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
            var origin = await Redis.OriginRedis.GetOriginAsync(id);
            if (origin == null)
            {
                origin = await _originRepository.GetSingleAsync(o => o.Id == id);
            }
            if (origin == null)
            {
                return NotFound();
            }
            return  Ok(origin);
        }

        /// <summary>
        /// Updates a origin
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Origin id</param>
        /// <param name="origin"></param>
        /// <returns></returns>
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutOrigin(int id, Origin origin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != origin.Id)
            {
                return BadRequest();
            }

            await _originRepository.UpdateAsync(origin);
            var originsRedis = await _originRepository.GetAllAsync();
            await Redis.OriginRedis.UpdateRedisStoreAsync(originsRedis);
            // updated elasticsearch.
            await _elasticsearch.UpdateOriginElasticSearch(originsRedis);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds origins.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <param name="originPosts"></param>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(IList<Origin>))]
        public async Task<IHttpActionResult> PostOrigin(IList<Origin> originPosts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var origins = originPosts.ToArray();
            await _originRepository.AddAsync(origins);

            var originsRedis = await _originRepository.GetAllAsync();
            await Redis.OriginRedis.UpdateRedisStoreAsync(originsRedis);
            // updated elasticsearch.
            await _elasticsearch.UpdateOriginElasticSearch(originsRedis);

            return CreatedAtRoute("DefaultApi", new { controller= "origins", }, origins);
        }

        /// <summary>
        /// Deletes a origin.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Origin id</param>
        /// <returns></returns>
        [Route("{id:int}")]
        [ResponseType(typeof(Origin))]
        public async Task<IHttpActionResult> DeleteOrigin(int id)
        {
            Origin origin = _originRepository.GetSingle(o => o.Id == id);
            if (origin == null)
            {
                return NotFound();
            }

            await _originRepository.RemoveAsync(origin);
            var originsRedis = await _originRepository.GetAllAsync();
            await Redis.OriginRedis.UpdateRedisStoreAsync(originsRedis);
            // updated elasticsearch.
            await _elasticsearch.UpdateOriginElasticSearch(originsRedis);

            return Ok(origin);
        }

        [Route("redis")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateOriginRedis()
        {
             var originsRedis = await _originRepository.GetAllAsync();
             await Redis.OriginRedis.UpdateRedisStoreAsync(originsRedis);
             // updated elasticsearch.
             await _elasticsearch.UpdateOriginElasticSearch(originsRedis);

             return Ok();
        }

        [HttpGet]
        [Route("")]
        public async Task<IList<Origin>> GetOriginBySearch(string query)
        {
            var result = await _elasticsearch.GetOrigins(query);
            return result.ToList();
        }
     
    }
}