using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch.Interface;
using Microbrewit.Service.Interface;
using Nest;

namespace Microbrewit.Service.Component
{
    public class BeerStyleService : IBeerStyleService
    {
        private IBeerStyleElasticsearch _beerStyleElasticsearch;
        private IBeerStyleRepository _beerStyleRepository;

        public BeerStyleService(IBeerStyleElasticsearch beerStyleElasticsearch, IBeerStyleRepository beerStyleRepository)
        {
            _beerStyleElasticsearch = beerStyleElasticsearch;
            _beerStyleRepository = beerStyleRepository;
        }

        public async  Task<IEnumerable<BeerStyleDto>> GetAllAsync(int from, int size)
        {
            var beerStyleDtos = await _beerStyleElasticsearch.GetAllAsync(from,size);
            if (beerStyleDtos.Any()) return beerStyleDtos;
            var beerStyles = await _beerStyleRepository.GetAllAsync(from,size,"SubStyles", "SuperStyle");
            return Mapper.Map<IEnumerable<BeerStyle>, IEnumerable<BeerStyleDto>>(beerStyles);
        }

        public async Task<BeerStyleDto> GetSingleAsync(int id)
        {
            var beerStyleDto = await _beerStyleElasticsearch.GetSingleAsync(id);
            if (beerStyleDto != null) return beerStyleDto;
            var beerStyle = await _beerStyleRepository.GetSingleAsync(id, "SubStyles", "SuperStyle");
            return Mapper.Map<BeerStyle, BeerStyleDto>(beerStyle);
        }

        public async Task<BeerStyleDto> AddAsync(BeerStyleDto otherDto)
        {
            var beerStyle = Mapper.Map<BeerStyleDto, BeerStyle>(otherDto);
            await _beerStyleRepository.AddAsync(beerStyle);
            var result = await _beerStyleRepository.GetSingleAsync(beerStyle.BeerStyleId, "SubStyles", "SuperStyle");
            var mappedResult = Mapper.Map<BeerStyle, BeerStyleDto>(result);
            await _beerStyleElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;

        }

        public async Task<BeerStyleDto> DeleteAsync(int id)
        {
            var beerStyle = await _beerStyleRepository.GetSingleAsync(id);
            var beerStyleDto = await _beerStyleElasticsearch.GetSingleAsync(id);
            if(beerStyle != null) await _beerStyleRepository.RemoveAsync(beerStyle);
            if (beerStyleDto != null) await _beerStyleElasticsearch.DeleteAsync(id);
            return beerStyleDto;
        }

        public async Task UpdateAsync(BeerStyleDto beerStyleDto)
        {
            var beerStyle = Mapper.Map<BeerStyleDto, BeerStyle>(beerStyleDto);
            await _beerStyleRepository.UpdateAsync(beerStyle);
            var result = await _beerStyleRepository.GetSingleAsync(beerStyleDto.Id, "SubStyles", "SuperStyle");
            var mappedResult = Mapper.Map<BeerStyle, BeerStyleDto>(result);
            await _beerStyleElasticsearch.UpdateAsync(mappedResult);
        }

        public async Task<IEnumerable<BeerStyleDto>> SearchAsync(string query, int @from, int size)
        {
            return await _beerStyleElasticsearch.SearchAsync(query,from,size);
        }

        public async Task ReIndexElasticSearch()
        {
            var beerStyles = await _beerStyleRepository.GetAllAsync(0,int.MaxValue,"SubStyles", "SuperStyle");
            var beerStyleDtos = Mapper.Map<IList<BeerStyle>, IList<BeerStyleDto>>(beerStyles);
            await _beerStyleElasticsearch.UpdateAllAsync(beerStyleDtos);
        }
    }
}
