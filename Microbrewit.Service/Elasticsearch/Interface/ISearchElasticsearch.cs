using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Service.Elasticsearch.Interface
{
    public interface ISearchElasticsearch
    {
        Task<string> SearchAllAsync(string query, int from, int size);
        Task<string> SearchIngredientsAsync(string query, int from, int size);
    }
}
