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
using Microbrewit.Api;
using Microbrewit.Repository;
using AutoMapper;
using log4net;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Api.Controllers
{
   // [TokenValidationAttribute]
    [RoutePrefix("fermentables")]
    public class FermentableController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private IFermentableRepository fermentableRepository = new FermentableRepository();
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

        //[Route("grains")]
        //public FermentablesCompleteDto GetGrains()
        //{
        //    return fermentableRepository.GetGrains();
        //}

        //[Route("sugars")]
        //public IList<Sugar> GetSugars()
        //{
        //    return fermentableRepository.GetSugars();
        //}

        //[Route("dryextracts")]
        //public IList<DryExtract> GetDryExtracts()
        //{
        //    return fermentableRepository.GetDryExtracts();
        //}

        //[Route("liquidextracts")]
        //public IList<LiquidExtract> GetLiquidExtracts()
        //{
        //    return fermentableRepository.GetLiquidExtracts();
        //}


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
        [ResponseType(typeof(FermentablePostDto))]
        public IHttpActionResult PostFermentable(FermentablePostDto fermentablePostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fermentable = fermentableRepository.AddFermentable(fermentablePostDto);
            var result = Mapper.Map<Fermentable, FermentableDto>(fermentable); 
            return CreatedAtRoute("DefaultApi", new { controller = "fermetable", id = result.Id}, result);
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