using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Api.DTOs;

namespace Microbrewit.Api.Automapper
{
    public class HopProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Hop, HopDto>()
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Origin.Name))
                .ForMember(dto => dto.Flavours, conf => conf.MapFrom(rec => rec.HopFlavours));

            Mapper.CreateMap<HopFlavour, FlavourDto>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Flavour.Name));
        }
    }
}