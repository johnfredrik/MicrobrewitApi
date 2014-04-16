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
        private readonly IBeerStyleRepository beerStyleRepository = new BeerStyleRepository();

        [Route("")]
        // GET api/BeerStyle
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

                    var beerStyles = Mapper.Map<IList<BeerStyle>, IList<BeerStyleDto>>(beerStyleRepository.GetAll("SubStyles", "SuperStyle"));
                    foreach (var item in beerStyles)
                    {
                        redisClient.HashSet("beerstyles", item.Id, JsonConvert.SerializeObject(item), flags: CommandFlags.FireAndForget);
                    }
                    result.BeerStyles = beerStyles;

                }
            }
            return result;
        }



        [Route("{id:int}")]
        // GET api/BeerStyle/5
        [ResponseType(typeof(BeerStyle))]
        [HttpGet]
        public IHttpActionResult GetBeerStyle(int id)
        {
            var beerStyle = Mapper.Map<BeerStyle, BeerStyleDto>(beerStyleRepository.GetSingle(b => b.Id == id, "SubStyles", "SuperStyle"));
            if (beerStyle == null)
            {
                return NotFound();
            }
            var result = new BeerStyleCompleteDto() { BeerStyles = new List<BeerStyleDto>() };
            result.BeerStyles.Add(beerStyle);

            return Ok(result);
        }

        // PUT api/BeerStyle/5
        [Route("{id:int}")]
        public IHttpActionResult PutBeerStyle(int id, BeerStyle beerstyle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beerstyle.Id)
            {
                return BadRequest();
            }

            beerStyleRepository.Update(beerstyle);
            

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/BeerStyle

        [Route("")]

        [ResponseType(typeof(IList<BeerStyle>))]
        public IHttpActionResult PostBeerStyle(IList<BeerStyle> beerstyles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            beerStyleRepository.Add(beerstyles.ToArray());
            var bs = Mapper.Map<IList<BeerStyle>, IList<BeerStyleDto>>(beerStyleRepository.GetAll("SubStyles", "SuperStyle"));
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

        // DELETE api/BeerStyle/5
        [ResponseType(typeof(BeerStyle))]
        public async Task<IHttpActionResult> DeleteBeerStyle(int id)
        {
            BeerStyle beerstyle = await db.BeerStyles.FindAsync(id);
            if (beerstyle == null)
            {
                return NotFound();
            }

            db.BeerStyles.Remove(beerstyle);
            await db.SaveChangesAsync();

            return Ok(beerstyle);
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