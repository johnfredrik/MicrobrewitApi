using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task SearchAll(string query)
        {
            //var result = _client.Search(s => s
            //    .From(0)
            //    .Size(20)
            //    .Query(q => q.Name, query));
        }


        public async Task UpdateYeastsElasticSearch(IList<YeastDto> yeasts)
        {
            foreach (var yeast in yeasts)
            {
                // Adds an analayzer to the name property in yeastDto object.
                _client.Map<YeastDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
                var index = _client.Index<YeastDto>(yeast);
            }
        }

        public async Task<IEnumerable<YeastDto>> GetYeasts(string query)
        {
            var searchResults = _client.Search<YeastDto>(s => s
                                                .From(0)
                                                .Size(10)
                                                .Query(q => q
                                                    .Term(y => y.Name, query)));

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

        public async Task<IEnumerable<FermentableDto>> GetFermentables(string query)
        {
            var searchResults = _client.Search<FermentableDto>(s => s
                                                .From(0)
                                                .Size(10)
                                                .Query(q => q
                                                    .Term(y => y.Name, query)));

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

        public async Task<IEnumerable<HopDto>> GetHops(string query)
        {
            var searchResults = _client.Search<HopDto>(s => s
                                                .From(0)
                                                .Size(10)
                                                .Query(q => q
                                                    .Term(y => y.Name, query)));

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

        public async Task<IEnumerable<OtherDto>> GetOthers(string query)
        {
            var searchResults = _client.Search<OtherDto>(s => s
                                                .From(0)
                                                .Size(10)
                                                .Query(q => q
                                                    .Term(y => y.Name, query)));

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

        public async Task<IEnumerable<SupplierDto>> GetSuppliers(string query)
        {
            var searchResults = _client.Search<SupplierDto>(s => s
                                                .From(0)
                                                .Size(10)
                                                .Query(q => q
                                                    .Term(y => y.Name, query)));

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

        public async Task<IEnumerable<Origin>> GetOrigins(string query)
        {
            var searchResults = _client.Search<Origin>(s => s
                                                .From(0)
                                                .Size(10)
                                                .Query(q => q
                                                    .Term(y => y.Name, query)));

            return searchResults.Documents;
        }
    }
}