using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Api.Automapper.CustomResolvers;

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
                .ForMember(dto => dto.Beers, conf => conf.MapFrom(rec => rec.Beers))
                .ForMember(dto => dto.GeoLocation, conf => conf.ResolveUsing<BreweryGeoLocationResolver>());


            Mapper.CreateMap<BreweryMember, DTOUser>()
                .ForMember(dto => dto.Username, conf => conf.MapFrom(rec => rec.MemberUsername))
                .ForMember(dto => dto.Role, conf => conf.MapFrom(rec => rec.Role))
                .ForMember(dto => dto.Gravatar, conf => conf.MapFrom(rec => rec.Member.Gravatar));

            Mapper.CreateMap<BreweryMember, BreweryDto>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Brewery.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Brewery.Id))
                .ForMember(dto => dto.GeoLocation, conf => conf.ResolveUsing<BreweryMemberGeoLocationResolver>())
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Brewery.Type));

            Mapper.CreateMap<Beer, DTO>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            Mapper.CreateMap<BreweryDto, Brewery>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Description, conf => conf.MapFrom(rec => rec.Description))
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type))
                .ForMember(dto => dto.Members, conf => conf.ResolveUsing<BreweryMemberResolver>())
                .ForMember(dto => dto.Beers, conf => conf.MapFrom(rec => rec.Beers))
                 .ForMember(dto => dto.Latitude, conf => conf.MapFrom(rec => rec.GeoLocation.Latitude))
                .ForMember(dto => dto.Longitude, conf => conf.MapFrom(rec => rec.GeoLocation.Longitude));

            Mapper.CreateMap<DTOUser, BreweryMember>()
               .ForMember(dto => dto.MemberUsername, conf => conf.MapFrom(rec => rec.Username))
               .ForMember(dto => dto.Role, conf => conf.MapFrom(rec => rec.Role));
        }
    }
}