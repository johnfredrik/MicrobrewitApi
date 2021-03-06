﻿using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Interface;
using Thinktecture.IdentityModel.Authorization;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("breweries")]
    public class BreweryController : ApiController
    {
        private readonly IBreweryService _breweryService;
        //private readonly string _blobPath = "https://microbrewit.blob.core.windows.net/images/";

        public BreweryController(IBreweryService breweryService)
        {
            _breweryService = breweryService;
        }

        /// <summary>
        /// Get all breweries.
        /// <response code="200">OK</response>
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public async Task<BreweryCompleteDto> GetBreweries(int from = 0, int size = 20)
        {
            if (size > 1000) size = 1000;
            var breweriesDto = await _breweryService.GetAllAsync(from, size);
            var result = new BreweryCompleteDto { Breweries = breweriesDto.OrderBy(b => b.Name).ToList() };
            return result;
        }


        /// <summary>
        /// Get brewery by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Beerstyle id</param>
        /// <returns></returns>
        [Route("{id:int}")]
        [ResponseType(typeof(BreweryCompleteDto))]
        public async Task<IHttpActionResult> GetBrewery(int id)
        {
            var breweryDto = await _breweryService.GetSingleAsync(id);
            if (breweryDto == null)
            {
                return NotFound();
            }
            var result = new BreweryCompleteDto() { Breweries = new List<BreweryDto>() };
            result.Breweries.Add(breweryDto);
            return Ok(result);
        }

        /// <summary>
        /// Updates a brewery member.
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Brewery id</param>
        /// <param name="breweryDto">Brewery object</param>
        /// <returns></returns>
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutBrewery(int id, BreweryDto breweryDto)
        {
            // Checks if login user is allowed to change brewery.
            var isAllowed = ClaimsAuthorization.CheckAccess("Post", "BreweryId", id.ToString());
            if (!isAllowed)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != breweryDto.Id)
            {
                return BadRequest();
            }
            await _breweryService.UpdateAsync(breweryDto);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add brewery.
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <param name="brewery">List of brewery objects</param>
        /// <returns></returns>
        [ClaimsAuthorize("Post", "Brewery")]
        [Route("")]
        [ResponseType(typeof(BreweryDto))]
        public async Task<IHttpActionResult> PostBrewery(BreweryDto brewery)
        {
            if (brewery == null) return BadRequest();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _breweryService.AddAsync(brewery);
                return CreatedAtRoute("DefaultApi", new {controller = "breweries"}, result);
            }
            catch (DbEntityValidationException exception)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = exception.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                return BadRequest(string.Concat(exception.Message, " The validation errors are: ", fullErrorMessage));
            }
            catch (DbUpdateException exception)
            {
                
                if (exception.ToString().Contains("IX_BreweryName"))
                    ModelState.AddModelError("Name", "Duplicate brewery name not allowed.");

                return BadRequest(ModelState);
            }
            
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
            var isAllowed = ClaimsAuthorization.CheckAccess("Post", "BreweryId", id.ToString());
            if (!isAllowed)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
            var brewery = await _breweryService.DeleteAsync(id);
            if (brewery == null)
            {
                return NotFound();
            }
            return Ok(brewery);
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
        [ResponseType(typeof(BreweryMemberDto))]
        public async Task<IHttpActionResult> DeleteBreweryMember(int id, string username)
        {
            var isAllowed = ClaimsAuthorization.CheckAccess("Delete", "BreweryId", id.ToString());
            if (!isAllowed)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
            var breweryMember = await _breweryService.DeleteMember(id, username);
            if (breweryMember == null)
            {
                return NotFound();
            }
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
        [ResponseType(typeof(BreweryMemberDto))]
        public async Task<IHttpActionResult> GetBreweryMember(int id, string username)
        {
            var breweryMember = await _breweryService.GetBreweryMember(id, username);
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
        [ResponseType(typeof(IList<BreweryMemberDto>))]
        public async Task<IHttpActionResult> GetBreweryMembers(int id)
        {
            var breweryMembers = await _breweryService.GetAllMembers(id);
            return Ok(breweryMembers);
        }

        /// <summary>
        /// Updates a brewery member for a brewery.
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Brewery id</param>
        /// <param name="username">Brewery admin users username</param>
        /// <param name="breweryMember">Member to be added</param>
        /// <returns></returns>
        [Route("{id:int}/members/{username}")]
        public async Task<IHttpActionResult> PutBreweryMember(int id, string username, BreweryMemberDto breweryMember)
        {
            var isAllowed = ClaimsAuthorization.CheckAccess("Put", "BreweryId", id.ToString());
            if (!isAllowed)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (username != breweryMember.Username)
            {
                return BadRequest();
            }
            await _breweryService.UpdateBreweryMember(id, breweryMember);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("{id:int}/members")]
        [ResponseType(typeof(BreweryMemberDto))]
        public async Task<IHttpActionResult> PostBreweryMember(int id, BreweryMemberDto breweryMember)
        {
            var isAllowed = ClaimsAuthorization.CheckAccess("Post", "BreweryId", id.ToString());
            if (!isAllowed)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _breweryService.AddBreweryMember(id, breweryMember);
            return Ok(result);
        }

        /// <summary>
        /// Updates elasticsearch with database data.
        /// </summary>
        /// <returns></returns>
        [ClaimsAuthorize("Reindex", "BeerStyle")]
        [Route("es")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateBreweryElasticSearch()
        {
            await _breweryService.ReIndexElasticSearch();
            return Ok();
        }


        /// <summary>
        /// Searches in breweries.
        /// </summary>
        /// <param name="query">The pharse you want to match.</param>
        /// <param name="from">Start point of the search.</param>
        /// <param name="size">Number of results returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<BreweryCompleteDto> GetBreweriesBySearch(string query, int from = 0, int size = 20)
        {
            if (size > 1000) size = 1000;
            var breweriesDto = await _breweryService.SearchAsync(query, from, size);
            return new BreweryCompleteDto { Breweries = breweriesDto.ToList() };
        }

        [Route("{id:int}/upload")]
        [HttpPost]
        public async Task<IHttpActionResult> UploadFile(int breweryId)
        {
            var isAllowed = ClaimsAuthorization.CheckAccess("Upload", "BreweryId", breweryId.ToString());
            if (!isAllowed)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var brewery = await _breweryService.GetSingleAsync(breweryId);
            if (brewery == null) return NotFound();
            MultipartStreamProvider provider = new BlobStorageMultipartStreamProvider(brewery);
            await Request.Content.ReadAsMultipartAsync(provider);
            var avatar = provider.Contents.SingleOrDefault(p => p.Headers.ContentDisposition.Name.Contains("avatar"));
            var headerImage =
                provider.Contents.SingleOrDefault(p => p.Headers.ContentDisposition.Name.Contains("headerImage"));
            if (avatar != null)
            {
                var fileName = avatar.Headers.ContentDisposition.FileName;
                brewery.Avatar = fileName;
            }
            if (headerImage != null)
            {
                var fileName = headerImage.Headers.ContentDisposition.FileName;
                brewery.HeaderImage = fileName;
            }
            await _breweryService.UpdateAsync(brewery);
            return Ok();
        }

    }
}