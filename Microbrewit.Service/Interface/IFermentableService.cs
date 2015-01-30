using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Interface
{
    public interface IFermentableService
    {
        Task<IEnumerable<FermentableDto>> GetAllAsync(string custom);
        Task<FermentableDto> GetSingleAsync(int id);
        Task<FermentableDto> AddAsync(FermentableDto fermentableDto);
        Task<FermentableDto> DeleteAsync(int id);
        Task UpdateAsync(FermentableDto fermentableDto);
        Task<IEnumerable<FermentableDto>> SearchAsync(string query, int from, int size);
        Task ReIndexElasticsearch();
    }
}
