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
using Microbrewit.Model.DTOs;
using AutoMapper;
using Microbrewit.Repository;
using System.Configuration;
using StackExchange.Redis;
using Newtonsoft.Json;
using Microbrewit.Api.Redis;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("beerstyles")]
    public class BeerStyleController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
       
        private IBeerStyleRepository _beerStyleRepository;

        public BeerStyleController(IBeerStyleRepository beerStyleRepository)
        {
            this._beerStyleRepository = beerStyleRepository;
        }

        /// <summary>
        /// Get all beerstyles.
        /// <response code="200">OK</response>
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public async Task<BeerStyleCompleteDto> GetBeerStyles()
        {
            var response = new BeerStyleCompleteDto() { BeerStyles = new List<BeerStyleDto>() };

            var beerStylesDto = await BeerStylesRedis.GetBeerStyles();
            if (beerStylesDto.Count < 1)
            {
                    var beerStyles = await _beerStyleRepository.GetAllAsync("SubStyles", "SuperStyle");
                    beerStylesDto = Mapper.Map<IList<BeerStyle>, IList<BeerStyleDto>>(beerStyles);

            }
            response.BeerStyles = beerStylesDto;
            return response;
        }


        /// <summary>
        /// Get beerstyle by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Beerstyle id</param>
        /// <returns></returns>
        [Route("{id:int}")]
        [ResponseType(typeof(BeerStyleCompleteDto))]
        public async Task<IHttpActionResult> GetBeerStyle(int id)
        {
            var beerStyleDto = await BeerStylesRedis.GetBeerStyle(id);
            if (beerStyleDto == null)
            {
                var beerStyle = await _beerStyleRepository.GetSingleAsync(b => b.Id == id, "SubStyles", "SuperStyle");
                beerStyleDto = Mapper.Map<BeerStyle, BeerStyleDto>(beerStyle);
            }
            if (beerStyleDto == null)
            {
                return NotFound();
            }
            var response = new BeerStyleCompleteDto() { BeerStyles = new List<BeerStyleDto>() };
            response.BeerStyles.Add(beerStyleDto);

            return Ok(response);
        }

        /// <summary>
        /// Updates a beerstyle.
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Beerstyle id</param>
        /// <param name="beerstyleDto">BeerStyle object</param>
        /// <returns></returns>
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutBeerStyle(int id, BeerStyleDto beerstyleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var beerstyle = Mapper.Map<BeerStyleDto, BeerStyle>(beerstyleDto);
            if (id != beerstyle.Id)
            {
                return BadRequest();
            }
            
            await _beerStyleRepository.UpdateAsync(beerstyle);
            var bs = await _beerStyleRepository.GetAllAsync("SubStyles", "SuperStyle");
            await BeerStylesRedis.UpdateRedisStore(bs);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add beerstyle.
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <param name="beerStylesDto">Beerstyle object.</param>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(BeerStyleCompleteDto))]
        public async Task<IHttpActionResult> PostBeerStyle(IList<BeerStyleDto> beerStylesDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var beerStyles = Mapper.Map<IList<BeerStyleDto>, BeerStyle[]>(beerStylesDto);
            await _beerStyleRepository.AddAsync(beerStyles);
            
            var bs = await _beerStyleRepository.GetAllAsync("SubStyles", "SuperStyle");
            await BeerStylesRedis.UpdateRedisStore(bs);
            var response = new BeerStyleCompleteDto(){BeerStyles = new List<BeerStyleDto>()};
            response.BeerStyles = beerStylesDto;
            return CreatedAtRoute("DefaultApi", new { controller = "beerstyles" }, response);
        }

        /// <summary>
        /// Deletes beerstyle by id.
        /// </summary>
        /// <response code="404">Node Found</response>
        /// <param name="id">Beerstyle id.</param>
        /// <returns></returns>
        [ResponseType(typeof(BeerStyleCompleteDto))]
        public async Task<IHttpActionResult> DeleteBeerStyle(int id)
        {
            var beerstyle = await _beerStyleRepository.GetSingleAsync(bs => bs.Id == id);
            if (beerstyle == null)
            {
                return NotFound();
            }

            await _beerStyleRepository.RemoveAsync(beerstyle);
            var beerStyleDto = Mapper.Map<BeerStyle, BeerStyleDto>(beerstyle);
            var response = new BeerStyleCompleteDto(){BeerStyles = new List<BeerStyleDto>()};
            response.BeerStyles.Add(beerStyleDto);
            return Ok(response);
        }

        [HttpGet]
        [Route("redis")]
        public async Task<IHttpActionResult> UpdateRedisBeerStyle()
        {
            var bs = await _beerStyleRepository.GetAllAsync("SubStyles", "SuperStyle");
            await BeerStylesRedis.UpdateRedisStore(bs);

            return Ok();
        }
    }
}