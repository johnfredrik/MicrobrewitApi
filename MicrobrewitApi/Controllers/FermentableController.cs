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
using MicrobrewitApi.Models;

namespace MicrobrewitApi.Controllers
{
    public class FermentableController : ApiController
    {
        private MicrobrewitApiContext db = new MicrobrewitApiContext();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET api/Fermentable
        public IQueryable<Fermentable> GetFermentables()
        {
            return db.Fermentables;
        }

        // GET api/Fermentable/5
        [ResponseType(typeof(Fermentable))]
        public async Task<IHttpActionResult> GetFermentable(int id)
        {
            Fermentable fermentable = await db.Fermentables.FindAsync(id);
            if (fermentable == null)
            {
                return NotFound();
            }

            return Ok(fermentable);
        }

        // PUT api/Fermentable/5
        public async Task<IHttpActionResult> PutFermentable(int id, Fermentable fermentable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fermentable.FermentableId)
            {
                return BadRequest();
            }

            db.Entry(fermentable).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FermentableExists(id))
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

        // POST api/Fermentable
        [ResponseType(typeof(Fermentable))]
        public async Task<IHttpActionResult> PostFermentable(Fermentable fermentable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Fermentables.Add(fermentable);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = fermentable.FermentableId }, fermentable);
        }

        // DELETE api/Fermentable/5
        [ResponseType(typeof(Fermentable))]
        public async Task<IHttpActionResult> DeleteFermentable(int id)
        {
            Fermentable fermentable = await db.Fermentables.FindAsync(id);
            if (fermentable == null)
            {
                return NotFound();
            }

            db.Fermentables.Remove(fermentable);
            await db.SaveChangesAsync();

            return Ok(fermentable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FermentableExists(int id)
        {
            return db.Fermentables.Count(e => e.FermentableId == id) > 0;
        }
    }
}