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
using Microbrewit.Model.DTOs;
using System.Web.Http.Results;

namespace Microbrewit.Test
{
    [TestFixture]
    public class YeastControllerTest
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IYeastRepository _repository;
        private MicrobrewitContext _context;
        private YeastController _controller;

        [TestFixtureSetUp]
        public void Init()
        {
            //TestUtil.DeleteDataInDatabase();
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
        public void GetAllYeastsNotNull()
        {
            var yeasts = _controller.GetYeasts() as YeastCompleteDto;
            Assert.NotNull(yeasts);
        }

        [Test]
        public void GetYeastWithValidIdNotNullOrEmpty()
        {
            var first = _context.Yeasts.FirstOrDefault();
            var yeast = _controller.GetYeast(first.Id) as OkNegotiatedContentResult<YeastCompleteDto>;;
            Assert.NotNull(yeast);
        }

        [Test]
        public void GetYeastWithInvalidIdShouldNotBeFound()
        {
            var yeast = _controller.GetYeast(int.MaxValue);
            Assert.IsInstanceOf<NotFoundResult>(yeast);
        } 
    }
}
