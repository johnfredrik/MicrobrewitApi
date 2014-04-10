﻿using System;
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
using AutoMapper;
using log4net;
using Microbrewit.Model.DTOs;
using ServiceStack.Redis;
using Newtonsoft.Json;
using System.Configuration;

namespace Microbrewit.Api.Controllers
{
   // [TokenValidationAttribute]
    [RoutePrefix("fermentables")]
    public class FermentableController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private IFermentableRepository fermentableRepository = new FermentableRepository();
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      
        // GET api/Fermentable 
        [Route("")]
        public FermentablesCompleteDto GetFermentables()
        {
            var fermentables = fermentableRepository.GetAll("Supplier");
            var fermDto = Mapper.Map<IList<Fermentable>,IList<FermentableDto>>(fermentables);
            var result = new FermentablesCompleteDto();
            result.Fermentables = fermDto;
            return result;
        }

        // GET api/Fermentable/5
        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(FermentablesCompleteDto))]
        public IHttpActionResult GetFermentable(int id)
        {
            var fermentable = Mapper.Map<Fermentable, FermentableDto>(fermentableRepository.GetSingle(f => f.Id == id, "Supplier")); 
            if (fermentable == null)
            {
                return NotFound();
            }
            var result = new FermentablesCompleteDto() { Fermentables = new List<FermentableDto>() };
            result.Fermentables.Add(fermentable);
            return Ok(result);
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
        [Route("")]
        [ResponseType(typeof(IList<FermentablePostDto>))]
        public IHttpActionResult PostFermentable(IList<FermentablePostDto> fermentablePostDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fermentablePost = Mapper.Map<IList<FermentablePostDto>, Fermentable[]>(fermentablePostDtos);
            fermentableRepository.Add(fermentablePost);
            var fermentables = Mapper.Map<IList<Fermentable>,IList<FermentableDto>>(fermentableRepository.GetAll("Supplier.Origin"));
           
            using (var redisClient = new RedisClient(redisStore))
            {
                foreach (var fermentable in fermentables)
                {
                    redisClient.SetEntryInHash("fermentables", fermentable.Id.ToString(), JsonConvert.SerializeObject(fermentable));
                }
            }
            return CreatedAtRoute("DefaultApi", new { controller = "fermetables",}, fermentablePostDtos);
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