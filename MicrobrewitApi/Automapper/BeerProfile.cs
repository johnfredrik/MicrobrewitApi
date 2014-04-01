using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

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
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Username))
               .ForMember(dto => dto.Id, conf => conf.MapFrom(rec  => 0));

            Mapper.CreateMap<ABV, ABVDto>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.Formula1, conf => conf.MapFrom(rec => rec.Formula1))
                .ForMember(dto => dto.Formula2, conf => conf.MapFrom(rec => rec.Formula2));

            Mapper.CreateMap<IBU, IBUDto>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.Formula1, conf => conf.MapFrom(rec => rec.Formula1))
                .ForMember(dto => dto.Formula2, conf => conf.MapFrom(rec => rec.Formula2));

            Mapper.CreateMap<SRM, SRMDto>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.Formula1, conf => conf.MapFrom(rec => rec.Formula1))
                .ForMember(dto => dto.Formula2, conf => conf.MapFrom(rec => rec.Formula2));
        }
    }
}