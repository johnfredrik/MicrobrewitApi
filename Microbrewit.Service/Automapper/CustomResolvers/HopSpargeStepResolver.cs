using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class HopSpargeStepResolver : ValueResolver<SpargeStep, IList<HopStepDto>>
    {
        private readonly IHopElasticsearch _hopElasticsearch = new HopElasticsearch();
        private readonly IHopRepository _hopRepository = new HopRepository();

        protected override IList<HopStepDto> ResolveCore(SpargeStep step)
        {
            var hopStepDtoList = new List<HopStepDto>();
            foreach (var item in step.Hops)
            {
                var hopStepDto = new HopStepDto()
                {
                    HopId = item.HopId,
                    StepNumber = item.StepNumber,
                    Amount = item.AaAmount,
                    AAValue = item.AaValue,
                    RecipeId = item.RecipeId,                    
                };
                var hop = _hopElasticsearch.GetSingle(item.HopId) ?? Mapper.Map<Hop, HopDto>(_hopRepository.GetSingle(f => f.Id == item.HopId));
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