using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Automapper.CustomResolvers;

namespace Microbrewit.Service.Automapper
{
    public class BoilStepProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BoilStep, BoilStepDto>()
                 .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.StepNumber))
                 .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
                 .ForMember(dto => dto.Hops, conf => conf.ResolveUsing<HopBoilStepResolver>())
                 .ForMember(dto => dto.Fermentables, conf => conf.ResolveUsing<FermentableBoilStepResolver>())
                 .ForMember(dto => dto.Others, conf => conf.ResolveUsing<OtherBoilStepResolver>());

            Mapper.CreateMap<BoilStepHop, HopStepDto>()
                .ForMember(dto => dto.HopId, conf => conf.MapFrom(rec => rec.HopId))
                .ForMember(dto => dto.Number, conf => conf.MapFrom(rec => rec.StepNumber))
                .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
                .ForMember(dto => dto.AAValue, conf => conf.MapFrom(rec => rec.AAValue))
                .ForMember(dto => dto.Flavours, conf => conf.MapFrom(rec => rec.Hop.Flavours))
                .ForMember(dto => dto.FlavourDescription, conf => conf.MapFrom(rec => rec.Hop.FlavourDescription));

            Mapper.CreateMap<BoilStepFermentable, FermentableStepDto>()
                .ForMember(dto => dto.FermentableId, conf => conf.MapFrom(rec => rec.FermentableId))
                .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.StepNumber))
                .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Fermentable.Name))
                .ForMember(dto => dto.Lovibond, conf => conf.MapFrom(rec => rec.Fermentable.EBC))
                .ForMember(dto => dto.PPG, conf => conf.MapFrom(rec => rec.Fermentable.PPG))
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Fermentable.Type))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            Mapper.CreateMap<BoilStepOther, OtherStepDto>()
               .ForMember(dto => dto.OtherId, conf => conf.MapFrom(rec => rec.OtherId))
               .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Other.Name))
               .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Other.Type))
               .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));


            //
            Mapper.CreateMap<BoilStepDto, BoilStep>()
                 .ForMember(dto => dto.Hops, conf => conf.MapFrom(rec => rec.Hops))
                 .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
                 .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.StepNumber))
                 .ForMember(dto => dto.Others, conf => conf.MapFrom(rec => rec.Others));

            Mapper.CreateMap<HopStepDto, BoilStepHop>()
               .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.Number))
               .ForMember(dto => dto.HopId, conf => conf.MapFrom(rec => rec.HopId))
               .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
               .ForMember(dto => dto.HopFormId, conf => conf.MapFrom(rec => rec.HopForm.Id))
               .ForMember(dto => dto.HopForm, conf => conf.ResolveUsing<SetHopFromNullResolver>())
               .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount))
               .ForMember(dto => dto.AAValue, conf => conf.MapFrom(rec => rec.AAValue));


            Mapper.CreateMap<FermentableStepDto, BoilStepFermentable>()
                .ForMember(dto => dto.FermentableId, conf => conf.MapFrom(rec => rec.FermentableId))
                .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
                .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.StepNumber))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            Mapper.CreateMap<OtherStepDto, BoilStepOther>()
               .ForMember(dto => dto.OtherId, conf => conf.MapFrom(rec => rec.OtherId))
               .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
               .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

        }
    }
}