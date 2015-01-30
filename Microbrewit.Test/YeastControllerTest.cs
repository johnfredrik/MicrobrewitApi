using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Elasticsearch.Net;
using Elasticsearch.Net.Connection;
using log4net;
using Microbrewit.Api.Controllers;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch;
using Microbrewit.Service.Automapper;
using Microbrewit.Service.Component;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;
using Microbrewit.Service.Interface;
using Nest;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Microbrewit.Test
{
    [TestFixture]
    public class YeastControllerTest
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IYeastRepository _repository;
        private MicrobrewitContext _context;
        private YeastController _controller;
        private IYeastService _yeastService;
        private IYeastElasticsearch _yeastElasticsearch;
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
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
            _yeastElasticsearch = new YeastElasticsearch();
            _yeastService = new YeastService(_repository,_yeastElasticsearch);
            _controller = new YeastController(_yeastService);
            _node = new Uri("http://localhost:9200");
            _settings = new ConnectionSettings(_node, defaultIndex: "mb");
            _client = new ElasticClient(_settings);
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
                //var count =  _controller.GetYeasts().Result.Yeasts.Count;
                //string yeastJson = await stream.ReadToEndAsync();
                //var yeasts = JsonConvert.DeserializeObject<List<YeastDto>>(yeastJson);

                //await _controller.PostYeast(yeasts);
                //var result = await _controller.GetYeasts();
                //Assert.AreEqual(count + yeasts.Count, result.Yeasts.Count);
            }
        }

        [Test]
        public async Task PutYeastGetsUpdated()
        {
            var first = _context.Yeasts.FirstOrDefault();
            var yeastCompleteDto = await _controller.GetYeast(first.Id) as OkNegotiatedContentResult<YeastCompleteDto>;
            var yeast = yeastCompleteDto.Content.Yeasts[0];
            yeast.Flocculation = "Medium";
            await _controller.PutYeast(yeast.Id,yeast);
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

        [Test]
        public async Task UpdateYeasts_Into_ElasticSearch()
        {
            //var yeasts = await _controller.GetYeasts();
            //await _elasticSearch.UpdateYeastsElasticSearch(yeasts.Yeasts);

            //var searchResults = _client.Search<YeastDto>(s => s
            //                                    .From(0)
            //                                    .Size(10)
            //                                    .Query(q => q
            //                                        .Term(y => y.Name, "safale")));

            //var result = searchResults.Documents;
            //Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task UpdateYeasts_Into_ElasticSearch_Value_Gets_Updated()
        {
            var yeasts = await _controller.GetYeasts();
            var updatedYeast = yeasts.Yeasts.FirstOrDefault();
            updatedYeast.Name = "NEWNAME";
            await _controller.PutYeast(updatedYeast.Id, updatedYeast);



            var searchResults = _client.Search<YeastDto>(s => s
                                                .From(0)
                                                .Size(10)
                                                .Query(q => q
                                                    .Term(y => y.Name, "new")));

           
            var result = searchResults.Documents;
            Assert.True(result.Any());
        }

        [Test]
        public async Task SearchAllElasticSearch()
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionConfiguration(node);
            var client = new ElasticsearchClient(settings);

            var query = "{\"query\" : { \"term\": { \"name\" : \"safale\"}}}";
            var res = client.Search<string>("mb", query);
            dynamic json = JsonConvert.DeserializeObject(res.Response);
            var result = json.hits.hits; 
           
         }

           

        

    }
}
