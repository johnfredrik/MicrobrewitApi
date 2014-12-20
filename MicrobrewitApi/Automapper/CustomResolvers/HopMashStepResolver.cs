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
    public class HopMashStepResolver : ValueResolver<MashStep, IList<HopStepDto>>
    {
        private Elasticsearch.ElasticSearch _elasticsearch = new Elasticsearch.ElasticSearch();
        private IHopRepository _hopRepository = new HopRepository();

        protected override IList<HopStepDto> ResolveCore(MashStep step)
        {
            var hopStepDtoList = new List<HopStepDto>();
            foreach (var item in step.Hops)
            {

                var hopStepDto = new HopStepDto()
                {
                    HopId = item.HopId,
                    Number = item.StepNumber,
                    Amount = item.AAAmount,
                    AAValue = item.AAValue,
                    RecipeId = item.RecipeId,
                };
                var hop = _elasticsearch.GetHop(item.HopId).Result;
                if (hop == null)
                {
                    hop = Mapper.Map<Hop, HopDto>(_hopRepository.GetSingle(f => f.Id == item.HopId));
                }
                hopStepDto.Name = hop.Name;
                hopStepDto.Origin = hop.Origin;
                hopStepDto.Flavours = hop.Flavours;
                hopStepDto.FlavourDescription = hop.FlavourDescription;
                //TODO: Add elasticsearch on hop form.
                hopStepDto.HopForm = Mapper.Map<HopForm, DTO>(_hopRepository.GetForm(h => h.Id == item.HopFormId));
                hopStepDtoList.Add(hopStepDto);

            }
                return hopStepDtoList;
        }
    }
}