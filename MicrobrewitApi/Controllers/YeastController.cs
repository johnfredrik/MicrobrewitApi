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
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using AutoMapper;
using System.Configuration;
using Newtonsoft.Json;
using Microbrewit.Api.Elasticsearch;
using log4net;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("yeasts")]
    public class YeastController : ApiController
    {
        private IYeastRepository _yeastRespository;
        private Elasticsearch.ElasticSearch _elasticsearch;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public YeastController(IYeastRepository yeastRepository)
        {
            this._yeastRespository = yeastRepository;
            this._elasticsearch = new Elasticsearch.ElasticSearch();
        }
        /// <summary>
        /// Gets all yeasts.
        /// </summary>
        /// <returns>200 OK</returns>
        [Route("")]
        public async Task<YeastCompleteDto> GetYeasts()
        {
            var yeastsDto = await _elasticsearch.GetAllYeasts();
            Log.Debug(string.Format("Number of yeasts: {0}",yeastsDto.Count()));
            if (yeastsDto.Count() <= 0)
            {
                var yeasts = await _yeastRespository.GetAllAsync("Supplier");
                yeastsDto = Mapper.Map<IList<Yeast>, IList<YeastDto>>(yeasts);
            }
            var result = new YeastCompleteDto();
            result.Yeasts = yeastsDto.OrderBy(y => y.Name).ToList();
            return result;
        }

        /// <summary>
        /// Gets single yeast.
        /// api.microbrew.it/yeasts/:id
        /// </summary>
        /// <param name="id">Yeast Id</param>
        /// <returns>200 OK Single yeast</returns>
        [Route("{id:int}")]
        [ResponseType(typeof(YeastCompleteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> GetYeast(int id)
        {

            var yeastDto = await _elasticsearch.GetYeast(id);
            if (yeastDto == null)
            {
                var yeast = await _yeastRespository.GetSingleAsync(y => y.Id == id, "Supplier");
                yeastDto = Mapper.Map<Yeast, YeastDto>(yeast);
            }
            if (yeastDto == null)
            {
                return NotFound();
            }
            var result = new YeastCompleteDto() { Yeasts = new List<YeastDto>() };
            result.Yeasts.Add(yeastDto);
            return Ok(result);
        }

        /// <summary>
        /// Updates a yeast.
        /// </summary>
        /// <param name="id">Yeast Id</param>
        /// <param name="yeastDto">Json of the YeastDto object</param>
        /// <returns>No Content 204</returns>
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutYeast(int id, YeastDto yeastDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != yeastDto.Id)
            {
                return BadRequest();
            }


            var yeast = Mapper.Map<YeastDto, Yeast>(yeastDto);
            await _yeastRespository.UpdateAsync(yeast);

            var yeasts = await _yeastRespository.GetAllAsync("Supplier");
            var yeastsDto = Mapper.Map<IList<Yeast>, IList<YeastDto>>(yeasts);
            // updated elasticsearch.
            await _elasticsearch.UpdateYeastsElasticSearch(yeastsDto);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Inserts new yeast.
        /// </summary>
        /// <param name="yeastPosts">Takes a list of YeastDto objects in form of json</param>
        /// <returns>201 Created</returns>
        [Route("")]
        [ResponseType(typeof(IList<YeastDto>))]
        public async Task<IHttpActionResult> PostYeast(IList<YeastDto> yeastPosts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var yeasts = Mapper.Map<IList<YeastDto>, Yeast[]>(yeastPosts);
            await _yeastRespository.AddAsync(yeasts);

            var yeastsES = await _yeastRespository.GetAllAsync("Supplier");
            var yeastsDto = Mapper.Map<IList<Yeast>, IList<YeastDto>>(yeastsES);
            // updated elasticsearch.
            await _elasticsearch.UpdateYeastsElasticSearch(yeastsDto);

            return CreatedAtRoute("DefaultApi", new { controller = "yeasts", }, yeastPosts);
        }

        /// <summary>
        /// Deletes a yeast
        /// </summary>
        /// <param name="id">Yeast id</param>
        /// <returns>200 OK</returns>
        [Route("{id:int}")]
        [ResponseType(typeof(YeastDto))]
        public async Task<IHttpActionResult> DeleteYeast(int id)
        {
            Yeast yeast = await _yeastRespository.GetSingleAsync(y => y.Id == id);
            if (yeast == null)
            {
                return NotFound();
            }

            _yeastRespository.Remove(yeast);

            var yeasts = await _yeastRespository.GetAllAsync("Supplier");
            var yeastsDto = Mapper.Map<IList<Yeast>, IList<YeastDto>>(yeasts);
            // updated elasticsearch.
            await _elasticsearch.UpdateYeastsElasticSearch(yeastsDto);

            var yeastDto = Mapper.Map<Yeast, YeastDto>(yeast);
            return Ok(yeastDto);
        }

        [HttpGet]
        [Route("es")]
        public async Task<IHttpActionResult> UpdateRedisYeast()
        {
            // Updates yeasts in the redis store.
            var yeasts = await _yeastRespository.GetAllAsync("Supplier");
            var yeastsDto = Mapper.Map<IList<Yeast>, IList<YeastDto>>(yeasts);
            // updated elasticsearch.
            await _elasticsearch.UpdateYeastsElasticSearch(yeastsDto);

            return Ok();
        }
        /// <summary>
        /// Searches in yeasts
        /// </summary>
        /// <param name="query">The pharse you want to match.</param>
        /// <param name="from">Start point of the search.</param>
        /// <param name="size">Number of results returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<YeastCompleteDto> GetYeastsBySearch(string query, int from = 0, int size = 20)
        {
            var yeastsDto = await _elasticsearch.SearchYeasts(query,from,size);
            
            var result = new YeastCompleteDto();
            result.Yeasts = yeastsDto.ToList();
            return result;
        }
    }
}