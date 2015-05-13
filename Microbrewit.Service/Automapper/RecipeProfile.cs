using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Automapper.CustomResolvers;

namespace Microbrewit.Service.Automapper
{
    public class RecipeProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Recipe, RecipeDto>()
                .ForMember(dto => dto.MashSteps, conf => conf.MapFrom(rec => rec.MashSteps))
                .ForMember(dto => dto.BoilSteps, conf => conf.MapFrom(rec => rec.BoilSteps))
                .ForMember(dto => dto.FermentationSteps, conf => conf.MapFrom(rec => rec.FermentationSteps))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.RecipeId))
                 .ForMember(dto => dto.Volume, conf => conf.MapFrom(rec => rec.Volume));

            Mapper.CreateMap<Recipe, RecipeSimpleDto>()
                 .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
                 .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                 .ForMember(dto => dto.Notes, conf => conf.MapFrom(rec => rec.Notes))
                 .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
                 .ForMember(dto => dto.Volume, conf => conf.MapFrom(rec => rec.Volume))
                 .ForMember(dto => dto.BeerStyle, conf => conf.MapFrom(rec => rec.Beer.BeerStyle));

            Mapper.CreateMap<BeerStyle, DTO>()
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerStyleId))
                 .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            Mapper.CreateMap<RecipeDto,Recipe>()
                .ForMember(dto => dto.Notes, conf => conf.MapFrom(rec => rec.Notes))
                .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.MashSteps, conf => conf.ResolveUsing<RecipeMashStepResolver>())
                .ForMember(dto => dto.BoilSteps, conf => conf.ResolveUsing<RecipeBoilStepResolver>())
                .ForMember(dto => dto.FermentationSteps, conf => conf.ResolveUsing<RecipeFermentationStepResolver>())
                .ForMember(dto => dto.Volume, conf => conf.MapFrom(rec => rec.Volume));
            
        }
    }
}