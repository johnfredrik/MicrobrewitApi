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
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using AutoMapper;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("suppliers")]
    public class SuppliersController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private readonly ISupplierRepository supplierRepository = new SupplierRepository();

        // GET api/Supplier
        [Route("")]
        public SupplierCompleteDTO GetSuppliers()
        {
            var suppliers = supplierRepository.GetAll("Origin");
            var result = new SupplierCompleteDTO();
            result.Suppliers = suppliers;
            return result;
        }

        // GET api/Supplier/5
        [Route("{id:int}")]
        [ResponseType(typeof(Supplier))]
        public IHttpActionResult GetSupplier(int id)
        {
            var supplier = supplierRepository.GetSingle(s => s.Id == id,"Origin");
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
                supplierRepository.Update(supplier);
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
            supplierRepository.Add(suppliers);
           

            return CreatedAtRoute("DefaultApi", new {controller = "others"}, supplierPosts);
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