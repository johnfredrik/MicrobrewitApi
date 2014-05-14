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
    public class BreweryControllerTest
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IBreweryRepository _repository;
        private MicrobrewitContext _context;
        private BreweryController _controller;
        private const string JSONPATH = @"..\..\JSON\";

        [TestFixtureSetUp]
        public void Init()
        {
            TestUtil.DeleteDataInDatabase();
            TestUtil.InsertDataDatabase();
            AutoMapperConfiguration.Configure();
            _context = new MicrobrewitContext();
            _repository = new BreweryRepository();
            _controller = new BreweryController(_repository);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetBreweriesNotNullAndNotEmpty()
        {
            var result = await _controller.GetBreweries();
            Assert.NotNull(result);
            Assert.True(result.Breweries.Any());
        }

        [Test]
        public async Task GetBreweryWithValidIdReturnsOk200WithObject()
        {
            var brewery = await _controller.GetBrewery(1) as OkNegotiatedContentResult<BreweryCompleteDto>;
            Assert.IsInstanceOf<OkNegotiatedContentResult<BreweryCompleteDto>>(brewery);
            Assert.True(brewery.Content.Breweries.Any());
        }

        [Test]
        public async Task GetBreweryWithInvalidIdReturns404NotFound()
        {
            var brewery = await _controller.GetBrewery(int.MaxValue) as NotFoundResult;
            Assert.IsInstanceOf<NotFoundResult>(brewery);
        }

        [Test]
        public async Task PostBreweryGetsAdded()
        {
            using (var stream = new StreamReader(JSONPATH + "brewery.json"))
            {

                var count = _controller.GetBreweries().Result.Breweries.Count();
                var breweryJson = await stream.ReadToEndAsync();
                var breweries = JsonConvert.DeserializeObject<List<BreweryDto>>(breweryJson);
                await _controller.PostBrewery(breweries);
                var total = _controller.GetBreweries().Result.Breweries.Count();
                Assert.AreEqual(count + breweries.Count, total);
            }
        }

        [Test]
        public async Task PutBreweryGetNameUpdated()
        {
            var breweryCompleteDto = await _controller.GetBrewery(1) as OkNegotiatedContentResult<BreweryCompleteDto>;
            var brewery = breweryCompleteDto.Content.Breweries[0];
            brewery.Name = "Big brewery";
            await _controller.PutBrewery(brewery.Id, brewery);
            var updatedBrewery = await _controller.GetBrewery(1) as OkNegotiatedContentResult<BreweryCompleteDto>;
            Assert.AreEqual(brewery.Name, updatedBrewery.Content.Breweries[0].Name);
        }

        [Test]
        public async Task PutBreweryAddNewMember()
        {
            var breweryCompleteDto = await _controller.GetBrewery(1) as OkNegotiatedContentResult<BreweryCompleteDto>;
            var brewery = breweryCompleteDto.Content.Breweries[0];
            var member = new DTOUser(){ Username = "johnfredrik"};
            brewery.Members.Add(member);
            var memberCount = brewery.Members.Count();
            await _controller.PutBrewery(brewery.Id, brewery);
            var updatedBrewery = await _controller.GetBrewery(1) as OkNegotiatedContentResult<BreweryCompleteDto>;
            Assert.AreEqual(memberCount, updatedBrewery.Content.Breweries[0].Members.Count());

        }

        [Test]
        public async Task PutBreweryAddNewMemberUpdateOldMemberRole()
        {
            var breweryCompleteDto = await _controller.GetBrewery(2) as OkNegotiatedContentResult<BreweryCompleteDto>;
            var brewery = breweryCompleteDto.Content.Breweries[0];
            brewery.Members[0].Role = "Brewmaster";
            var member = new DTOUser() { Username = "johnfredrik" };
            brewery.Members.Add(member);
            var memberCount = brewery.Members.Count();
            await _controller.PutBrewery(brewery.Id, brewery);
            
            var updatedBrewery = await _controller.GetBrewery(brewery.Id) as OkNegotiatedContentResult<BreweryCompleteDto>;
            Assert.AreEqual(memberCount, updatedBrewery.Content.Breweries[0].Members.Count());
            
            var torstein = updatedBrewery.Content.Breweries[0].Members.SingleOrDefault(m => m.Username.Equals("torstein"));
            Assert.AreEqual("Brewmaster", torstein.Role);

        }

        [Test]
        public async Task DeleteBreweryReturnStatusCode200WithBreweryMember() 
        {
            var breweryCompleteDto = await _controller.GetBrewery(4) as OkNegotiatedContentResult<BreweryCompleteDto>;
            var brewery = breweryCompleteDto.Content.Breweries[0];
            var member = new DTOUser() { Username = "johnfredrik" };
            brewery.Members.Add(member);
            await _controller.PutBrewery(brewery.Id, brewery);

            var response = await _controller.DeleteBrewery(4) as OkNegotiatedContentResult<BreweryDto>;
            Assert.IsInstanceOf<OkNegotiatedContentResult<BreweryDto>>(response);
        }

        [Test]
        public async Task DeleteBreweryReturnStatusCode200()
        {

            var response = await _controller.DeleteBrewery(3) as OkNegotiatedContentResult<BreweryDto>;
            Assert.IsInstanceOf<OkNegotiatedContentResult<BreweryDto>>(response);
        }




    }
}
