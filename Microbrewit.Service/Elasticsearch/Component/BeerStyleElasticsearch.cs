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
    public class BeerStyleElasticsearch : IBeerStyleElasticsearch
    {
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private const int BigNumber = 10000;
        private readonly string _url ;

        public BeerStyleElasticsearch()
        {
            _url = WebConfigurationManager.AppSettings["elasticsearch"];
            this._node = new Uri(_url);
            this._settings = new ConnectionSettings(_node, defaultIndex: Setting.ElasticSearchIndex);
            this._client = new ElasticClient(_settings);
        }

        public async Task UpdateAsync(BeerStyleDto beerStyleDto)
        {
            await _client.MapAsync<BeerStyleDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            var index = await _client.IndexAsync<BeerStyleDto>(beerStyleDto);
        }

        public async Task<IEnumerable<BeerStyleDto>> GetAllAsync(int from, int size)
        {
            var result = await _client.SearchAsync<BeerStyleDto>(s => s
                .Filter(f => f.Term(t => t.DataType, "beerstyle"))
                .Size(size)
                .From(from)
                );
            return result.Documents;
        }

        public async Task<BeerStyleDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest(Setting.ElasticSearchIndex, "beerStyle", id.ToString());
            var result = await _client.GetAsync<BeerStyleDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<BeerStyleDto>> SearchAsync(string query, int @from, int size)
        {
            var searchResults = await _client.SearchAsync<BeerStyleDto>(s => s
                                               .From(from)
                                               .Size(size)
                                               .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                         .Query(query))));
            return searchResults.Documents;
        }

        public async Task UpdateAllAsync(IEnumerable<BeerStyleDto> beerStyleDtos)
        {
            await _client.MapAsync<BeerStyleDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            var index = await _client.IndexManyAsync<BeerStyleDto>(beerStyleDtos);
        }

        public async Task DeleteAsync(int id)
        {
            await _client.DeleteAsync<BeerStyleDto>(id);
        }

        public BeerStyleDto GetSingle(int id)
        {
            IGetRequest getRequest = new GetRequest(Setting.ElasticSearchIndex, "beerStyle", id.ToString());
            var result = _client.Get<BeerStyleDto>(getRequest);
            return result.Source;
        }

        public IEnumerable<BeerStyleDto> Search(string query, int @from, int size)
        {
            var searchResults =  _client.Search<BeerStyleDto>(s => s
                                               .From(from)
                                               .Size(size)
                                               .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                         .Query(query))));
            return searchResults.Documents;
        }
    }
}
