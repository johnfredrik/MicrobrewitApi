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
                .ForMember(dto => dto.Purpose, conf => conf.MapFrom(rec => rec.Purpose))
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Origin))
                .ForMember(dto => dto.Oils, conf => conf.ResolveUsing<HopOilResolver>())
                .ForMember(dto => dto.Acids, conf => conf.ResolveUsing<HopAcidResolver>())
                .ForMember(dto => dto.AromaWheel, conf => conf.ResolveUsing<HopAromaWheelResolver>())
                .ForMember(dto => dto.Aliases, conf => conf.ResolveUsing<HopAliasesResolver>())
                .ForMember(dto => dto.Flavours, conf => conf.ResolveUsing<HopFlavoursResolver>())
                .ForMember(dto => dto.Substituts, conf => conf.MapFrom(rec => rec.Substituts));


            //Mapper.CreateMap<HopFlavour, DTO>()
            //      .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Flavour.Name))
            //    .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.FlavourId));

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
                .ForMember(dto => dto.AALow, conf => conf.MapFrom(rec => rec.Acids.AlphaAcid.Low))
                .ForMember(dto => dto.AAHigh, conf => conf.MapFrom(rec => rec.Acids.AlphaAcid.High))
                .ForMember(dto => dto.BetaLow, conf => conf.MapFrom(rec => rec.Acids.BetaAcid.Low))
                .ForMember(dto => dto.BetaHigh, conf => conf.MapFrom(rec => rec.Acids.BetaAcid.High))
                .ForMember(dto => dto.BPineneHigh, conf => conf.MapFrom(rec => rec.Oils.BPineneDto.High))
                .ForMember(dto => dto.BPineneLow, conf => conf.MapFrom(rec => rec.Oils.BPineneDto.Low))
                .ForMember(dto => dto.CaryophylleneHigh, conf => conf.MapFrom(rec => rec.Oils.CaryophylleneDto.High))
                .ForMember(dto => dto.CaryophylleneLow, conf => conf.MapFrom(rec => rec.Oils.CaryophylleneDto.Low))
                .ForMember(dto => dto.HumuleneHigh, conf => conf.MapFrom(rec => rec.Oils.HumuleneDto.High))
                .ForMember(dto => dto.HumuleneLow, conf => conf.MapFrom(rec => rec.Oils.HumuleneDto.Low))
                .ForMember(dto => dto.FarneseneHigh, conf => conf.MapFrom(rec => rec.Oils.FarneseneDto.High))
                .ForMember(dto => dto.FarneseneLow, conf => conf.MapFrom(rec => rec.Oils.FarneseneDto.Low))
                .ForMember(dto => dto.GeraniolHigh, conf => conf.MapFrom(rec => rec.Oils.GeraniolDto.High))
                .ForMember(dto => dto.GeraniolLow, conf => conf.MapFrom(rec => rec.Oils.GeraniolDto.Low))
                .ForMember(dto => dto.LinaloolHigh, conf => conf.MapFrom(rec => rec.Oils.LinalLoolDto.High))
                .ForMember(dto => dto.LinaloolLow, conf => conf.MapFrom(rec => rec.Oils.LinalLoolDto.Low))
                .ForMember(dto => dto.MyrceneHigh, conf => conf.MapFrom(rec => rec.Oils.MyrceneDto.High))
                .ForMember(dto => dto.MyrceneLow, conf => conf.MapFrom(rec => rec.Oils.MyrceneDto.Low))
                .ForMember(dto => dto.TotalOilHigh, conf => conf.MapFrom(rec => rec.Oils.TotalOilDto.High))
                .ForMember(dto => dto.TotalOilLow, conf => conf.MapFrom(rec => rec.Oils.TotalOilDto.Low))
                .ForMember(dto => dto.OtherOilHigh, conf => conf.MapFrom(rec => rec.Oils.OtherOilDto.High))
                .ForMember(dto => dto.OtherOilLow, conf => conf.MapFrom(rec => rec.Oils.OtherOilDto.Low))
                .ForMember(dto => dto.Aliases, conf => conf.ResolveUsing<HopPostAliasesResolver>())
                .ForMember(dto => dto.OriginId, conf => conf.ResolveUsing<HopPostOriginResolver>())
                .ForMember(dto => dto.Flavours, conf => conf.ResolveUsing<HopPostFlavoursResolver>())
                .ForMember(dto => dto.AromaWheel, conf => conf.ResolveUsing<HopPostFlavoursResolver>())
                .ForMember(dto => dto.Substituts, conf => conf.ResolveUsing<SubstitutResolver>());

            //TODO: Check if AA is handles correct her.
            Mapper.CreateMap<HopDto, HopStepDto>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.AAValue,
                    conf => conf.MapFrom(rec => (rec.Acids.AlphaAcid.Low + rec.Acids.AlphaAcid.High)/2))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));
                //.ForMember(dto => dto.FlavourDescription, conf => conf.MapFrom(rec => rec.FlavourDescription))
                //.ForMember(dto => dto.Flavours, conf => conf.ResolveUsing<HopPostFlavoursResolver>());

            Mapper.CreateMap<DTO, Origin>()
                   .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                   .ForMember(dto => dto.OriginId, conf => conf.MapFrom(rec => rec.Id));

            Mapper.CreateMap<DTO, HopForm>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            Mapper.CreateMap<HopForm, DTO>()
                  .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            //Mapper.CreateMap<DTO,HopFlavour>()
            //     .ForMember(dto => dto.FlavourId, conf => conf.MapFrom(rec => rec.Id));
        }
    }
}