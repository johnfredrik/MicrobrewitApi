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
    public class HopFermentationStepResolver : ValueResolver<FermentationStep, IList<HopStepDto>>
    {
        private readonly IHopElasticsearch _hopElasticsearch = new HopElasticsearch();
        private readonly IHopRepository _hopRepository = new HopRepository();

        protected override IList<HopStepDto> ResolveCore(FermentationStep step)
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
                    var hop = _hopElasticsearch.GetSingle(item.HopId);
                    if (hop == null)
                    {
                        hop = Mapper.Map<Hop, HopDto>(_hopRepository.GetSingle(f => f.Id == item.HopId));
                    }
                    hopStepDto.Name = hop.Name;
                    hopStepDto.Origin = hop.Origin;
                    hopStepDto.Flavours = hop.Flavours;
                    hopStepDto.FlavourDescription = hop.FlavourDescription;
                    hopStepDto.HopForm = Mapper.Map<HopForm, DTO>(_hopRepository.GetForm(h => h.Id == item.HopFormId));
                    hopStepDtoList.Add(hopStepDto);
                }
                return hopStepDtoList;
            
        }
    }
}