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
    [RoutePrefix("beerstyles")]
    public class BeerStyleController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private IBeerStyleRepository _beerStyleRepository;

        public BeerStyleController(IBeerStyleRepository beerStyleRepository)
        {
            this._beerStyleRepository = beerStyleRepository;
        }

        /// <summary>
        /// Get all beerstyles.
        /// <response code="200">OK</response>
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public async Task<BeerStyleCompleteDto> GetBeerStyles()
        {
            var response = new BeerStyleCompleteDto() { BeerStyles = new List<BeerStyleDto>() };

            using (var redis = ConnectionMultiplexer.Connect(redisStore))
            {

                var redisClient = redis.GetDatabase();

                if (redisClient.KeyExists("beerstyles"))
                {
                    var json = await redisClient.HashValuesAsync("beerstyles");
                    foreach (var item in json)
                    {
                        response.BeerStyles.Add(JsonConvert.DeserializeObject<BeerStyleDto>(item));
                    }

                }
                else
                {
                    var beerStyles = await _beerStyleRepository.GetAllAsync("SubStyles", "SuperStyle");
                    var beerStylesDto = Mapper.Map<IList<BeerStyle>, IList<BeerStyleDto>>(beerStyles);
                    foreach (var item in beerStylesDto)
                    {
                        await redisClient.HashSetAsync("beerstyles", item.Id, JsonConvert.SerializeObject(item), flags: CommandFlags.FireAndForget);
                    }
                    response.BeerStyles = beerStylesDto;

                }
            }
            return response;
        }


        /// <summary>
        /// Get beerstyle by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Beerstyle id</param>
        /// <returns></returns>
        [Route("{id:int}")]
        [ResponseType(typeof(BeerStyleCompleteDto))]
        public async Task<IHttpActionResult> GetBeerStyle(int id)
        {
            var beerStyle = await _beerStyleRepository.GetSingleAsync(b => b.Id == id, "SubStyles", "SuperStyle");
            var beerStyleDto = Mapper.Map<BeerStyle, BeerStyleDto>(beerStyle);
            if (beerStyleDto == null)
            {
                return NotFound();
            }
            var response = new BeerStyleCompleteDto() { BeerStyles = new List<BeerStyleDto>() };
            response.BeerStyles.Add(beerStyleDto);

            return Ok(response);
        }

        /// <summary>
        /// Updates a beerstyle.
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Beerstyle id</param>
        /// <param name="beerstyleDto">BeerStyle object</param>
        /// <returns></returns>
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutBeerStyle(int id, BeerStyleDto beerstyleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var beerstyle = Mapper.Map<BeerStyleDto, BeerStyle>(beerstyleDto);
            if (id != beerstyle.Id)
            {
                return BadRequest();
            }

            await _beerStyleRepository.UpdateAsync(beerstyle);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add beerstyle.
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <param name="beerStylesDto">Beerstyle object.</param>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(BeerStyleCompleteDto))]
        public async Task<IHttpActionResult> PostBeerStyle(IList<BeerStyleDto> beerStylesDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var beerStyles = Mapper.Map<IList<BeerStyleDto>, BeerStyle[]>(beerStylesDto);
            await _beerStyleRepository.AddAsync(beerStyles);
            
            var bs = Mapper.Map<IList<BeerStyle>, IList<BeerStyleDto>>(_beerStyleRepository.GetAll("SubStyles", "SuperStyle"));
            using (var redis = ConnectionMultiplexer.Connect(redisStore))
            {
                var redisClient = redis.GetDatabase();

                foreach (var item in bs)
                {
                    await redisClient.HashSetAsync("beerstyles", item.Id, JsonConvert.SerializeObject(item),flags: CommandFlags.FireAndForget);
                }

            }
            var response = new BeerStyleCompleteDto(){BeerStyles = new List<BeerStyleDto>()};
            response.BeerStyles = beerStylesDto;
            return CreatedAtRoute("DefaultApi", new { controller = "beerstyles" }, response);
        }

        /// <summary>
        /// Deletes beerstyle by id.
        /// </summary>
        /// <response code="404">Node Found</response>
        /// <param name="id">Beerstyle id.</param>
        /// <returns></returns>
        [ResponseType(typeof(BeerStyleCompleteDto))]
        public async Task<IHttpActionResult> DeleteBeerStyle(int id)
        {
            var beerstyle = await _beerStyleRepository.GetSingleAsync(bs => bs.Id == id);
            if (beerstyle == null)
            {
                return NotFound();
            }

            await _beerStyleRepository.RemoveAsync(beerstyle);
            var beerStyleDto = Mapper.Map<BeerStyle, BeerStyleDto>(beerstyle);
            var response = new BeerStyleCompleteDto(){BeerStyles = new List<BeerStyleDto>()};
            response.BeerStyles.Add(beerStyleDto);
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

        private bool BeerStyleExists(int id)
        {
            return db.BeerStyles.Count(e => e.Id == id) > 0;
        }
    }
}