using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using log4net;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Elasticsearch.Interface;
using Nest;

namespace Microbrewit.Service.Elasticsearch.Component
{
    public class OriginElasticsearch : IOriginElasticsearch
    {
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;
        private readonly string _url ;

        public OriginElasticsearch()
        {
            _url = WebConfigurationManager.AppSettings["elasticsearch"];
            this._node = new Uri(_url);
            this._settings = new ConnectionSettings(_node, defaultIndex: Setting.ElasticSearchIndex);
            this._client = new ElasticClient(_settings);
        }

        public async Task UpdateAsync(OriginDto originDto)
        {
            await _client.MapAsync<OriginDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            var index =  await _client.IndexAsync<OriginDto>(originDto);
        }

        public async Task<IEnumerable<OriginDto>> GetAllAsync(string custom)
        {
            var result = await _client.SearchAsync<OriginDto>(s => s
                .Filter(f => f.Term(t => t.DataType, "origin"))
                .Size(_bigNumber)
                );
            return result.Documents;
        }

        public async Task<OriginDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest(Setting.ElasticSearchIndex, "origin", id.ToString());
            var result = await  _client.GetAsync<OriginDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<OriginDto>> SearchAsync(string query, int @from, int size)
        {
            var searchResults = await _client.SearchAsync<OriginDto>(s => s
                                               .From(from)
                                               .Size(size)
                                               .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                         .Query(query))));
            return searchResults.Documents;
        }

        public async Task UpdateAllAsync(IEnumerable<OriginDto> originDtos)
        {
            await _client.MapAsync<OriginDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            var index = await _client.IndexManyAsync<OriginDto>(originDtos);
        }

        public async Task DeleteAsync(int id)
        {
            await _client.DeleteAsync<OriginDto>(id);
        }
    }
}
