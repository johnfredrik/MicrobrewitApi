using log4net;
using Microbrewit.Api.Automapper;
using Microbrewit.Api.Controllers;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Newtonsoft.Json;

namespace Microbrewit.Test
{
    public class BeerStyleControllerTest
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IBeerStyleRepository _repository;
        private MicrobrewitContext _context;
        private BeerStyleController _controller;
        private const string JSONPATH = @"..\..\JSON\";

        [TestFixtureSetUp]
        public void Init()
        {
            TestUtil.DeleteDataInDatabase();
            TestUtil.InsertDataDatabase();
            AutoMapperConfiguration.Configure();
            _context = new MicrobrewitContext();
            _repository = new BeerStyleRepository();
            _controller = new BeerStyleController(_repository);
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
            var beerStyles = new List<BeerStyleDto>();
            beerStyles.Add(beerStyle);
            var response = await _controller.PostBeerStyle(beerStyles) as CreatedAtRouteNegotiatedContentResult<BeerStyleCompleteDto>;
            Assert.IsInstanceOf<CreatedAtRouteNegotiatedContentResult<BeerStyleCompleteDto>>(response);
            Assert.True(response.Content.BeerStyles.Any());
        }
    }
}
