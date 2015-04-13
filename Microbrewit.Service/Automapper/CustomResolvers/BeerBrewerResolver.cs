using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class BeerBrewerResolver : ValueResolver<BeerDto, IList<UserBeer>>
    {
        protected override IList<UserBeer> ResolveCore(BeerDto beer)
        {
            var userBeers = new List<UserBeer>();
            if (beer.Brewers == null || !beer.Brewers.Any()) return userBeers;
            foreach (var item in beer.Brewers)
            {
            var userBeer = new UserBeer()
            {
                BeerId = beer.Id,
                Username = item.Username
            };
                userBeers.Add(userBeer);
            }
            return userBeers;
        }
    }
}