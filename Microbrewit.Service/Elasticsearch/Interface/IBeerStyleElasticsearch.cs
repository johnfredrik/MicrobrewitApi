using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Microbrewit.Model;
using Microbrewit.Model.BeerXml;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Elasticsearch.Interface
{
    public interface IBeerStyleElasticsearch
    {
        Task UpdateAsync(BeerStyleDto beerStyleDto);
        Task<IEnumerable<BeerStyleDto>> GetAllAsync(int from, int size);
        Task<BeerStyleDto> GetSingleAsync(int id);
        Task<IEnumerable<BeerStyleDto>> SearchAsync(string query, int from, int size);
        Task UpdateAllAsync(IEnumerable<BeerStyleDto> beerStyleDtos);
        Task DeleteAsync(int id);
        BeerStyleDto GetSingle(int id);
        IEnumerable<BeerStyleDto> Search(string query, int from, int size);
    }
}
