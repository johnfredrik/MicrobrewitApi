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
    public class BeerProfile : Profile
    {
        protected override void Configure()
        {

            // Creates a mapper for the beer class to a simpler beer class.
            Mapper.CreateMap<Beer, BeerSimpleDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.ABV))
                .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.IBU))
                .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.SRM))
                .ForMember(dto => dto.Recipe, conf => conf.MapFrom(rec => rec.Recipe))
                .ForMember(dto => dto.Breweries, conf => conf.MapFrom(rec => rec.Breweries))
                .ForMember(dto => dto.Brewers, conf => conf.MapFrom(rec => rec.Brewers));

            Mapper.CreateMap<Beer, BeerDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.ABV))
                .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.IBU))
                .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.SRM))
                .ForMember(dto => dto.BeerStyle, conf => conf.ResolveUsing<BeerStyleResolver>())
                .ForMember(dto => dto.Recipe, conf => conf.MapFrom(rec => rec.Recipe))
                .ForMember(dto => dto.Breweries, conf => conf.MapFrom(rec => rec.Breweries))
                .ForMember(dto => dto.Brewers, conf => conf.MapFrom(rec => rec.Brewers));

            Mapper.CreateMap<Recipe, DTO>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id));

            Mapper.CreateMap<Brewery, DTO>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id));

            Mapper.CreateMap<UserBeer , DTOUser>()
               .ForMember(dto => dto.Username, conf => conf.MapFrom(rec => rec.Username))
               .ForMember(dto => dto.Email, conf => conf.MapFrom(rec => rec.User.Email));

            Mapper.CreateMap<UserBeer, BeerSimpleDto>()
                 .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerId));


            Mapper.CreateMap<ABV, ABVDto>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard));
               

            Mapper.CreateMap<IBU, IBUDto>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.Tinseth, conf => conf.MapFrom(rec => rec.Tinseth))
                .ForMember(dto => dto.Rager, conf => conf.MapFrom(rec => rec.Rager));

            Mapper.CreateMap<SRM, SRMDto>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.Mosher, conf => conf.MapFrom(rec => rec.Mosher))
                .ForMember(dto => dto.Daniels, conf => conf.MapFrom(rec => rec.Daniels));

            Mapper.CreateMap<BeerDto,Beer>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.ABV))
                .ForMember(dto => dto.BeerStyleId, conf => conf.MapFrom(rec => rec.BeerStyle.Id))
                .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.IBU))
                .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.SRM))
                .ForMember(dto => dto.Recipe, conf => conf.MapFrom(rec => rec.Recipe))
                .ForMember(dto => dto.Breweries, conf => conf.MapFrom(rec => rec.Breweries))
                .ForMember(dto => dto.Brewers, conf => conf.ResolveUsing<BeerBrewerResolver>());

            Mapper.CreateMap<ABVDto, ABV>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id));

            Mapper.CreateMap<IBUDto, IBU>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.Rager, conf => conf.MapFrom(rec => rec.Rager))
                .ForMember(dto => dto.Tinseth, conf => conf.MapFrom(rec => rec.Tinseth))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id));

            Mapper.CreateMap<SRMDto, SRM>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.Mosher, conf => conf.MapFrom(rec => rec.Mosher))
                .ForMember(dto => dto.Daniels, conf => conf.MapFrom(rec => rec.Daniels))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id));

            Mapper.CreateMap<DTO, Brewery>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id));

            Mapper.CreateMap<DTOUser, UserBeer>()
               .ForMember(dto => dto.Username, conf => conf.MapFrom(rec => rec.Username));
               

            Mapper.CreateMap<DTO, BeerStyle>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            
        }
    }
}