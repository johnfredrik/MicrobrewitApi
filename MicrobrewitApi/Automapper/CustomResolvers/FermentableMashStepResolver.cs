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
    public class FermentableMashStepResolver : ValueResolver<MashStep, IList<FermentableStepDto>>
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        protected override IList<FermentableStepDto> ResolveCore(MashStep step)
        {
            var fermentableStepDtoList = new List<FermentableStepDto>();
            using (var redisClient = new RedisClient(redisStore))
            {

                foreach (var item in step.Fermentables)
                {
                    var fermJson = redisClient.GetValueFromHash("fermentables", item.FermentableId.ToString());
                    var fermentable = JsonConvert.DeserializeObject<FermentableDto>(fermJson);
                    var fermentableStepDto = new FermentableStepDto();
                    
                    fermentableStepDto.FermentableId = item.FermentableId;
                    fermentableStepDto.StepId = item.MashStepId;
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
            }
            return fermentableStepDtoList;
        }

    }
}