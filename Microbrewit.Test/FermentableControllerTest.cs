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
    public class FermentableControllerTest
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IFermentableRepository _repository;
        private IFermentableService _fermentableService;
        private IFermentableElasticsearch _fermentableElasticsearch;
        private MicrobrewitContext _context;
        private FermentableController _controller;
        private const string JSONPATH = @"..\..\JSON\";

        [TestFixtureSetUp]
        public void Init()
        {
            TestUtil.DeleteDataInDatabase();
            TestUtil.InsertDataDatabase();
            AutoMapperConfiguration.Configure();
            _fermentableElasticsearch = new FermentableElasticsearch();
            _context = new MicrobrewitContext();
            _repository = new FermentableRepository();
            _fermentableService = new FermentableService(_repository,_fermentableElasticsearch);
            _controller = new FermentableController(_fermentableService);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetFermentablesNotNullAndNotEmpty()
        {
            var fermentables = await _controller.GetFermentables();
            Assert.NotNull(fermentables);
            Assert.True(fermentables.Fermentables.Any());
        }

        [Test]
        public async Task GetFermentableReturnsOK200WithObject()
        {
            var fermentable = await _controller.GetFermentable(1) as OkNegotiatedContentResult<FermentablesCompleteDto>;
            Assert.IsInstanceOf<OkNegotiatedContentResult<FermentablesCompleteDto>>(fermentable);
        }

        [Test]
        public async Task GetFermentableWithInvalidIdReturn404NotFound()
        {
            var fermentable = await _controller.GetFermentable(int.MaxValue) as NotFoundResult;
            Assert.IsInstanceOf<NotFoundResult>(fermentable);
        }

        [Test]
        public async Task PostFermentableReturnsCreated201WithObject()
        {
            //var fermentable = new FermentableDto() 
            //{
            //    Name = "Aromatic",
            //    PPG = 38,
            //    Lovibond = 33,
            //    Type =  "Grain",
            //};
            //var fermentables = new List<FermentableDto> {fermentable};
            //var response = await _controller.PostFermentable(fermentable) as CreatedAtRouteNegotiatedContentResult<IList<FermentableDto>>;
            //Assert.IsInstanceOf<CreatedAtRouteNegotiatedContentResult<IList<FermentableDto>>>(response);
        }

        [Test]
        public async Task PostFermentableGetsAdded()
        {
            //var fermentable = new FermentableDto()
            //{
            //    Name = "Super Malt",
            //    PPG = 38,
            //    Lovibond = 33,
            //    Type = "Grain",
            //};
            //var fermentables = new List<FermentableDto>();
            //fermentables.Add(fermentable);
            //var response = await _controller.PostFermentable(fermentables) as CreatedAtRouteNegotiatedContentResult<IList<FermentableDto>>;
            //var allFermentables = await _controller.GetFermentables();
            //var exists = allFermentables.Fermentables.Any(f => f.Name.Equals("Super Malt"));
            //Assert.True(exists);
        }

        [Test]
        public async Task PutFermentablePPGGetsUpdated()
        {
            var response = await _controller.GetFermentable(1) as OkNegotiatedContentResult<FermentablesCompleteDto>;
            var fermentable = response.Content.Fermentables.FirstOrDefault();
            fermentable.PPG = 99;
            await _controller.PutFermentable(fermentable.Id, fermentable);
            var updatedResponse = await _controller.GetFermentable(1) as OkNegotiatedContentResult<FermentablesCompleteDto>;
            Assert.AreEqual(fermentable.PPG, updatedResponse.Content.Fermentables.FirstOrDefault().PPG);
        }

        [Test]
        public async Task PutFermentableNameAndLoviBondGetsUpdated()
        {
            var response = await _controller.GetFermentable(3) as OkNegotiatedContentResult<FermentablesCompleteDto>;
            var fermentable = response.Content.Fermentables.FirstOrDefault();
            fermentable.Lovibond = 100;
            fermentable.Name = "Malllty";
            await _controller.PutFermentable(fermentable.Id, fermentable);
            var updatedResponse = await _controller.GetFermentable(fermentable.Id) as OkNegotiatedContentResult<FermentablesCompleteDto>;
            Assert.AreEqual(fermentable.PPG, updatedResponse.Content.Fermentables.FirstOrDefault().PPG);
        }

        [Test]
        public async Task DeleteFermentableReturns200OKWithObject()
        {
            var response = await _controller.DeleteFermentable(2) as OkNegotiatedContentResult<FermentablesCompleteDto>;
            Assert.IsInstanceOf<OkNegotiatedContentResult<FermentablesCompleteDto>>(response);
            Assert.True(response.Content.Fermentables.Any());
        }

        [Test]
        public async Task DeleteFermentableInvalidIdReturns404NotFound()
        {
            var response = await _controller.DeleteFermentable(int.MaxValue) as NotFoundResult;
            Assert.IsInstanceOf<NotFoundResult>(response);
        }

        [Test]
        public async Task DeleteFermentableReturn404NotFoundOnGetFermentable()
        {
            await _controller.DeleteFermentable(4);
            var response = await _controller.GetFermentable(4) as NotFoundResult;
            Assert.IsInstanceOf<NotFoundResult>(response);
        }
    }
}
