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
    public class FermentableBoilStepResolver : ValueResolver<BoilStep, IList<FermentableStepDto>>
    {
        private IFermentableElasticsearch _fermentableElasticsearch = new FermentableElasticsearch();
        private IFermentableRepository _fermentableRepository = new FermentableRepository();

        protected override IList<FermentableStepDto> ResolveCore(BoilStep step)
        {
            var fermentableStepDtoList = new List<FermentableStepDto>();
            foreach (var item in step.Fermentables)
            {
                var fermentable = _fermentableElasticsearch.GetSingle(item.FermentableId);
                if (fermentable == null)
                {
                    fermentable = Mapper.Map<Fermentable,FermentableDto>(_fermentableRepository.GetSingle(item.FermentableId));
                }
                var fermentableStepDto = new FermentableStepDto();

                fermentableStepDto.FermentableId = item.FermentableId;
                fermentableStepDto.StepNumber = item.StepNumber;
                fermentableStepDto.Amount = item.Amount;
                fermentableStepDto.Supplier = fermentable.Supplier;
                fermentableStepDto.Type = fermentable.Type;
                fermentableStepDto.Name = fermentable.Name;
                fermentableStepDto.PPG = fermentable.PPG;
                fermentableStepDto.RecipeId = item.RecipeId;
                if (item.Lovibond == 0)
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