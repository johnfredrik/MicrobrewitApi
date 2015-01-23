using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Automapper.CustomResolvers;

namespace Microbrewit.Service.Automapper
{
    public class MashStepProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<MashStep, MashStepDto>()
                .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.StepNumber))
                .ForMember(dto => dto.Hops, conf => conf.ResolveUsing<HopMashStepResolver>())
                .ForMember(dto => dto.Fermentables, conf => conf.ResolveUsing<FermentableMashStepResolver>())
                .ForMember(dto => dto.Others, conf => conf.ResolveUsing<OtherMashStepResolver>());

            Mapper.CreateMap<MashStepHop, HopStepDto>()
                .ForMember(dto => dto.HopId, conf => conf.MapFrom(rec => rec.HopId))
                .ForMember(dto => dto.Number, conf => conf.MapFrom(rec => rec.StepNumber))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Hop.Name))
                .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Hop.Origin.Name))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.AAAmount))
                .ForMember(dto => dto.AAValue, conf => conf.MapFrom(rec => rec.AAValue))
                .ForMember(dto => dto.Flavours, conf => conf.MapFrom(rec => rec.Hop.Flavours))
                .ForMember(dto => dto.FlavourDescription, conf => conf.MapFrom(rec => rec.Hop.FlavourDescription));

            Mapper.CreateMap<MashStepFermentable, FermentableStepDto>()
                .ForMember(dto => dto.FermentableId, conf => conf.MapFrom(rec => rec.FermentableId))
                 .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.StepNumber))
                 .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Fermentable.Name))
                .ForMember(dto => dto.Lovibond, conf => conf.MapFrom(rec => rec.Fermentable.EBC))
                .ForMember(dto => dto.PPG, conf => conf.MapFrom(rec => rec.Fermentable.PPG))
                .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Fermentable.SupplierId))
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Fermentable.Type))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            Mapper.CreateMap<MashStepOther, OtherStepDto>()
               .ForMember(dto => dto.OtherId, conf => conf.MapFrom(rec => rec.OtherId))
               .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Other.Name))
               .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Other.Type))
               .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            Mapper.CreateMap<MashStepDto, MashStep>()
               .ForMember(dto => dto.Hops, conf => conf.MapFrom(rec => rec.Hops))
               .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.StepNumber))
               .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
                .ForMember(dto => dto.Fermentables, conf => conf.MapFrom(rec => rec.Fermentables))
                .ForMember(dto => dto.Others, conf => conf.MapFrom(rec => rec.Others));

            Mapper.CreateMap<HopStepDto, MashStepHop>()
                .ForMember(dto => dto.HopId, conf => conf.MapFrom(rec => rec.HopId))
                .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.Number))
                .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
                .ForMember(dto => dto.HopFormId, conf => conf.MapFrom(rec => rec.HopForm.Id))
                .ForMember(dto => dto.AAAmount, conf => conf.MapFrom(rec => rec.Amount))
                .ForMember(dto => dto.AAValue, conf => conf.MapFrom(rec => rec.AAValue));


            Mapper.CreateMap<FermentableStepDto, MashStepFermentable>()
                .ForMember(dto => dto.FermentableId, conf => conf.MapFrom(rec => rec.FermentableId))
                .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
                 .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.StepNumber))
                 .ForMember(dto => dto.PPG, conf => conf.MapFrom(rec => rec.PPG))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            Mapper.CreateMap<OtherStepDto, MashStepOther>()
               .ForMember(dto => dto.OtherId, conf => conf.MapFrom(rec => rec.OtherId))
               .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
               .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));


        }
    }
}