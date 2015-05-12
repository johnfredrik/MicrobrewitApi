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
    public class GlassService : IGlassService
    {
        private IGlassElasticsearch _glassElasticsearch;
        private IGlassRepository _glassRepository;

        public GlassService(IGlassElasticsearch glassElasticsearch, IGlassRepository glassRepository)
        {
            _glassElasticsearch = glassElasticsearch;
            _glassRepository = glassRepository;
        }

        public async  Task<IEnumerable<GlassDto>> GetAllAsync()
        {
            var glassDtos = await _glassElasticsearch.GetAllAsync();
            if (glassDtos.Any()) return glassDtos;
            var glasss = await _glassRepository.GetAllAsync();
            return Mapper.Map<IEnumerable<Glass>, IEnumerable<GlassDto>>(glasss);
        }

        public async Task<GlassDto> GetSingleAsync(int id)
        {
            var glassDto = await _glassElasticsearch.GetSingleAsync(id);
            if (glassDto != null) return glassDto;
            var glass = await _glassRepository.GetSingleAsync(id);
            return Mapper.Map<Glass, GlassDto>(glass);
        }

        public async Task<GlassDto> AddAsync(GlassDto glassDto)
        {
            var glass = Mapper.Map<GlassDto, Glass>(glassDto);
            await _glassRepository.AddAsync(glass);
            var result = await _glassRepository.GetSingleAsync(glass.GlassId);
            var mappedResult = Mapper.Map<Glass, GlassDto>(result);
            await _glassElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;

        }

        public async Task<GlassDto> DeleteAsync(int id)
        {
            var glass = await _glassRepository.GetSingleAsync(id);
            var glassDto = await _glassElasticsearch.GetSingleAsync(id);
            if(glass != null) await _glassRepository.RemoveAsync(glass);
            if (glassDto != null) await _glassElasticsearch.DeleteAsync(id);
            return glassDto;
        }

        public async Task UpdateAsync(GlassDto glassDto)
        {
            var glass = Mapper.Map<GlassDto, Glass>(glassDto);
            await _glassRepository.UpdateAsync(glass);
            var result = await _glassRepository.GetSingleAsync(glassDto.Id);
            var mappedResult = Mapper.Map<Glass, GlassDto>(result);
            await _glassElasticsearch.UpdateAsync(mappedResult);
        }

        public async Task<IEnumerable<GlassDto>> SearchAsync(string query, int @from, int size)
        {
            return await _glassElasticsearch.SearchAsync(query,from,size);
        }

        public async Task ReIndexElasticSearch()
        {
            var glasss = await _glassRepository.GetAllAsync();
            var glassDtos = Mapper.Map<IList<Glass>, IList<GlassDto>>(glasss);
            await _glassElasticsearch.UpdateAllAsync(glassDtos);
        }
    }
}
