using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Interface
{
    public interface IOtherService
    {
        Task<IEnumerable<OtherDto>> GetAllAsync(string custom);
        Task<OtherDto> GetSingleAsync(int id);
        Task<OtherDto> AddAsync(OtherDto otherDto);
        Task<OtherDto> DeleteAsync(int id);
        Task UpdateAsync(OtherDto otherDto);
        Task<IEnumerable<OtherDto>> SearchAsync(string query, int from, int size);
        Task ReIndexElasticSearch();
    }
}
