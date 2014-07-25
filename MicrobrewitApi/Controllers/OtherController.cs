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
using AutoMapper;
using Microbrewit.Repository;
using System.Configuration;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace Microbrewit.Api.Controllers
{
    
    [RoutePrefix("others")]
    public class OtherController : ApiController
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private IOtherRepository _otherRepository;
        private Elasticsearch.ElasticSearch _elasticsearch;
       
        public OtherController(IOtherRepository otherRepository)
        {
            
            this._otherRepository = otherRepository;
            this._elasticsearch = new Elasticsearch.ElasticSearch();
        }

        /// <summary>
        /// Gets a collection of others
        /// </summary>
        /// <returns>Ok 200 on success</returns>
        /// <errorCode code="400"></errorCode>
        [Route("")]
        public async Task<OtherCompleteDto> GetOthers()
        {
            var othersDto = await Redis.OtherRedis.GetOthersAsync();
            if (othersDto.Count <= 0)
            {
            var others = await _otherRepository.GetAllAsync();
            othersDto = Mapper.Map<IList<Other>, IList<OtherDto>>(others);

            }
            var result = new OtherCompleteDto();
            result.Others = othersDto;
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
            var otherDto = await Redis.OtherRedis.GetOtherAsync(id);
            if (otherDto == null)
            {
                var other = await _otherRepository.GetSingleAsync(o => o.Id == id);
                otherDto = Mapper.Map<Other, OtherDto>(other);

            }
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
            var other = Mapper.Map<OtherDto, Other>(otherDto);
            await _otherRepository.UpdateAsync(other);

            // Updates redis store.
            var othersRedis = await _otherRepository.GetAllAsync();
            var othersDto = Mapper.Map<IList<Other>, IList<OtherDto>>(othersRedis);
            await Redis.OtherRedis.UpdateRedisStoreAsync(othersDto);
            // updated elasticsearch.
            await _elasticsearch.UpdateOtherElasticSearch(othersDto);
            return StatusCode(HttpStatusCode.NoContent);
        }
        /// <summary>
        /// Add new other.
        /// </summary>
        /// <param name="otherDtos"></param>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(OtherCompleteDto))]
        public async Task<IHttpActionResult> PostOther(IList<OtherDto> otherDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var others = Mapper.Map<IList<OtherDto>, Other[]>(otherDtos);
            await _otherRepository.AddAsync(others);

            var result = Mapper.Map<IList<Other>, IList<OtherDto>>(_otherRepository.GetAll());

            // Updates redis store.
            var othersRedis = await _otherRepository.GetAllAsync();
            var othersDto = Mapper.Map<IList<Other>, IList<OtherDto>>(othersRedis);
            await Redis.OtherRedis.UpdateRedisStoreAsync(othersDto);
            // updated elasticsearch.
            await _elasticsearch.UpdateOtherElasticSearch(othersDto);

            var response = new OtherCompleteDto() { Others = new List<OtherDto>() };
            response.Others = otherDtos;
            return CreatedAtRoute("DefaultApi", new { controller = "others", }, response);
        }

        
        /// <summary>
        /// Delets other by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Other id</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi=true)]
        [Route("{id:int}")]
        [ResponseType(typeof(OtherCompleteDto))]
        public async Task<IHttpActionResult> DeleteOther(int id)
        {
            var other = await _otherRepository.GetSingleAsync(o => o.Id == id);
            if (other == null)
            {
                return NotFound();
            }
            await _otherRepository.RemoveAsync(other);

            // Updates redis store.
            var othersRedis = await _otherRepository.GetAllAsync();
            var othersDto = Mapper.Map<IList<Other>, IList<OtherDto>>(othersRedis);
            await Redis.OtherRedis.UpdateRedisStoreAsync(othersDto);
            // updated elasticsearch.
            await _elasticsearch.UpdateOtherElasticSearch(othersDto);

            var response = new OtherCompleteDto(){Others = new List<OtherDto>()};
            response.Others.Add(Mapper.Map<Other,OtherDto>(other));
            return Ok(response);
        }

        [Route("redis")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateOtherRedis()
        {
            var othersRedis = await _otherRepository.GetAllAsync();
            var othersDto = Mapper.Map<IList<Other>, IList<OtherDto>>(othersRedis);
            await Redis.OtherRedis.UpdateRedisStoreAsync(othersDto);
            // updated elasticsearch.
            await _elasticsearch.UpdateOtherElasticSearch(othersDto);

            return Ok();
        }

        [HttpGet]
        [Route("")]
        public async Task<OtherCompleteDto> GetOthersBySearch(string query)
        {
            var hopDto = await _elasticsearch.GetOthers(query);

            var result = new OtherCompleteDto();
            result.Others = hopDto.ToList();
            return result;
        }
    }
}