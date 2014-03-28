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
using Microbrewit.Repository;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("api/suppliers")]
    public class SuppliersController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private readonly ISupplierRepository repository = new SupplierRepository();

        // GET api/Supplier
        [Route("")]
        public SupplierCompleteDTO GetSuppliers()
        {
            var suppliers = repository.GetAll("Origin");
            var result = new SupplierCompleteDTO();
            result.Suppliers = suppliers;
            return result;
        }

        // GET api/Supplier/5
        [Route("{id:int}")]
        [ResponseType(typeof(Supplier))]
        public IHttpActionResult GetSupplier(int id)
        {
            var supplier = repository.GetSingle(s => s.Id == id,"Origin");
            if (supplier == null)
            {
                return NotFound();
            }
            var result = new SupplierCompleteDTO() { Suppliers = new List<Supplier>() };
            result.Suppliers.Add(supplier);
            return Ok(result);
        }

        // PUT api/Supplier/5
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutSupplier(int id, Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != supplier.Id)
            {
                return BadRequest();
            }

            db.Entry(supplier).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
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

        // POST api/Supplier
        [Route("")]
        [ResponseType(typeof(Supplier))]
        public async Task<IHttpActionResult> PostSupplier(Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (supplier.OriginId > 0)
            {
                supplier.Origin = null;
            }
            db.Suppliers.Add(supplier);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = supplier.Id }, supplier);
        }

        // DELETE api/Supplier/5
        [Route("{id}")]
        [ResponseType(typeof(Supplier))]
        public async Task<IHttpActionResult> DeleteSupplier(int id)
        {
            Supplier supplier = await db.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            db.Suppliers.Remove(supplier);
            await db.SaveChangesAsync();

            return Ok(supplier);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SupplierExists(int id)
        {
            return db.Suppliers.Count(e => e.Id == id) > 0;
        }
    }
}