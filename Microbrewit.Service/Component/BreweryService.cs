using System;
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
        private static IUserService _userService;

        public BreweryService(IBreweryRepository breweryRepository, IBreweryElasticsearch breweryElasticsearch, IUserService userService)
        {
            _breweryRepository = breweryRepository;
            _breweryElasticsearch = breweryElasticsearch;
            _userService = userService;
        }

        public async Task<IEnumerable<BreweryDto>> GetAllAsync()
        {
            var brewerysDto = await _breweryElasticsearch.GetAllAsync();
            if (brewerysDto .Any()) return brewerysDto ;
            var brewerys = await _breweryRepository.GetAllAsync("Members.Member", "Beers", "Socials","Origin", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle");
            brewerysDto = Mapper.Map<IEnumerable<Brewery>, IEnumerable<BreweryDto>>(brewerys);
            return brewerysDto;
        }

        public async Task<BreweryDto> GetSingleAsync(int id)
        {
            var breweryDto = await _breweryElasticsearch.GetSingleAsync(id);
            //if (breweryDto != null) return breweryDto;
            var brewery = await _breweryRepository.GetSingleAsync(id, "Members.Member","Origin", "Beers", "Socials", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle");
            breweryDto = Mapper.Map<Brewery, BreweryDto>(brewery);
            return breweryDto;
        }

        public async Task<BreweryDto> AddAsync(BreweryDto breweryDto)
        {
            var brewery = Mapper.Map<BreweryDto, Brewery>(breweryDto);
            await _breweryRepository.AddAsync(brewery);
            var result = await _breweryRepository.GetSingleAsync(brewery.BreweryId, "Members.Member", "Origin", "Beers", "Socials", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle");
            var mappedResult = Mapper.Map<Brewery,BreweryDto>(result);
            await _breweryElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;

        }

        public async Task<BreweryDto> DeleteAsync(int id)
        {
            var brewery = await _breweryRepository.GetSingleAsync(id);
            var breweryDto = await _breweryElasticsearch.GetSingleAsync(id);
            if (brewery != null) await _breweryRepository.RemoveAsync(brewery);
            if (breweryDto == null) return breweryDto;
            await _breweryElasticsearch.DeleteAsync(id);
            if(breweryDto.Members.Any()) await _userService.ReIndexBreweryRelationElasticSearch(breweryDto);
            return breweryDto;
        }

        public async Task UpdateAsync(BreweryDto breweryDto)
        {
            var brewery = Mapper.Map<BreweryDto, Brewery>(breweryDto);
            await _breweryRepository.UpdateAsync(brewery);
            var result = await _breweryRepository.GetSingleAsync(breweryDto.Id, "Members.Member", "Origin", "Beers", "Socials", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle");
            var mappedResult = Mapper.Map<Brewery, BreweryDto>(result);
            if (brewery.Members.Any()) await _userService.ReIndexBreweryRelationElasticSearch(mappedResult);
            await _breweryElasticsearch.UpdateAsync(mappedResult);
        }

        public async Task<IEnumerable<BreweryDto>> SearchAsync(string query, int from, int size)
        {
            return await _breweryElasticsearch.SearchAsync(query, from, size);
        }

        public async Task ReIndexElasticSearch()
        {
            var brewerys = await _breweryRepository.GetAllAsync("Members.Member", "Origin", "Beers", "Socials", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle");
            var brewerysDto = Mapper.Map<IEnumerable<Brewery>, IEnumerable<BreweryDto>>(brewerys);
            await _breweryElasticsearch.UpdateAllAsync(brewerysDto);
        }

        public async Task ReIndexBeerRelationElasticSearch(BeerDto beerDto)
        {
            foreach (var dtoBrewery in beerDto.Breweries)
            {
                var brewery = dtoBrewery;
                var result = await _breweryRepository.GetSingleAsync(brewery.Id, "Members.Member", "Origin", "Beers", "Socials", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle");
                var mappedResult = Mapper.Map<Brewery, BreweryDto>(result);
                await _breweryElasticsearch.UpdateAsync(mappedResult);
            }
        }

        public async Task ReIndexUserRelationElasticSearch(UserDto userDto)
        {
            foreach (var breweryDto in userDto.Breweries)
            {
                var brewery = breweryDto;
                var result = await _breweryRepository.GetSingleAsync(brewery.Id, "Members.Member", "Origin", "Beers", "Socials", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle");
                var mappedResult = Mapper.Map<Brewery, BreweryDto>(result);
                await _breweryElasticsearch.UpdateAsync(mappedResult);
            }
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
            var brewery = await _breweryRepository.GetSingleAsync(breweryId, "Members.Member", "Origin", "Beers", "Socials", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle");
            var breweryDto = Mapper.Map<Brewery, BreweryDto>(brewery);
            await _breweryElasticsearch.UpdateAsync(breweryDto);
            await _userService.ReIndexUserElasticSearch(username);
            return breweryMemberDto;
        }

        public async Task UpdateBreweryMember(int breweryId,BreweryMemberDto breweryMemberDto)
        {
            var breweryMember = Mapper.Map<BreweryMemberDto, BreweryMember>(breweryMemberDto);
            breweryMember.BreweryId = breweryId;
            await _breweryRepository.UpdateMemberAsync(breweryMember);
            var brewery = await _breweryRepository.GetSingleAsync(breweryId, "Members.Member", "Origin", "Beers", "Socials", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle");
            var breweryDto = Mapper.Map<Brewery, BreweryDto>(brewery);
            await _breweryElasticsearch.UpdateAsync(breweryDto);
            await _userService.ReIndexUserElasticSearch(breweryMemberDto.Username);
        }

        public async Task<BreweryMemberDto> AddBreweryMember(int breweryId,BreweryMemberDto breweryMemberDto)
        {
            var breweryMember = Mapper.Map<BreweryMemberDto, BreweryMember>(breweryMemberDto);
            breweryMember.BreweryId = breweryId;
            await _breweryRepository.AddMemberAsync(breweryMember);
            var brewery = await _breweryRepository.GetSingleAsync(breweryId, "Members.Member", "Origin", "Beers", "Socials", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle");
            var breweryDto = Mapper.Map<Brewery, BreweryDto>(brewery);
            await _breweryElasticsearch.UpdateAsync(breweryDto);
            await _userService.ReIndexUserElasticSearch(breweryMemberDto.Username);
            return breweryDto.Members.SingleOrDefault(b => b.Username.Equals(breweryMemberDto.Username));
        }

        public IEnumerable<BreweryMember> GetMemberships(string username)
        {
            //var breweryMemberDtos =  _breweryElasticsearch.GetMemberships(username);
            //if (breweryMemberDtos != null) return breweryMemberDtos;
            return _breweryRepository.GetMemberships(username);
        }

        public BeerDto GetSingle(int beerId)
        {
            throw new NotImplementedException();
        }
    }
}
