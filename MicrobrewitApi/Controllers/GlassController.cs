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

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("glasses")]
    public class GlassController : ApiController
    {
        private IGlassRepository _glassRepository;
        private Elasticsearch.ElasticSearch _elasticsearch;

        public GlassController(IGlassRepository glassRepository)
        {
            this._glassRepository = glassRepository;
            this._elasticsearch = new Elasticsearch.ElasticSearch();
        }

        /// <summary>
        /// Gets all glassware from the database
        /// </summary>
        /// <returns>200 ok</returns>
        [Route("")]
        public async Task<GlassCompleteDto> GetGlasses()
        {
            var glassesDto = await _elasticsearch.GetGlasses();
            if (glassesDto == null)
            {
                var glasses = await _glassRepository.GetAllAsync();
                glassesDto = Mapper.Map<IList<Glass>, IList<GlassDto>>(glasses);
            }
            var result = new GlassCompleteDto();
            result.Glasses = glassesDto.OrderBy(g => g.Name).ToList();
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
            var glassDto = await _elasticsearch.GetGlass(id);
            if (glassDto == null)
            {
                var glass = await _glassRepository.GetSingleAsync(g => g.Id == id);
                glassDto = Mapper.Map<Glass, GlassDto>(glass);
            }
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
            var other = Mapper.Map<GlassDto, Glass>(glassDto);
            await _glassRepository.UpdateAsync(other);

            var glass = await _glassRepository.GetSingleAsync(g => g.Id == id);
            var glassEs = Mapper.Map<Glass, GlassDto>(glass);
            // updated elasticsearch.
            await _elasticsearch.UpdateGlassElasticSearch(glassEs);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add new glass.
        /// </summary>
        /// <param name="glassDtos"></param>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(GlassCompleteDto))]
        public async Task<IHttpActionResult> PostGlass(IList<GlassDto> glassDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var glasses = Mapper.Map<IList<GlassDto>, Glass[]>(glassDtos);
            await _glassRepository.AddAsync(glasses);

            //var result = Mapper.Map<IList<Glass>, IList<GlassDto>>(_glassRepository.GetAll());

            var glassEs = await _glassRepository.GetAllAsync();
            var glassDto = Mapper.Map<IList<Glass>, IList<GlassDto>>(glassEs);
            // updated elasticsearch.
            await _elasticsearch.UpdateGlassesElasticSearch(glassDto);

            var response = new GlassCompleteDto() { Glasses = new List<GlassDto>() };
            response.Glasses = glassDtos;
            return CreatedAtRoute("DefaultApi", new { controller = "others", }, response);
        }

        /// <summary>
        /// Delets glass by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Glass id</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("{id:int}")]
        [ResponseType(typeof(OtherCompleteDto))]
        public async Task<IHttpActionResult> DeleteGlass(int id)
        {
            var glass = await _glassRepository.GetSingleAsync(o => o.Id == id);
            if (glass == null)
            {
                return NotFound();
            }
            await _glassRepository.RemoveAsync(glass);

            //TODO: DELETE from elasticsearch/

            var response = new GlassCompleteDto() { Glasses = new List<GlassDto>() };
            response.Glasses.Add(Mapper.Map<Glass, GlassDto>(glass));
            return Ok(response);
        }

        [Route("es")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateGlassElasticSearch()
        {
            var glasses = await _glassRepository.GetAllAsync();
            var glassesDto = Mapper.Map<IList<Glass>, IList<GlassDto>>(glasses);
            // updated elasticsearch.
            await _elasticsearch.UpdateGlassesElasticSearch(glassesDto);

            return Ok();
        }

        [HttpGet]
        [Route("")]
        public async Task<GlassCompleteDto> GetGlassBySearch(string query, int from = 0, int size = 20)
        {
            var glassesDto = await _elasticsearch.SearchByGlass(query, from, size);

            var result = new GlassCompleteDto();
            result.Glasses = glassesDto.ToList();
            return result;
        }
    }
}
