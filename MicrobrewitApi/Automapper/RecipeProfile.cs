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
            Mapper.CreateMap<Recipe, RecipeDto>()
                .ForMember(dto => dto.RecipeName, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.MashSteps, conf => conf.MapFrom(rec => rec.MashSteps))
                .ForMember(dto => dto.BoilSteps, conf => conf.MapFrom(rec => rec.BoilSteps))
                .ForMember(dto => dto.FermentationSteps, conf => conf.MapFrom(rec => rec.FermentationSteps))
                .ForMember(dto => dto.Brewers, conf => conf.MapFrom(rec => rec.Brewers));
        }
    }
}