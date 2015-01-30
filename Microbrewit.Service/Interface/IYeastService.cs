using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Interface
{
    public interface IYeastService
    {
        Task<IEnumerable<YeastDto>> GetAllAsync(string custom);
        Task<YeastDto> GetSingleAsync(int id);
        Task<YeastDto> AddAsync(YeastDto yeastDto);
        Task<YeastDto> DeleteAsync(int id);
        Task UpdateAsync(YeastDto yeastDto);
        Task<IEnumerable<YeastDto>> SearchAsync(string query, int from, int size);
        Task ReIndexElasticSearch();
    }
}
