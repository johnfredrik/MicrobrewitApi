using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Api.Automapper
{
    public class UserProfile : Profile 
    {
        protected override void Configure()
        {
            Mapper.CreateMap<User, UserDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Username))
                .ForMember(dto => dto.Username, conf => conf.MapFrom(rec => rec.Username))
                .ForMember(dto => dto.Gravatar, conf => conf.MapFrom(rec => rec.Gravatar))
                .ForMember(dto => dto.Breweries, conf => conf.MapFrom(rec => rec.Breweries))
                .ForMember(dto => dto.Beers, conf => conf.MapFrom(rec => rec.Beers))
                .ForMember(dto => dto.Settings, conf => conf.MapFrom(rec => rec.Settings));


            Mapper.CreateMap<UserPostDto, User>()
               .ForMember(dto => dto.Username, conf => conf.MapFrom(rec => rec.Username))
               .ForMember(dto => dto.Settings, conf => conf.MapFrom(rec => rec.Settings));
        }
    }
}