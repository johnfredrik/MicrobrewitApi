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
using Microbrewit.Repository;
using AutoMapper;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("suppliers")]
    public class SupplierController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private readonly ISupplierRepository _supplierRepository;

        public SupplierController(ISupplierRepository supplierRepository)
        {
            this._supplierRepository = supplierRepository;
        }

        // GET api/Supplier
        [Route("")]
        public SupplierCompleteDTO GetSuppliers()
        {
            var suppliers = _supplierRepository.GetAll("Origin");
            var result = new SupplierCompleteDTO();
            result.Suppliers = suppliers;
            return result;
        }

        // GET api/Supplier/5
        [Route("{id:int}")]
        [ResponseType(typeof(Supplier))]
        public IHttpActionResult GetSupplier(int id)
        {
            var supplier = _supplierRepository.GetSingle(s => s.Id == id,"Origin");
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
        public IHttpActionResult PutSupplier(int id, Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != supplier.Id)
            {
                return BadRequest();
            }

            try
            {
                _supplierRepository.Update(supplier);
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
        [ResponseType(typeof(IList<SupplierDto>))]
        public IHttpActionResult PostSupplier(IList<SupplierDto> supplierPosts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var suppliers = Mapper.Map<IList<SupplierDto>, Supplier[]>(supplierPosts);
            _supplierRepository.Add(suppliers);
           

            return CreatedAtRoute("DefaultApi", new {controller = "others"}, supplierPosts);
        }

        // DELETE api/Supplier/5
        [Route("{id}")]
        [ResponseType(typeof(Supplier))]
        public IHttpActionResult DeleteSupplier(int id)
        {
            Supplier supplier = _supplierRepository.GetSingle(s => s.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }

            _supplierRepository.Remove(supplier);

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