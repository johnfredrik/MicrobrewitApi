using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Api.Automapper.CustomResolvers;
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
                .ForMember(dto => dto.GeoLocation, conf => conf.ResolveUsing<UserGeoLocationResolver>())
                .ForMember(dto => dto.Settings, conf => conf.MapFrom(rec => rec.Settings));


            Mapper.CreateMap<UserPostDto, User>()
               .ForMember(dto => dto.Username, conf => conf.MapFrom(rec => rec.Username))
               .ForMember(dto => dto.Settings, conf => conf.MapFrom(rec => rec.Settings))
               .ForMember(dto => dto.Latitude, conf => conf.MapFrom(rec => rec.GeoLocation.Latitude))
               .ForMember(dto => dto.Longitude, conf => conf.MapFrom(rec => rec.GeoLocation.Longitude));

            Mapper.CreateMap<UserPostDto, UserModel>()
                .ForMember(dto => dto.UserName, conf => conf.MapFrom(rec => rec.Username))
                .ForMember(dto => dto.Password, conf => conf.MapFrom(rec => rec.Password))
                .ForMember(dto => dto.ConfirmPassword, conf => conf.MapFrom(rec => rec.ConfirmPassword))
                .ForMember(dto => dto.Email, conf => conf.MapFrom(rec => rec.Email));

            Mapper.CreateMap<UserPutDto, User>()
            .ForMember(dto => dto.Username, conf => conf.MapFrom(rec => rec.Username))
            .ForMember(dto => dto.Settings, conf => conf.MapFrom(rec => rec.Settings))
            .ForMember(dto => dto.Latitude, conf => conf.MapFrom(rec => rec.GeoLocation.Latitude))
            .ForMember(dto => dto.Longitude, conf => conf.MapFrom(rec => rec.GeoLocation.Longitude));

            Mapper.CreateMap<UserPutDto, UserModel>()
                .ForMember(dto => dto.UserName, conf => conf.MapFrom(rec => rec.Username))
                .ForMember(dto => dto.Email, conf => conf.MapFrom(rec => rec.Email));


            Mapper.CreateMap<User,DTOUser>()
                .ForMember(dto => dto.Username, conf => conf.MapFrom(rec => rec.Username))
                .ForMember(dto => dto.Gravatar, conf => conf.MapFrom(rec => rec.Gravatar));
            }

            
    }
}