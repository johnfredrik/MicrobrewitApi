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
using Microbrewit.Service.Interface;

namespace Microbrewit.Service.Component
{
    public class HopService : IHopService
    {
        private IHopRepository _hopRepository;
        private ElasticSearch _elasticsearch;

        public HopService(IHopRepository hopRepository)
        {
            _hopRepository = hopRepository;
            _elasticsearch = new ElasticSearch();
        }
        public async Task<IEnumerable<HopDto>> GetHopsAsync(string custom)
        {
            var hopsDto = await _elasticsearch.GetHops(custom);
            if (hopsDto.Any()) return hopsDto;
            var hops = await _hopRepository.GetAllAsync("Flavours.Flavour", "Origin", "Substituts");
            hopsDto = Mapper.Map<IList<Hop>, IList<HopDto>>(hops);
            return hopsDto;
        }

        public async Task<HopDto> GetHopAsync(int id)
        {
            var hopDto = await _elasticsearch.GetHopAsync(id);
            if (hopDto != null) return hopDto;
            var hop = await _hopRepository.GetSingleAsync(h => h.Id == id, "Flavours.Flavour", "Origin", "Substituts");
            hopDto = Mapper.Map<Hop, HopDto>(hop);
            return hopDto;
        }

        public async Task<HopDto> AddHopAsync(HopDto hopDto)
        {
            var hop = Mapper.Map<HopDto,Hop>(hopDto);
            await _hopRepository.AddAsync(hop);
            var result = await _hopRepository.GetSingleAsync(h => h.Id == hop.Id,"Flavours.Flavour", "Origin", "Substituts");
            var mappedResult = Mapper.Map<Hop, HopDto>(result);
            await _elasticsearch.UpdateHopAsync(mappedResult);
            return mappedResult;
        }

        public async Task<HopDto> DeleteHopAsync(int id)
        {
            var hop = await _hopRepository.GetSingleAsync(h => h.Id == id);
            var hopDto = await _elasticsearch.GetHopAsync(id);
            if(hop != null) await _hopRepository.RemoveAsync(hop);
            if (hopDto != null) await _elasticsearch.DeleteHop(id);
            return hopDto;
        }

        public async Task UpdateHopAsync(HopDto hopDto)
        {
            var hop = Mapper.Map<HopDto, Hop>(hopDto);
            await _hopRepository.UpdateAsync(hop);
            var result = await _hopRepository.GetSingleAsync(h => h.Id == hopDto.Id,"Flavours.Flavour", "Origin", "Substituts");
            var mappedResult = Mapper.Map<Hop, HopDto>(result);
            await _elasticsearch.UpdateHopAsync(mappedResult);
        }

        public async Task<IEnumerable<HopDto>> SearchHop(string query, int from, int size)
        {
            return await _elasticsearch.SearchHops(query, from, size);
        }

        public async Task ReIndexHopsElasticSearch()
        {
            var hops = await _hopRepository.GetAllAsync("Flavours.Flavour", "Origin", "Substituts");
            var hopsDto = Mapper.Map<IList<Hop>, IList<HopDto>>(hops);
            await _elasticsearch.UpdateHopsAsync(hopsDto);
        }

        public async Task<IList<DTO>> GetHopFroms()
        {
            var hopforms = await _hopRepository.GetHopFormsAsync();
            return Mapper.Map<IList<HopForm>, IList<DTO>>(hopforms);
        }
    }
}
