using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Automapper
{
    public class OtherProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Other, OtherDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.OtherId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type));

            Mapper.CreateMap<OtherDto, Other>()
               .ForMember(dto => dto.OtherId, conf => conf.MapFrom(rec => rec.Id))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
               .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type));

            Mapper.CreateMap<OtherDto, OtherStepDto>()
              .ForMember(dto => dto.OtherId, conf => conf.MapFrom(rec => rec.Id))
              .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
              .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type));
        }
    }
}