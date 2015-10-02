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
    public class HopBoilStepResolver : ValueResolver<BoilStep, IList<HopStepDto>>
    {
        private IHopElasticsearch _hopElasticsearch = new HopElasticsearch();
        private IHopRepository _hopRepository = new HopDapperRepository();


        protected override IList<HopStepDto> ResolveCore(BoilStep step)
        {
                var hopStepDtoList = new List<HopStepDto>();
                foreach (var item in step.Hops)
                {

                    var hopStepDto = new HopStepDto()
                    {
                        HopId = item.HopId,
                        StepNumber = item.StepNumber,
                        Amount = item.Amount,
                        AAValue = item.AAValue,
                        RecipeId = item.RecipeId
                    };
                    var hop = _hopElasticsearch.GetSingle(item.HopId);
                    if (hop == null)
                    {
                        hop = Mapper.Map<Hop, HopDto>(_hopRepository.GetSingle(item.HopId));
                    }
                    hopStepDto.Name = hop.Name;
                    hopStepDto.Origin = hop.Origin;
                    //hopStepDto.Flavours = hop.Flavours;
                    //hopStepDto.FlavourDescription = hop.FlavourDescription;
                    //TODO: Add elasticsearch on hop form.
                    hopStepDto.HopForm = Mapper.Map<HopForm,DTO>(_hopRepository.GetForm(item.HopFormId));
                    hopStepDtoList.Add(hopStepDto);

                }
                return hopStepDtoList;
            
        }
    }
}