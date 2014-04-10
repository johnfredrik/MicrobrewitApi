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
    public class OtherMashStepResolver : ValueResolver<MashStep, IList<OtherStepDto>>
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        protected override IList<OtherStepDto> ResolveCore(MashStep step)
        {
            using (var redisClient = new RedisClient(redisStore))
            {
                var otherStepDtoList = new List<OtherStepDto>();
                foreach (var item in step.Others)
                {

                    var otherStepDto = new OtherStepDto()
                    {
                        OtherId = item.OtherId,
                        Amount = item.Amount,
                    };
                    var otherJson = redisClient.GetValueFromHash("Others", otherStepDto.OtherId.ToString());
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