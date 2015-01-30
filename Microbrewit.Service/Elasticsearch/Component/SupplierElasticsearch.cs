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
    public class SupplierElasticsearch : ISupplierElasticsearch
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;

        public SupplierElasticsearch()
        {
            string url = WebConfigurationManager.AppSettings["elasticsearch"];
            Log.Debug("Elasticsearch Url: " + url);
            this._node = new Uri(url);
            this._settings = new ConnectionSettings(_node, defaultIndex: "mb");
            this._client = new ElasticClient(_settings);
        }

        public async Task UpdateAsync(SupplierDto supplierDto)
        {
            // Adds an analayzer to the name property in FermentableDto object.
            await _client.MapAsync<SupplierDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            var index = await _client.IndexAsync<SupplierDto>(supplierDto);
        }

        public async Task<IEnumerable<SupplierDto>> GetAllAsync(string custom)
        {
            //TODO: add custom to supplier.
            var res = await _client.SearchAsync<SupplierDto>(s => s
                .Size(_bigNumber).Filter(f => f.Term(t => t.DataType, "supplier")));
            return res.Documents;


        }

        public async Task<SupplierDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "supplier", id.ToString());
            var result = await _client.GetAsync<SupplierDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<SupplierDto>> SearchAsync(string query, int @from, int size)
        {
            var searchResults = await _client.SearchAsync<SupplierDto>(s => s
                                               .From(from)
                                               .Size(size)
                                               .Query(q => q.Match(m => m.OnField(f => f.Name)
                                                                         .Query(query))));
            return searchResults.Documents;
        }

        public async Task UpdateAllAsync(IEnumerable<SupplierDto> supplierDtos)
        {
            await _client.MapAsync<SupplierDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            var index = await _client.IndexManyAsync<SupplierDto>(supplierDtos);
        }

        public async Task DeleteAsync(int id)
        {
            await _client.DeleteAsync<SupplierDto>(id);
        }
    }
}
