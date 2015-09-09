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
    public class BeerElasticsearch : IBeerElasticsearch
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Uri _node;
        private readonly ConnectionSettings _settings;
        private readonly ElasticClient _client;
        private const int BigNumber = 10000;

        public BeerElasticsearch()
        {
            string url = WebConfigurationManager.AppSettings["elasticsearch"];
            this._node = new Uri(url);
            this._settings = new ConnectionSettings(_node, defaultIndex: Setting.ElasticSearchIndex);
            this._client = new ElasticClient(_settings);
        }

        public async Task<IIndexResponse> UpdateAsync(BeerDto beerDto)
        {
            // Adds an analayzer to the name property in FermentableDto object.
            await _client.MapAsync<BeerDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            return await _client.IndexAsync<BeerDto>(beerDto);
        }

        public async Task<IEnumerable<BeerDto>> GetAllAsync(int from, int size)
        {
            var res =
             await _client.SearchAsync<BeerDto>(
                 s => s
                     .From(from)
                     .Size(size)
                     .Filter(f => f.Term(h => h.DataType, "beer")));
            return res.Documents;
        }

        public async Task<BeerDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest(Setting.ElasticSearchIndex, "beer", id.ToString());
            var result = await _client.GetAsync<BeerDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<BeerDto>> SearchAsync(string query, int @from, int size)
        {
            var searchResults = await _client.SearchAsync<BeerDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                          .Query(query))));

            return searchResults.Documents;
        }

        public async Task<IBulkResponse> UpdateAllAsync(IEnumerable<BeerDto> beers)
        {
            await _client.MapAsync<BeerDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            return await _client.IndexManyAsync(beers);
        }

        public Task<IDeleteResponse> DeleteAsync(int id)
        {
            return _client.DeleteAsync<BeerDto>(id);
        }

        public async Task<IEnumerable<BeerDto>> GetLastAsync(int @from, int size)
        {
            var result = await _client.SearchAsync<BeerDto>(s => s
                                                        .Sort(p => p
                                                            .OnField("createdDate")
                                                            .Descending())
                                                        .From(from)
                                                        .Size(size));
            return result.Documents;
        }

        public async Task<IEnumerable<BeerDto>> GetUserBeersAsync(string username)
        {
            var result = await _client.SearchAsync<BeerDto>(s => s
                .Size(BigNumber)
                .Query(q => q
                .Filtered(f => f
                    .Query(q2 => q2
                    .Term("brewers.username", username))
                    .Filter(filter => filter
                        .Term(t => t.DataType, "beer")
                        ))));
            return result.Documents;
        }

        public IEnumerable<BeerDto> GetUserBeers(string username)
        {
            var result = _client.Search<BeerDto>(s => s
                .Size(BigNumber)
                .Query(q => q
                .Filtered(f => f
                    .Query(q2 => q2
                    .Term("brewers.username", username))
                    .Filter(filter => filter
                        .Term(t => t.DataType, "beer")
                        ))));
            return result.Documents;
        }

        public IEnumerable<BeerDto> GetAllBreweryBeers(int breweryId)
        {
            var result = _client.Search<BeerDto>(s => s
                .Size(BigNumber)
                .Query(q => q
                .Filtered(f => f
                    .Query(q2 => q2
                    .Term("breweries.breweryId", breweryId))
                    .Filter(filter => filter
                        .Term(t => t.DataType, "beer")
                        ))));
            return result.Documents;
        }

        public BeerDto GetSingle(int id)
        {
            IGetRequest getRequest = new GetRequest(Setting.ElasticSearchIndex, "beer", id.ToString());
            var result = _client.Get<BeerDto>(getRequest);
            return result.Source;
        }
    }
}
