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
    public interface IHopElasticsearch
    {
        Task UpdateAsync(HopDto hopDto);
        Task<IEnumerable<HopDto>> GetAllAsync(string custom);
        Task<HopDto> GetSingleAsync(int id);
        Task<IEnumerable<HopDto>> SearchAsync(string query, int from, int size);
        Task UpdateAllAsync(IEnumerable<HopDto> hops);
        Task DeleteAsync(int id);
        HopDto GetSingle(int hopId);
        IEnumerable<HopDto> Search(string query, int from, int size);
    }
}
