using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ServiceStack.Redis;
using System.Configuration;
using Microbrewit.Model.DTOs;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections;
using Microbrewit.Model;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class BeerStyleResolver : ValueResolver<Beer,DTO>
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        protected override DTO ResolveCore(Beer beer)
        {
            using (var redisClient = new RedisClient(redisStore))
            {
                var dto = new DTO();
                var beerStyleJson = redisClient.GetValueFromHash("beerstyles", beer.BeerStyleId.ToString());
                var beerStyle = JsonConvert.DeserializeObject<BeerStyle>(beerStyleJson);
                dto.Id = beerStyle.Id;
                dto.Name = beerStyle.Name;

                return dto;
            }
        }
    }
}