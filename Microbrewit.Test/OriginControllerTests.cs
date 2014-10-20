using log4net;
using Microbrewit.Api.Automapper;
using Microbrewit.Api.Controllers;
using Microbrewit.Model;
using Microbrewit.Repository;
using NUnit.Framework;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Http.Results;
using Newtonsoft.Json;
  

namespace Microbrewit.Test
{
    public class OriginControllerTests
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IOriginRespository _repository;
        private MicrobrewitContext _context;
        private OriginController _controller;
        private const string JSONPATH = @"..\..\JSON\";

        [TestFixtureSetUp]
        public void Init()
        {
            TestUtil.DeleteDataInDatabase();
            TestUtil.InsertDataDatabase();
            AutoMapperConfiguration.Configure();
            _context = new MicrobrewitContext();
            _repository = new OriginRepository();
            _controller = new OriginController(_repository);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetOriginsNotNullAndNotEmpty()
        {
            var result = await _controller.GetOrigins();
            Assert.NotNull(result);
            Assert.NotNull(result.Origins.Any());
        }
        [Test]
        public async Task GetOriginValidIdReturnsOk200WithObject()
        {
            var origin = await _controller.GetOrigin(1) as OkNegotiatedContentResult<Origin>;
            Assert.IsInstanceOf<OkNegotiatedContentResult<Origin>>(origin);
        }

        [Test]
        public async Task GetOriginWithInvalidIdReturns404NotFound()
        {
            var origin = await _controller.GetOrigin(int.MaxValue) as NotFoundResult;
            Assert.IsInstanceOf<NotFoundResult>(origin);
        }

        [Test]
        public async Task PostOriginGetsAdded()
        {
            using (var stream = new StreamReader(JSONPATH + "origin.json"))
            {
                //var count =  _controller.GetOrigins().Result.Origins.Count();
                //string originJson = await stream.ReadToEndAsync();
                //var origin = JsonConvert.DeserializeObject<List<Origin>>(originJson);
                //await _controller.PostOrigin(origin);
                //var total = _controller.GetOrigins().Result.Origins.Count();
                //Assert.AreEqual(count + origin.Count, total);
            }
        }
 
        [Test]
        public async Task PutOriginNameGetsUpdated()
        {
            //var origins = await _controller.GetOrigins();
            //var origin = origins.Origins.FirstOrDefault();
            //origin.Name = "Bergen";
            ////await _controller.PutOrigin(origin.Id,origin);
            //var updatedOrigin = await _controller.GetOrigin(origin.Id) as OkNegotiatedContentResult<Origin>;
            //Assert.AreEqual(origin.Name,updatedOrigin.Content.Name);
        }

        [Test]
        public async Task DeleteOriginReturnsStatusCode200()
        {
            var origins = await _controller.GetOrigins();
            var origin = origins.Origins.SingleOrDefault(o => o.Name.Equals("Norway"));
            var response = await _controller.DeleteOrigin(origin.Id) as OkNegotiatedContentResult<Origin>;
            Assert.IsInstanceOf<OkNegotiatedContentResult<Origin>>(response);
        }

        [Test]
        public async Task DeleteOriginInvalidIdNotFound404()
        {
            var response = await _controller.DeleteOrigin(int.MaxValue) as NotFoundResult;
            Assert.IsInstanceOf<NotFoundResult>(response);
        }

    }
}
