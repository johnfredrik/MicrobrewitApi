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
    [RoutePrefix("api/others")]
    public class OthersController : ApiController
    {
        private MicrobrewitApiContext db = new MicrobrewitApiContext();

        // GET api/Others
        [Route("")]
        public IQueryable<Other> GetOthers()
        {
            return db.Others;
        }

        [Route("spices")]
        public IQueryable<Spice> GetSpice()
        {
            return db.Others.OfType<Spice>();
        }

        [Route("fruits")]
        public IQueryable<Fruit> GetFruit()
        {
            return db.Others.OfType<Fruit>();
        }

        [Route("nonefermentablesugars")]
        public IQueryable<NoneFermentableSugar> GetNoneFermentableSugars()
        {
            return db.Others.OfType<NoneFermentableSugar>();
        }

        // GET api/Others/5
        [Route("{id:int}")]
        [ResponseType(typeof(Other))]
        public async Task<IHttpActionResult> GetOther(int id)
        {
            Other other = await db.Others.FindAsync(id);
            if (other == null)
            {
                return NotFound();
            }

            return Ok(other);
        }

        // PUT api/Others/5
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutOther(int id, Spice other)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != other.Id)
            {
                return BadRequest();
            }

            db.Entry(other).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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

        // POST api/others/spices
        [Route("spices")]
        [ResponseType(typeof(Spice))]
        public async Task<IHttpActionResult> PostOther(Spice spice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Others.Add(spice);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { controller="others", id = spice.Id }, spice);
        }
        // POST api/others/fruits
        [Route("fruits")]
        [ResponseType(typeof(Fruit))]
        public async Task<IHttpActionResult> PostOther(Fruit fruit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Others.Add(fruit);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { controller = "others", id = fruit.Id }, fruit);
        }

        // POST api/others/fruits
        [Route("nonefermentablesugars")]
        [ResponseType(typeof(NoneFermentableSugar))]
        public async Task<IHttpActionResult> PostOther(NoneFermentableSugar nonefermentablesugar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Others.Add(nonefermentablesugar);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { controller = "others", id = nonefermentablesugar.Id }, nonefermentablesugar);
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