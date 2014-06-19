using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NUnit.Framework;
using Microbrewit.Repository;
using Microbrewit.Model;
using Microbrewit.Api.Automapper;
using Microbrewit.Api.Controllers;
using log4net;
using Microbrewit.Model.DTOs;
using System.Web.Http.Results;
using System.IO;
using Newtonsoft.Json;
using AutoMapper;

namespace Microbrewit.Test
{
    [TestFixture]
    public class HopControllerTests
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IHopRepository _repository;
        private MicrobrewitContext _context;
        private HopController _controller;
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
            _controller = new HopController(_repository);

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
            using (var stream = new StreamReader(JSONPATH + "hop.json"))
            {
                var count = _controller.GetHops().Result.Hops.Count;
                string hopJson = await stream.ReadToEndAsync();
                var hop = JsonConvert.DeserializeObject<List<HopDto>>(hopJson);
                await _controller.PostHop(hop);
                var result = await _controller.GetHops();
                Assert.AreEqual(count + hop.Count, result.Hops.Count);
            }
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
            var response = await _controller.PutHop(hop.Id, hop) as System.Web.Http.HttpResponseException;
            
            var hopTemp = _context.Hops.SingleOrDefault(h => h.Id == first.Id);
            var result = await _controller.GetHop(first.Id) as OkNegotiatedContentResult<HopCompleteDto>;
            var updatedHop = result.Content.Hops[0];
            Assert.AreEqual(hop.Notes, hopTemp.Notes);
            Assert.AreEqual(hop.BetaHigh,hopTemp.BetaHigh);
        }

        
    }
}
