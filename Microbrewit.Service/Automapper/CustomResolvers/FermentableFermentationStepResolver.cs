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
    public class FermentableFermentationStepResolver : ValueResolver<FermentationStep, IList<FermentableStepDto>>
    {
        private readonly IFermentableElasticsearch _fermentableElasticsearch = new FermentableElasticsearch();
        private readonly IFermentableRepository _fermentableRepository = new FermentableDapperRepository();

        protected override IList<FermentableStepDto> ResolveCore(FermentationStep step)
        {
            var fermentableStepDtoList = new List<FermentableStepDto>();
            

                foreach (var item in step.Fermentables)
                {
                    var fermentable = _fermentableElasticsearch.GetSingle(item.FermentableId);
                    if (fermentable == null)
                    {
                        fermentable = Mapper.Map<Fermentable, FermentableDto>(_fermentableRepository.GetSingle(item.FermentableId));
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
                    fermentableStepDto.StepNumber = item.StepNumber;
                        
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