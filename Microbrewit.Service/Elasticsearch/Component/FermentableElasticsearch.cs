using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using AutoMapper.Internal;
using log4net;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Elasticsearch.Interface;
using Nest;

namespace Microbrewit.Service.Elasticsearch.Component
{
    public class FermentableElasticsearch : IFermentableElasticsearch
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;

        public FermentableElasticsearch()
        {
            string url = WebConfigurationManager.AppSettings["elasticsearch"];
            this._node = new Uri(url);
            this._settings = new ConnectionSettings(_node, defaultIndex: Setting.ElasticSearchIndex);
            this._client = new ElasticClient(_settings);
        }

        public async Task UpdateAsync(FermentableDto fermentableDto)
        {
            await _client.MapAsync<FermentableDto>(d => d.Properties(p => p
                .String(s => s.Name(n => n.Name).Analyzer("autocomplete"))
                ));
            await _client.IndexAsync(fermentableDto);
        }

        public async Task<IEnumerable<FermentableDto>> GetAllAsync(string custom)
        {
            var res = await _client.SearchAsync<FermentableDto>(s => s.Size(_bigNumber).Filter(f => f.Term(t => t.DataType, "fermentable") && f.Term(t => t.Custom, custom)));
            return res.Documents;
        }

        public async Task<FermentableDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest(Setting.ElasticSearchIndex, "fermentable", id.ToString());
            var result = await _client.GetAsync<FermentableDto>(getRequest);
            return (FermentableDto)result.Source;
        }

        public async Task<IEnumerable<FermentableDto>> SearchAsync(string query, int from, int size)
        {
            var fields = new List<string> { "name" };
            var searchResults = await _client.SearchAsync<FermentableDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.MultiMatch(m => m.OnFields(fields).Query(query))));
            return searchResults.Documents;
        }

        public async Task UpdateAllAsync(IEnumerable<FermentableDto> fermentableDtos)
        {
            await _client.MapAsync<FermentableDto>(d => d.Properties(p => p
               .String(s => s.Name(n => n.Name).Analyzer("autocomplete"))
               ));
            await _client.IndexManyAsync(fermentableDtos);
        }

        public async Task DeleteAsync(int id)
        {
            await _client.DeleteAsync<FermentableDto>(id);
        }

        public FermentableDto GetSingle(int id)
        {
            IGetRequest getRequest = new GetRequest(Setting.ElasticSearchIndex, "fermentable", id.ToString());
            var result = _client.Get<FermentableDto>(getRequest);
            return (FermentableDto)result.Source;
        }

        public IEnumerable<FermentableDto> GetAll(string custom)
        {
            var res = _client.Search<FermentableDto>(s => s.Size(_bigNumber).Filter(f => f.Term(t => t.DataType, "fermentable") && f.Term(t => t.Custom, custom)));
            return res.Documents;
        }

        public IEnumerable<FermentableDto> Search(string query, int @from, int size)
        {
            //var fields = new List<string> { "name" };
            var searchResults = _client.Search<FermentableDto>(s => s
                .From(from)
                .Size(size)
                .Query(q => q.Match(m => m.OnField("name").Query(query))));

            return searchResults.Documents;
        }
    }

}
