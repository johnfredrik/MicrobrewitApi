using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Automapper.CustomResolvers;

namespace Microbrewit.Service.Automapper
{
    public class HopProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Hop, HopDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.HopId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.AALow, conf => conf.MapFrom(rec => rec.AALow))
                .ForMember(dto => dto.AAHigh, conf => conf.MapFrom(rec => rec.AAHigh))
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Origin))
                .ForMember(dto => dto.Flavours, conf => conf.MapFrom(rec => rec.Flavours))
                .ForMember(dto => dto.Substituts, conf => conf.MapFrom(rec => rec.Substituts));


            Mapper.CreateMap<HopFlavour,DTO>()
                  .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Flavour.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.FlavourId));

            Mapper.CreateMap<Hop, DTO>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.HopId));

            Mapper.CreateMap<Substitute, DTO>()
              .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Sub.Name))
               .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.SubstituteId));

            Mapper.CreateMap<Origin, DTO>()
              .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
               .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.OriginId));

            Mapper.CreateMap<HopDto, Hop>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.AALow, conf => conf.MapFrom(rec => rec.AALow))
                .ForMember(dto => dto.AAHigh, conf => conf.MapFrom(rec => rec.AAHigh))
                .ForMember(dto => dto.OriginId, conf => conf.ResolveUsing<HopPostOriginResolver>())
                .ForMember(dto => dto.Flavours, conf => conf.ResolveUsing<HopFlavoursResolver>())
                .ForMember(dto => dto.Substituts, conf => conf.ResolveUsing<SubstitutResolver>());

            Mapper.CreateMap<HopDto, HopStepDto>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.AAValue, conf => conf.MapFrom(rec => (rec.AALow + rec.AAHigh)/2))
                .ForMember(dto => dto.FlavourDescription, conf => conf.MapFrom(rec => rec.FlavourDescription))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Flavours, conf => conf.ResolveUsing<HopFlavoursResolver>());

           Mapper.CreateMap<DTO,Origin>()
                  .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                  .ForMember(dto => dto.OriginId, conf => conf.MapFrom(rec => rec.Id));

           Mapper.CreateMap<DTO, HopForm>()
               .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            Mapper.CreateMap<HopForm,DTO>()
                  .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

           //Mapper.CreateMap<DTO,HopFlavour>()
           //     .ForMember(dto => dto.FlavourId, conf => conf.MapFrom(rec => rec.Id));
        }
    }
}