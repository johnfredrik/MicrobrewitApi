using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Api.DTOs;

namespace Microbrewit.Api.Automapper
{
    public class BeerProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Beer, BeerSimpleDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.ABV))
                .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.IBU))
                .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.SRM))
                .ForMember(dto => dto.Recipe, conf => conf.MapFrom(rec => rec.Recipe))
                .ForMember(dto => dto.Breweries, conf => conf.MapFrom(rec => rec.Breweries))
                .ForMember(dto => dto.Brewers, conf => conf.MapFrom(rec => rec.Brewers));

            Mapper.CreateMap<Recipe, DTO>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id));

            Mapper.CreateMap<Brewery, DTO>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id));

            Mapper.CreateMap<User, DTO>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Username));

        }
    }
}