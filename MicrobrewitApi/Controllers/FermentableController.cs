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
using StackExchange.Redis;
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
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public FermentableController(IFermentableRepository fermentableRepository)
        {
            this._fermentableRepository = fermentableRepository;
        }
      
        /// <summary>
        /// Gets all fermentables.
        /// </summary>
        /// <response code="200">OK</response>
        /// <returns></returns>
        [Route("")]
        public async Task<FermentablesCompleteDto> GetFermentables()
        {
            var fermentables = await _fermentableRepository.GetAllAsync("Supplier");
            var fermDto = Mapper.Map<IList<Fermentable>,IList<FermentableDto>>(fermentables);
            var result = new FermentablesCompleteDto();
            result.Fermentables = fermDto;
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
            var fermentable = await _fermentableRepository.GetSingleAsync(f => f.Id == id, "Supplier"); 
            if (fermentable == null)
            {
                return NotFound();
            }
            var fermentableDto = Mapper.Map<Fermentable, FermentableDto>(fermentable);
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
            
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds fermentables.
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <param name="FermentableDtos">List of fermentable transfer objects</param>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(IList<FermentableDto>))]
        public async Task<IHttpActionResult> PostFermentable(IList<FermentableDto> FermentableDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fermentablePost = Mapper.Map<IList<FermentableDto>, Fermentable[]>(FermentableDtos);
            await _fermentableRepository.AddAsync(fermentablePost);
            var fermentables = Mapper.Map<IList<Fermentable>,IList<FermentableDto>>(_fermentableRepository.GetAll("Supplier.Origin"));
            using (var redis = ConnectionMultiplexer.Connect(redisStore))
            {

            var redisClient = redis.GetDatabase();
            
                foreach (var fermentable in fermentables)
                {
                    redisClient.HashSet("fermentables", fermentable.Id.ToString(), JsonConvert.SerializeObject(fermentable), flags: CommandFlags.FireAndForget);
                }
            
            }
            return CreatedAtRoute("DefaultApi", new { controller = "fermetables", }, FermentableDtos);
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

            await _fermentableRepository.RemoveAsync(fermentable);
            var response = new FermentablesCompleteDto() { Fermentables = new List<FermentableDto>() };
            response.Fermentables.Add(Mapper.Map<Fermentable, FermentableDto>(fermentable));
            return Ok(response);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FermentableExists(int id)
        {
            return db.Fermentables.Count(e => e.Id == id) > 0;
        }
    }
}