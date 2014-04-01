using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Api.Automapper
{
    public class FermentationStepProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<FermentationStep, FermentationStepDto>()
                 .ForMember(dto => dto.Hops, conf => conf.MapFrom(rec => rec.Hops))
                 .ForMember(dto => dto.Fermentables, conf => conf.MapFrom(rec => rec.Fermentables))
                 .ForMember(dto => dto.Others, conf => conf.MapFrom(rec => rec.Others));

            Mapper.CreateMap<FermentationStepHop, HopStepDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.HopId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Hop.Name))
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Hop.Origin.Name))
                .ForMember(dto => dto.AAAmount, conf => conf.MapFrom(rec => rec.AAAmount))
                .ForMember(dto => dto.AAValue, conf => conf.MapFrom(rec => rec.AAValue))
                .ForMember(dto => dto.Flavours, conf => conf.MapFrom(rec => rec.Hop.Flavours))
                .ForMember(dto => dto.FlavourDescription, conf => conf.MapFrom(rec => rec.Hop.FlavourDescription));

            Mapper.CreateMap<FermentationStepFermentable, FermentableStepDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.FermentableId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Fermentable.Name))
                .ForMember(dto => dto.Colour, conf => conf.MapFrom(rec => rec.Fermentable.EBC))
                .ForMember(dto => dto.PPG, conf => conf.MapFrom(rec => rec.Fermentable.PPG))
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Fermentable.Supplier.Origin.Name))
                .ForMember(dto => dto.SuppliedById, conf => conf.MapFrom(rec => rec.Fermentable.SupplierId))
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Fermentable.Type))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            Mapper.CreateMap<FermentationStepOther, OtherStepDto>()
               .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.OtherId))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Other.Name))
               .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Other.Type))
               .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            Mapper.CreateMap<FermentationStepYeast, YeastStepDto>()
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                  .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Yeast.Name))
                   .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount))
                    .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Yeast.Supplier));

            Mapper.CreateMap<Supplier, DTO>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));
        }
    }
}