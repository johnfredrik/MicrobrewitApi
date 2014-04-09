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
    public class OtherFermentationStepResolver : ValueResolver<FermentationStep, IList<OtherStepDto>>
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        protected override IList<OtherStepDto> ResolveCore(FermentationStep step)
        {
            using (var redisClient = new RedisClient())
            {
                var otherStepDtoList = new List<OtherStepDto>();
                foreach (var item in step.Others)
                {

                    var otherStepDto = new OtherStepDto()
                    {
                        OtherId = item.OtherId,
                        Amount = item.Amount,
                    };
                    var otherJson = redisClient.GetValueFromHash("others", otherStepDto.OtherId.ToString());
                    var other = JsonConvert.DeserializeObject<OtherDto>(otherJson);
                    otherStepDto.Name = other.Name;
                    otherStepDto.Type = other.Type;
                   

                  

                    
                    otherStepDtoList.Add(otherStepDto);

                }
                return otherStepDtoList;
            }
        }
    }
}