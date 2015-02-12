using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch.Interface;
using Microbrewit.Service.Interface;

namespace Microbrewit.Service.Component
{
    public class UserService : IUserService
    {
        private readonly IUserElasticsearch _userElasticsearch;
        private readonly IUserRepository _userRepository;
       // private readonly IBreweryService _breweryService;
        public UserService(IUserElasticsearch userElasticsearch, IUserRepository userRepository)
        {
            _userElasticsearch = userElasticsearch;
            _userRepository = userRepository;
           
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var userDtos = await _userElasticsearch.GetAllAsync();
            if (userDtos.Any()) return userDtos;
            var users = await _userRepository.GetAllAsync("Breweries.Brewery", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle", "Socials");
            return Mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetSingleAsync(string username)
        {
            var userDto = await _userElasticsearch.GetSingleAsync(username);
            if (userDto != null) return userDto;
            var user = await _userRepository.GetSingleAsync(o => o.Username == username, "Breweries.Brewery", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle", "Socials");
            return Mapper.Map<User, UserDto>(user);
        }

        public async Task<UserDto> AddAsync(UserDto userDto)
        {
            var user = Mapper.Map<UserDto, User>(userDto);
            await _userRepository.AddAsync(user);
            var result = await _userRepository.GetSingleAsync(o => o.Username == user.Username, "Breweries.Brewery", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle", "Socials");
            var mappedResult = Mapper.Map<User, UserDto>(result);
            await _userElasticsearch.UpdateAsync(mappedResult);
            //if (mappedResult.Breweries.Any())
            //    await _breweryService.ReIndexUserRelationElasticSearch(mappedResult);
            return mappedResult;

        }

        public async Task<UserDto> DeleteAsync(string username)
        {
            var user = await _userRepository.GetSingleAsync(o => o.Username == username);
            var userDto = await _userElasticsearch.GetSingleAsync(username);
            if (user != null) await _userRepository.RemoveAsync(user);
            if (userDto == null) return userDto;
            await _userElasticsearch.DeleteAsync(username);
            //if (userDto.Breweries.Any())
            //    await _breweryService.ReIndexUserRelationElasticSearch(userDto);
            return userDto;
        }

        public async Task UpdateAsync(UserDto userDto)
        {
            var user = Mapper.Map<UserDto, User>(userDto);
            await _userRepository.UpdateAsync(user);
            var result = await _userRepository.GetSingleAsync(o => o.Username == userDto.Username, "Breweries.Brewery", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle", "Socials");
            var mappedResult = Mapper.Map<User, UserDto>(result);
            await _userElasticsearch.UpdateAsync(mappedResult);
            //if (mappedResult.Breweries.Any())
            //    await _breweryService.ReIndexUserRelationElasticSearch(mappedResult);
        }

        public async Task<IEnumerable<UserDto>> SearchAsync(string query, int @from, int size)
        {
            return await _userElasticsearch.SearchAsync(query, from, size);
        }

        public async Task ReIndexElasticSearch()
        {
            var users = await _userRepository.GetAllAsync("Breweries.Brewery", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle", "Socials");
            var userDtos = Mapper.Map<IList<User>, IList<UserDto>>(users);
            await _userElasticsearch.UpdateAllAsync(userDtos);
        }

        public async Task ReIndexBeerRelationElasticSearch(BeerDto beerDto)
        {
            foreach (var dtoUser in beerDto.Brewers)
            {
                var user = dtoUser;
                var result = await _userRepository.GetSingleAsync(o => o.Username == user.Username, "Breweries.Brewery", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle", "Socials");
                await _userElasticsearch.UpdateAsync(Mapper.Map<User, UserDto>(result));
            }
        }

        public async Task ReIndexBreweryRelationElasticSearch(BreweryDto brewerDto)
        {
            foreach (var memberDto in brewerDto.Members)
            {
                var member = memberDto;
                var result = await _userRepository.GetSingleAsync(o => o.Username == member.Username, "Breweries.Brewery", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle", "Socials");
                if (result != null)
                    await _userElasticsearch.UpdateAsync(Mapper.Map<User, UserDto>(result));
            }
        }

        public async Task ReIndexUserElasticSearch(string username)
        {
            var result = await _userRepository.GetSingleAsync(o => o.Username == username, "Breweries.Brewery", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle", "Socials");
            if (result != null)
                await _userElasticsearch.UpdateAsync(Mapper.Map<User, UserDto>(result));
        }
    }
}
