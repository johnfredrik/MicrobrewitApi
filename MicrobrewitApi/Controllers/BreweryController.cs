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
using Microbrewit.Repository;
using AutoMapper;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("breweries")]
    public class BreweryController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private readonly IBreweryRepository breweryRepository = new BreweryRepository();

        // GET api/Brewery
        [Route("")]
        public BreweryCompleteDto GetBreweries()
        {
            var breweries = Mapper.Map<IList<Brewery>, IList<BreweryDto>>(breweryRepository.GetAll("Members.Member","Beers"));
            var result = new BreweryCompleteDto();
            result.Breweries = breweries;
            return result;
        }

        // GET api/Brewery/5
        [Route("{id}")]
        [ResponseType(typeof(BreweryCompleteDto))]
        public IHttpActionResult GetBrewery(int id)
        {
            var brewery = breweryRepository.GetSingle(b => b.Id == id, "Members.Member", "Beers");
            if (brewery == null)
            {
                return NotFound();
            }
            var result = new BreweryCompleteDto() { Breweries = new List<BreweryDto>() };
            result.Breweries.Add(Mapper.Map<Brewery, BreweryDto>(brewery));
            return Ok(result);
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
        [Route("")]
        [ResponseType(typeof(IList<Brewery>))]
        public IHttpActionResult PostBrewery(IList<Brewery> breweryPosts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var breweries = breweryPosts.ToArray();
            breweryRepository.Add(breweries);

            return CreatedAtRoute("DefaultApi", new { controller = "breweries" }, breweryPosts);
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