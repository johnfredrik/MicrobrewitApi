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
    public class BreweryController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();

        // GET api/Brewery
        public IQueryable<Brewery> GetBreweries()
        {
            return db.Breweries.Include(b => b.Members);
        }

        // GET api/Brewery/5
        [ResponseType(typeof(Brewery))]
        public async Task<IHttpActionResult> GetBrewery(int id)
        {
            Brewery brewery = await db.Breweries.Include("Members.Member").Where(b => b.Id == id).SingleOrDefaultAsync();
            if (brewery == null)
            {
                return NotFound();
            }

            return Ok(brewery);
        }

        // PUT api/Brewery/5
        public async Task<IHttpActionResult> PutBrewery(int id, Brewery brewery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != brewery.Id)
            {
                return BadRequest();
            }

            db.Entry(brewery).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BreweryExists(id))
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

        // POST api/Brewery
        [ResponseType(typeof(Brewery))]
        public async Task<IHttpActionResult> PostBrewery(Brewery brewery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Breweries.Add(brewery);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = brewery.Id }, brewery);
        }

        // DELETE api/Brewery/5
        [ResponseType(typeof(Brewery))]
        public async Task<IHttpActionResult> DeleteBrewery(int id)
        {
            Brewery brewery = await db.Breweries.FindAsync(id);
            if (brewery == null)
            {
                return NotFound();
            }

            db.Breweries.Remove(brewery);
            await db.SaveChangesAsync();

            return Ok(brewery);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BreweryExists(int id)
        {
            return db.Breweries.Count(e => e.Id == id) > 0;
        }
    }
}