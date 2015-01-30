using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Interface
{
    public interface IOriginService
    {
        Task<IEnumerable<OriginDto>> GetAllAsync(string custom);
        Task<OriginDto> GetSingleAsync(int id);
        Task<OriginDto> AddAsync(OriginDto originDto);
        Task<OriginDto> DeleteAsync(int id);
        Task UpdateAsync(OriginDto originDto);
        Task<IEnumerable<OriginDto>> SearchAsync(string query, int from, int size);
        Task ReIndexElasticSearch();
    }
}
