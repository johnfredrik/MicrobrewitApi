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
        private readonly IBreweryRepository _breweryRepository;
        private readonly IBeerRepository _beerRepository;
        private readonly IBeerElasticsearch _beerElasticsearch;
        private readonly IBreweryElasticsearch _breweryElasticsearch;
        private string[] _breweryInclude = { "Members.Member","Origin", "Beers", "Socials", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle"};
        private string[] _userInclude =
        {
            "Breweries.Brewery", 
            "Beers.Beer.IBU", 
            "Beers.Beer.ABV", 
            "Beers.Beer.SRM",
            "Beers.Beer.BeerStyle", 
            "Socials"
        };
        private string[] _beerInclude =  {
                "Recipe.SpargeStep",
                "Recipe.MashSteps.Hops",
                "Recipe.MashSteps.Fermentables",
                "Recipe.MashSteps.Others",
                "Recipe.BoilSteps.Hops",
                "Recipe.BoilSteps.Fermentables",
                "Recipe.BoilSteps.Others",
                "Recipe.FermentationSteps.Hops",
                "Recipe.FermentationSteps.Fermentables",
                "Recipe.FermentationSteps.Others",
                "Recipe.FermentationSteps.Yeasts",
                "Forks.ABV",
                "Forks.BeerStyle",
                "Forks.IBU",
                "Forks.SRM",
                "ABV", "IBU", "SRM", "Brewers.User", "Breweries"};

        public UserService(IUserElasticsearch userElasticsearch, IUserRepository userRepository, IBreweryElasticsearch breweryElasticsearch, IBreweryRepository breweryRepository, IBeerRepository beerRepository, IBeerElasticsearch beerElasticsearch)
        {
            _userElasticsearch = userElasticsearch;
            _userRepository = userRepository;
            _breweryElasticsearch = breweryElasticsearch;
            _breweryRepository = breweryRepository;
            _beerRepository = beerRepository;
            _beerElasticsearch = beerElasticsearch;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var userDtos = await _userElasticsearch.GetAllAsync();
            if (userDtos.Any()) return userDtos;
            var users = await _userRepository.GetAllAsync(_userInclude);
            return Mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetSingleAsync(string username)
        {
            var userDto = await _userElasticsearch.GetSingleAsync(username);
            if (userDto != null) return userDto;
            var user = await _userRepository.GetSingleAsync(username, _userInclude);
            return Mapper.Map<User, UserDto>(user);
        }

        public async Task<UserDto> AddAsync(UserDto userDto)
        {
            var user = Mapper.Map<UserDto, User>(userDto);
            await _userRepository.AddAsync(user);
            var result = await _userRepository.GetSingleAsync(user.Username, _userInclude);
            var mappedResult = Mapper.Map<User, UserDto>(result);
            await _userElasticsearch.UpdateAsync(mappedResult);
            if (mappedResult.Breweries.Any())
                await ReIndexUserRelationElasticSearch(mappedResult);
            return mappedResult;

        }

        public async Task<UserDto> DeleteAsync(string username)
        {
            var user = await _userRepository.GetSingleAsync(username);
            var userDto = await _userElasticsearch.GetSingleAsync(username);
            if (user != null) await _userRepository.RemoveAsync(user);
            if (userDto == null) return userDto;
            await _userElasticsearch.DeleteAsync(username);
            if (userDto.Breweries.Any())
                await ReIndexUserRelationElasticSearch(userDto);
            return userDto;
        }

        public async Task UpdateAsync(UserDto userDto)
        {
            var user = Mapper.Map<UserDto, User>(userDto);
            await _userRepository.UpdateAsync(user);
            var result = await _userRepository.GetSingleAsync(userDto.Username, _userInclude);
            var mappedResult = Mapper.Map<User, UserDto>(result);
            await _userElasticsearch.UpdateAsync(mappedResult);
            if (mappedResult.Breweries.Any())
                await ReIndexUserRelationElasticSearch(mappedResult);
        }

        public async Task<IEnumerable<UserDto>> SearchAsync(string query, int @from, int size)
        {
            return await _userElasticsearch.SearchAsync(query, from, size);
        }

        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string username)
        {
            var notifications = new List<NotificationDto>();
            var memberships = _breweryRepository.GetMemberships(username);
            foreach (var membership in memberships)
            {
                if (!membership.Confirmed)
                {
                    var notification = new NotificationDto
                    {
                        NotificationId = membership.BreweryId,
                        Type = "New Membership",
                        Message = "Added to a new brewery"
                    };
                    notifications.Add(notification);
                }
            }
            var userBeers = await _userRepository.GetAllUserBeersAsync(username);
            foreach (var userBeer in userBeers)
            {
                if (!userBeer.Confirmed)
                {
                    var notification = new NotificationDto
                    {
                        NotificationId = userBeer.BeerId,
                        Type = "New Beer",
                        Message = "Added to a new beer"
                    };
                    notifications.Add(notification);
                }
            }
            return notifications;
        }

        public async Task ReIndexElasticSearch()
        {
            var users = await _userRepository.GetAllAsync(_userInclude);
            var userDtos = Mapper.Map<IList<User>, IList<UserDto>>(users);
            await _userElasticsearch.UpdateAllAsync(userDtos);
        }

        public async Task ReIndexBeerRelationElasticSearch(BeerDto beerDto)
        {
            foreach (var dtoUser in beerDto.Brewers)
            {
                var user = dtoUser;
                var result = await _userRepository.GetSingleAsync(user.Username, _userInclude);
                await _userElasticsearch.UpdateAsync(Mapper.Map<User, UserDto>(result));
            }
        }

        public async Task ReIndexBreweryRelationElasticSearch(BreweryDto brewerDto)
        {
            foreach (var memberDto in brewerDto.Members)
            {
                var member = memberDto;
                var result = await _userRepository.GetSingleAsync(member.Username, _userInclude);
                if (result != null)
                    await _userElasticsearch.UpdateAsync(Mapper.Map<User, UserDto>(result));
            }
        }

        public async Task ReIndexUserElasticSearch(string username)
        {
            var result = await _userRepository.GetSingleAsync(username, _userInclude);
            if (result != null)
                await _userElasticsearch.UpdateAsync(Mapper.Map<User, UserDto>(result));
        }

        public async Task<bool> UpdateNotification(string username, NotificationDto notificationDto)
        {
            if (notificationDto.Type == "BreweryMember")
            {
                var changed = await _userRepository.ConfirmBreweryMemberAsync(username, notificationDto);
                if (!changed) return false;
                await ReIndexUserElasticSearch(username);
                var brewery = await _breweryRepository.GetSingleAsync(notificationDto.Id, _breweryInclude);
                if (brewery == null) return false;
                var breweryDto = Mapper.Map<Brewery,BreweryDto>(brewery);
                await _breweryElasticsearch.UpdateAsync(breweryDto);
                return true;
            }
            if (notificationDto.Type == "UserBeer")
            {
                var changed = await _userRepository.ConfirmUserBeerAsync(username, notificationDto);
                if (!changed) return false;
                await ReIndexUserElasticSearch(username);
                var beer = await _beerRepository.GetSingleAsync(b => b.Id == notificationDto.Id, _beerInclude);
                if (beer == null) return false;
                var beerDto = Mapper.Map<Beer, BeerDto>(beer);
                await _beerElasticsearch.UpdateAsync(beerDto);
                return true;
            }
            return false;
        }

        private async Task ReIndexUserRelationElasticSearch(UserDto userDto)
        {
            foreach (var breweryDto in userDto.Breweries)
            {
                var brewery = breweryDto;
                var result = await _breweryRepository.GetSingleAsync(brewery.Id, "Members.Member", "Origin", "Beers", "Socials", "Beers.Beer.IBU", "Beers.Beer.ABV", "Beers.Beer.SRM", "Beers.Beer.BeerStyle");
                var mappedResult = Mapper.Map<Brewery, BreweryDto>(result);
                await _breweryElasticsearch.UpdateAsync(mappedResult);
            }
        }
    }
}
