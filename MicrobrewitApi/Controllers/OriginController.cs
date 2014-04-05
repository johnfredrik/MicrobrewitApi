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
using log4net;
using Microbrewit.Repository;
using AutoMapper;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("origins")]
    public class OriginController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly IOriginRespository originRepsository = new OriginRepository();

        // GET api/Origin
        [Route("")]
        public IList<Origin> GetOrigins()
        {
            return originRepsository.GetAll();
        }

        // GET api/Origin/5
        [Route("{id:int}")]
        [ResponseType(typeof(Origin))]
        public IHttpActionResult GetOrigin(int id)
        {
            Origin origin = originRepsository.GetSingle(o => o.Id == id);
            if (origin == null)
            {
                return NotFound();
            }

            return Ok(origin);
        }

        // PUT api/Origin/5
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutOrigin(int id, Origin origin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != origin.Id)
            {
                return BadRequest();
            }

            db.Entry(origin).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OriginExists(id))
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

        // POST api/Origin
        [Route("")]
        [ResponseType(typeof(IList<Origin>))]
        public IHttpActionResult PostOrigin(IList<Origin> originPosts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var origins = originPosts.ToArray();
            originRepsository.Add(origins);


            return CreatedAtRoute("DefaultApi", new { controller= "origins", }, origins);
        }

        // DELETE api/Origin/5
        [Route("{id:int}")]
        [ResponseType(typeof(Origin))]
        public async Task<IHttpActionResult> DeleteOrigin(int id)
        {
            Origin origin = await db.Origins.FindAsync(id);
            if (origin == null)
            {
                return NotFound();
            }

            db.Origins.Remove(origin);
            await db.SaveChangesAsync();

            return Ok(origin);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OriginExists(int id)
        {
            return db.Origins.Count(e => e.Id == id) > 0;
        }
    }
}