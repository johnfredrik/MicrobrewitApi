﻿using System;
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
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Origin))
                .ForMember(dto => dto.Flavours, conf => conf.MapFrom(rec => rec.HopFlavours))
                .ForMember(dto => dto.Substitutions, conf => conf.MapFrom(rec => rec.Substituts));
               

           
            Mapper.CreateMap<HopFlavour, DTO>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Flavour.Name))
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Flavour.Id));

            Mapper.CreateMap<Hop, DTO>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id));

            Mapper.CreateMap<Origin, DTO>()
              .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
               .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id));
        }
    }
}