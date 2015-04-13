using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Automapper.CustomResolvers;

namespace Microbrewit.Service.Automapper
{
    public class YeastsProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Yeast, YeastDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ProductCode, conf => conf.MapFrom(rec => rec.ProductCode))
                .ForMember(dto => dto.TemperatureLow, conf => conf.MapFrom(rec => rec.TemperatureLow))
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type))
                .ForMember(dto => dto.Notes, conf => conf.MapFrom(rec => rec.Notes))
                .ForMember(dto => dto.Flocculation, conf => conf.MapFrom(rec => rec.Flocculation))
                .ForMember(dto => dto.AlcoholTolerance, conf => conf.MapFrom(rec => rec.AlcoholTolerance))
                .ForMember(dto => dto.BrewerySource, conf => conf.MapFrom(rec => rec.BrewerySource))
                .ForMember(dto => dto.Species, conf => conf.MapFrom(rec => rec.Species))
                .ForMember(dto => dto.AttenutionRange, conf => conf.MapFrom(rec => rec.AttenutionRange))
                .ForMember(dto => dto.PitchingFermentationNotes, conf => conf.MapFrom(rec => rec.PitchingFermentationNotes))
                .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Supplier));
               

            Mapper.CreateMap<Supplier,DTO>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                 .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            Mapper.CreateMap<YeastDto,Yeast>()
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ProductCode, conf => conf.MapFrom(rec => rec.ProductCode))
                .ForMember(dto => dto.TemperatureLow, conf => conf.MapFrom(rec => rec.TemperatureLow))
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type))
                .ForMember(dto => dto.Notes, conf => conf.MapFrom(rec => rec.Notes))
                .ForMember(dto => dto.Flocculation, conf => conf.MapFrom(rec => rec.Flocculation))
                .ForMember(dto => dto.AlcoholTolerance, conf => conf.MapFrom(rec => rec.AlcoholTolerance))
                .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Supplier))
                .ForMember(dto => dto.SupplierId, conf => conf.ResolveUsing<YeastSupplierResolver>());

            Mapper.CreateMap<YeastDto, YeastStepDto>()
                .ForMember(dto => dto.YeastId, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ProductCode, conf => conf.MapFrom(rec => rec.ProductCode))
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type))
                .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Supplier));
        }
    }
}