using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Api.Automapper
{
    public class FermentableProfile : Profile
    {
        protected override void Configure()
        {

            Mapper.CreateMap<Fermentable, FermentableDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.EBC, conf => conf.MapFrom(rec => rec.EBC))
                .ForMember(dto => dto.PPG, conf => conf.MapFrom(rec => rec.PPG))
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type))
                .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Supplier));
               
            
            Mapper.CreateMap<Supplier,DTO>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            Mapper.CreateMap<FermentableDto,FermentablesCompleteDto>();

            Mapper.CreateMap<FermentablePostDto, Fermentable>()
               .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
               .ForMember(dto => dto.EBC, conf => conf.MapFrom(rec => rec.EBC))
               .ForMember(dto => dto.PPG, conf => conf.MapFrom(rec => rec.PPG))
               .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type))
               .ForMember(dto => dto.SupplierId, conf => conf.MapFrom(rec => rec.Supplier.Id))
               .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Supplier));
          
        }
    }
}