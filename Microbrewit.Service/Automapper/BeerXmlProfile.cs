using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microbrewit.Model.BeerXml;
using Microbrewit.Model.DTOs;
using Microbrewit.Model.ModelBuilder;
using Microbrewit.Service.Automapper.CustomResolvers;

namespace Microbrewit.Service.Automapper
{
    public class BeerXmlProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Recipe, BeerDto>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Name))
                .ForMember(dest => dest.ABV, conf => conf.MapFrom(src => new ABVDto()))
                .ForMember(dest => dest.SRM, conf => conf.MapFrom(src => new SRMDto()))
                .ForMember(dest => dest.IBU, conf => conf.MapFrom(src => new IBUDto()))
                .ForMember(dest => dest.BeerStyle, conf => conf.ResolveUsing<BeerXmlBeerStyleSimpleResolver>())
                .ForMember(dest => dest.Recipe, conf => conf.ResolveUsing<BeerXmlResolver>());
        }
    }
}
