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
        public async Task<IHttpActionResult> PutYeast(int id, Yeast yeast)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != yeast.Id)
            {
                return BadRequest();
            }

            db.Entry(yeast).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!YeastExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Yeasts
        [Route("")]
        [ResponseType(typeof(IList<YeastPostDto>))]
        public IHttpActionResult PostYeast(IList<YeastPostDto> yeastPosts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var yeasts = Mapper.Map<IList<YeastPostDto>,Yeast[]>(yeastPosts);
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

        //// POST api/Yeasts
        //[Route("liquidyeasts")]
        //[ResponseType(typeof(LiquidYeast))]
        //public async Task<IHttpActionResult> PostYeast(LiquidYeast yeast)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    if (yeast.SupplierId > 0)
        //    {
        //        yeast.Supplier = null;
        //    }
        //    db.Yeasts.Add(yeast);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { controller="yeasts", id = yeast.Id }, yeast);
        //}

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