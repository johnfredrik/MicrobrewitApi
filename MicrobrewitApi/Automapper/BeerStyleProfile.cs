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
    public class BeerStyleProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BeerStyle, BeerStyleDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.SuperBeerStyle, conf => conf.MapFrom(rec => rec.SuperStyle))
                .ForMember(dto => dto.SubBeerStyles, conf => conf.MapFrom(rec => rec.SubStyles));
           
            Mapper.CreateMap<BeerStyle, DTO>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id));
    
        }
    }
}