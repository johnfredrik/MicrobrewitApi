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
    public class UserElasticsearch : IUserElasticsearch
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;

        public UserElasticsearch()
        {
            string url = WebConfigurationManager.AppSettings["elasticsearch"];
            this._node = new Uri(url);
            this._settings = new ConnectionSettings(_node, defaultIndex: "mb");
            this._client = new ElasticClient(_settings);
        }

        public async Task<IIndexResponse> UpdateAsync(UserDto userDto)
        {
            // Adds an analayzer to the name property in FermentableDto object.
            await _client.MapAsync<UserDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Username).Analyzer("autocomplete"))));
            return await _client.IndexAsync<UserDto>(userDto);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var result = await _client.SearchAsync<UserDto>(s => s
                                                .Filter(f => f.Term(t => t.DataType, "user"))
                                                .Filter(f => f.Term(t => t.EmailConfirmed, "true"))
                                                .Size(_bigNumber)
                                                );
            return result.Documents;
        }

        public async Task<UserDto> GetSingleAsync(string username)
        {
            IGetRequest getRequest = new GetRequest("mb", "user", username);
            var result = await _client.GetAsync<UserDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<UserDto>> SearchAsync(string query, int @from, int size)
        {
            var searchResults = await _client.SearchAsync<UserDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.OnField(f => f.Username)
                                                                          .Query(query))));

            return searchResults.Documents;
        }

        public async Task<IBulkResponse> UpdateAllAsync(IEnumerable<UserDto> users)
        {
            await _client.MapAsync<UserDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Username).Analyzer("autocomplete"))));
            return await _client.IndexManyAsync(users);
        }

        public Task<IDeleteResponse> DeleteAsync(string username)
        {
            return _client.DeleteAsync<UserDto>(username);
        }
    }
}
