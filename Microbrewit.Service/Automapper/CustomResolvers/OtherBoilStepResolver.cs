using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class OtherBoilStepResolver : ValueResolver<BoilStep, IList<OtherStepDto>>
    {
        private ElasticSearch _elasticsearch = new ElasticSearch();
        private IOtherRepository _otherRepository = new OtherRepository();

        protected override IList<OtherStepDto> ResolveCore(BoilStep step)
        {
            var otherStepDtoList = new List<OtherStepDto>();
            foreach (var item in step.Others)
            {

                var otherStepDto = new OtherStepDto()
                {
                    OtherId = item.OtherId,
                    Amount = item.Amount,
                    RecipeId = item.RecipeId,
                };
                var other = _elasticsearch.GetOther(item.OtherId).Result;
                if (other == null)
                {
                    other = Mapper.Map<Other, OtherDto>(_otherRepository.GetSingle(f => f.Id == item.OtherId));
                }
                otherStepDto.Name = other.Name;
                otherStepDto.Type = other.Type;
                otherStepDtoList.Add(otherStepDto);
            }
            return otherStepDtoList;

        }
    }
}