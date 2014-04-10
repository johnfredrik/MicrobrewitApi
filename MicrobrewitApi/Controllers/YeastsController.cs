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
using ServiceStack.Redis;
using Newtonsoft.Json;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("yeasts")]
    public class YeastsController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private IYeastRepository yeastRespository = new YeastRepository();
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];

        // GET api/Yeasts
        [Route("")]
        public YeastCompleteDto GetYeasts()
        {
            var yeasts = Mapper.Map<IList<Yeast>,IList<YeastDto>>(yeastRespository.GetAll("Supplier"));
            var result = new YeastCompleteDto();
            result.Yeasts = yeasts;
            return result;
        }

        // GET api/Yeasts/5
        [Route("{id:int}")]
        [ResponseType(typeof(YeastCompleteDto))]
        [HttpGet]
        public IHttpActionResult GetYeast(int id)
        {
            var yeast = Mapper.Map<Yeast,YeastDto>(yeastRespository.GetSingle(y => y.Id == id,"Supplier"));
            if (yeast == null)
            {
                return NotFound();
            }
            var result = new YeastCompleteDto(){Yeasts = new List<YeastDto>()};
            result.Yeasts.Add(yeast);
            return Ok(result);
        }

        // PUT api/Yeasts/5
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
            yeastRespository.Update(yeast);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Yeasts
        [Route("")]
        [ResponseType(typeof(IList<YeastDto>))]
        public IHttpActionResult PostYeast(IList<YeastDto> yeastPosts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var yeasts = Mapper.Map<IList<YeastDto>,Yeast[]>(yeastPosts);
            yeastRespository.Add(yeasts);

            var y = Mapper.Map<IList<Yeast>, IList<YeastDto>>(yeastRespository.GetAll("Supplier.Origin"));

            using (var redisClient = new RedisClient(redisStore))
            {
                foreach (var item in y)
                {
                    redisClient.SetEntryInHash("yeasts", item.Id.ToString(), JsonConvert.SerializeObject(item));
                }
            }
            return CreatedAtRoute("DefaultApi", new {controller = "yeasts",}, yeastPosts);
        }

        // DELETE api/Yeasts/5
        [Route("{id:int}")]
        [ResponseType(typeof(Yeast))]
        public async Task<IHttpActionResult> DeleteYeast(int id)
        {
            Yeast yeast = await db.Yeasts.FindAsync(id);
            if (yeast == null)
            {
                return NotFound();
            }

            db.Yeasts.Remove(yeast);
            await db.SaveChangesAsync();

            return Ok(yeast);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool YeastExists(int id)
        {
            return db.Yeasts.Count(e => e.Id == id) > 0;
        }
    }
}