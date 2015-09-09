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
    public class YeastElasticsearch : IYeastElasticsearch
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;

        public YeastElasticsearch()
        {
            string url = WebConfigurationManager.AppSettings["elasticsearch"];
            this._node = new Uri(url);
            this._settings = new ConnectionSettings(_node, defaultIndex: Setting.ElasticSearchIndex);
            this._client = new ElasticClient(_settings);
        }

        public async Task UpdateAsync(YeastDto yeastDto)
        {
            await _client.MapAsync<YeastDto>(d => d.Properties(p => p
                .String(s => s.Name(n => n.Name).Analyzer("autocomplete"))
                .String(s => s.Name(n => n.ProductCode).Analyzer("autocomplete"))
                ));
            await _client.IndexAsync(yeastDto);
        }

        public async Task<IEnumerable<YeastDto>> GetAllAsync(string custom)
        {
            var res = await _client.SearchAsync<YeastDto>(s => s.Size(_bigNumber).Filter(f => f.Term(t => t.DataType, "yeast") && f.Term(t => t.Custom, custom)));
            return res.Documents;
        }

        public async Task<YeastDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest(Setting.ElasticSearchIndex, "yeast", id.ToString());
            var result = await _client.GetAsync<YeastDto>(getRequest);
            return (YeastDto)result.Source;
        }

        public async Task<IEnumerable<YeastDto>> SearchAsync(string query, int from, int size)
        {
            var fields = new List<string> { "name", "productCode" };
            var searchResults = await _client.SearchAsync<YeastDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.MultiMatch(m => m.OnFields(fields).Query(query))));
            return searchResults.Documents;
        }

        public async Task UpdateAllAsync(IEnumerable<YeastDto> yeasts)
        {
            await _client.MapAsync<YeastDto>(d => d.Properties(p => p
               .String(s => s.Name(n => n.Name).Analyzer("autocomplete"))
               .String(s => s.Name(n => n.ProductCode).Analyzer("autocomplete"))
               ));
            await _client.IndexManyAsync(yeasts);
        }

        public async Task DeleteAsync(int id)
        {
            await _client.DeleteAsync<YeastDto>(id);
        }

        public YeastDto GetSingle(int id)
        {
            IGetRequest getRequest = new GetRequest(Setting.ElasticSearchIndex, "yeast", id.ToString());
            var result = _client.Get<YeastDto>(getRequest);
            return (YeastDto)result.Source;
        }

        public IEnumerable<YeastDto> Search(string query, int @from, int size)
        {
            var fields = new List<string> { "name", "productCode" };
            var searchResults = _client.Search<YeastDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.MultiMatch(m => m.OnFields(fields).Query(query))));
            return searchResults.Documents;
        }
    }

}
