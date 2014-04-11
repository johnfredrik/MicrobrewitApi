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
using System.Linq.Expressions;
using log4net;
using Newtonsoft.Json;
using Microbrewit.Model.DTOs;
using AutoMapper;
using Microbrewit.Repository;
using System.Configuration;
using StackExchange.Redis;

namespace Microbrewit.Api.Controllers
{

    [RoutePrefix("hops")]
    public class HopsController : ApiController
    {

        private MicrobrewitContext db = new MicrobrewitContext();
        private IHopRepository hopRepository = new HopRepository();
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        // GET api/Hops
        [Route("")]
        public HopCompleteDto GetHops()
        {
            var hops = Mapper.Map<IList<Hop>, IList<HopDto>>(hopRepository.GetAll("Flavours.Flavour", "Origin", "Substituts"));
            var result = new HopCompleteDto() { Hops = hops };
            return result;
        }

        // GET api/Hops/5
        [Route("{id:int}")]
        [ResponseType(typeof(HopCompleteDto))]
        [HttpGet]
        public IHttpActionResult GetHop(int id)
        {

            var hop = Mapper.Map<Hop, HopDto>(hopRepository.GetSingle(h => h.Id == id, "Flavours.Flavour", "Origin", "Substituts"));

            if (hop == null)
            {
                return NotFound();
            }
            var result = new HopCompleteDto() { Hops = new List<HopDto>() };
            result.Hops.Add(hop);

            return Ok(result);
        }

        [Route("{id:int}/details")]
        [ResponseType(typeof(Hop))]
        public async Task<IHttpActionResult> GetHopDetail(int id)
        {
            var hop = await db.Hops.Include(h => h.Origin).Where(h => h.Id == id).FirstOrDefaultAsync();
            if (hop == null)
            {
                return NotFound();
            }
            return Ok(hop);
        }

        [Route("{origin}")]
        public IQueryable<Hop> GetHopByOrigin(string origin)
        {
            Log.Debug("Origin: " + origin);
            return db.Hops.Include(h => h.Origin)
                .Where(h => h.Origin.Name.Equals(origin, StringComparison.OrdinalIgnoreCase));
        }

        // PUT api/Hops/5
        [Route("{id:int}")]
        public IHttpActionResult PutHop(int id, HopDto hopDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hopDto.Id)
            {
                return BadRequest();
            }
            var hop = Mapper.Map<HopDto, Hop>(hopDto);

            hopRepository.Update(hop);


            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Hops

        [Route("")]
        [ResponseType(typeof(IList<HopDto>))]
        public IHttpActionResult PostHop(IList<HopDto> HopDtos)
        {
            Log.Debug("Hops Post");
            if (!ModelState.IsValid)
            {
                Log.Debug("Invalid ModelState");

                return BadRequest(ModelState);
            }
            var hops = Mapper.Map<IList<HopDto>, Hop[]>(HopDtos);

            hopRepository.Add(hops);

            var results = Mapper.Map<IList<Hop>, IList<HopDto>>(hopRepository.GetAll("Flavours.Flavour", "Origin", "Substituts"));
            using (var redis = ConnectionMultiplexer.Connect(redisStore))
            {
                var redisClient = redis.GetDatabase();
                foreach (var hop in results)
                {
                    redisClient.HashSet("hops", hop.Id, JsonConvert.SerializeObject(hop),flags:CommandFlags.FireAndForget);
                }
            }
            return CreatedAtRoute("DefaultApi", new { controller = "hops", }, results);
        }

        // DELETE api/Hopd/5
        [Route("{id:int}")]
        [ResponseType(typeof(Hop))]
        public async Task<IHttpActionResult> DeleteHop(int id)
        {
            Hop hop = await db.Hops.FindAsync(id);
            if (hop == null)
            {
                return NotFound();
            }

            db.Hops.Remove(hop);
            await db.SaveChangesAsync();

            return Ok(hop);
        }

        [Route("hopforms")]
        public IList<HopForm> GetHopForm()
        {
            var hopforms = db.HopForms.ToList();
            using (var redis = ConnectionMultiplexer.Connect(redisStore))
            {
                var redisClient = redis.GetDatabase();
                foreach (var item in hopforms)
                {
                    redisClient.HashSet("hopforms", item.Id, JsonConvert.SerializeObject(item), flags: CommandFlags.FireAndForget);
                }
            }

            return hopforms;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HopExists(int id)
        {
            return db.Hops.Count(e => e.Id == id) > 0;
        }
    }
}