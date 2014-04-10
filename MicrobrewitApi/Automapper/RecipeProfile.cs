using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Api.Automapper
{
    public class RecipeProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Recipe, RecipeDto>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                .ForMember(dto => dto.BeerStyle, conf => conf.MapFrom(rec => rec.Beer.BeerStyle))
                .ForMember(dto => dto.MashSteps, conf => conf.MapFrom(rec => rec.MashSteps))
                .ForMember(dto => dto.BoilSteps, conf => conf.MapFrom(rec => rec.BoilSteps))
                .ForMember(dto => dto.FermentationSteps, conf => conf.MapFrom(rec => rec.FermentationSteps))
                 .ForMember(dto => dto.Volume, conf => conf.MapFrom(rec => rec.Volume));

            Mapper.CreateMap<Recipe, RecipeSimpleDto>()
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                 .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                 .ForMember(dto => dto.Notes, conf => conf.MapFrom(rec => rec.Notes))
                 .ForMember(dto => dto.Volume, conf => conf.MapFrom(rec => rec.Volume))
                 .ForMember(dto => dto.BeerStyle, conf => conf.MapFrom(rec => rec.Beer.BeerStyle));

            Mapper.CreateMap<BeerStyle, DTO>()
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                 .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            Mapper.CreateMap<RecipePostDto,Recipe>()
                .ForMember(dto => dto.Notes, conf => conf.MapFrom(rec => rec.Notes))
                .ForMember(dto => dto.MashSteps, conf => conf.MapFrom(rec => rec.MashSteps))
                .ForMember(dto => dto.BoilSteps, conf => conf.MapFrom(rec => rec.BoilSteps))
                .ForMember(dto => dto.FermentationSteps, conf => conf.MapFrom(rec => rec.FermentationSteps))
                .ForMember(dto => dto.Volume, conf => conf.MapFrom(rec => rec.Volume));
            
        }
    }
}