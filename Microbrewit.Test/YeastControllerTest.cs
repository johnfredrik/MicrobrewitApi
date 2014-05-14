using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Microbrewit.Api.Automapper;
using Microbrewit.Model;
using Microbrewit.Repository;
using log4net;
using Microbrewit.Api.Controllers;
using System.Net.Http;
using System.IO;
using Microbrewit.Model.DTOs;
using System.Web.Http.Results;
using Newtonsoft.Json;
using System.Net;

namespace Microbrewit.Test
{
    [TestFixture]
    public class YeastControllerTest
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IYeastRepository _repository;
        private MicrobrewitContext _context;
        private YeastController _controller;
        private const string JSONPATH = @"..\..\JSON\";

        [TestFixtureSetUp]
        public void Init()
        {
            TestUtil.FlushRedisStore();
            TestUtil.DeleteDataInDatabase();
            TestUtil.InsertDataDatabase();
            AutoMapperConfiguration.Configure();
            _context = new MicrobrewitContext();
            _repository = new YeastRepository();
            _controller = new YeastController(_repository);

        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAllYeastsNotNull()
        {
            var yeasts = await _controller.GetYeasts() as YeastCompleteDto;
            Assert.NotNull(yeasts);
        }

        [Test]
        public async Task GetYeastWithValidIdNotNullOrEmpty()
        {
            var first = _context.Yeasts.FirstOrDefault();
            var yeast = await _controller.GetYeast(first.Id) as OkNegotiatedContentResult<YeastCompleteDto>;
            Assert.NotNull(yeast);
        }

        [Test]
        public async Task GetYeastWithInvalidIdShouldNotBeFound()
        {
            var yeast = await _controller.GetYeast(int.MaxValue);
            Assert.IsInstanceOf<NotFoundResult>(yeast);
        }

        [Test]
        public async Task PostYeastGetsAdded()
        {
            using (var stream = new StreamReader(JSONPATH + "yeast.json"))
            {
                var count =  _controller.GetYeasts().Result.Yeasts.Count;
                string yeastJson = await stream.ReadToEndAsync();
                var yeasts = JsonConvert.DeserializeObject<List<YeastDto>>(yeastJson);

                await _controller.PostYeast(yeasts);
                var result = await _controller.GetYeasts();
                Assert.AreEqual(count + yeasts.Count, result.Yeasts.Count);
            }
        }

        [Test]
        public async Task PutYeastGetsUpdated()
        {
            var first = _context.Yeasts.FirstOrDefault();
            var yeastCompleteDto = await _controller.GetYeast(first.Id) as OkNegotiatedContentResult<YeastCompleteDto>;
            var yeast = yeastCompleteDto.Content.Yeasts[0];
            yeast.Flocculation = "Medium";
            _controller.PutYeast(yeast.Id,yeast);
            var result = await _controller.GetYeast(first.Id) as OkNegotiatedContentResult<YeastCompleteDto>;
            var updatedYeast = result.Content.Yeasts[0];
            Assert.AreEqual(yeast.Flocculation,updatedYeast.Flocculation);
        }

        [Test]
        public async Task DeleteYeasteReturnsStatusCode200()
        {
            var first = _context.Yeasts.FirstOrDefault();
            var yeastCompleteDto = await _controller.GetYeast(first.Id) as OkNegotiatedContentResult<YeastCompleteDto>;
            var yeast = yeastCompleteDto.Content.Yeasts[0];
            var result = await _controller.DeleteYeast(yeast.Id) as OkNegotiatedContentResult<YeastDto>;
            Assert.IsInstanceOf<OkNegotiatedContentResult<YeastDto>>(result);
        }

        [Test]
        public async Task DeleteYeasteReturnsNotFound()
        {
            var first = _context.Yeasts.FirstOrDefault();
            var yeastCompleteDto = await _controller.GetYeast(first.Id) as OkNegotiatedContentResult<YeastCompleteDto>;
            var yeast = yeastCompleteDto.Content.Yeasts[0];
            await _controller.DeleteYeast(yeast.Id);
            var result = await _controller.GetYeast(yeast.Id);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }


    }
}
