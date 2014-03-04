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
using MicrobrewitApi.DTOs;
using System.Linq.Expressions;
using log4net;
using Newtonsoft.Json;

namespace MicrobrewitApi.Controllers
{

    [RoutePrefix("api/hops")]
    public class HopsController : ApiController
    {

        private MicrobrewitApiContext db = new MicrobrewitApiContext();

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Expression<Func<Hop,HopDto>> AsBookDto =
            x => new HopDto
            {
                Name = x.Name,
                Origin = x.Origin.Name
            };

        // GET api/Hops
        [Route("")]
        public IQueryable<Hop> GetHops()
        {
            return db.Hops.Include(h => h.Origin);
        }

        // GET api/Hops/5
        [Route("{id:int}")]
        [ResponseType(typeof(Hop))]
        public async Task<IHttpActionResult> GetHop(int id)
        {
            Log.Debug("Get Hops by id: " + id);
            Hop hop = await db.Hops.Include(h => h.Origin)
                .Where(h => h.HopId == id)
                .FirstOrDefaultAsync();
            if (hop == null)
            {
                return NotFound();
            }

            return Ok(hop);
        }

        [Route("{id:int}/details")]
        [ResponseType(typeof(HopDetailDto))]
        public async Task<IHttpActionResult> GetHopDetail(int id)
        {
            var hop = await (from h in db.Hops.Include(h => h.Origin)
                             where h.HopId == id
                             select new HopDetailDto
                             {
                                 Name = h.Name,
                                 AALow = h.AALow,
                                 AAHigh = h.AAHigh,
                                 Origin = h.Origin.Name
                             }).FirstOrDefaultAsync();
            if (hop == null)
            {
                return NotFound();
            }
            return Ok(hop);
        }

        [Route("{origin}")]
        public IQueryable<HopDto> GetHopByOrigin(string origin)
        {
            Log.Debug("Origin: " + origin);
            return db.Hops.Include(h => h.Origin)
                .Where(h => h.Origin.Name.Equals(origin, StringComparison.OrdinalIgnoreCase))
                .Select(AsBookDto);
        }

        //// PUT api/Hopd/5
        //public async Task<IHttpActionResult> PutHop(int id, Hop hop)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != hop.HopId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(hop).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!HopExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST api/Hops
        [Route("")]
        [ResponseType(typeof(Hop))]
        public async Task<IHttpActionResult> PostHop(Hop hop)
        {
            Log.Debug("Hops Post");
            if (!ModelState.IsValid)
            {
                Log.Debug("Invalid ModelState");

                return BadRequest(ModelState);
            }

            db.Hops.Add(hop);
            await db.SaveChangesAsync();

            Log.DebugFormat("Hop name:{0}, AAHigh: {1}, AALow: {2}, id: {3}", hop.Name, hop.AAHigh, hop.AALow,hop.HopId);
            return CreatedAtRoute("api/hops/",new {id = hop.HopId},hop);
        }

        //// DELETE api/Hopd/5
        //[ResponseType(typeof(Hop))]
        //public async Task<IHttpActionResult> DeleteHop(int id)
        //{
        //    Hop hop = await db.Hops.FindAsync(id);
        //    if (hop == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Hops.Remove(hop);
        //    await db.SaveChangesAsync();

        //    return Ok(hop);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HopExists(int id)
        {
            return db.Hops.Count(e => e.HopId == id) > 0;
        }
    }
}