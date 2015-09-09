using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Elasticsearch.Interface
{
    public interface IOriginElasticsearch
    {
        Task UpdateAsync(OriginDto originDto);

        Task<IEnumerable<OriginDto>> GetAllAsync(int from, int size,string custom);
        Task<OriginDto> GetSingleAsync(int id);
        Task<IEnumerable<OriginDto>> SearchAsync(string query, int from, int size);
        Task UpdateAllAsync(IEnumerable<OriginDto> originDtos);
        Task DeleteAsync(int id);
    }
}
