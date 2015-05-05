using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch;
using Microbrewit.Service.Elasticsearch.Interface;
using Microbrewit.Service.Interface;

namespace Microbrewit.Service.Component
{
    public class HopService : IHopService
    {
        private IHopRepository _hopRepository;
        private IHopElasticsearch _hopElasticsearch;

        public HopService(IHopRepository hopRepository,IHopElasticsearch hopElasticsearch)
        {
            _hopRepository = hopRepository;
            _hopElasticsearch = hopElasticsearch;
        }
        public async Task<IEnumerable<HopDto>> GetHopsAsync(string custom)
        {
            var hopsDto = await _hopElasticsearch.GetAllAsync(custom);
            if (hopsDto.Any()) return hopsDto;
            var hops = await _hopRepository.GetAllAsync("Flavours.Flavour", "Origin", "Substituts");
            hopsDto = Mapper.Map<IList<Hop>, IList<HopDto>>(hops);
            return hopsDto;
        }

        public async Task<HopDto> GetSingleAsync(int id)
        {
            var hopDto = await _hopElasticsearch.GetSingleAsync(id);
            if (hopDto != null) return hopDto;
            var hop = await _hopRepository.GetSingleAsync(id, "Flavours.Flavour", "Origin", "Substituts");
            hopDto = Mapper.Map<Hop, HopDto>(hop);
            return hopDto;
        }

        public async Task<HopDto> AddHopAsync(HopDto hopDto)
        {
            var hop = Mapper.Map<HopDto,Hop>(hopDto);
            await _hopRepository.AddAsync(hop);
            var result = await _hopRepository.GetSingleAsync(hop.HopId,"Flavours.Flavour", "Origin", "Substituts");
            var mappedResult = Mapper.Map<Hop, HopDto>(result);
            await _hopElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;
        }

        public async Task<HopDto> DeleteHopAsync(int id)
        {
            var hop = await _hopRepository.GetSingleAsync(id);
            var hopDto = await _hopElasticsearch.GetSingleAsync(id);
            if(hop != null) await _hopRepository.RemoveAsync(hop);
            if (hopDto != null) await _hopElasticsearch.DeleteAsync(id);
            return hopDto;
        }

        public async Task UpdateHopAsync(HopDto hopDto)
        {
            var hop = Mapper.Map<HopDto, Hop>(hopDto);
            await _hopRepository.UpdateAsync(hop);
            var result = await _hopRepository.GetSingleAsync(hopDto.Id,"Flavours.Flavour", "Origin", "Substituts");
            var mappedResult = Mapper.Map<Hop, HopDto>(result);
            await _hopElasticsearch.UpdateAsync(mappedResult);
        }

        public async Task<IEnumerable<HopDto>> SearchHop(string query, int from, int size)
        {
            return await _hopElasticsearch.SearchAsync(query, from, size);
        }

        public async Task ReIndexHopsElasticSearch()
        {
            var hops = await _hopRepository.GetAllAsync("Flavours.Flavour", "Origin", "Substituts");
            var hopsDto = Mapper.Map<IList<Hop>, IList<HopDto>>(hops);
            await _hopElasticsearch.UpdateAllAsync(hopsDto);
        }

        public async Task<IList<DTO>> GetHopFromsAsync()
        {
            var hopforms = await _hopRepository.GetHopFormsAsync();
            return Mapper.Map<IList<HopForm>, IList<DTO>>(hopforms);
        }

        public IEnumerable<DTO> GetHopFroms()
        {
            var hopforms = _hopRepository.GetHopForms();
            return Mapper.Map<IList<HopForm>, IList<DTO>>(hopforms);
        }

        public HopDto GetSingle(int id)
        {
            var hopDto = _hopElasticsearch.GetSingle(id);
            if (hopDto != null) return hopDto;
            var hop = _hopRepository.GetSingle(id, "Flavours.Flavour", "Origin", "Substituts");
            hopDto = Mapper.Map<Hop, HopDto>(hop);
            return hopDto;
        }
    }
}
