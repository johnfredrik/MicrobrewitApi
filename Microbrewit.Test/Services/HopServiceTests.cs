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

namespace Microbrewit.Test.Services
{
    [TestFixture]
    public class HopServiceTests
    {
        private IHopService _hopService;
        private IHopElasticsearch _hopElasticsearch;
        private IHopRepository _hopRepository;

        [TestFixtureSetUp]
        public void TextFixtureSetup()
        {
            AutoMapperConfiguration.Configure();
           
            _hopElasticsearch = new HopElasticsearch();
            _hopRepository = new HopRepository();
           // _hopService = new BeerService(_beerElasticsearch,_hopElasticsearch,_userService,_breweryService);
        }

    }
}
