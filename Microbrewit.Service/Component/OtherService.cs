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
    public class OtherService : IOtherService
    {
        private IOtherElasticsearch _otherElasticsearch;
        private IOtherRepository _otherRepository;

        public OtherService(IOtherElasticsearch otherElasticsearch, IOtherRepository otherRepository)
        {
            _otherElasticsearch = otherElasticsearch;
            _otherRepository = otherRepository;
        }

        public async  Task<IEnumerable<OtherDto>> GetAllAsync(string custom)
        {
            var otherDtos = await _otherElasticsearch.GetAllAsync(custom);
            if (otherDtos.Any()) return otherDtos;
            var others = await _otherRepository.GetAllAsync();
            return Mapper.Map<IEnumerable<Other>, IEnumerable<OtherDto>>(others);
        }

        public async Task<OtherDto> GetSingleAsync(int id)
        {
            var otherDto = await _otherElasticsearch.GetSingleAsync(id);
            if (otherDto != null) return otherDto;
            var other = await _otherRepository.GetSingleAsync(id);
            return Mapper.Map<Other, OtherDto>(other);
        }

        public async Task<OtherDto> AddAsync(OtherDto otherDto)
        {
            var other = Mapper.Map<OtherDto, Other>(otherDto);
            await _otherRepository.AddAsync(other);
            var result = await _otherRepository.GetSingleAsync(other.Id);
            var mappedResult = Mapper.Map<Other, OtherDto>(result);
            await _otherElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;

        }

        public async Task<OtherDto> DeleteAsync(int id)
        {
            var other = await _otherRepository.GetSingleAsync(id);
            var otherDto = await _otherElasticsearch.GetSingleAsync(id);
            if(other != null) await _otherRepository.RemoveAsync(other);
            if (otherDto != null) await _otherElasticsearch.DeleteAsync(id);
            return otherDto;
        }

        public async Task UpdateAsync(OtherDto otherDto)
        {
            var other = Mapper.Map<OtherDto, Other>(otherDto);
            await _otherRepository.UpdateAsync(other);
            var result = await _otherRepository.GetSingleAsync(otherDto.Id);
            var mappedResult = Mapper.Map<Other, OtherDto>(result);
            await _otherElasticsearch.UpdateAsync(mappedResult);
        }

        public async Task<IEnumerable<OtherDto>> SearchAsync(string query, int @from, int size)
        {
            return await _otherElasticsearch.SearchAsync(query,from,size);
        }

        public async Task ReIndexElasticSearch()
        {
            var others = await _otherRepository.GetAllAsync();
            var otherDtos = Mapper.Map<IList<Other>, IList<OtherDto>>(others);
            await _otherElasticsearch.UpdateAllAsync(otherDtos);
        }
    }
}
