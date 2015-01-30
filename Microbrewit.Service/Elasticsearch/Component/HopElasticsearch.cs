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
    public class HopElasticsearch : IHopElasticsearch
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;

        public HopElasticsearch()
        {
            string url = WebConfigurationManager.AppSettings["elasticsearch"];
            Log.Debug("Elasticsearch Url: " + url);
            this._node = new Uri(url);
            this._settings = new ConnectionSettings(_node, defaultIndex: "mb");
            this._client = new ElasticClient(_settings);
        }

        public async Task UpdateAllAsync(IEnumerable<HopDto> hops)
        {
            foreach (var hop in hops)
            {
                UpdateAsync(hop);
            }
        }

        public async Task UpdateAsync(HopDto hop)
        {
            // Adds an analayzer to the name property in FermentableDto object.
            await _client.MapAsync<HopDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            var index = await _client.IndexAsync<HopDto>(hop);
        }

        public async Task<IEnumerable<HopDto>> GetAllAsync(string custom)
        {
            var res =
               await  _client.SearchAsync<HopDto>(
                    s => s
                        .Size(_bigNumber)
                        .Query(q => q
                            .Filtered(fd => fd
                                .Filter(f => f
                                    .Term(h => h.DataType, "hop") && f.Term(p => p.Custom, custom)
                                    ))));
            return res.Documents;
        }

        public async Task<HopDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "hop", id.ToString());
            var result = await _client.GetAsync<HopDto>(getRequest);
            return result.Source;
        }


        public async Task<IEnumerable<HopDto>> SearchAsync(string query, int from, int size)
        {
            var searchResults = await _client.SearchAsync<HopDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Filter(f => f.Term(t => t.DataType, "hop"))
                                                .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                          .Query(query))));

            return searchResults.Documents;
        }

        public async Task DeleteAsync(int id)
        {
            await _client.DeleteAsync<HopDto>(id);
        }

        public HopDto GetSingle(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "hop", id.ToString());
            var result = _client.Get<HopDto>(getRequest);
            return result.Source;
        }
    }
}
