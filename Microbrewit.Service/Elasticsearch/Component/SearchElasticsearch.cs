using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Elasticsearch.Net;
using Elasticsearch.Net.Connection;
using log4net;
using Microbrewit.Service.Elasticsearch.Interface;
using Nest;

namespace Microbrewit.Service.Elasticsearch.Component
{
    public class SearchElasticsearch : ISearchElasticsearch
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;
        private readonly string _url ;

        public SearchElasticsearch()
        {
            _url = WebConfigurationManager.AppSettings["elasticsearch"];
            this._node = new Uri(_url);
            this._settings = new ConnectionSettings(_node, defaultIndex: "mb");
            this._client = new ElasticClient(_settings);
        }


        public async Task<string> SearchAllAsync(string query, int @from, int size)
        {
            var client = new ElasticsearchClient(_settings);
            var queryString = "{\"from\" : " + from + ", \"size\" : " + size + ", \"query\":{\"match\": {\"name\": {\"query\": \" " + query + " \",\"operator\": \"and\"}}}}";
            var res = await client.SearchAsync<string>("mb",queryString);
            return res.Response;
        }

        public async Task<string> SearchIngredientsAsync(string query, int @from, int size)
        {
            var client = new ElasticsearchClient(_settings);
            
            var queryString = "{\"from\": " + from +", \"size\": " + size +", \"filter\": { \"or\": [{\"term\": { \"dataType\": \"hop\"}},{\"term\": {\"dataType\": \"fermentable\"}},{\"term\": {\"dataType\": \"yeast\"}},{\"term\": {\"dataType\": \"other\"}}]},\"query\": {\"match\": {\"name\": {\"query\": \"" + query +"\"}}}}";
            var res = await client.SearchAsync<string>("mb", queryString);
            return res.Response;
        }
    }
}
