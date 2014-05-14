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
        public BeerStyleCompleteDto GetBeerStyles()
        {
            var result = new BeerStyleCompleteDto() { BeerStyles = new List<BeerStyleDto>() };

            using (var redis = ConnectionMultiplexer.Connect(redisStore))
            {

                var redisClient = redis.GetDatabase();

                if (redisClient.KeyExists("beerstyles"))
                {
                    var json = redisClient.HashValues("beerstyles");
                    foreach (var item in json)
                    {
                        result.BeerStyles.Add(JsonConvert.DeserializeObject<BeerStyleDto>(item));
                    }

                }
                else
                {

                    var beerStyles = Mapper.Map<IList<BeerStyle>, IList<BeerStyleDto>>(_beerStyleRepository.GetAll("SubStyles", "SuperStyle"));
                    foreach (var item in beerStyles)
                    {
                        redisClient.HashSet("beerstyles", item.Id, JsonConvert.SerializeObject(item), flags: CommandFlags.FireAndForget);
                    }
                    result.BeerStyles = beerStyles;

                }
            }
            return result;
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
        [HttpGet]
        public IHttpActionResult GetBeerStyle(int id)
        {
            var beerStyle = Mapper.Map<BeerStyle, BeerStyleDto>(_beerStyleRepository.GetSingle(b => b.Id == id, "SubStyles", "SuperStyle"));
            if (beerStyle == null)
            {
                return NotFound();
            }
            var result = new BeerStyleCompleteDto() { BeerStyles = new List<BeerStyleDto>() };
            result.BeerStyles.Add(beerStyle);

            return Ok(result);
        }

        /// <summary>
        /// Updates a beerstyle.
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Beerstyle id</param>
        /// <param name="beerstyle">BeerStyle object</param>
        /// <returns></returns>
        [Route("{id:int}")]
        public IHttpActionResult PutBeerStyle(int id, BeerStyleDto beerstyleDto)
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

            _beerStyleRepository.Update(beerstyle);
            

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add beerstyle.
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <param name="beerstyles">Beerstyle object.</param>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(IList<BeerStyleDto>))]
        public IHttpActionResult PostBeerStyle(IList<BeerStyleDto> beerstyles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var beerDto = Mapper.Map<IList<BeerStyleDto>, IList<BeerStyle>>(beerstyles);
            _beerStyleRepository.Add(beerDto.ToArray());
            var bs = Mapper.Map<IList<BeerStyle>, IList<BeerStyleDto>>(_beerStyleRepository.GetAll("SubStyles", "SuperStyle"));
            using (var redis = ConnectionMultiplexer.Connect(redisStore))
            {
                var redisClient = redis.GetDatabase();

                foreach (var item in bs)
                {
                    redisClient.HashSet("beerstyles", item.Id, JsonConvert.SerializeObject(item),flags: CommandFlags.FireAndForget);
                }

            }
            return CreatedAtRoute("DefaultApi", new { controller = "beerstyles" }, bs);
        }

        /// <summary>
        /// Deletes beerstyle by id.
        /// </summary>
        /// <response code="404">Node Found</response>
        /// <param name="id">Beerstyle id.</param>
        /// <returns></returns>
        [ResponseType(typeof(BeerStyleDto))]
        public async Task<IHttpActionResult> DeleteBeerStyle(int id)
        {
            BeerStyle beerstyle = await db.BeerStyles.FindAsync(id);
            if (beerstyle == null)
            {
                return NotFound();
            }

            db.BeerStyles.Remove(beerstyle);
            await db.SaveChangesAsync();
            var beerStyleDto = Mapper.Map<BeerStyle, BeerStyleDto>(beerstyle);
            return Ok(beerStyleDto);
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