using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Repository.Repository;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class FermentableMashStepResolver : ValueResolver<MashStep, IList<FermentableStepDto>>
    {
        private readonly IFermentableElasticsearch _fermentableElasticsearch = new FermentableElasticsearch();
        private readonly IFermentableRepository _fermentableRepository = new FermentableDapperRepository();

        protected override IList<FermentableStepDto> ResolveCore(MashStep step)
        {
            var fermentableStepDtoList = new List<FermentableStepDto>();
                foreach (var item in step.Fermentables)
                {
                    var fermentable = _fermentableElasticsearch.GetSingle(item.FermentableId);
                    if (fermentable == null)
                    {
                        fermentable = Mapper.Map<Fermentable, FermentableDto>(_fermentableRepository.GetSingle(item.FermentableId));
                    }
                    var fermentableStepDto = new FermentableStepDto
                    {
                        FermentableId = item.FermentableId,
                        StepNumber = item.StepNumber,
                        Amount = item.Amount,
                        Supplier = fermentable.Supplier,
                        Type = fermentable.Type,
                        Name = fermentable.Name,
                        PPG = fermentable.PPG,
                        RecipeId = item.RecipeId
                    };
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