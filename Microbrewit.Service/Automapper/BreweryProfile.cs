using System.Configuration;
using System.Linq;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Automapper.CustomResolvers;

namespace Microbrewit.Service.Automapper
{
    public class BreweryProfile : Profile
    {
        private string _imagePath = ConfigurationManager.AppSettings["imagePath"];

        protected override void Configure()
        {
            Mapper.CreateMap<Brewery, BreweryDto>()
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Description))
                .ForMember(dest => dest.Type, conf => conf.MapFrom(src => src.Type))
                .ForMember(dest => dest.Members, conf => conf.MapFrom(src => src.Members))
                .ForMember(dest => dest.Beers, conf => conf.MapFrom(src => src.Beers))
                .ForMember(dest => dest.Origin, conf => conf.MapFrom(src => src.Origin))
                .ForMember(dest => dest.Avatar, conf => conf.MapFrom(src => (src.Avatar != null && src.Avatar.Any()) ? _imagePath + "avatar/" + src.Avatar : null))
                .ForMember(dest => dest.HeaderImage, conf => conf.MapFrom(src => (src.HeaderImage != null && src.HeaderImage.Any()) ? _imagePath + "header/" + src.HeaderImage : null))
                .ForMember(dest => dest.GeoLocation, conf => conf.ResolveUsing<BreweryGeoLocationResolver>())
                .ForMember(dest => dest.Socials, conf => conf.ResolveUsing<BrewerySocialResolver>());


            Mapper.CreateMap<BreweryMember, DTOUser>()
                .ForMember(dest => dest.Username, conf => conf.MapFrom(src => src.MemberUsername))
                .ForMember(dest => dest.Role, conf => conf.MapFrom(src => src.Role))
                .ForMember(dest => dest.Gravatar, conf => conf.MapFrom(src => src.Member.Gravatar));

            Mapper.CreateMap<BreweryMember, BreweryMemberDto>()
                .ForMember(dest => dest.Username, conf => conf.MapFrom(src => src.MemberUsername))
                .ForMember(dest => dest.Role, conf => conf.MapFrom(src => src.Role))
                .ForMember(dest => dest.Avatar, conf => conf.MapFrom(src => (src.Member.Avatar != null && src.Member.Avatar.Any()) ? _imagePath + "avatar/" + src.Member.Avatar : null))
                .ForMember(dest => dest.Gravatar, conf => conf.MapFrom(src => src.Member.Gravatar));

            Mapper.CreateMap<BreweryMember, BreweryDto>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Brewery.Name))
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Brewery.Id))
                .ForMember(dest => dest.GeoLocation, conf => conf.ResolveUsing<BreweryMemberGeoLocationResolver>())
                .ForMember(dest => dest.Type, conf => conf.MapFrom(src => src.Brewery.Type));

            Mapper.CreateMap<Beer, DTO>()
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Name));

            Mapper.CreateMap<BreweryDto, Brewery>()
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Description))
                .ForMember(dest => dest.Type, conf => conf.MapFrom(src => src.Type))
                .ForMember(dest => dest.Members, conf => conf.ResolveUsing<BreweryMemberResolver>())
                .ForMember(dest => dest.Beers, conf => conf.MapFrom(src => src.Beers))
                .ForMember(dest => dest.OriginId, conf => conf.MapFrom(src => src.Origin.Id))
                .ForMember(dest => dest.Address, conf => conf.MapFrom(src => src.Address))
                .ForMember(dest => dest.Latitude, conf => conf.MapFrom(src => src.GeoLocation.Latitude))
                .ForMember(dest => dest.Longitude, conf => conf.MapFrom(src => src.GeoLocation.Longitude))
                .ForMember(dest => dest.Avatar, conf => conf.ResolveUsing<BreweryAvatarResolver>())
                .ForMember(dest => dest.HeaderImage, conf => conf.ResolveUsing<BrewerHeaderImageResolver>())
                .ForMember(dest => dest.Socials, conf => conf.ResolveUsing<BreweryDtoSocialResolver>());

            Mapper.CreateMap<BreweryMemberDto, BreweryMember>()
               .ForMember(dest => dest.MemberUsername, conf => conf.MapFrom(src => src.Username))
               .ForMember(dest => dest.Role, conf => conf.MapFrom(src => src.Role));

        }
    }
}