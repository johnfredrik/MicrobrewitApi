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
    [TestFixture]
    public class BeerStyleControllerTest
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IBeerStyleRepository _repository;
        private MicrobrewitContext _context;
        private IBeerStyleElasticsearch _beerStyleElasticsearch;
        private IBeerStyleService _beerStyleService;
        private BeerStyleController _controller;
        private const string JSONPATH = @"..\..\JSON\";

        [TestFixtureSetUp]
        public void Init()
        {
            TestUtil.DeleteDataInDatabase();
            TestUtil.InsertDataDatabase();
            AutoMapperConfiguration.Configure();
            _context = new MicrobrewitContext();
            _beerStyleElasticsearch = new BeerStyleElasticsearch();
            _repository = new BeerStyleRepository();
            _beerStyleService = new BeerStyleService(_beerStyleElasticsearch,_repository);
            _controller = new BeerStyleController(_beerStyleService);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetBeerStylesNotNullAndNotEmpty()
        {
            var beerstyles = await _controller.GetBeerStyles();
            Assert.NotNull(beerstyles);
            Assert.True(beerstyles.BeerStyles.Any());
        }


        [Test]
        public async Task GetBeerStyleWithValidIdReturn200OKWithObject()
        {
            var beerstyle = await _controller.GetBeerStyle(1) as OkNegotiatedContentResult<BeerStyleCompleteDto>;
            Assert.IsInstanceOf<OkNegotiatedContentResult<BeerStyleCompleteDto>>(beerstyle);
            Assert.True(beerstyle.Content.BeerStyles.Any());
        }

        [Test]
        public async Task GetBeerStyleWithInvalidIdReturn404NotFound()
        {
            var beerStyle = await _controller.GetBeerStyle(int.MaxValue) as NotFoundResult;
            Assert.IsInstanceOf<NotFoundResult>(beerStyle);
        }

        [Test]
        public async Task PostBeerStyleReturns201CreatedWithObject()
        {
            var beerStyle = new BeerStyleDto() { Name = "Lager" };
            var response = await _controller.PostBeerStyle(beerStyle) as CreatedAtRouteNegotiatedContentResult<BeerStyleCompleteDto>;
            Assert.IsInstanceOf<CreatedAtRouteNegotiatedContentResult<BeerStyleCompleteDto>>(response);
            Assert.True(response.Content.BeerStyles.Any());
        }

        [Test]
        public async Task PutBeerStyleValueGetsUpdated()
        {
            var beerStyles = await _controller.GetBeerStyles();
            var firstBeerStyle = beerStyles.BeerStyles.FirstOrDefault();
            firstBeerStyle.Name = "New Super Style";
            await _controller.PutBeerStyle(firstBeerStyle.Id, firstBeerStyle);
            var updatedBeerStyle = await _controller.GetBeerStyle(firstBeerStyle.Id) as OkNegotiatedContentResult<BeerStyleCompleteDto>;
            Assert.AreEqual(firstBeerStyle.Name, updatedBeerStyle.Content.BeerStyles[0].Name);
            
        }
    }
}
