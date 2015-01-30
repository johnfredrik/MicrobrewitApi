﻿using System;
using System.CodeDom;
using System.Collections;
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
    public class BreweryService : IBreweryService
    {
        private static IBreweryRepository _breweryRepository;
        private static IBreweryElasticsearch _breweryElasticsearch;

        public BreweryService(IBreweryRepository breweryRepository, IBreweryElasticsearch breweryElasticsearch)
        {
            _breweryRepository = breweryRepository;
            _breweryElasticsearch = breweryElasticsearch;
        }

        public async Task<IEnumerable<BreweryDto>> GetAllAsync()
        {
            var brewerysDto = await _breweryElasticsearch.GetAllAsync();
            if (brewerysDto .Any()) return brewerysDto ;
            var brewerys = await _breweryRepository.GetAllAsync("Members.Member", "Beers");
            brewerysDto = Mapper.Map<IEnumerable<Brewery>, IEnumerable<BreweryDto>>(brewerys);
            return brewerysDto;
        }

        public async Task<BreweryDto> GetSingleAsync(int id)
        {
            var breweryDto = await _breweryElasticsearch.GetSingleAsync(id);
            if (breweryDto != null) return breweryDto;
            var brewery = await _breweryRepository.GetSingleAsync(y => y.Id == id, "Members.Member", "Beers");
            breweryDto = Mapper.Map<Brewery, BreweryDto>(brewery);
            return breweryDto;
        }

        public async Task<BreweryDto> AddAsync(BreweryDto breweryDto)
        {
            var brewery = Mapper.Map<BreweryDto, Brewery>(breweryDto);
            await _breweryRepository.AddAsync(brewery);
            var result = await _breweryRepository.GetSingleAsync(y => y.Id == brewery.Id, "Members.Member", "Beers");
            var mappedResult = Mapper.Map<Brewery,BreweryDto>(result);
            await _breweryElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;

        }

        public async Task<BreweryDto> DeleteAsync(int id)
        {
            var brewery = await _breweryRepository.GetSingleAsync(y => y.Id == id);
            var breweryDto = await _breweryElasticsearch.GetSingleAsync(id);
            if (brewery != null) await _breweryRepository.RemoveAsync(brewery);
            if (breweryDto != null) await _breweryElasticsearch.DeleteAsync(id);
            return breweryDto;
        }

        public async Task UpdateAsync(BreweryDto breweryDto)
        {
            var brewery = Mapper.Map<BreweryDto, Brewery>(breweryDto);
            await _breweryRepository.UpdateAsync(brewery);
            var result = await _breweryRepository.GetSingleAsync(h => h.Id == breweryDto.Id, "Members.Member", "Beers");
            var mappedResult = Mapper.Map<Brewery, BreweryDto>(result);
            await _breweryElasticsearch.UpdateAsync(mappedResult);
        }

        public async Task<IEnumerable<BreweryDto>> SearchAsync(string query, int from, int size)
        {
            return await _breweryElasticsearch.SearchAsync(query, from, size);
        }

        public async Task ReIndexElasticSearch()
        {
            var brewerys = await _breweryRepository.GetAllAsync("Members.Member", "Beers");
            var brewerysDto = Mapper.Map<IEnumerable<Brewery>, IEnumerable<BreweryDto>>(brewerys);
            await _breweryElasticsearch.UpdateAllAsync(brewerysDto);
        }

        public async Task<BreweryMemberDto> GetBreweryMember(int breweryId, string username)
        {
            var breweryMemberDto = await _breweryElasticsearch.GetSingleMemberAsync(breweryId, username);
            if (breweryMemberDto != null) return breweryMemberDto;
            var breweryMember = await _breweryRepository.GetSingleMemberAsync(breweryId, username);
            return Mapper.Map<BreweryMember, BreweryMemberDto>(breweryMember);
        }

        public async Task<IEnumerable<BreweryMemberDto>> GetAllMembers(int breweryId)
        {
            var breweryMembersDto = await _breweryElasticsearch.GetAllMembersAsync(breweryId);
            if (breweryMembersDto.Any()) return breweryMembersDto;
            var breweryMembers = await _breweryRepository.GetAllMembersAsync(breweryId);
            return Mapper.Map<IList<BreweryMember>, IEnumerable<BreweryMemberDto>>(breweryMembers);
        }

        public async Task<BreweryMemberDto> DeleteMember(int breweryId, string username)
        {
            var breweryMemberDto = await _breweryElasticsearch.GetSingleMemberAsync(breweryId, username);
            await _breweryRepository.DeleteMember(breweryId,username);
            var brewery = await _breweryRepository.GetSingleAsync(b => b.Id == breweryId, "Members.Member", "Beers");
            var breweryDto = Mapper.Map<Brewery, BreweryDto>(brewery);
            await _breweryElasticsearch.UpdateAsync(breweryDto);
            return breweryMemberDto;
        }

        public async Task UpdateBreweryMember(int breweryId,BreweryMemberDto breweryMemberDto)
        {
            var breweryMember = Mapper.Map<BreweryMemberDto, BreweryMember>(breweryMemberDto);
            breweryMember.BreweryId = breweryId;
            await _breweryRepository.UpdateMemberAsync(breweryMember);
            var brewery = await _breweryRepository.GetSingleAsync(b => b.Id == breweryId,"Members.Member","Beers");
            var breweryDto = Mapper.Map<Brewery, BreweryDto>(brewery);
            await _breweryElasticsearch.UpdateAsync(breweryDto);
        }

        public async Task<BreweryMemberDto> AddBreweryMember(int breweryId,BreweryMemberDto breweryMemberDto)
        {
            var breweryMember = Mapper.Map<BreweryMemberDto, BreweryMember>(breweryMemberDto);
            breweryMember.BreweryId = breweryId;
            await _breweryRepository.AddMemberAsync(breweryMember);
            var brewery = await _breweryRepository.GetSingleAsync(b => b.Id == breweryId, "Members.Member", "Beers");
            var breweryDto = Mapper.Map<Brewery, BreweryDto>(brewery);
            await _breweryElasticsearch.UpdateAsync(breweryDto);
            return breweryDto.Members.SingleOrDefault(b => b.Username.Equals(breweryMemberDto.Username));
        }

        public IEnumerable<BreweryMemberDto> GetMemberships(string username)
        {
            var breweryMemberDtos =  _breweryElasticsearch.GetMemberships(username);
            if (breweryMemberDtos != null) return breweryMemberDtos;
            var breweryMemberships = _breweryRepository.GetMemberships(username);
            return Mapper.Map<IList<BreweryMember>, IEnumerable<BreweryMemberDto>>(breweryMemberships);
        }

        public BeerDto GetSingle(int beerId)
        {
            throw new NotImplementedException();
        }
    }
}