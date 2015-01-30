using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using log4net;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch;
using Microbrewit.Service.Elasticsearch.Interface;
using Microbrewit.Service.Interface;
using Thinktecture.IdentityModel.Authorization;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("yeasts")]
    public class YeastController : ApiController
    {
        private IYeastService _yeastService;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public YeastController(IYeastService yeastService)
        {
            _yeastService = yeastService;
        }


        /// <summary>
        /// Gets all yeasts.
        /// </summary>
        /// <returns>200 OK</returns>
        [Route("")]
        public async Task<YeastCompleteDto> GetYeasts(string custom = "false")
        {
            var yeasts = await _yeastService.GetAllAsync(custom);
            return new YeastCompleteDto{Yeasts = yeasts.ToList()};
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

            //var yeastDto = await _elasticsearch.GetYeast(id);
            //if (yeastDto == null)
            //{
            //    var yeast = await _yeastRespository.GetSingleAsync(y => y.Id == id, "Supplier");
            //    yeastDto = Mapper.Map<Yeast, YeastDto>(yeast);
            //}
            var yeastDto = await _yeastService.GetSingleAsync(id);
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
        [ClaimsAuthorize("Put","Yeast")]
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
            await _yeastService.UpdateAsync(yeastDto);

            //var yeast = Mapper.Map<YeastDto, Yeast>(yeastDto);
            //await _yeastRespository.UpdateAsync(yeast);

            //var yeasts = await _yeastRespository.GetAllAsync("Supplier");
            //var yeastsDto = Mapper.Map<IList<Yeast>, IList<YeastDto>>(yeasts);
            //// updated elasticsearch.
            //await _elasticsearch.UpdateYeastsElasticSearch(yeastsDto);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Inserts new yeast.
        /// </summary>
        /// <param name="yeastDto">Takes a list of YeastDto objects in form of json</param>
        /// <returns>201 Created</returns>
        [ClaimsAuthorize("Post","Yeast")]
        [Route("")]
        [ResponseType(typeof(YeastDto))]
        public async Task<IHttpActionResult> PostYeast(YeastDto yeastDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var yeasts = Mapper.Map<IList<YeastDto>, Yeast[]>(yeastPosts);
            //await _yeastRespository.AddAsync(yeasts);

            //var yeastsES = await _yeastRespository.GetAllAsync("Supplier");
            //var yeastsDto = Mapper.Map<IList<Yeast>, IList<YeastDto>>(yeastsES);
            //// updated elasticsearch.
            //await _elasticsearch.UpdateYeastsElasticSearch(yeastsDto);
            var result = await _yeastService.AddAsync(yeastDto);

            return CreatedAtRoute("DefaultApi", new { controller = "yeasts", }, result);
        }

        /// <summary>
        /// Deletes a yeast
        /// </summary>
        /// <param name="id">Yeast id</param>
        /// <returns>200 OK</returns>
        [ClaimsAuthorize("Delete","Yeast")]
        [Route("{id:int}")]
        [ResponseType(typeof(YeastDto))]
        public async Task<IHttpActionResult> DeleteYeast(int id)
        {
            //Yeast yeast = await _yeastRespository.GetSingleAsync(y => y.Id == id);
            var yeast = await _yeastService.DeleteAsync(id);
            if (yeast == null)
            {
                return NotFound();
            }

            //_yeastRespository.Remove(yeast);

            //var yeasts = await _yeastRespository.GetAllAsync("Supplier");
            //var yeastsDto = Mapper.Map<IList<Yeast>, IList<YeastDto>>(yeasts);
            //// updated elasticsearch.
            //await _elasticsearch.DeleteYeast(id);

            //var yeastDto = Mapper.Map<Yeast, YeastDto>(yeast);
            return Ok(yeast);
        }

        [ClaimsAuthorize("Reindex","Yeast")]
        [HttpGet]
        [Route("es")]
        public async Task<IHttpActionResult> UpdateElasticSearchYeast()
        {
            // Updates yeasts in the redis store.
            //var yeasts = await _yeastRespository.GetAllAsync("Supplier");
            //var yeastsDto = Mapper.Map<IList<Yeast>, IList<YeastDto>>(yeasts);
            // updated elasticsearch.
            //await _elasticsearch.UpdateYeastsElasticSearch(yeastsDto);
            await _yeastService.ReIndexElasticSearch();
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
            //var yeastsDto = await _elasticsearch.SearchYeasts(query,from,size);
            var yeastsDto = await _yeastService.SearchAsync(query, from, size);
            return new YeastCompleteDto {Yeasts = yeastsDto.ToList()};
        }
    }
}