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
using MicrobrewitModel;

namespace MicrobrewitApi.Controllers
{
    public class OriginController : ApiController
    {
        private MicrobrewitApiContext db = new MicrobrewitApiContext();

        // GET api/Origin
        public IQueryable<Origin> GetOrigins()
        {
            return db.Origins;
        }

        // GET api/Origin/5
        [ResponseType(typeof(Origin))]
        public async Task<IHttpActionResult> GetOrigin(int id)
        {
            Origin origin = await db.Origins.FindAsync(id);
            if (origin == null)
            {
                return NotFound();
            }

            return Ok(origin);
        }

        // PUT api/Origin/5
        public async Task<IHttpActionResult> PutOrigin(int id, Origin origin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != origin.OriginId)
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
        [ResponseType(typeof(Origin))]
        public async Task<IHttpActionResult> PostOrigin(Origin origin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Origins.Add(origin);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = origin.OriginId }, origin);
        }

        // DELETE api/Origin/5
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
            return db.Origins.Count(e => e.OriginId == id) > 0;
        }
    }
}