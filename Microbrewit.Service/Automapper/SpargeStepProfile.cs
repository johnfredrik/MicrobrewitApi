using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Automapper.CustomResolvers;
using Nest;

namespace Microbrewit.Service.Automapper
{
    public class SpargeStepProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<SpargeStep, SpargeStepDto>()
                .ForMember(dest => dest.StepNumber, conf => conf.MapFrom(src => src.StepNumber))
                .ForMember(dest => dest.RecipeId, conf => conf.MapFrom(src => src.RecipeId))
                .ForMember(dest => dest.Amount, conf => conf.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Notes, conf => conf.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Type, conf => conf.MapFrom(src => src.Type))
                .ForMember(dest => dest.Hops, conf => conf.ResolveUsing<HopSpargeStepResolver>())
                .ForMember(dest => dest.Temperature, conf => conf.MapFrom(src => src.Temperature));
         
            Mapper.CreateMap<SpargeStepDto, SpargeStep>()
               .ForMember(dest => dest.StepNumber, conf => conf.MapFrom(src => src.StepNumber))
               .ForMember(dest => dest.RecipeId, conf => conf.MapFrom(src => src.RecipeId))
               .ForMember(dest => dest.Amount, conf => conf.MapFrom(src => src.Amount))
               .ForMember(dest => dest.Notes, conf => conf.MapFrom(src => src.Notes))
               .ForMember(dest => dest.Type, conf => conf.MapFrom(src => src.Type))
               .ForMember(dest => dest.Temperature, conf => conf.MapFrom(src => src.Temperature));

            Mapper.CreateMap<SpargeStepHop, HopStepDto>()
                .ForMember(dest => dest.HopId, conf => conf.MapFrom(src => src.HopId))
                .ForMember(dest => dest.StepNumber, conf => conf.MapFrom(src => src.StepNumber))
                .ForMember(dest => dest.RecipeId, conf => conf.MapFrom(src => src.RecipeId))
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Hop.Name))
                .ForMember(dest => dest.Origin, conf => conf.MapFrom(src => src.Hop.Origin.Name))
                .ForMember(dest => dest.Amount, conf => conf.MapFrom(src => src.AaAmount))
                .ForMember(dest => dest.AAValue, conf => conf.MapFrom(src => src.AaValue));
               //.ForMember(dest => dest.Flavours, conf => conf.MapFrom(src => src.Hop.Flavours))
               //.ForMember(dest => dest.FlavourDescription, conf => conf.MapFrom(src => src.Hop.FlavourDescription));

            Mapper.CreateMap<HopStepDto, SpargeStepHop>()
              .ForMember(dto => dto.HopId, conf => conf.MapFrom(rec => rec.HopId))
              .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.StepNumber))
              .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
              .ForMember(dto => dto.HopFormId, conf => conf.MapFrom(rec => rec.HopForm.Id))
              .ForMember(dto => dto.AaAmount, conf => conf.MapFrom(rec => rec.Amount))
              .ForMember(dto => dto.AaValue, conf => conf.MapFrom(rec => rec.AAValue));
        }
    }
}