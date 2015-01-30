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
    public interface IOtherElasticsearch
    {
        Task<IIndexResponse> UpdateAsync(OtherDto otherDto);
        Task<IEnumerable<OtherDto>> GetAllAsync(string custom);
        Task<OtherDto> GetSingleAsync(int id);
        Task<IEnumerable<OtherDto>> SearchAsync(string query, int from, int size);
        Task<IBulkResponse> UpdateAllAsync(IEnumerable<OtherDto> others);
        Task<IDeleteResponse> DeleteAsync(int id);
        OtherDto GetSingle(int id);
    }
}
