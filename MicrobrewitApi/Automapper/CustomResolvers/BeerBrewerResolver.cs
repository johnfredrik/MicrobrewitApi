using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using StackExchange.Redis;
using System.Configuration;
using Microbrewit.Model.DTOs;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections;
using Microbrewit.Model;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class BeerBrewerResolver : ValueResolver<BeerDto, IList<UserBeer>>
    {
        protected override IList<UserBeer> ResolveCore(BeerDto beer)
        {
            var userBeers = new List<UserBeer>();
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