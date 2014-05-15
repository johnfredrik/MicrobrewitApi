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
        [Route("{id:int}")]
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

        /// <summary>
        /// Deletes a brewery member from a brewery.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Brewery id</param>
        /// <param name="username">Username for member</param>
        /// <returns>Deleted brewery member</returns>
        [Route("{id:int}/members/{username}")]
        [ResponseType(typeof(BreweryMember))]
        public async Task<IHttpActionResult> DeleteBreweryMember(int id, string username)
        {
            var breweryMember = await _breweryRepository.GetBreweryMember(id, username);
            if (breweryMember == null)
            {
                return NotFound();
            }
            await _breweryRepository.DeleteBreweryMember(breweryMember.BreweryId, breweryMember.MemberUsername);
            return Ok(breweryMember);
        }

        /// <summary>
        /// Gets a brewery member of a brewery
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Brewery id</param>
        /// <param name="username">Member username</param>
        /// <returns>Returns brewery member</returns>
        [Route("{id:int}/members/{username}")]
        [ResponseType(typeof(BreweryMember))]
        public async Task<IHttpActionResult> GetBreweryMember(int id,string username)
        {
            var breweryMember = await _breweryRepository.GetBreweryMember(id,username);
            if (breweryMember == null)
            {
                return NotFound();
            }
            
            return Ok(breweryMember);
        }

        /// <summary>
        /// Gets all brewery members for a brewery.
        /// </summary>
        /// <response code="200">Ok</response>
        /// <param name="id">Brewery id</param>
        /// <returns>Returns list of brewery members</returns>
        [Route("{id:int}/members")]
        [ResponseType(typeof(IList<BreweryMember>))]
        public async Task<IHttpActionResult> GetBreweryMembers(int id)
        {
            var breweryMembers = await _breweryRepository.GetBreweryMembers(id);
            return Ok(breweryMembers);
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