using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.Automapper
{
    public class OriginProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Origin, OriginDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            Mapper.CreateMap<OriginDto, Origin>()
               .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));
        }
    }
}