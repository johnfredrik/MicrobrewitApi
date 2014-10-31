using Elasticsearch.Net;
using Elasticsearch.Net.Connection;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Microbrewit.Api.Elasticsearch
{
    public class ElasticSearch
    {
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;

        public ElasticSearch()
        {
            this._node = new Uri("http://localhost:9200");
            this._settings = new ConnectionSettings(_node, defaultIndex: "mb");
            this._client = new ElasticClient(_settings);
        }

        public async Task<string> SearchAll(string query, int from, int size)
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionConfiguration(node);
            var client = new ElasticsearchClient(settings);

            //var queryString = "{\"query\" : { \"match\": { \"name\" : {\"query\" : \"" + query + "\", and \"operator\" : \"and\"}}}}";
            var queryString = "{\"from\" : " + from + ", \"size\" : " + size +", \"query\":{\"match\": {\"name\": {\"query\": \" " + query + " \",\"operator\": \"and\"}}}}";
            var res = client.Search<string>("mb",queryString);
            return res.Response;
        }

        public async Task<string> SearchAllIngredients(string query, int from, int size)
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionConfiguration(node);
            var client = new ElasticsearchClient(settings);

            //var queryString = "{\"query\" : { \"match\": { \"name\" : {\"query\" : \"" + query + "\", and \"operator\" : \"and\"}}}}";
            var queryString = "{\"from\" : " + from + ", \"size\" : " + size + ", \"query\":{\"match\": {\"name\": {\"query\": \" " + query + " \",\"operator\": \"and\"}}}}";
            var res = client.Search<string>("mb","yeastdto,hopdto,fermentabledto,otherdto",queryString);
            return res.Response;
        }

        public async Task UpdateYeastsElasticSearch(IList<YeastDto> yeasts)
        {
            _client.Map<YeastDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            _client.IndexMany(yeasts, "mb");
           
        }

        public async Task<IEnumerable<YeastDto>> SearchYeasts(string query, int from, int size)
        {
            var searchResults = _client.Search<YeastDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                          .Query(query))));

            return searchResults.Documents;
        }

        public async Task<IEnumerable<YeastDto>> GetAllYeasts()
        {
             return _client.Search<YeastDto>(s => s
                                                .Types(typeof(YeastDto))
                                                .Size(_bigNumber)
                                                ).Documents.OrderBy(y => y.Name);
        }

        public async Task<YeastDto> GetYeast(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "yeastdto", id.ToString());
            var result = _client.Get<YeastDto>(getRequest);
            return (YeastDto)result.Source;
        }

        public async Task UpdateFermentableElasticSearch(IList<FermentableDto> fermentables)
        {
            foreach (var fermentable in fermentables)
            {
                // Adds an analayzer to the name property in FermentableDto object.
                _client.Map<FermentableDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
                var index = _client.Index<FermentableDto>(fermentable);
            }
        }

        public async Task<IEnumerable<FermentableDto>> SearchFermentables(string query, int from, int size)
        {
            var searchResults = _client.Search<FermentableDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                          .Query(query))));

            return searchResults.Documents;
        }

        public async Task<IEnumerable<FermentableDto>> GetFermentables()
        {
            return _client.Search<FermentableDto>(s => s
                                                .Types(typeof(FermentableDto))
                                                .Size(_bigNumber)
                                                ).Documents;
        }

        public async Task<FermentableDto> GetFermentable(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "fermentabledto", id.ToString());
            var result = _client.Get<FermentableDto>(getRequest);
            return result.Source;
        }

        public async Task UpdateHopElasticSearch(IList<HopDto> hops)
        {
            foreach (var hop in hops)
            {
                // Adds an analayzer to the name property in FermentableDto object.
                _client.Map<HopDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
                var index = _client.Index<HopDto>(hop);
            }
        }

        public async Task<IEnumerable<HopDto>> GetHops()
        {
            return _client.Search<HopDto>(s => s
                                                .Types(typeof(HopDto))
                                                .Size(_bigNumber)
                                                ).Documents;
        }

        public async Task<HopDto> GetHop(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "hopdto", id.ToString());
            var result = _client.Get<HopDto>(getRequest);
            return result.Source;
        }


        public async Task<IEnumerable<HopDto>> SearchHops(string query, int from, int size)
        {
            var searchResults = _client.Search<HopDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                          .Query(query))));

            return searchResults.Documents;
        }

        public async Task UpdateOtherElasticSearch(IList<OtherDto> others)
        {
            foreach (var other in others)
            {
                // Adds an analayzer to the name property in FermentableDto object.
                _client.Map<OtherDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
                var index = _client.Index<OtherDto>(other);
            }
        }

        public async Task<IEnumerable<OtherDto>> GetOthers()
        {
            return _client.Search<OtherDto>(s => s
                                                .Types(typeof(OtherDto))
                                                .Size(_bigNumber)
                                                ).Documents;
        }

        public async Task<OtherDto> GetOther(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "otherdto", id.ToString());
            var result = _client.Get<OtherDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<OtherDto>> SearchOthers(string query, int from, int size)
        {
            var searchResults = _client.Search<OtherDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                          .Query(query))));

            return searchResults.Documents;
        }

        public async Task UpdateSupplierElasticSearch(IList<SupplierDto> suppliers)
        {
            foreach (var supplier in suppliers)
            {
                // Adds an analayzer to the name property in FermentableDto object.
                _client.Map<SupplierDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
                var index = _client.Index<SupplierDto>(supplier);
            }
        }

        public async Task<IEnumerable<SupplierDto>> GetSuppliers()
        {
            return _client.Search<SupplierDto>(s => s
                                                .Types(typeof(SupplierDto))
                                                .Size(_bigNumber)
                                                ).Documents;
        }

        public async Task<SupplierDto> GetSupplier(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "supplierdto", id.ToString());
            var result = _client.Get<SupplierDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<SupplierDto>> SearchSuppliers(string query, int from, int size)
        {
            var searchResults = _client.Search<SupplierDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                          .Query(query))));

            return searchResults.Documents;
        }

        public async Task UpdateOriginElasticSearch(IList<OriginDto> origins)
        {
            foreach (var origin in origins)
            {
                // Adds an analayzer to the name property in FermentableDto object.
                _client.Map<OriginDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
                var index = _client.Index<OriginDto>(origin);
            }
        }

        public async Task<IEnumerable<OriginDto>> GetOrigins()
        {
            return _client.Search<OriginDto>(s => s
                                                .Types(typeof(OriginDto))
                                                .Size(_bigNumber)
                                                ).Documents;
        }

        public async Task<OriginDto> GetOrigin(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "origindto", id.ToString());
            var result = _client.Get<OriginDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<OriginDto>> SearchOrigins(string query, int from, int size)
        {
            var searchResults = _client.Search<OriginDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                          .Query(query))));
            return searchResults.Documents;
        }

        public async Task UpdateBreweryElasticSearch(IList<BreweryDto> breweries)
        {
            foreach (var brewery in breweries)
            {
                // Adds an analayzer to the name property in FermentableDto object.
                _client.Map<BreweryDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
                var index = _client.Index<BreweryDto>(brewery);
            }
        }

        public async Task<IEnumerable<BreweryDto>> GetBreweries()
        {
            return _client.Search<BreweryDto>(s => s
                                                .Types(typeof(BreweryDto))
                                                .Size(_bigNumber)
                                                ).Documents;
        }

        public async Task<BreweryDto> GetBrewery(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "brewerydto", id.ToString());
            var result = _client.Get<BreweryDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<BreweryDto>> GetBreweries(string query, int from, int size)
        {
            var searchResults = _client.Search<BreweryDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                          .Query(query))));
            return searchResults.Documents;
        }

        public async Task UpdateUsersElasticSearch(IList<UserDto> users)
        {
            foreach (var user in users)
            {
                // Adds an analayzer to the name property in FermentableDto object.
                _client.Map<UserDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Username).Analyzer("autocomplete"))));
                var index = _client.Index<UserDto>(user);
            }
        }

        public async Task<IEnumerable<UserDto>> GetUsers(string query, int from, int size)
        {
            var searchResults = _client.Search<UserDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.OnField(f => f.Username)
                                                                          .Query(query))));
            return searchResults.Documents;
        }

        public async Task UpdateBeerStylesElasticSearch(IList<BeerStyleDto> beerstyles)
        {
            foreach (var beerstyle in beerstyles)
            {
                // Adds an analayzer to the name property in FermentableDto object.
                _client.Map<BeerStyleDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
                var index = _client.Index<BeerStyleDto>(beerstyle);
            }
        }

        public async Task<IEnumerable<BeerStyleDto>> GetBeerStyles()
        {
            return _client.Search<BeerStyleDto>(s => s
                                                .Types(typeof(BeerStyleDto))
                                                .Size(_bigNumber)
                                                ).Documents;
        }

        public async Task<BeerStyleDto> GetBeerStyle(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "beerstyledto", id.ToString());
            var result = _client.Get<BeerStyleDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<BeerStyleDto>> SearchBeerStyles(string query, int from, int size)
        {
            var searchResults = _client.Search<BeerStyleDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                          .Query(query))));
            return searchResults.Documents;
        }




        public async Task UpdateBeerElasticSearch(IList<BeerDto> beersDto)
        {
            foreach (var beer in beersDto)
            {
                // Adds an analayzer to the name property in FermentableDto object.
                _client.Map<BeerDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
                var index = _client.Index<BeerDto>(beer);
            }
        }

        public async Task<IEnumerable<BeerDto>> SearchBeers(string query, int from, int size)
        {
            var searchResults = _client.Search<BeerDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                          .Query(query))));
            return searchResults.Documents;
        }

        public async Task<IList<BeerDto>> GetLastBeers(int from, int size)
        {
            var result =  _client.Search<BeerDto>(s => s
                                                        .Sort(p => p
                                                            .OnField("createdDate")
                                                            .Descending())
                                                        .From(from)
                                                        .Size(size));
            return result.Documents.ToList();
        }
    }
}