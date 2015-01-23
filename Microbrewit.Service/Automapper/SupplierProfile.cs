using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Automapper
{
    public class SupplierProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Supplier, SupplierDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Origin));
            
            Mapper.CreateMap<Origin,DTO>()
               .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            Mapper.CreateMap<SupplierDto,Supplier>()
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.OriginId, conf => conf.MapFrom(rec => rec.Origin.Id))
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Origin));

            Mapper.CreateMap<DTO,Origin>()
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            }
    }
}