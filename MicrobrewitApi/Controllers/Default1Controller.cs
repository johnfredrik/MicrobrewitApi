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

namespace Microbrewit.Api.Controllers
{
    public class Default1Controller : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();

        // GET api/Default1
        public IQueryable<Beer> GetBeers()
        {
            return db.Beers;
        }

        // GET api/Default1/5
        [ResponseType(typeof(Beer))]
        public async Task<IHttpActionResult> GetBeer(int id)
        {
            Beer beer = await db.Beers.FindAsync(id);
            if (beer == null)
            {
                return NotFound();
            }

            return Ok(beer);
        }

        // PUT api/Default1/5
        public async Task<IHttpActionResult> PutBeer(int id, Beer beer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beer.Id)
            {
                return BadRequest();
            }

            db.Entry(beer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeerExists(id))
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

        // POST api/Default1
        [ResponseType(typeof(Beer))]
        public async Task<IHttpActionResult> PostBeer(Beer beer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Beers.Add(beer);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BeerExists(beer.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = beer.Id }, beer);
        }

        // DELETE api/Default1/5
        [ResponseType(typeof(Beer))]
        public async Task<IHttpActionResult> DeleteBeer(int id)
        {
            Beer beer = await db.Beers.FindAsync(id);
            if (beer == null)
            {
                return NotFound();
            }

            db.Beers.Remove(beer);
            await db.SaveChangesAsync();

            return Ok(beer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BeerExists(int id)
        {
            return db.Beers.Count(e => e.Id == id) > 0;
        }
    }
}