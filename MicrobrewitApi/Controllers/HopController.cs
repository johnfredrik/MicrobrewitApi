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
using Microbrewit.Api.Redis;

namespace Microbrewit.Api.Controllers
{

    [RoutePrefix("hops")]
    public class HopController : ApiController
    {

        private MicrobrewitContext db = new MicrobrewitContext();
        private IHopRepository _hopRepository;
        private Elasticsearch.ElasticSearch _elasticsearch;
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HopController(IHopRepository hopRepository)
        {
            this._hopRepository = hopRepository;
            this._elasticsearch = new Elasticsearch.ElasticSearch();
        }

        // GET api/Hops
        [Route("")]
        public async Task<HopCompleteDto> GetHops()
        {
            IList<HopDto> hopsDto = new List<HopDto>();
            IList<Hop> hops;
            hopsDto = await HopsRedis.GetHopsRedis();
            if (hopsDto.Count <= 0)
            {
                hops = await _hopRepository.GetAllAsync("Flavours.Flavour", "Origin", "Substituts");
                hopsDto = Mapper.Map<IList<Hop>, IList<HopDto>>(hops);
            } 
            var result = new HopCompleteDto() { Hops = hopsDto };
            return result;
        }

      

        // GET api/Hops/5
        [Route("{id:int}")]
        [ResponseType(typeof(HopCompleteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> GetHop(int id)
        {
            HopDto hopDto = null;
            hopDto = await HopsRedis.GetHopRedis(id);
            if (hopDto == null)
            {
                var hop = await _hopRepository.GetSingleAsync(h => h.Id == id, "Flavours.Flavour", "Origin", "Substituts");
                hopDto = Mapper.Map<Hop, HopDto>(hop);
            }

            if (hopDto == null)
            {
                return NotFound();
            }
         
            var result = new HopCompleteDto() { Hops = new List<HopDto>() };
            result.Hops.Add(hopDto);

            return Ok(result);
        }

        // PUT api/Hops/5
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutHop(int id, HopDto hopDto)
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
            await _hopRepository.UpdateAsync(hop);
            var hops = await _hopRepository.GetAllAsync("Flavours.Flavour", "Origin", "Substituts");
            var hopsDto = Mapper.Map<IList<Hop>, IList<HopDto>>(hops);
            await HopsRedis.UpdateRedisStore(hopsDto);
            // updated elasticsearch.
            await _elasticsearch.UpdateHopElasticSearch(hopsDto);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Hops

        [Route("")]
        [ResponseType(typeof(IList<HopDto>))]
        public async Task<IHttpActionResult> PostHop(IList<HopDto> HopDtos)
        {
            Log.Debug("Hops Post");
            if (!ModelState.IsValid)
            {
                Log.Debug("Invalid ModelState");

                return BadRequest(ModelState);
            }
            var hops = Mapper.Map<IList<HopDto>, Hop[]>(HopDtos);

            await _hopRepository.AddAsync(hops);

            var result = await _hopRepository.GetAllAsync("Flavours.Flavour", "Origin", "Substituts");
            var resultsDto = Mapper.Map<IList<Hop>, IList<HopDto>>(result);
            var hopsDto = Mapper.Map<IList<Hop>, IList<HopDto>>(hops);
            await HopsRedis.UpdateRedisStore(hopsDto);
            // updated elasticsearch.
            await _elasticsearch.UpdateHopElasticSearch(hopsDto);
            return CreatedAtRoute("DefaultApi", new { controller = "hops", }, resultsDto);
        }

       

        // DELETE api/Hopd/5
        [Route("{id:int}")]
        [ResponseType(typeof(Hop))]
        public async Task<IHttpActionResult> DeleteHop(int id)
        {
            Hop hop = await _hopRepository.GetSingleAsync(h => h.Id == id);
            if (hop == null)
            {
                return NotFound();
            }

            await _hopRepository.RemoveAsync(hop);
            var hops = await _hopRepository.GetAllAsync("Flavours.Flavour", "Origin", "Substituts");
            var hopsDto = Mapper.Map<IList<Hop>, IList<HopDto>>(hops);
            await HopsRedis.UpdateRedisStore(hopsDto);
            // updated elasticsearch.
            await _elasticsearch.UpdateHopElasticSearch(hopsDto);
            return Ok(hop);
        }

        [Route("forms")]
        public IList<HopForm> GetHopForm()
        {
            var hopforms = db.HopForms.ToList();
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    foreach (var item in hopforms)
                    {
                        redisClient.HashSet("hopforms", item.Id, JsonConvert.SerializeObject(item), flags: CommandFlags.FireAndForget);
                    }
                }

            }
            catch (RedisConnectionException)
            {
                Log.ErrorFormat("RedisConnectionException was thrown");
            }
            return hopforms;
        }
        
        [Route("redis")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateHopsRedis()
        {
            var hops = await _hopRepository.GetAllAsync("Flavours.Flavour", "Origin", "Substituts");
            var hopsDto = Mapper.Map<IList<Hop>, IList<HopDto>>(hops);
            await HopsRedis.UpdateRedisStore(hopsDto);
            // updated elasticsearch.
            await _elasticsearch.UpdateHopElasticSearch(hopsDto);
           return Ok();
        }

        [HttpGet]
        [Route("")]
        public async Task<HopCompleteDto> GetYeastsBySearch(string query, int from = 0, int size = 20)
        {
            var hopDto = await _elasticsearch.GetHops(query,from,size);

            var result = new HopCompleteDto   ();
            result.Hops = hopDto.ToList();
            return result;
        }
    }
}