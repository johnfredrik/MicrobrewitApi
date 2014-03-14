using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Api.DTOs;

namespace Microbrewit.Api.Automapper
{
    public class RecipeProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Recipe, RecipeDto>().ForMember(dto => dto.Hops, conf => conf.MapFrom(rec => rec.RecipeHops));
            Mapper.CreateMap<RecipeHop, RecipeHopDto>()
                    .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Hop.Name))
                    .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.HopId))
                    .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Hop.Origin.Name));
                    
           // Mapper.AssertConfigurationIsValid();
        }
    }
}