using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using log4net;
using Microbrewit.Repository;
using Microbrewit.Model;
using Microbrewit.Api.Automapper;
using Microbrewit.Api.Controllers;

namespace Microbrewit.Test
{
    [TestFixture]
    public class HopControllerTest
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IHopRepository _repository;
        private MicrobrewitContext _context;

        [TestFixtureSetUp]
        public void Init()
        {
            TestUtil.DeleteDataInDatabase();
            AutoMapperConfiguration.Configure();
            _context = new MicrobrewitContext();
            _repository = new HopRepository(new MicrobrewitContext());
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void GetHopsGetsValuesFromRedisStore()
        {
            var controller = new HopController(_repository);
            var hops = controller.GetHops();
            Assert.NotNull(hops);
        }
    }
}
