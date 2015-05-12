using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Automapper;
using Microbrewit.Service.Component;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;
using Microbrewit.Service.Interface;
using NUnit.Framework;

namespace Microbrewit.Test
{
    [TestFixture]
    public class BeerServiceTests
    {
        private IBeerService _beerService;
        private IBeerRepository _beerRepository;
        private IBeerElasticsearch _beerElasticsearch;
        private IUserService _userService;
        private IUserRepository _userRepository;
        private IUserElasticsearch _userElasticsearch;
        private IBreweryService _breweryService;
        private IBreweryRepository _breweryRepository;
        private IBreweryElasticsearch _breweryElasticsearch;

        [TestFixtureSetUp]
        public void TextFixtureSetup()
        {
            AutoMapperConfiguration.Configure();
            _breweryElasticsearch = new BreweryElasticsearch();
            _breweryRepository = new BreweryRepository();
            _userElasticsearch = new UserElasticsearch();
            _userRepository = new UserRepository();
            _userService = new UserService(_userElasticsearch,_userRepository,_breweryElasticsearch,_breweryRepository,_beerRepository,_beerElasticsearch);
            _breweryService = new BreweryService(_breweryRepository, _breweryElasticsearch, _userService);
            _beerRepository = new BeerRepository();
            _beerElasticsearch = new BeerElasticsearch();
            _beerService = new BeerService(_beerElasticsearch,_beerRepository,_userService,_breweryService);
        }



        [Test]
        public async void AddBeer_Gets_Added()
        {
            var beer = new BeerDto
            {
                Name = "Test Beer",
                BeerStyle = new BeerStyleSimpleDto {Id = 1, Name = "American-Style Wheat Beer"},
                Brewers = new List<DTOUser> { new DTOUser { Username = "johnfredrik"} },
            };

            var recipe = new RecipeDto
            {
                MashSteps = new List<MashStepDto>{new MashStepDto{StepNumber = 1}},
                //SpargeStep = new List<SpargeStepDto>{new SpargeStepDto{StepNumber = 2,Amount = 1, Temperature = 80}},
                SpargeStep = new SpargeStepDto{StepNumber = 2,Amount = 1, Temperature = 80},
                BoilSteps = new List<BoilStepDto> {new BoilStepDto {StepNumber = 3}},
                FermentationSteps = new List<FermentationStepDto> {new FermentationStepDto{StepNumber = 4}},
                Volume = 20,
                Efficiency = 75,
            };

            beer.Recipe = recipe;
            var beerDto = await _beerService.AddAsync(beer);
            Assert.NotNull(beerDto);
        }

    }
}
