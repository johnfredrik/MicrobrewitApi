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
using Microbrewit.Repository;
namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class OtherFermentationStepResolver : ValueResolver<FermentationStep, IList<OtherStepDto>>
    {
        private Elasticsearch.ElasticSearch _elasticsearch = new Elasticsearch.ElasticSearch();
        private IOtherRepository _otherRepository = new OtherRepository();

        protected override IList<OtherStepDto> ResolveCore(FermentationStep step)
        {
            var otherStepDtoList = new List<OtherStepDto>();
            foreach (var item in step.Others)
            {

                var otherStepDto = new OtherStepDto()
                {
                    OtherId = item.OtherId,
                    Amount = item.Amount,
                    RecipeId = item.RecipeId,
                    Number = item.StepNumber,
                };
                var other = _elasticsearch.GetOther(item.OtherId).Result;
                if (other == null)
                {
                    other = Mapper.Map<Other, OtherDto>(_otherRepository.GetSingle(f => f.Id == item.OtherId));
                }
                otherStepDto.Name = other.Name;
                otherStepDto.Type = other.Type;
         




                otherStepDtoList.Add(otherStepDto);

            }
            return otherStepDtoList;

        }
    }
}