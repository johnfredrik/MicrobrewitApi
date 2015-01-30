using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using Newtonsoft.Json;
using NUnit.Framework;

namespace Microbrewit.Test
{
    [TestFixture]
    public class OtherControllerTest
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IOtherRepository _repository;
        private MicrobrewitContext _context;
        private IOtherService _service;
        private IOtherElasticsearch _otherElasticsearch;
        private OtherController _controller;
        private const string JSONPATH = @"..\..\JSON\";

        [TestFixtureSetUp]
        public void Init()
        {
            TestUtil.DeleteDataInDatabase();
            TestUtil.InsertDataDatabase();
            AutoMapperConfiguration.Configure();
            _repository = new OtherRepository();
            _context = new MicrobrewitContext();
            _otherElasticsearch = new OtherElasticsearch();
            _service = new OtherService(_otherElasticsearch,_repository);
            _controller = new OtherController(_service);


        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
       

        [Test]
        public async Task GetAllOthersReturnsNotNullAndNotEmpty()
        {
            _repository = new OtherRepository();
            var controller = new OtherController(_service);
            var others = await controller.GetOthers();
            Assert.NotNull(others);
            Assert.True(others.Others.Any());
        }

        [Test]
        public async Task GetOtherReturn200OKWithObject()
        {
            var response = await _controller.GetOther(1) as OkNegotiatedContentResult<OtherCompleteDto>;
            Assert.IsInstanceOf<OkNegotiatedContentResult<OtherCompleteDto>>(response);
            Assert.True(response.Content.Others.Any());
        }

        [Test]
        public async Task PostOtherToDatabaseGetsAdded()
        {
                //var others = new List<OtherDto>
                //{
                //    new OtherDto{ Name = "Sun Flower", Type = "Flower"}
                //};

                //await _controller.PostOther(others);
                //var result = await _controller.GetOthers();
                //Assert.True(result.Others.Any(o => o.Name.Equals(others[0].Name)));
        }

        [Test]
        public async Task PostOtherMultipleInserts()
        {
            using (var file = new StreamReader(JSONPATH + "other.json"))
            {
                //string jsonString = file.ReadToEnd();
                //var others = JsonConvert.DeserializeObject<List<OtherDto>>(jsonString);
                //var count = _controller.GetOthers().Result.Others.Count();
                //for (int i = 0; i < 100; i++)
                //{
                //    await _controller.PostOther(others);
                //}
                //var result = await _controller.GetOthers();
                //Assert.AreEqual(count + (others.Count * 100), result.Others.Count());
            }
        }

        [Test]
        public async Task PutOtherReturnStatusCode204NoContent()
        {
            var respone = await _controller.GetOther(1) as OkNegotiatedContentResult<OtherCompleteDto>;
            var other = respone.Content.Others.FirstOrDefault();
            other.Name = "Black";
            var statusCode = await _controller.PutOther(other.Id, other) as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.NoContent, statusCode.StatusCode);
        }


        [Test]
        public async Task PutOtherNameGetsChanged()
        {
            var respone = await _controller.GetOther(1) as OkNegotiatedContentResult<OtherCompleteDto>;
            var other = respone.Content.Others.FirstOrDefault();
            Log.DebugFormat("other.Name= {0}", other.Name);
            other.Name = "YELLOW";
            await _controller.PutOther(other.Id, other);
            var updatedOther = await _controller.GetOther(other.Id) as OkNegotiatedContentResult<OtherCompleteDto>;
            Assert.AreEqual(other.Name, updatedOther.Content.Others.FirstOrDefault().Name);
        }


        [Test]
        public async Task DeleteOtherReturns200OK()
        {
            var response = await _controller.DeleteOther(4) as OkNegotiatedContentResult<OtherCompleteDto>;
            Assert.IsInstanceOf<OkNegotiatedContentResult<OtherCompleteDto>>(response);
        }

        [Test]
        public async Task DeleteOtherGetOther404NotFound()
        {
            await _controller.DeleteOther(5);
            var response = await _controller.GetOther(5) as NotFoundResult;
            Assert.IsInstanceOf<NotFoundResult>(response);
        }


    }
}
