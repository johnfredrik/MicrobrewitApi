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
    public class YeastFermentationStepResolver : ValueResolver<FermentationStep, IList<YeastStepDto>>
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        protected override IList<YeastStepDto> ResolveCore(FermentationStep step)
        {
            using (var redisClient = new RedisClient())
            {
                var yeastStepDtoList = new List<YeastStepDto>();
                foreach (var item in step.Yeasts)
                {

                    var yeastStepDto = new YeastStepDto()
                    {
                        YeastId = item.YeastId,
                        Amount = item.Amount
                    };
                    var yeastJson = redisClient.GetValueFromHash("yeasts", yeastStepDto.YeastId.ToString());
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
}