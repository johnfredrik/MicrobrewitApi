using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http.Results;
using log4net;
using Microbrewit.Api.Controllers;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Automapper;
using Microbrewit.Service.Component;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;
using Microbrewit.Service.Interface;
using NUnit.Framework;

namespace Microbrewit.Test
{
    public class SupplierControllerTests
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ISupplierRepository _repository;
        private MicrobrewitContext _context;
        private SupplierController _controller;
        private ISupplierElasticsearch _supplierElasticsearch;
        private ISupplierService _supplierService;
        private const string JSONPATH = @"..\..\JSON\";

        [TestFixtureSetUp]
        public void Init()
        {
            TestUtil.DeleteDataInDatabase();
            TestUtil.InsertDataDatabase();
            AutoMapperConfiguration.Configure();
            _context = new MicrobrewitContext();
            _repository = new SupplierRepository();
            _supplierElasticsearch = new SupplierElasticsearch();
            _supplierService = new SupplierService(_repository,_supplierElasticsearch);
            _controller = new SupplierController(_supplierService);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetSuppliersNotNullAndNotEmpty()
        {
            var response = await _controller.GetSuppliers();
            Assert.NotNull(response);
            Assert.True(response.Suppliers.Any());
        }

        [Test]
        public async Task GetSupplierReturns200OKWithObject()
        {
            var response = await _controller.GetSupplier(1) as OkNegotiatedContentResult<SupplierCompleteDto>;
            Assert.IsInstanceOf<OkNegotiatedContentResult<SupplierCompleteDto>>(response);
        }

        [Test]
        public async Task PostSupplierReturns201CreatedWithObject()
        {
            //var supplierDto = new SupplierDto() 
            //{
            //    Name = "Awsome Inc",
            //    Origin = new DTO() { Id = 1, Name = "United States" }
            //};
            //var supplierDtos = new List<SupplierDto>();
            //supplierDtos.Add(supplierDto);
            //var response = await _controller.PostSupplier(supplierDtos) as CreatedAtRouteNegotiatedContentResult<SupplierCompleteDto>;
            //Assert.IsInstanceOf<CreatedAtRouteNegotiatedContentResult<SupplierCompleteDto>>(response);
        }

        [Test]
        public async Task PostSupplierOrginIdIsSameAsSentIn()
        {
            //var supplierDto = new SupplierDto()
            //{
            //    Name = "Uber Inc",
            //    Origin = new DTO() { Id = 1, Name = "United States" }
            //};
            //var supplierDtos = new List<SupplierDto>();
            //supplierDtos.Add(supplierDto);
            //await _controller.PostSupplier(supplierDtos);
            //var response = await _controller.GetSuppliers();
            //var uberInc = response.Suppliers.SingleOrDefault(s => s.Name.Equals("Uber Inc"));
            //Assert.AreEqual(supplierDto.Origin.Id, uberInc.Origin.Id);
        }

        [Test]
        public async Task PutSupplierNameGetsUpdated()
        {
            var response = await _controller.GetSupplier(1) as OkNegotiatedContentResult<SupplierCompleteDto>;
            var supplier = response.Content.Suppliers.FirstOrDefault();
            supplier.Name = "New Better Corp";
            await _controller.PutSupplier(supplier.Id, supplier);
            var updatedSupplier = await _controller.GetSupplier(supplier.Id) as OkNegotiatedContentResult<SupplierCompleteDto>;
            Assert.AreEqual(supplier.Name, updatedSupplier.Content.Suppliers.FirstOrDefault().Name);
        }

        
        [Test]
        public async Task PutSupplierOriginGetsUpdated()
        {
            var response = await _controller.GetSupplier(1) as OkNegotiatedContentResult<SupplierCompleteDto>;
            var supplier = response.Content.Suppliers.FirstOrDefault();
            supplier.Origin = new DTO(){Name = "United Kingdom", Id = 2}; 
            await _controller.PutSupplier(supplier.Id, supplier);
            var updatedSupplier = await _controller.GetSupplier(supplier.Id) as OkNegotiatedContentResult<SupplierCompleteDto>;
            Assert.AreEqual(supplier.Origin.Id, updatedSupplier.Content.Suppliers.FirstOrDefault().Origin.Id);
        }

        [Test]
        public async Task PutSupplierInvalidIdReturns400BadRequest()
        {
            var response = await _controller.GetSupplier(1) as OkNegotiatedContentResult<SupplierCompleteDto>;
            var supplier = response.Content.Suppliers.FirstOrDefault();
            supplier.Origin = new DTO() { Name = "United Kingdom", Id = 2 };
            var putResponse = await _controller.PutSupplier(int.MaxValue, supplier) as BadRequestResult;
            Assert.IsInstanceOf<BadRequestResult>(putResponse);
         
        }

        [Test]
        public async Task DeleteSupplierReturns200OKWithObject()
        {
            var response = await _controller.DeleteSupplier(7) as OkNegotiatedContentResult<SupplierCompleteDto>;
            Assert.IsInstanceOf<OkNegotiatedContentResult<SupplierCompleteDto>>(response);
            Assert.True(response.Content.Suppliers.Any());
        }

        [Test]
        public async Task DeleteSupplierGetSupplier404NotFound()
        {
            await _controller.DeleteSupplier(8);
            var response = await _controller.GetSupplier(8) as NotFoundResult;
            Assert.IsInstanceOf<NotFoundResult>(response);
        }

        [Test]
        public async Task DeleteSupplierSomeError()
        {
            var response = await _controller.DeleteSupplier(1) as BadRequestErrorMessageResult;
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(response);
        }
    }
}
