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
    public class FermentableMashStepResolver : ValueResolver<MashStep, IList<FermentableStepDto>>
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisStore);

        protected override IList<FermentableStepDto> ResolveCore(MashStep step)
        {
            var fermentableStepDtoList = new List<FermentableStepDto>();
            var redisClient = redis.GetDatabase();
            

                foreach (var item in step.Fermentables)
                {
                    var fermJson = redisClient.HashGet("fermentables", item.FermentableId);
                    var fermentable = JsonConvert.DeserializeObject<FermentableDto>(fermJson);
                    var fermentableStepDto = new FermentableStepDto();
                    
                    fermentableStepDto.FermentableId = item.FermentableId;
                    fermentableStepDto.StepId = item.StepId;
                    fermentableStepDto.Amount = item.Amount;
                    fermentableStepDto.Supplier = fermentable.Supplier;
                    fermentableStepDto.Type = fermentable.Type;
                    fermentableStepDto.Name = fermentable.Name;
                    fermentableStepDto.PPG = fermentable.PPG;
                    
                    if(item.Lovibond == 0)
                    {
                        fermentableStepDto.Lovibond = item.Lovibond;
                    }
                    else
                    {
                        fermentableStepDto.Lovibond = fermentable.Lovibond;
                    }
                    fermentableStepDtoList.Add(fermentableStepDto);
                }
            
            return fermentableStepDtoList;
        }

    }
}