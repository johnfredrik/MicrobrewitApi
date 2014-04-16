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
    public class OthersController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private IOtherRepository otherRepository = new OtherRepository();



        // GET api/Others
        [Route("")]
        public OtherCompleteDto GetOthers()
        {
            var others = Mapper.Map<IList<Other>, IList<OtherDto>>(otherRepository.GetAll());
            var result = new OtherCompleteDto();
            result.Others = others;
            return result;
        }


        // GET api/Others/5
        [Route("{id:int}")]
        [ResponseType(typeof(Other))]
        [HttpGet]
        public IHttpActionResult GetOther(int id)
        {
            var other = Mapper.Map<Other, OtherDto>(otherRepository.GetSingle(o => o.Id == id));
            if (other == null)
            {
                return NotFound();
            }
            var result = new OtherCompleteDto() { Others = new List<OtherDto>() };
            result.Others.Add(other);

            return Ok(result);
        }

        // PUT api/Others/5
        [Route("{id:int}")]
        public IHttpActionResult PutOther(int id, Other other)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != other.Id)
            {
                return BadRequest();
            }

            try
            {
                otherRepository.Update(other);               
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OtherExists(id))
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

        // POST api/others
        [Route("")]
        [ResponseType(typeof(IList<Other>))]
        public IHttpActionResult PostOther(IList<Other> otherPosts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var others = Mapper.Map<IList<Other>, Other[]>(otherPosts);
            otherRepository.Add(others);

            var result = Mapper.Map<IList<Other>, IList<OtherDto>>(otherRepository.GetAll());

            using (var redis = ConnectionMultiplexer.Connect(redisStore))
            {
                var redisClient = redis.GetDatabase();
                foreach (var item in result)
                {
                    redisClient.HashSet("others", item.Id, JsonConvert.SerializeObject(item), flags: CommandFlags.FireAndForget);
                }
            }

            return CreatedAtRoute("DefaultApi", new { controller = "others", }, otherPosts);
        }


        // DELETE api/Others/5
        [Route("{id:int}")]
        [ResponseType(typeof(Other))]
        public async Task<IHttpActionResult> DeleteOther(int id)
        {
            Other other = await db.Others.FindAsync(id);
            if (other == null)
            {
                return NotFound();
            }

            db.Others.Remove(other);
            await db.SaveChangesAsync();

            return Ok(other);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OtherExists(int id)
        {
            return db.Others.Count(e => e.Id == id) > 0;
        }
    }
}