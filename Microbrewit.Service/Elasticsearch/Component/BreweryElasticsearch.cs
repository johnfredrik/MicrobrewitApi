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
    public class BreweryElasticsearch : IBreweryElasticsearch
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private const int BigNumber = 10000;

        public BreweryElasticsearch()
        {
            string url = WebConfigurationManager.AppSettings["elasticsearch"];
            this._node = new Uri(url);
            this._settings = new ConnectionSettings(_node, defaultIndex: Setting.ElasticSearchIndex);
            this._client = new ElasticClient(_settings);

        }

        public async Task UpdateAsync(BreweryDto breweryDto)
        {
            await _client.MapAsync<BreweryDto>(d => d.Properties(p => p
                .String(s => s.Name(n => n.Name).Analyzer("autocomplete"))
                ));
            await _client.IndexAsync(breweryDto);
        }

        public async Task<IEnumerable<BreweryDto>> GetAllAsync()
        {
            var res = await _client.SearchAsync<BreweryDto>(s => s.Size(BigNumber).Filter(f => f.Term(t => t.DataType, "brewery")));
            return res.Documents;
        }

        public async Task<BreweryDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest(Setting.ElasticSearchIndex, "brewery", id.ToString());
            var result = await _client.GetAsync<BreweryDto>(getRequest);
            return (BreweryDto)result.Source;
        }

        public async Task<IEnumerable<BreweryDto>> SearchAsync(string query, int from, int size)
        {
            var fields = new List<string> { "name" };
            var searchResults = await _client.SearchAsync<BreweryDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.MultiMatch(m => m.OnFields(fields).Query(query))));
            return searchResults.Documents;
        }

        public async Task UpdateAllAsync(IEnumerable<BreweryDto> brewerys)
        {
            await _client.MapAsync<BreweryDto>(d => d.Properties(p => p
               .String(s => s.Name(n => n.Name).Analyzer("autocomplete"))
               //.NestedObject<BreweryMemberDto>(no => no.Name("members"))
               ));
            await _client.IndexManyAsync(brewerys);
        }

        public async Task DeleteAsync(int id)
        {
            await _client.DeleteAsync<BreweryDto>(id);
        }

        public async Task<IEnumerable<BreweryMemberDto>> GetAllMembersAsync(int breweryId)
        {
            var res = await GetSingleAsync(breweryId);
            return res.Members;
        }

        public async Task<BreweryMemberDto> GetSingleMemberAsync(int breweryId, string username)
        {
            var result = await GetSingleAsync(breweryId);
            return result.Members.SingleOrDefault(m => m.Username.Equals(username));
        }

        public IEnumerable<BreweryMemberDto> GetMemberships(string username)
        {
            var breweryDto =_client.Search<BreweryDto>(s => s
                .Size(BigNumber)
                .Query(q => q
                    .Filtered(f => f
                        .Query(qu => qu.MatchAll())
                        .Filter(fi => fi
                            .Nested(n => n
                                .Path("members")
                               .Filter(fl => fl
                                    .Bool(b => b
                                        .Must(m => m
                                            .Term("username",username)
                        ))))))));
            var breweryMemberDtos = from brewery in breweryDto.Documents
                from member in brewery.Members
                where member.Username.Equals(username)
                select member;
            return breweryMemberDtos;
        }
    }

}
