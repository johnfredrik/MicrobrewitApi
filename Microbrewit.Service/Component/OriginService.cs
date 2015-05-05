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
    public class OriginService : IOriginService
    {
        private IOriginElasticsearch _originElasticsearch;
        private IOriginRespository _originRespository;

        public OriginService(IOriginElasticsearch originElasticsearch, IOriginRespository originRespository)
        {
            _originElasticsearch = originElasticsearch;
            _originRespository = originRespository;
        }

        public async  Task<IEnumerable<OriginDto>> GetAllAsync(string custom)
        {
            var originDtos = await _originElasticsearch.GetAllAsync(custom);
            if (originDtos.Any()) return originDtos;
            var origins = await _originRespository.GetAllAsync();
            return Mapper.Map<IEnumerable<Origin>, IEnumerable<OriginDto>>(origins);
        }

        public async Task<OriginDto> GetSingleAsync(int id)
        {
            var originDto = await _originElasticsearch.GetSingleAsync(id);
            if (originDto != null) return originDto;
            var origin = await _originRespository.GetSingleAsync(id);
            return Mapper.Map<Origin, OriginDto>(origin);
        }

        public async Task<OriginDto> AddAsync(OriginDto otherDto)
        {
            var origin = Mapper.Map<OriginDto, Origin>(otherDto);
            await _originRespository.AddAsync(origin);
            var result = await _originRespository.GetSingleAsync(origin.OriginId);
            var mappedResult = Mapper.Map<Origin, OriginDto>(result);
            await _originElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;

        }

        public async Task<OriginDto> DeleteAsync(int id)
        {
            var origin = await _originRespository.GetSingleAsync(id);
            var originDto = await _originElasticsearch.GetSingleAsync(id);
            if(origin != null) await _originRespository.RemoveAsync(origin);
            if (originDto != null) await _originElasticsearch.DeleteAsync(id);
            return originDto;
        }

        public async Task UpdateAsync(OriginDto originDto)
        {
            var origin = Mapper.Map<OriginDto, Origin>(originDto);
            await _originRespository.UpdateAsync(origin);
            var result = await _originRespository.GetSingleAsync(originDto.Id);
            var mappedResult = Mapper.Map<Origin, OriginDto>(result);
            await _originElasticsearch.UpdateAsync(mappedResult);
        }

        public async Task<IEnumerable<OriginDto>> SearchAsync(string query, int @from, int size)
        {
            return await _originElasticsearch.SearchAsync(query,from,size);
        }

        public async Task ReIndexElasticSearch()
        {
            var origins = await _originRespository.GetAllAsync();
            var originDtos = Mapper.Map<IList<Origin>, IList<OriginDto>>(origins);
            await _originElasticsearch.UpdateAllAsync(originDtos);
        }
    }
}
