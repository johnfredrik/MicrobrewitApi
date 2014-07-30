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

        public async Task UpdateOriginElasticSearch(IList<Origin> origins)
        {
            foreach (var origin in origins)
            {
                // Adds an analayzer to the name property in FermentableDto object.
                _client.Map<Origin>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
                var index = _client.Index<Origin>(origin);
            }
        }
        
        public async Task<IEnumerable<Origin>> GetOrigins(string query, int from, int size)
        {
            //var searchResults = _client.Search<Origin>(s => s
            //                                    .From(from)
            //                                    .Size(size)
            //                                    .Query(q => q
            //                                        .Term(y => y.Name, query)));

            var searchResults = _client.Search<Origin>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                          .Query(query))));

            return searchResults.Documents;
        }
    }
}