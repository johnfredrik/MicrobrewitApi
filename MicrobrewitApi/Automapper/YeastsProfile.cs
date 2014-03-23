﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Api.DTOs;
using Microbrewit.Model;

namespace Microbrewit.Api.Automapper
{
    public class YeastsProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Yeast, YeastDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ProductCode, conf => conf.MapFrom(rec => rec.ProductCode))
                .ForMember(dto => dto.TemperatureLow, conf => conf.MapFrom(rec => rec.TemperatureLow))
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type))
                .ForMember(dto => dto.Comment, conf => conf.MapFrom(rec => rec.Comment))
                .ForMember(dto => dto.Flocculation, conf => conf.MapFrom(rec => rec.Flocculation))
                .ForMember(dto => dto.AlcoholTolerance, conf => conf.MapFrom(rec => rec.AlcoholTolerance))
                .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Supplier.Name))
                .ForMember(dto => dto.Links, conf => conf.MapFrom(rec => rec.Supplier));

            Mapper.CreateMap<Supplier,YeastLinks>()
                .ForMember(dto => dto.SupplierId, conf => conf.MapFrom(rec => rec.Id));
        }
    }
}