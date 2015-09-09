using System.Web.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microbrewit.Service.Elasticsearch.Interface;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("search")]
    public class SearchController : ApiController
    {
        private ISearchElasticsearch _searchElasticsearch;
        
        public SearchController(ISearchElasticsearch searchElasticsearch)
        {
            _searchElasticsearch = searchElasticsearch;
        }

        /// <summary>
        /// Searches in all things Microbrew.it.
        /// </summary>
        /// <param name="query">The thing you want found.</param>
        /// <param name="from">Start of result returns.</param>
        /// <param name="size">Size of return result.</param>
        /// <returns>All things.</returns>
        [Route("")]
        public async Task<IHttpActionResult> GetAll(string query, int from = 0, int size = 20)
        {
            if (size > 1000) size = 1000;
            var result = await _searchElasticsearch.SearchAllAsync(query, from, size);
            return Ok(JObject.Parse(result));
        }

        /// <summary>
        /// Search in all ingedients in Microberew.it store
        /// </summary>
        /// <param name="query">The thing you want found.</param>
        /// <param name="from">Start of result returns.</param>
        /// <param name="size">Size of return result.</param>
        /// <returns>List of ingredients with score.</returns>
        [Route("ingredients")]
        public async Task<IHttpActionResult> GetAllIngredients(string query, int from = 0, int size = 20)
        {
            if (size > 1000) size = 1000; 
            var result = await _searchElasticsearch.SearchIngredientsAsync(query, from, size);
            return Ok(JObject.Parse(result));
        }
    }
}
