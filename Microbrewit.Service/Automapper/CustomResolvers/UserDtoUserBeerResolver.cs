using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class UserDtoUserBeerResolver : ValueResolver<UserDto, IList<UserBeer>>
    {
        protected override IList<UserBeer> ResolveCore(UserDto userDto)
        {
            var userBeers = new List<UserBeer>();
            if (userDto.Beers == null || !userDto.Beers.Any()) return userBeers;
            foreach (var beerDto in userDto.Beers)
            {
                var userBeer = new UserBeer
                {
                    Username = userDto.Username,
                    BeerId = beerDto.Id,
                };
                userBeers.Add(userBeer);
            }
            return userBeers;
        }
    }
}