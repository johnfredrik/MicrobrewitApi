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
    public class FermentableMashStepResolver : ValueResolver<MashStep, IList<FermentableStepDto>>
    {
        private Elasticsearch.ElasticSearch _elasticsearch = new Elasticsearch.ElasticSearch();
        private IFermentableRepository _fermentableRepository = new FermentableRepository();

        protected override IList<FermentableStepDto> ResolveCore(MashStep step)
        {
            var fermentableStepDtoList = new List<FermentableStepDto>();
                foreach (var item in step.Fermentables)
                {
                    var fermentable = _elasticsearch.GetFermentable(item.FermentableId).Result;
                    if (fermentable == null)
                    {
                        fermentable = Mapper.Map<Fermentable, FermentableDto>(_fermentableRepository.GetSingle(f => f.Id == item.FermentableId));
                    }
                    var fermentableStepDto = new FermentableStepDto();
                    
                    fermentableStepDto.FermentableId = item.FermentableId;
                    fermentableStepDto.Number = item.StepNumber;
                    fermentableStepDto.Amount = item.Amount;
                    fermentableStepDto.Supplier = fermentable.Supplier;
                    fermentableStepDto.Type = fermentable.Type;
                    fermentableStepDto.Name = fermentable.Name;
                    fermentableStepDto.PPG = fermentable.PPG;
                    
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