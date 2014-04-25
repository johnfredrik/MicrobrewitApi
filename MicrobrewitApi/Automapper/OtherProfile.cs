using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Api.Automapper
{
    public class OtherProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Other, OtherDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type));


        }
    }
}