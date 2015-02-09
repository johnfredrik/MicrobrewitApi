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
using Microbrewit.Service.Elasticsearch;
using Microbrewit.Service.Interface;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("suppliers")]
    public class SupplierController : ApiController
    {
        private ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        /// <summary>
        /// Gets all suppliers
        /// </summary>
        /// <response code="200">OK</response>
        /// <returns></returns>
        [Route("")]
        public async Task<SupplierCompleteDto> GetSuppliers(string custom = "false")
        {
            var suppliersDto = await _supplierService.GetAllAsync(custom);
            var result = new SupplierCompleteDto {Suppliers = suppliersDto.OrderBy(s => s.Name).ToList()};
            return result;
        }

        /// <summary>
        /// Get a supplier.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Supplier id</param>
        /// <returns></returns>
        [Route("{id:int}")]
        [ResponseType(typeof(SupplierCompleteDto))]
        public async Task<IHttpActionResult> GetSupplier(int id)
        {
            var supplierDto = await _supplierService.GetSingleAsync(id);
            if (supplierDto == null)
            {
                return NotFound();
            }
            var result = new SupplierCompleteDto() { Suppliers = new List<SupplierDto>() };
            result.Suppliers.Add(supplierDto);
            return Ok(result);
        }

        /// <summary>
        /// Updates a supplier.
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Supplier id</param>
        /// <param name="supplierDto">Supplier transfer object</param>
        /// <returns></returns>
        [ClaimsAuthorize("Put","Supplier")]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutSupplier(int id, SupplierDto supplierDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != supplierDto.Id)
            {
                return BadRequest();
            }
            await _supplierService.UpdateAsync(supplierDto);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds suppliers.
        /// </summary>
        /// <param name="supplierDtos">List of supplier tranfer objects</param>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <returns></returns>
        [ClaimsAuthorize("Post", "Supplier")]
        [Route("")]
        [ResponseType(typeof(SupplierDto))]
        public async Task<IHttpActionResult> PostSupplier(SupplierDto supplierDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _supplierService.AddAsync(supplierDto);
            return CreatedAtRoute("DefaultApi", new {controller = "others"}, result);
        }

        /// <summary>
        /// Delets supplier.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <param name="id"></param>
        /// <returns></returns>
        [ClaimsAuthorize("Delete", "Supplier")]
        [Route("{id}")]
        [ResponseType(typeof(SupplierDto))]
        public async Task<IHttpActionResult> DeleteSupplier(int id)
        {
            var supplier = await _supplierService.DeleteAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return Ok(supplier);
        }

        [ClaimsAuthorize("Reindex", "Supplier")]    
        [Route("es")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateSupplierElasticSearch()
        {
            await _supplierService.ReIndexElasticSearch();
            return Ok();
        }

        [HttpGet]
        [Route("")]
        public async Task<SupplierCompleteDto> GetSuppliersBySearch(string query, int from = 0, int size = 20)
        {
            var supplierDto = await _supplierService.SearchAsync(query,from,size);
            var result = new SupplierCompleteDto {Suppliers = supplierDto.ToList()};
            return result;
        }
    }
}