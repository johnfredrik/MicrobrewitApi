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
using Microbrewit.Api;
using Microbrewit.Repository;
using AutoMapper;
using log4net;
using Microbrewit.Model.DTOs;
using Newtonsoft.Json;
using System.Configuration;

namespace Microbrewit.Api.Controllers
{
   // [TokenValidationAttribute]
    [RoutePrefix("fermentables")]
    public class FermentableController : ApiController
    { 
        private MicrobrewitContext db = new MicrobrewitContext();
        private IFermentableRepository _fermentableRepository;
        private Elasticsearch.ElasticSearch _elasticsearch;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public FermentableController(IFermentableRepository fermentableRepository)
        {
            this._fermentableRepository = fermentableRepository;
            this._elasticsearch = new Elasticsearch.ElasticSearch();
        }
      
        /// <summary>
        /// Gets all fermentables.
        /// </summary>
        /// <response code="200">OK</response>
        /// <returns></returns>
        [Route("")]
        public async Task<FermentablesCompleteDto> GetFermentables()
        {
            var fermentablesDto = await Redis.FermentableRedis.GetFermentablesAsync();
            if (fermentablesDto.Count <= 0)
            {
                var fermentables = await _fermentableRepository.GetAllAsync("Supplier.Origin");
                fermentablesDto = Mapper.Map<IList<Fermentable>,IList<FermentableDto>>(fermentables);

            }
            var result = new FermentablesCompleteDto();
            result.Fermentables = fermentablesDto;
            return result;
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
            var fermentableDto = await Redis.FermentableRedis.GetFermentableAsync(id);
            if (fermentableDto == null)
            {
                var fermentable = await _fermentableRepository.GetSingleAsync(f => f.Id == id, "Supplier.Origin"); 
                fermentableDto = Mapper.Map<Fermentable, FermentableDto>(fermentable);

            }
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

            var fermentable = Mapper.Map<FermentableDto, Fermentable>(fermentableDto);
            await _fermentableRepository.UpdateAsync(fermentable);
            var fermentablesRedis = await _fermentableRepository.GetAllAsync("Supplier.Origin");
            var fermentablesDto = Mapper.Map<IList<Fermentable>, IList<FermentableDto>>(fermentablesRedis);
            await Redis.FermentableRedis.UpdateRedisStoreAsync(fermentablesDto);
            // updated elasticsearch.
            await _elasticsearch.UpdateFermentableElasticSearch(fermentablesDto);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds fermentables.
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <param name="fermentableDtos">List of fermentable transfer objects</param>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(IList<FermentableDto>))]
        public async Task<IHttpActionResult> PostFermentable(IList<FermentableDto> fermentableDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fermentablePost = Mapper.Map<IList<FermentableDto>, Fermentable[]>(fermentableDtos);
            await _fermentableRepository.AddAsync(fermentablePost);
            var fermentables = await _fermentableRepository.GetAllAsync("Supplier.Origin");
            var fermentablesDto = Mapper.Map<IList<Fermentable>, IList<FermentableDto>>(fermentables);
            await Redis.FermentableRedis.UpdateRedisStoreAsync(fermentablesDto);
            // updated elasticsearch.
            await _elasticsearch.UpdateFermentableElasticSearch(fermentablesDto);

            return CreatedAtRoute("DefaultApi", new { controller = "fermetables", }, fermentableDtos);
        }

        /// <summary>
        /// Deletes a fermentable by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Fermentable id</param>
        /// <returns></returns>
        [Route("{id:int}")]
        [ResponseType(typeof(Fermentable))]
        public async Task<IHttpActionResult> DeleteFermentable(int id)
        {
            var fermentable = await _fermentableRepository.GetSingleAsync(f => f.Id == id);
            if (fermentable == null)
            {
                return NotFound();
            }
            //Removes fermentable from database.
            await _fermentableRepository.RemoveAsync(fermentable);
            
            //Updates the redis store.
            var fermentablesRedis = await _fermentableRepository.GetAllAsync("Supplier.Origin");
            var fermentablesDto = Mapper.Map<IList<Fermentable>, IList<FermentableDto>>(fermentablesRedis);
            await Redis.FermentableRedis.UpdateRedisStoreAsync(fermentablesDto);
            // updated elasticsearch.
            await _elasticsearch.UpdateFermentableElasticSearch(fermentablesDto);

            var response = new FermentablesCompleteDto() { Fermentables = new List<FermentableDto>() };
            response.Fermentables.Add(Mapper.Map<Fermentable, FermentableDto>(fermentable));
            return Ok(response);
        }

        [HttpGet]
        [Route("redis")]
        public async Task<IHttpActionResult> UpdateRedisFermentable()
        {
            // Updates yeasts in the redis store.
            var fermentablesRedis = await _fermentableRepository.GetAllAsync("Supplier.Origin");
            var fermentablesDto = Mapper.Map<IList<Fermentable>, IList<FermentableDto>>(fermentablesRedis);
            await Redis.FermentableRedis.UpdateRedisStoreAsync(fermentablesDto);
            // updated elasticsearch.
            await _elasticsearch.UpdateFermentableElasticSearch(fermentablesDto);

            return Ok();
        }

        [HttpGet]
        [Route("")]
        public async Task<FermentablesCompleteDto> GetFermentablesBySearch(string query, int from = 0, int size = 20)
        {
            var fermentablesDto = await _elasticsearch.GetFermentables(query,from,size);

            var result = new FermentablesCompleteDto();
            result.Fermentables = fermentablesDto.ToList();
            return result;
        }
    }
}