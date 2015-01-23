using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Automapper
{
    public class BrewerProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<User, BrewerDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Username));
        }

    }
}