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
using Microbrewit.Api;
using Microbrewit.Repository;
using log4net;

namespace Microbrewit.Api.Controllers
{
   // [TokenValidationAttribute]
    [RoutePrefix("api/fermentables")]
    public class FermentableController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private IFermentableRepository fermentableRepository = new FermentableRepository();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      
        // GET api/Fermentable 
        [Route("")]
        public IList<Fermentable> GetFermentables()
        {
            return fermentableRepository.GetFermentables();
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
        public IList<Grain> GetGrains()
        {
            return fermentableRepository.GetGrains();      
        }

        [Route("sugars")]
        public IList<Sugar> GetSugars()
        {
            return fermentableRepository.GetSugars();
        }

        [Route("dryextracts")]
        public IList<DryExtract> GetDryExtracts()
        {
            return fermentableRepository.GetDryExtracts();
        }

        [Route("liquidextracts")]
        public IList<LiquidExtract> GetLiquidExtracts()
        {
            return fermentableRepository.GetLiquidExtracts();
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