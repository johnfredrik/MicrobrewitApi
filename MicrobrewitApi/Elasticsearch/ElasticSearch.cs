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
    }
}