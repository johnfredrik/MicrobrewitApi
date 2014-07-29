using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microbrewit.Repository;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("search")]
    public class SearchController : ApiController
    {
        private Elasticsearch.ElasticSearch _elasticsearch;

        public SearchController()
        {
            this._elasticsearch = new Elasticsearch.ElasticSearch();
        }
        [Route("")]
        public async Task<IHttpActionResult> GetAll(string query, int from = 0, int size = 20)
        {
            var result = await _elasticsearch.SearchAll(query,from,size);
            return Ok(JObject.Parse(result));
        }

    }
}
