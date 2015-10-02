using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Nest;

namespace Microbrewit.Service.Elasticsearch.Interface
{
    public interface IBeerElasticsearch
    {
        Task<IIndexResponse> UpdateAsync(BeerDto beerDto);
        Task<IEnumerable<BeerDto>> GetAllAsync(int from, int size);
        Task<BeerDto> GetSingleAsync(int id);
        Task<IEnumerable<BeerDto>> SearchAsync(string query, int from, int size);
        Task<IBulkResponse> UpdateAllAsync(IEnumerable<BeerDto> beers);
        Task<IBulkResponse> ReIndexBulk(IEnumerable<BeerDto> beers, string index);
        Task<IDeleteResponse> DeleteAsync(int id);
        Task<IEnumerable<BeerDto>> GetLastAsync(int from, int size);
        Task<IEnumerable<BeerDto>> GetUserBeersAsync(string username);
        IEnumerable<BeerDto>  GetUserBeers(string username);
        IEnumerable<BeerDto> GetAllBreweryBeers(int breweryId);
        BeerDto GetSingle(int id);
    }
}
