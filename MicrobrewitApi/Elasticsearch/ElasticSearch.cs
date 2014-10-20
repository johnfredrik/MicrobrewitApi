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
            var res = client.Search<string>("mb", queryString);
            return res.Response;
        }


        public async Task UpdateYeastsElasticSearch(IList<YeastDto> yeasts)
        {
            _client.Map<YeastDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            _client.IndexMany(yeasts, "mb");
           
        }

        public async Task<IEnumerable<YeastDto>> GetYeasts(string query, int from, int size)
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
            var result = _client.Search<YeastDto>(s => s
                                                .Types(typeof(YeastDto))
                                                ).Documents.OrderBy(y => y.Name);

            return result;
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

        public async Task<IEnumerable<FermentableDto>> GetFermentables(string query, int from, int size)
        {
            var searchResults = _client.Search<FermentableDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                          .Query(query))));

            return searchResults.Documents;
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

        public async Task<IEnumerable<HopDto>> GetHops(string query, int from, int size)
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

        public async Task<IEnumerable<OtherDto>> GetOthers(string query, int from, int size)
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

        public async Task<IEnumerable<SupplierDto>> GetSuppliers(string query, int from, int size)
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
        
        public async Task<IEnumerable<OriginDto>> GetOrigins(string query, int from, int size)
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

    }
}