using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
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
using Newtonsoft.Json;
using NUnit.Framework;

namespace Microbrewit.Test
{
    [TestFixture]
    public class HopControllerTests
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IHopRepository _repository;
        private MicrobrewitContext _context;
        private HopController _controller;
        private IHopService _service;
        private IHopElasticsearch _elasticsearch;
        private const string JSONPATH = @"..\..\JSON\";

        [TestFixtureSetUp]
        public void Init()
        {
            TestUtil.FlushRedisStore();
            TestUtil.DeleteDataInDatabase();
            TestUtil.InsertDataDatabase();
            AutoMapperConfiguration.Configure();
            _context = new MicrobrewitContext();
            _repository = new HopRepository();
            _elasticsearch = new HopElasticsearch();
            _service = new HopService(_repository,_elasticsearch);
            _controller = new HopController(_service);

        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAllHopsNotNull()
        {
            var hops = await _controller.GetHops() as HopCompleteDto;
            Assert.NotNull(hops);
        }

        [Test]
        public async Task GetHopWithValidIdStatusCode200OKWithObject()
        {
            var first = _context.Hops.FirstOrDefault();
            var hop = await _controller.GetHop(first.Id) as OkNegotiatedContentResult<HopCompleteDto>;
            Assert.IsInstanceOf<OkNegotiatedContentResult<HopCompleteDto>>(hop);
        }

        [Test]
        public async Task GetHopWithInvalidIdShouldReturnStatusCode404NotFound()
        {
            var hop = await _controller.GetHop(int.MaxValue);
            Assert.IsInstanceOf<NotFoundResult>(hop);
        }

        [Test]
        public async Task PostHopGetAdded()
        {
            //using (var stream = new StreamReader(JSONPATH + "hop.json"))
            //{
            //    var count = _controller.GetHops().Result.Hops.Count;
            //    string hopJson = await stream.ReadToEndAsync();
            //    var hop = JsonConvert.DeserializeObject<List<HopDto>>(hopJson);
            //    await _controller.PostHop(hop);
            //    var result = await _controller.GetHops();
            //    Assert.AreEqual(count + hop.Count, result.Hops.Count);
            //}
        }

        [Test]
        public async Task PutHopNameAndBetaHighGetsUpdated()
        {
            var firstResult = await _controller.GetHops();
            var first = firstResult.Hops.FirstOrDefault();
            var hopCompleteDto = await _controller.GetHop(first.Id) as OkNegotiatedContentResult<HopCompleteDto>;
            var hop = hopCompleteDto.Content.Hops[0];
            hop.Notes = "Something hoppy";
            hop.BetaHigh = 99;
            var response = await _controller.PutHop(hop.Id, hop) as HttpResponseException;
            
            var result = await _controller.GetHop(first.Id) as OkNegotiatedContentResult<HopCompleteDto>;
            var updatedHop = result.Content.Hops[0];
            Assert.AreEqual(hop.Notes, updatedHop.Notes);
            Assert.AreEqual(hop.BetaHigh, updatedHop.BetaHigh);
        }

        
    }
}
