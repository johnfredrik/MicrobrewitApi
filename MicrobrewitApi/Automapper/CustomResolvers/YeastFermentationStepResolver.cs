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
    public class YeastFermentationStepResolver : ValueResolver<FermentationStep, IList<YeastStepDto>>
    {
        private Elasticsearch.ElasticSearch _elasticsearch = new Elasticsearch.ElasticSearch();
        private IYeastRepository _yeastRepository = new YeastRepository();

        protected override IList<YeastStepDto> ResolveCore(FermentationStep step)
        {
                var yeastStepDtoList = new List<YeastStepDto>();
                foreach (var item in step.Yeasts)
                {
                    var yeastStepDto = new YeastStepDto()
                    {
                        YeastId = item.YeastId,
                        Amount = item.Amount
                    };
                    var yeast = _elasticsearch.GetYeast(item.YeastId).Result;
                    if (yeast == null)
                    {
                        yeast = Mapper.Map<Yeast, YeastDto>(_yeastRepository.GetSingle(f => f.Id == item.YeastId));
                    }
                    yeastStepDto.Name = yeast.Name;
                    yeastStepDto.Supplier = yeast.Supplier;
                    yeastStepDto.Type = yeast.Type;

                    yeastStepDtoList.Add(yeastStepDto);

                }
                return yeastStepDtoList;
            
        }
    }
}