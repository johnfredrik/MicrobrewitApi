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
    public class FermentableService : IFermentableService
    {
        private static IFermentableRepository _fermentableRepository;
        private static IFermentableElasticsearch _fermentableElasticsearch;

        public FermentableService(IFermentableRepository fermentableRepository,IFermentableElasticsearch fermentableElasticsearch)
        {
            _fermentableRepository = fermentableRepository;
            _fermentableElasticsearch = fermentableElasticsearch;
        }

        public async Task<IEnumerable<FermentableDto>> GetAllAsync(string custom)
        {
            var fermentableDtos = await _fermentableElasticsearch.GetAllAsync(custom);
            if (fermentableDtos .Any()) return fermentableDtos ;
            var yeasts = await _fermentableRepository.GetAllAsync("Supplier.Origin", "SubFermentables");
            fermentableDtos = Mapper.Map<IEnumerable<Fermentable>, IEnumerable<FermentableDto>>(yeasts);
            return fermentableDtos;
        }

        public async Task<FermentableDto> GetSingleAsync(int id)
        {
            var fermentableDto = await _fermentableElasticsearch.GetSingleAsync(id);
            if (fermentableDto != null) return fermentableDto;
            var fermentable = await _fermentableRepository.GetSingleAsync(id, "Supplier.Origin", "SubFermentables");
            fermentableDto = Mapper.Map<Fermentable, FermentableDto>(fermentable);
            return fermentableDto;
        }

        public async Task<FermentableDto> AddAsync(FermentableDto fermentableDto)
        {
            var fermantable = Mapper.Map<FermentableDto, Fermentable>(fermentableDto);
            await _fermentableRepository.AddAsync(fermantable);
            var result = await _fermentableRepository.GetSingleAsync(fermantable.FermentableId, "Supplier.Origin", "SubFermentables");
            var mappedResult = Mapper.Map<Fermentable,FermentableDto>(result);
            await _fermentableElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;

        }

        public async Task<FermentableDto> DeleteAsync(int id)
        {
            var fermentable = await _fermentableRepository.GetSingleAsync(id);
            var fermentableDto = await _fermentableElasticsearch.GetSingleAsync(id);
            if (fermentable != null) await _fermentableRepository.RemoveAsync(fermentable);
            if (fermentableDto != null) await _fermentableElasticsearch.DeleteAsync(id);
            return fermentableDto;
        }

        public async Task UpdateAsync(FermentableDto fermentableDto)
        {
            var fermentable = Mapper.Map<FermentableDto, Fermentable>(fermentableDto);
            await _fermentableRepository.UpdateAsync(fermentable);
            var result = await _fermentableRepository.GetSingleAsync(fermentableDto.Id, "Supplier.Origin", "SubFermentables");
            var mappedResult = Mapper.Map<Fermentable, FermentableDto>(result);
            await _fermentableElasticsearch.UpdateAsync(mappedResult);
        }

        public async Task<IEnumerable<FermentableDto>> SearchAsync(string query, int from, int size)
        {
            return await _fermentableElasticsearch.SearchAsync(query, from, size);
        }

        public async Task ReIndexElasticsearch()
        {
            var fermentables = await _fermentableRepository.GetAllAsync("Supplier.Origin", "SubFermentables");
            var fermentableDtos = Mapper.Map<IEnumerable<Fermentable>, IEnumerable<FermentableDto>>(fermentables);
            await _fermentableElasticsearch.UpdateAllAsync(fermentableDtos);
        }
    }
}
