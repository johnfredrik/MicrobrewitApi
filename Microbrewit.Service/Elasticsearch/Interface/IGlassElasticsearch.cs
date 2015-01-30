using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Nest;

namespace Microbrewit.Service.Elasticsearch.Interface
{
    public interface IGlassElasticsearch
    {
        Task<IIndexResponse> UpdateAsync(GlassDto glassDto);
        Task<IEnumerable<GlassDto>> GetAllAsync();
        Task<GlassDto> GetSingleAsync(int id);
        Task<IEnumerable<GlassDto>> SearchAsync(string query, int from, int size);
        Task<IBulkResponse> UpdateAllAsync(IEnumerable<GlassDto> glasss);
        Task<IDeleteResponse> DeleteAsync(int id);
    }
}
