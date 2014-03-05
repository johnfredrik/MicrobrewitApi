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
    [RoutePrefix("api/yeasts")]
    public class YeastsController : ApiController
    {
        private MicrobrewitApiContext db = new MicrobrewitApiContext();

        // GET api/Yeasts
        [Route("")]
        public IQueryable<Yeast> GetYeasts()
        {
            return db.Yeasts.Include(y => y.Supplier).Include(y => y.Supplier.Origin);
        }

        // GET api/Yeasts/5
        [Route("{id:int}")]
        [ResponseType(typeof(Yeast))]
        public async Task<IHttpActionResult> GetYeast(int id)
        {
            Yeast yeast = await db.Yeasts.Include(y => y.Supplier).Where(y => y.Id == id).Include(y => y.Supplier.Origin).FirstOrDefaultAsync();
            if (yeast == null)
            {
                return NotFound();
            }

            return Ok(yeast);
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
        [Route("dryyeasts")]
        [ResponseType(typeof(DryYeast))]
        public async Task<IHttpActionResult> PostYeast(DryYeast yeast)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (yeast.SupplierId > 0)
            {
                yeast.Supplier = null;
            }
            db.Yeasts.Add(yeast);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = yeast.Id }, yeast);
        }

        // POST api/Yeasts
        [Route("liquidyeasts")]
        [ResponseType(typeof(LiquidYeast))]
        public async Task<IHttpActionResult> PostYeast(LiquidYeast yeast)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (yeast.SupplierId > 0)
            {
                yeast.Supplier = null;
            }
            db.Yeasts.Add(yeast);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { controller="yeasts", id = yeast.Id }, yeast);
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