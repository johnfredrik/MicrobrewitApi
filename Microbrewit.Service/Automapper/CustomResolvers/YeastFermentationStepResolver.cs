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
    public class YeastFermentationStepResolver : ValueResolver<FermentationStep, IList<YeastStepDto>>
    {
        private readonly IYeastElasticsearch _elasticsearch = new YeastElasticsearch();
        private readonly IYeastRepository _yeastRepository = new YeastRepository();

        protected override IList<YeastStepDto> ResolveCore(FermentationStep step)
        {
                var yeastStepDtoList = new List<YeastStepDto>();
                foreach (var item in step.Yeasts)
                {
                    var yeastStepDto = new YeastStepDto()
                    {
                        YeastId = item.YeastId,
                        Amount = item.Amount,
                        RecipeId = item.RecipeId,
                        Number = item.StepNumber,

                    };
                    var yeast = _elasticsearch.GetSingle(item.YeastId);
                    if (yeast == null)
                    {
                        yeast = Mapper.Map<Yeast, YeastDto>(_yeastRepository.GetSingle(item.YeastId));
                    }
                    yeastStepDto.Name = yeast.Name;
                    yeastStepDto.Supplier = yeast.Supplier;
                    yeastStepDto.Type = yeast.Type;
                    yeastStepDto.ProductCode = yeast.ProductCode;

                    yeastStepDtoList.Add(yeastStepDto);

                }
                return yeastStepDtoList;
            
        }
    }
}