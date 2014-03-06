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
using MicrobrewitApi.Util;
using log4net;

namespace MicrobrewitApi.Controllers
{
    [RoutePrefix("api/fermentables")]
    public class FermentableController : ApiController
    {
        private MicrobrewitApiContext db = new MicrobrewitApiContext();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      
        // GET api/Fermentable 
        [BasicAuthenticationAttibute]
        [Route("")]
        public IQueryable<Fermentable> GetFermentables()
        {
            return db.Fermentables.Include(f => f.Supplier).Include(f => f.Supplier.Origin);
        }

        // GET api/Fermentable/5
        [Route("{id:int}")]
        [ResponseType(typeof(Fermentable))]
        public async Task<IHttpActionResult> GetFermentable(int id)
        {
            Fermentable fermentable  = await db.Fermentables.Include(f => f.Supplier).Include(f => f.Supplier.Origin)
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();
            if (fermentable == null)
            {
                return NotFound();
            }
            Log.Debug(fermentable.GetType());
            return Ok(fermentable);
        }

        [Route("grains")]
        public IQueryable<Grain> GetGrains()
        {
            return db.Fermentables.OfType<Grain>();           
        }

        [Route("sugars")]
        public IQueryable<Sugar> GetSugars()
        {
            return db.Fermentables.OfType<Sugar>();
        }

        [Route("dryextracts")]
        public IQueryable<DryExtract> GetDryExtracts()
        {
            return db.Fermentables.OfType<DryExtract>();
        }

        [Route("liquidextracts")]
        public IQueryable<LiquidExtract> GetLiquidExtracts()
        {
            return db.Fermentables.OfType<LiquidExtract>();
        }


        [Route("extracts")]
        public IQueryable<Fermentable> GetExtracts()
        {
            return db.Fermentables.Where(f => f.GetType().Equals("Liquid Extract") || f.GetType().Equals("Dry Extract"));
        }

        // PUT api/Fermentable/5
        public async Task<IHttpActionResult> PutFermentable(int id, Fermentable fermentable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fermentable.Id)
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

            return CreatedAtRoute("DefaultApi", new { controller = "fermetable", id = fermentable.Id}, fermentable);
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
            return db.Fermentables.Count(e => e.Id == id) > 0;
        }
    }
}