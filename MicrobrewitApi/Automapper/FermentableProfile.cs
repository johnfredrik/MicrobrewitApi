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
    public class FermentableProfile : Profile
    {
        protected override void Configure()
        {

            Mapper.CreateMap<Fermentable, FermentableDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Lovibond, conf => conf.MapFrom(rec => rec.Lovibond))
                .ForMember(dto => dto.PPG, conf => conf.MapFrom(rec => rec.PPG))
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type))
                .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Supplier));
               
            
            Mapper.CreateMap<Supplier,DTO>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            Mapper.CreateMap<Supplier, SupplierDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            Mapper.CreateMap<FermentableDto,FermentablesCompleteDto>();

            Mapper.CreateMap<FermentableDto, Fermentable>()
               .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
               .ForMember(dto => dto.PPG, conf => conf.MapFrom(rec => rec.PPG))
               .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type))
               .ForMember(dto => dto.SupplierId, conf => conf.ResolveUsing<FermentableSupplierResolver>())
               .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Supplier));
          
            Mapper.CreateMap<DTO,Supplier>()
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            Mapper.CreateMap<SupplierDto, Supplier>()
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));
        }
    }
}