using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Interface
{
    public interface IGlassService
    {
        Task<IEnumerable<GlassDto>> GetAllAsync();
        Task<GlassDto> GetSingleAsync(int id);
        Task<GlassDto> AddAsync(GlassDto glassDto);
        Task<GlassDto> DeleteAsync(int id);
        Task UpdateAsync(GlassDto glassDto);
        Task<IEnumerable<GlassDto>> SearchAsync(string query, int from, int size);
        Task ReIndexElasticSearch();
    }
}
