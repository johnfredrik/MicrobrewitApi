﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Api.Automapper
{
    public class BreweryProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Brewery, BreweryDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Description, conf => conf.MapFrom(rec => rec.Description))
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type))
                .ForMember(dto => dto.Members, conf => conf.MapFrom(rec => rec.Members))
                .ForMember(dto => dto.Beers, conf => conf.MapFrom(rec => rec.Beers));

            Mapper.CreateMap<BreweryMember, DTOUser>()
                .ForMember(dto => dto.Username, conf => conf.MapFrom(rec => rec.Member.Username))
                .ForMember(dto => dto.Email, conf => conf.MapFrom(rec => rec.Member.Email));

            Mapper.CreateMap<Beer, DTO>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));
        }
    }
}