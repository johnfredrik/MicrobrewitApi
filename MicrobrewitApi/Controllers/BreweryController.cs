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
        [Route("{id:int}")]
        public IHttpActionResult PutBrewery(int id, BreweryDto breweryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != breweryDto.Id)
            {
                return BadRequest();
            }
            var brewery = Mapper.Map<BreweryDto, Brewery>(breweryDto);
            breweryRepository.Update(brewery);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Brewery
        [Route("")]
        [ResponseType(typeof(IList<BreweryDto>))]
        public IHttpActionResult PostBrewery(IList<BreweryDto> breweryPosts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var breweries = Mapper.Map<IList<BreweryDto>, Brewery[]>(breweryPosts);
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