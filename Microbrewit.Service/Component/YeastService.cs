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

namespace Microbrewit.Service.Component
{
    public class YeastService : IYeastService
    {
        private static IYeastRepository _yeastRepository;
        private static IYeastElasticsearch _yeastElasticsearch;

        public YeastService(IYeastRepository  yeastRepository,IYeastElasticsearch yeastElasticsearch)
        {
            _yeastRepository = yeastRepository;
            _yeastElasticsearch = yeastElasticsearch;
        }

        public async Task<IEnumerable<YeastDto>> GetAllAsync(string custom)
        {
            var yeastsDto = await _yeastElasticsearch.GetAllAsync(custom);
            if (yeastsDto .Any()) return yeastsDto ;
            var yeasts = await _yeastRepository.GetAllAsync("Supplier");
            yeastsDto = Mapper.Map<IEnumerable<Yeast>, IEnumerable<YeastDto>>(yeasts);
            return yeastsDto;
        }

        public async Task<YeastDto> GetSingleAsync(int id)
        {
            var yeastDto = await _yeastElasticsearch.GetSingleAsync(id);
            if (yeastDto != null) return yeastDto;
            var yeast = await _yeastRepository.GetSingleAsync(y => y.Id == id, "Supplier");
            yeastDto = Mapper.Map<Yeast, YeastDto>(yeast);
            return yeastDto;
        }

        public async Task<YeastDto> AddAsync(YeastDto yeastDto)
        {
            var yeast = Mapper.Map<YeastDto, Yeast>(yeastDto);
            await _yeastRepository.AddAsync(yeast);
            var result = await _yeastRepository.GetSingleAsync(y => y.Id == yeast.Id, "Supplier");
            var mappedResult = Mapper.Map<Yeast,YeastDto>(result);
            await _yeastElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;

        }

        public async Task<YeastDto> DeleteAsync(int id)
        {
            var yeast = await _yeastRepository.GetSingleAsync(y => y.Id == id);
            var yeastDto = await _yeastElasticsearch.GetSingleAsync(id);
            if (yeast != null) await _yeastRepository.RemoveAsync(yeast);
            if (yeastDto != null) await _yeastElasticsearch.DeleteAsync(id);
            return yeastDto;
        }

        public async Task UpdateAsync(YeastDto yeastDto)
        {
            var yeast = Mapper.Map<YeastDto, Yeast>(yeastDto);
            await _yeastRepository.UpdateAsync(yeast);
            var result = await _yeastRepository.GetSingleAsync(h => h.Id == yeastDto.Id, "Supplier");
            var mappedResult = Mapper.Map<Yeast, YeastDto>(result);
            await _yeastElasticsearch.UpdateAsync(mappedResult);
        }

        public async Task<IEnumerable<YeastDto>> SearchAsync(string query, int from, int size)
        {
            return await _yeastElasticsearch.SearchAsync(query, from, size);
        }

        public async Task ReIndexElasticSearch()
        {
            var yeasts = await _yeastRepository.GetAllAsync("Supplier");
            var yeastsDto = Mapper.Map<IEnumerable<Yeast>, IEnumerable<YeastDto>>(yeasts);
            await _yeastElasticsearch.UpdateAllAsync(yeastsDto);
        }
    }
}
