using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Interface
{
    public interface IBeerService
    {
        Task<IEnumerable<BeerDto>> GetAllAsync(int from, int size);
        Task<IEnumerable<BeerDto>> GetAllSqlAsync(int from, int size);
        Task<BeerDto> GetSingleAsync(int id);
        Task<BeerDto> AddAsync(BeerDto beerDto);
        Task<BeerDto> AddAsync(BeerDto beerDto, string username);
        Task<BeerDto> DeleteAsync(int id);
        Task UpdateAsync(BeerDto beerDto);
        Task<IEnumerable<BeerDto>> SearchAsync(string query, int from, int size);
        Task<IEnumerable<BeerDto>> GetUserBeersAsync(string username); 
        Task ReIndexElasticSearch(string index);
        Task ReIndexSingleElasticSearchAsync(int beerId);
        Task<IEnumerable<BeerDto>> GetLastAsync(int @from, int size);

        IEnumerable<BeerDto> GetAllUserBeer(string username);
        IEnumerable<BeerDto> GetAllBreweryBeers(int breweryId);
        BeerDto GetSingle(int beerId);
    }
}
