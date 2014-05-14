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
        private IBreweryRepository _breweryRepository;

        public BreweryController(IBreweryRepository breweryRepository)
        {
            this._breweryRepository = breweryRepository;
        }

        /// <summary>
        /// Get all breweries.
        /// <response code="200">OK</response>
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public async Task<BreweryCompleteDto> GetBreweries()
        {
            var brewery = await _breweryRepository.GetAllAsync("Members.Member", "Beers");
            var breweriesDto = Mapper.Map<IList<Brewery>, IList<BreweryDto>>(brewery);
            var result = new BreweryCompleteDto();
            result.Breweries = breweriesDto;
            return result;
        }

        /// <summary>
        /// Get brewery by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Beerstyle id</param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(BreweryCompleteDto))]
        public async Task<IHttpActionResult> GetBrewery(int id)
        {
            var brewery = await _breweryRepository.GetSingleAsync(b => b.Id == id, "Members.Member", "Beers");
            if (brewery == null)
            {
                return NotFound();
            }
            var result = new BreweryCompleteDto() { Breweries = new List<BreweryDto>() };
            result.Breweries.Add(Mapper.Map<Brewery, BreweryDto>(brewery));
            return Ok(result);
        }

        /// <summary>
        /// Updates a beerstyle.
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Brewery id</param>
        /// <param name="beerstyle">Brewery object</param>
        /// <returns></returns>
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutBrewery(int id, BreweryDto breweryDto)
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
            var response = await _breweryRepository.UpdateAsync(brewery);
         
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add brewery.
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <param name="beerstyles">Brewery object.</param>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(IList<BreweryDto>))]
        public async Task<IHttpActionResult> PostBrewery(IList<BreweryDto> breweryPosts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var breweries = Mapper.Map<IList<BreweryDto>, Brewery[]>(breweryPosts);
            await _breweryRepository.AddAsync(breweries);

            return CreatedAtRoute("DefaultApi", new { controller = "breweries" }, breweryPosts);
        }

        /// <summary>
        /// Deletes brewery by id.
        /// </summary>
        /// <response code="404">Node Found</response>
        /// <param name="id">Brewery id.</param>
        /// <returns></returns>
        [ResponseType(typeof(BreweryDto))]
        public async Task<IHttpActionResult> DeleteBrewery(int id)
        {
            var brewery = await _breweryRepository.GetSingleAsync(b => b.Id == id);
            if (brewery == null)
            {
                return NotFound();
            }
            await _breweryRepository.RemoveAsync(brewery);
            var breweryDto = Mapper.Map<Brewery, BreweryDto>(brewery);
            return Ok(breweryDto);
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