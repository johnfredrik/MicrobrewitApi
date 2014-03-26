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
using Microbrewit.Api.DTOs;
using AutoMapper;
using Microbrewit.Repository;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("api/beerstyles")]
    public class BeerStyleController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private readonly IBeerStyleRepository repository = new BeerStyleRepository();

        [Route("")]
        // GET api/BeerStyle
        public BeerStyleCompleteDto GetBeerStyles()
        {
            var beerStyles = Mapper.Map<IList<BeerStyle>,IList<BeerStyleDto>>(repository.GetAll("SubStyles","SuperStyle"));
            var result = new BeerStyleCompleteDto();
            result.BeerStyles = beerStyles;
            return result;
        }

        [Route("{id:int}")]
        // GET api/BeerStyle/5
        [ResponseType(typeof(BeerStyle))]
        public async Task<IHttpActionResult> GetBeerStyle(int id)
        {
            var beerStyle = Mapper.Map<BeerStyle, BeerStyleDto>(repository.GetSingle(b => b.Id == id, "SubStyles", "SuperStyle"));
            if (beerStyle == null)
            {
                return NotFound();
            }
            var result = new BeerStyleCompleteDto() { BeerStyles = new List<BeerStyleDto>()};
            result.BeerStyles.Add(beerStyle);

            return Ok(result);
        }

        // PUT api/BeerStyle/5
        public async Task<IHttpActionResult> PutBeerStyle(int id, BeerStyle beerstyle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beerstyle.Id)
            {
                return BadRequest();
            }

            db.Entry(beerstyle).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeerStyleExists(id))
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

        // POST api/BeerStyle
        [ResponseType(typeof(BeerStyle))]
        public async Task<IHttpActionResult> PostBeerStyle(BeerStyle beerstyle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BeerStyles.Add(beerstyle);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = beerstyle.Id }, beerstyle);
        }

        // DELETE api/BeerStyle/5
        [ResponseType(typeof(BeerStyle))]
        public async Task<IHttpActionResult> DeleteBeerStyle(int id)
        {
            BeerStyle beerstyle = await db.BeerStyles.FindAsync(id);
            if (beerstyle == null)
            {
                return NotFound();
            }

            db.BeerStyles.Remove(beerstyle);
            await db.SaveChangesAsync();

            return Ok(beerstyle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BeerStyleExists(int id)
        {
            return db.BeerStyles.Count(e => e.Id == id) > 0;
        }
    }
}