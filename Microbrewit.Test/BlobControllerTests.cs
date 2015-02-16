using Microbrewit.Api.Controllers;
using Microbrewit.Repository;
using Microbrewit.Service.Component;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;
using Microbrewit.Service.Interface;
using Nest;
using NUnit.Framework;

namespace Microbrewit.Test
{
    [TestFixture]
    public class BlobControllerTests
    {
        private static BlobController _blobController;
        private static IUserService _userService;
        private static IUserElasticsearch _userElasticsearch;
        private static IUserRepository _userRepository;
        private static IBreweryRepository _breweryRepository;
        private static IBreweryElasticsearch _breweryElasticsearch;

        [TestFixtureSetUp]
        public void TestFixureSetup()
        {
            
            _userElasticsearch = new UserElasticsearch();
            _userRepository = new UserRepository();
            _userService = new UserService(_userElasticsearch,_userRepository,_breweryElasticsearch,_breweryRepository);
            _blobController = new BlobController(_userService);

        }

        [Test]
        public async void CleanBlobGetsCleaned()
        {
            await _blobController.GetCleanBlobStorage();

        }
    }
}