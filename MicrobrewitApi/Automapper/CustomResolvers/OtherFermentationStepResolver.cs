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
    public class OtherFermentationStepResolver : ValueResolver<FermentationStep, IList<OtherStepDto>>
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisStore);

        protected override IList<OtherStepDto> ResolveCore(FermentationStep step)
        {
            var redisClient = redis.GetDatabase();

            var otherStepDtoList = new List<OtherStepDto>();
            foreach (var item in step.Others)
            {

                var otherStepDto = new OtherStepDto()
                {
                    OtherId = item.OtherId,
                    Amount = item.Amount,
                };
                var otherJson = redisClient.HashGet("others", otherStepDto.OtherId);
                var other = JsonConvert.DeserializeObject<OtherDto>(otherJson);
                otherStepDto.Name = other.Name;
                otherStepDto.Type = other.Type;





                otherStepDtoList.Add(otherStepDto);

            }
            return otherStepDtoList;

        }
    }
}