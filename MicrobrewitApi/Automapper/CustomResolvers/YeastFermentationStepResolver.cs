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
    public class YeastFermentationStepResolver : ValueResolver<FermentationStep, IList<YeastStepDto>>
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisStore);

        protected override IList<YeastStepDto> ResolveCore(FermentationStep step)
        {
            var redisClient = redis.GetDatabase();
            
                var yeastStepDtoList = new List<YeastStepDto>();
                foreach (var item in step.Yeasts)
                {

                    var yeastStepDto = new YeastStepDto()
                    {
                        YeastId = item.YeastId,
                        Amount = item.Amount
                    };
                    var yeastJson = redisClient.HashGet("yeasts", yeastStepDto.YeastId);
                    var yeast = JsonConvert.DeserializeObject<YeastDto>(yeastJson);
                    yeastStepDto.Name = yeast.Name;
                    yeastStepDto.Supplier = yeast.Supplier;
                    yeastStepDto.Type = yeast.Type;

                    yeastStepDtoList.Add(yeastStepDto);

                }
                return yeastStepDtoList;
            
        }
    }
}