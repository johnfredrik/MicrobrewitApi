﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using log4net;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Interface;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Microbrewit.Api.Controllers
{

    [RoutePrefix("hops")]
    public class HopController : ApiController
    {
        private readonly IHopService _hopService;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public HopController(IHopService hopService)
        {
            _hopService = hopService;
        }

        // GET api/Hops
        [Route("")]
        public async Task<HopCompleteDto> GetHops(string custom = "false")
        {
            var hops = await _hopService.GetHopsAsync(custom);
            var result = new HopCompleteDto() { Hops = hops.OrderBy(h => h.Name).ToList() };
            return result;
        }

      

        // GET api/Hops/5
        [Route("{id:int}")]
        [ResponseType(typeof(HopCompleteDto))]
        [HttpGet]
        public async Task<IHttpActionResult> GetHop(int id)
        {
            var hopDto = await _hopService.GetSingleAsync(id);
            if (hopDto == null)
            {
                return NotFound();
            }
            var result = new HopCompleteDto() { Hops = new List<HopDto>() };
            result.Hops.Add(hopDto);
            return Ok(result);
        }

        // PUT api/Hops/5
        [ClaimsAuthorize("Put","Hop")]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutHop(int id, HopDto hopDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hopDto.Id)
            {
                return BadRequest();
            }
            await _hopService.UpdateHopAsync(hopDto);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Hops
        [ClaimsAuthorize("Post","Hop")]
        [Route("")]
        [ResponseType(typeof(HopDto))]
        public async Task<IHttpActionResult> PostHop(HopDto hopDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _hopService.AddHopAsync(hopDto);
            return CreatedAtRoute("DefaultApi", new { controller = "hops", }, result);
        }

       

        // DELETE api/Hopd/5
        [ClaimsAuthorize("Delete","Hop")]
        [Route("{id:int}")]
        [ResponseType(typeof(HopDto))]
        public async Task<IHttpActionResult> DeleteHop(int id)
        {
            var hop = await _hopService.DeleteHopAsync(id);
            if (hop == null)
            {
                return NotFound();
            }
            return Ok(hop);
        }

        [Route("forms")]
        public async Task<IList<DTO>> GetHopForm()
        {
            return await _hopService.GetHopFromsAsync();
        }
        
        [ClaimsAuthorize("Reindex","Hop")]
        [Route("es")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateHopsElasticSearch()
        {
            await _hopService.ReIndexHopsElasticSearch();
            return Ok();
        }

        [HttpGet]
        [Route("")]
        public async Task<HopCompleteDto> GeHopsBySearch(string query, int from = 0, int size = 20)
        {
            var hops = await _hopService.SearchHop(query, from, size);
            return new HopCompleteDto{Hops = hops.ToList()};
        }
    }
}