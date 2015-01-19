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
using System.Web.Configuration;
using log4net;

namespace Microbrewit.Api.Elasticsearch
{

    public class ElasticSearch
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;

        public ElasticSearch()
        {
            string url = WebConfigurationManager.AppSettings["elasticsearch"];
            Log.Debug("Elasticsearch Url: " + url);
            this._node = new Uri(url);
            this._settings = new ConnectionSettings(_node, defaultIndex: "mb");
            this._client = new ElasticClient(_settings);
        }

        public async Task<string> SearchAll(string query, int from, int size)
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionConfiguration(node);
            var client = new ElasticsearchClient(settings);

            //var queryString = "{\"query\" : { \"match\": { \"name\" : {\"query\" : \"" + query + "\", and \"operator\" : \"and\"}}}}";
            var queryString = "{\"from\" : " + from + ", \"size\" : " + size + ", \"query\":{\"match\": {\"name\": {\"query\": \" " + query + " \",\"operator\": \"and\"}}}}";
            var res = await client.SearchAsync<string>("mb", queryString);
            return res.Response;
        }

        public async Task<string> SearchAllIngredients(string query, int from, int size)
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionConfiguration(node);
            var client = new ElasticsearchClient(settings);

            //var queryString = "{\"query\" : { \"match\": { \"name\" : {\"query\" : \"" + query + "\", and \"operator\" : \"and\"}}}}";
            var queryString = "{\"from\" : " + from + ", \"size\" : " + size + ", \"query\":{\"filter\":{\"dataType\":hop\",\"dataType\":fermentable\",\"dataType\":yeasts\",\"dataType\":other\",} \"match\": {\"name\": {\"query\": \" " + query + " \"}}}}";
           
            var res = await client.SearchAsync<string>("mb", queryString);
            return res.Response;
        }

        public async Task UpdateYeastsElasticSearch(IList<YeastDto> yeasts)
        {
            _client.Map<YeastDto>(d => d.Properties(p => p
                .String(s => s.Name(n => n.Name).Analyzer("autocomplete"))
                .String(s => s.Name(m => m.ProductCode).Analyzer("autocomplete"))));
           await _client.IndexManyAsync(yeasts, "mb");

        }

        public async Task<IEnumerable<YeastDto>> SearchYeasts(string query, int from, int size)
        {
            //var searchResults = _client.Search<YeastDto>(s => s
            //                                    .From(from)
            //                                    .Size(size)
            //                                    .Query(q => q.Match(m => m.OnField(f => f.ProductCode)
            //                                                              .Query(query))));
            var fields = new List<string> { "name", "productCode" };
            var searchResults = await _client.SearchAsync<YeastDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.MultiMatch(m => m.OnFields(fields).Query(query))));

            return searchResults.Documents;
        }

        public async Task<IEnumerable<YeastDto>> GetAllYeasts(string custom)
        {
            var res = _client.Search<YeastDto>(s => s.Size(_bigNumber).Filter(f => f.Term(t => t.DataType, "yeast") && f.Term(t => t.Custom, custom)));

            return res.Documents;
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

        public async Task<IEnumerable<FermentableDto>> GetFermentables(string custom)
        {
            var res = _client.Search<FermentableDto>(s => s.Size(_bigNumber).Filter(f => f.Term(t => t.DataType, "fermentable") && f.Term(t => t.Custom,custom)));
            return res.Documents;
        }

        public async Task<FermentableDto> GetFermentable(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "fermentable", id.ToString());
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

        public async Task<IEnumerable<HopDto>> GetHops(string custom)
        {
            var res =
                _client.Search<HopDto>(
                    s => s
                        .Size(_bigNumber)
                        .Query(q => q
                            .Filtered(fd => fd
                                .Filter(f => f
                                    .Term(h => h.DataType, "hop") && f.Term(p => p.Custom, custom)
                                    ))));
            return res.Documents;
        }

        public async Task<HopDto> GetHop(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "hop", id.ToString());
            var result = _client.Get<HopDto>(getRequest);
            return result.Source;
        }


        public async Task<IEnumerable<HopDto>> SearchHops(string query, int from, int size)
        {
            var searchResults = _client.Search<HopDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Filter(f => f.Term(t => t.DataType,"hop"))
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
                _client.Index<OtherDto>(other);
            }
        }

        public async Task<IEnumerable<OtherDto>> GetOthers(string custom)
        {
            var res =
              _client.Search<OtherDto>(
                  s => s
                      .Size(_bigNumber)
                      .Filter(f => f.Term(h => h.DataType, "other") && f.Term(p => p.Custom, custom)));
            return res.Documents;
        }

        public async Task<OtherDto> GetOther(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "other", id.ToString());
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
                                                .Filter(f => f.Term(t => t.DataType,"supplier"))
                                                .Size(_bigNumber)
                                                ).Documents;
        }

        public async Task<SupplierDto> GetSupplier(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "supplier", id.ToString());
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
                                                .Filter(f => f.Term(t => t.DataType,"origin"))
                                                .Size(_bigNumber)
                                                ).Documents;
        }

        public async Task<OriginDto> GetOrigin(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "origin", id.ToString());
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
                                                .Filter(f => f.Term(t => t.DataType,"brewery"))
                                                .Size(_bigNumber)
                                                ).Documents;
        }

        public async Task<BreweryDto> GetBrewery(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "brewery", id.ToString());
            var result = _client.Get<BreweryDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<BreweryDto>> SearchBreweries(string query, int from, int size)
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
               await  UpdateUserElasticSearch(user);
            }
        }

        public async Task UpdateUserElasticSearch(UserDto user)
        {
            // Adds an analayzer to the name property in FermentableDto object.
            await _client.MapAsync<UserDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Username).Analyzer("autocomplete"))));
            var index = _client.Index<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(int from, int size)
        {
            var result = await _client.SearchAsync<UserDto>(s => s
                                                .Filter(f => f.Term(t => t.DataType, "user"))
                                                .Filter(f => f.Term(t => t.EmailConfirmed,"true"))
                                                .Size(_bigNumber)
                                                );
            return result.Documents;
        }

        public async Task<IEnumerable<UserDto>> SearchUsers(string query, int from, int size)
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
                                                .Filter(f => f.Term(t => t.DataType,"beerstyle"))
                                                .Size(_bigNumber)
                                                ).Documents;
        }

        public async Task<BeerStyleDto> GetBeerStyle(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "beerstyle", id.ToString());
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




        public async Task UpdateBeer(IList<BeerDto> beersDto)
        {
            foreach (var beerDto in beersDto)
            {
                UpdateBeer(beerDto);
            }
        }

        public async Task UpdateBeer(BeerDto beerDto)
        {
            // Adds an analayzer to the name property in FermentableDto object.
            _client.Map<BeerDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            var index = _client.Index<BeerDto>(beerDto);
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
            var result = _client.Search<BeerDto>(s => s
                                                        .Sort(p => p
                                                            .OnField("createdDate")
                                                            .Descending())
                                                        .From(from)
                                                        .Size(size));
            return result.Documents.ToList();
        }

        public async Task UpdateGlassElasticSearch(GlassDto glassDto)
        {
                // Adds an analayzer to the name property in FermentableDto object.
                _client.Map<GlassDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
                var index = _client.Index<GlassDto>(glassDto);
        }

        public async Task UpdateGlassesElasticSearch(IList<GlassDto> glassesDto)
        {
            foreach (var glassDto in glassesDto)
            {
                await UpdateGlassElasticSearch(glassDto);
            }
        }

        public async Task<IEnumerable<GlassDto>> SearchByGlass(string query, int from, int size)
        {
            var searchResults = _client.Search<GlassDto>(s => s
                                               .From(from)
                                               .Size(size)
                                               .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                         .Query(query))));
            return searchResults.Documents;
        }

        public async Task<IEnumerable<GlassDto>> GetGlasses()
        {
            var res = _client.Search<GlassDto>(s => s
                .Filter(f => f.Term(t => t.DataType, "glass"))
                .Size(_bigNumber)
                );
            return res.Documents;
        }

        public async Task<GlassDto> GetGlass(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "glass", id.ToString());
            var result = _client.Get<GlassDto>(getRequest);
            return result.Source;
        }

        public IEnumerable<BeerDto> GetAllBeers(int from, int size)
        {
            var result = _client.Search<BeerDto>(s => s
                .From(from)
                .Size(size)
                .Query(q => q
                    .Filtered(f => f
                        .Filter(filter => filter
                            .Term(t => t.DataType,"beer")
                            ))));
            return result.Documents;
        }

        public IEnumerable<BeerDto> GetUserBeer(string username)
        {
            var result = _client.Search<BeerDto>(s => s
                .Query(q => q
                .Filtered(f => f
                    .Query(q2 => q2
                    .Term("brewers.userName",username))
                    .Filter(filter => filter
                        .Term(t => t.DataType,"beer")
                        ))));
            return result.Documents;
        }
    }
}