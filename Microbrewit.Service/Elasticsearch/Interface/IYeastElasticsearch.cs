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
    public interface IYeastElasticsearch
    {
        Task UpdateAsync(YeastDto yeastDto);

        Task<IEnumerable<YeastDto>> GetAllAsync(string custom);
        Task<YeastDto> GetSingleAsync(int id);
        Task<IEnumerable<YeastDto>> SearchAsync(string query, int from, int size);
        Task UpdateAllAsync(IEnumerable<YeastDto> yeasts);
        Task DeleteAsync(int id);
        YeastDto GetSingle(int id);
    }
}
