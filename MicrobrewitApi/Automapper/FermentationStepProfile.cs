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
    public class FermentationStepProfile : Profile
    {
        protected override void Configure()
        {
            // from db out
            Mapper.CreateMap<FermentationStep, FermentationStepDto>()
                 .ForMember(dto => dto.Hops, conf => conf.ResolveUsing<HopFermentationStepResolver>())
                 .ForMember(dto => dto.Fermentables, conf => conf.ResolveUsing<FermentableFermentationStepResolver>())
                 .ForMember(dto => dto.Others, conf => conf.ResolveUsing<OtherFermentationStepResolver>())
                  .ForMember(dto => dto.Yeasts, conf => conf.ResolveUsing<YeastFermentationStepResolver>());

            Mapper.CreateMap<FermentationStepHop, HopStepDto>()
                .ForMember(dto => dto.HopId, conf => conf.MapFrom(rec => rec.HopId))
                .ForMember(dto => dto.StepId, conf => conf.MapFrom(rec => rec.FermentationStepId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Hop.Name))
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Hop.Origin.Name))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.AAAmount))
                .ForMember(dto => dto.AAValue, conf => conf.MapFrom(rec => rec.AAValue))
                .ForMember(dto => dto.Flavours, conf => conf.MapFrom(rec => rec.Hop.Flavours))
                .ForMember(dto => dto.FlavourDescription, conf => conf.MapFrom(rec => rec.Hop.FlavourDescription));

            Mapper.CreateMap<FermentationStepFermentable, FermentableStepDto>()
                .ForMember(dto => dto.FermentableId, conf => conf.MapFrom(rec => rec.FermentableId))
                .ForMember(dto => dto.StepId, conf => conf.MapFrom(rec => rec.FermentationStepId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Fermentable.Name))
                .ForMember(dto => dto.Lovibond, conf => conf.MapFrom(rec => rec.Fermentable.EBC))
                .ForMember(dto => dto.PPG, conf => conf.MapFrom(rec => rec.Fermentable.PPG))
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Fermentable.Type))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            Mapper.CreateMap<FermentationStepOther, OtherStepDto>()
               .ForMember(dto => dto.OtherId, conf => conf.MapFrom(rec => rec.OtherId))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Other.Name))
               .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Other.Type))
               .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            Mapper.CreateMap<FermentationStepYeast, YeastStepDto>()
                 .ForMember(dto => dto.YeastId, conf => conf.MapFrom(rec => rec.YeastId))
                  .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Yeast.Name))
                   .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount))
                    .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Yeast.Supplier));

            Mapper.CreateMap<Supplier, DTO>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            // from web and to db
            Mapper.CreateMap<FermentationStepDto, FermentationStep>()
                 .ForMember(dto => dto.Hops, conf => conf.MapFrom(rec => rec.Hops))
                 .ForMember(dto => dto.Fermentables, conf => conf.MapFrom(rec => rec.Fermentables))
                 .ForMember(dto => dto.Others, conf => conf.MapFrom(rec => rec.Others));

            Mapper.CreateMap<HopStepDto, FermentationStepHop>()
               .ForMember(dto => dto.HopId, conf => conf.MapFrom(rec => rec.HopId))
               .ForMember(dto => dto.FermentationStepId, conf => conf.MapFrom(rec => rec.StepId))
               .ForMember(dto => dto.HopFormId, conf => conf.MapFrom(rec => rec.HopForm.Id))
               .ForMember(dto => dto.AAAmount, conf => conf.MapFrom(rec => rec.Amount))
               .ForMember(dto => dto.AAValue, conf => conf.MapFrom(rec => rec.AAValue));
            

            Mapper.CreateMap<FermentableStepDto,FermentationStepFermentable>()
                .ForMember(dto => dto.FermentableId, conf => conf.MapFrom(rec => rec.FermentableId))
                .ForMember(dto => dto.FermentationStepId, conf => conf.MapFrom(rec => rec.StepId))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            Mapper.CreateMap<OtherStepDto, FermentationStepOther>()
               .ForMember(dto => dto.OtherId, conf => conf.MapFrom(rec => rec.OtherId))
               .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));
               



            Mapper.CreateMap<YeastStepDto, FermentationStepYeast>()
                 .ForMember(dto => dto.YeastId, conf => conf.MapFrom(rec => rec.YeastId))
                   .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));
        }
    }
}