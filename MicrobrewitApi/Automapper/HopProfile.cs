using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Api.DTOs;
using Microbrewit.Api.Automapper.CustomResolvers;

namespace Microbrewit.Api.Automapper
{
    public class HopProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Hop, HopDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.AALow, conf => conf.MapFrom(rec => rec.AALow))
                .ForMember(dto => dto.AAHigh, conf => conf.MapFrom(rec => rec.AAHigh))
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Origin.Name))
                .ForMember(dto => dto.Flavours, conf => conf.ResolveUsing<FlavorResolver>())
                .ForMember(dto => dto.Substitutions, conf => conf.ResolveUsing<SubstituteResolver>())
                 .ForMember(dto => dto.Links, conf => conf.ResolveUsing<HopLinksResolver>());

           
            Mapper.CreateMap<HopFlavour, FlavourDto>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Flavour.Name));
        }
    }
}