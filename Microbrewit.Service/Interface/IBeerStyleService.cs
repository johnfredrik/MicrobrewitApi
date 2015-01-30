using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Interface
{
    public interface IBeerStyleService
    {
        Task<IEnumerable<BeerStyleDto>> GetAllAsync();
        Task<BeerStyleDto> GetSingleAsync(int id);
        Task<BeerStyleDto> AddAsync(BeerStyleDto beerStyleDto);
        Task<BeerStyleDto> DeleteAsync(int id);
        Task UpdateAsync(BeerStyleDto beerStyleDto);
        Task<IEnumerable<BeerStyleDto>> SearchAsync(string query, int from, int size);
        Task ReIndexElasticSearch();
    }
}
