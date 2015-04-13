using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using log4net;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Elasticsearch.Interface;
using Nest;

namespace Microbrewit.Service.Elasticsearch.Component
{
    public class OtherElasticsearch : IOtherElasticsearch
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;

        public OtherElasticsearch()
        {
            string url = WebConfigurationManager.AppSettings["elasticsearch"];
            this._node = new Uri(url);
            this._settings = new ConnectionSettings(_node, defaultIndex: "mb");
            this._client = new ElasticClient(_settings);
        }

        public async Task<IIndexResponse> UpdateAsync(OtherDto otherDto)
        {
            // Adds an analayzer to the name property in FermentableDto object.
            await _client.MapAsync<OtherDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            return await _client.IndexAsync<OtherDto>(otherDto);
        }

        public async Task<IEnumerable<OtherDto>> GetAllAsync(string custom)
        {
            var res =
             await _client.SearchAsync<OtherDto>(
                 s => s
                     .Size(_bigNumber)
                     .Filter(f => f.Term(h => h.DataType, "other") && f.Term(p => p.Custom, custom)));
            return res.Documents;
        }

        public async Task<OtherDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "other", id.ToString());
            var result = await _client.GetAsync<OtherDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<OtherDto>> SearchAsync(string query, int @from, int size)
        {
            var searchResults = await _client.SearchAsync<OtherDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                          .Query(query))));

            return searchResults.Documents;
        }

        public async Task<IBulkResponse> UpdateAllAsync(IEnumerable<OtherDto> others)
        {
            await _client.MapAsync<OtherDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            return await _client.IndexManyAsync(others);
        }

        public Task<IDeleteResponse> DeleteAsync(int id)
        {
            return _client.DeleteAsync<OtherDto>(id);
        }

        public OtherDto GetSingle(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "other", id.ToString());
            var result = _client.Get<OtherDto>(getRequest);
            return result.Source;
        }

        public IEnumerable<OtherDto> Search(string query, int @from, int size)
        {
            var searchResults =  _client.Search<OtherDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                          .Query(query))));

            return searchResults.Documents;
        }
    }
}
