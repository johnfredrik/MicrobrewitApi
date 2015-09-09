using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Automapper.CustomResolvers;

namespace Microbrewit.Service.Automapper
{
    public class BeerProfile : Profile
    {
        protected override void Configure()
        {
            // Creates a mapper for the beer class to a simpler beer class.
            Mapper.CreateMap<Beer, BeerSimpleDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.ABV))
                .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.IBU))
                .ForMember(dto => dto.BeerStyle, conf => conf.ResolveUsing<BeerStyleResolver>())
                .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.SRM));
               
            Mapper.CreateMap<Beer, BeerDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ForkOfId, conf => conf.MapFrom(rec => rec.ForkeOfId))
                .ForMember(dto => dto.ForkOf, conf => conf.ResolveUsing<ForkOfResolver>())
                .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.ABV))
                .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.IBU))
                .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.SRM))
                .ForMember(dto => dto.BeerStyle, conf => conf.ResolveUsing<BeerStyleResolver>())
                .ForMember(dto => dto.Recipe, conf => conf.MapFrom(rec => rec.Recipe))
                .ForMember(dto => dto.Breweries, conf => conf.MapFrom(rec => rec.Breweries))
                .ForMember(dto => dto.Brewers, conf => conf.MapFrom(rec => rec.Brewers));

            Mapper.CreateMap<Recipe, DTO>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.RecipeId));

            Mapper.CreateMap<Brewery, DTO>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BreweryId));

            Mapper.CreateMap<Brewery,BrewerySimpleDto>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BreweryId));


            Mapper.CreateMap<BreweryBeer, BeerDto>()
                 .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                 .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.Beer.IBU))
                 .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.Beer.ABV))
                 .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.Beer.SRM))
                 .ForMember(dto => dto.BeerStyle, conf => conf.MapFrom(rec => rec.Beer.BeerStyle))
                 .ForMember(dto => dto.CreatedDate, conf => conf.MapFrom(rec => rec.Beer.CreatedDate))
                 .ForMember(dto => dto.UpdatedDate, conf => conf.MapFrom(rec => rec.Beer.UpdatedDate))
                 .ForMember(dto => dto.ForkOfId, conf => conf.MapFrom(rec => rec.Beer.ForkeOfId))
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerId));

            Mapper.CreateMap<UserBeer , DTOUser>()
               .ForMember(dto => dto.Username, conf => conf.MapFrom(rec => rec.Username));

            Mapper.CreateMap<UserBeer, BeerSimpleDto>()
                 .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerId));

            Mapper.CreateMap<BreweryBeer, BeerSimpleDto>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.Beer.IBU))
                .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.Beer.ABV))
                .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.Beer.SRM))
                .ForMember(dto => dto.BeerStyle, conf => conf.MapFrom(rec => rec.Beer.BeerStyle))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerId));

            Mapper.CreateMap<UserBeer, BeerDto>()
                 .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                 .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.Beer.IBU))
                 .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.Beer.ABV))
                 .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.Beer.SRM))
                 .ForMember(dto => dto.BeerStyle, conf => conf.MapFrom(rec => rec.Beer.BeerStyle))
                 .ForMember(dto => dto.CreatedDate, conf => conf.MapFrom(rec => rec.Beer.CreatedDate))
                 .ForMember(dto => dto.UpdatedDate, conf => conf.MapFrom(rec => rec.Beer.UpdatedDate))
                 .ForMember(dto => dto.ForkOfId, conf => conf.MapFrom(rec => rec.Beer.ForkeOfId))
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerId));

            Mapper.CreateMap<ABV, ABVDto>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.AbvId, conf => conf.MapFrom(rec => rec.AbvId));
               

            Mapper.CreateMap<IBU, IBUDto>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.Tinseth, conf => conf.MapFrom(rec => rec.Tinseth))
                .ForMember(dto => dto.Rager, conf => conf.MapFrom(rec => rec.Rager));

            Mapper.CreateMap<SRM, SRMDto>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.Mosher, conf => conf.MapFrom(rec => rec.Mosher))
                .ForMember(dto => dto.Daniels, conf => conf.MapFrom(rec => rec.Daniels));

            Mapper.CreateMap<BeerDto,Beer>()
                .ForMember(dto => dto.BeerId, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.ABV))
                .ForMember(dto => dto.BeerStyleId, conf => conf.MapFrom(rec => rec.BeerStyle.Id))
                .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.IBU))
                .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.SRM))
                .ForMember(dto => dto.Recipe, conf => conf.MapFrom(rec => rec.Recipe))
                .ForMember(dto => dto.ForkeOfId, conf => conf.MapFrom(rec => rec.ForkOfId))
                .ForMember(dto => dto.Breweries, conf => conf.MapFrom(rec => rec.Breweries))
                .ForMember(dto => dto.Brewers, conf => conf.ResolveUsing<BeerBrewerResolver>());

            Mapper.CreateMap<BeerDto, BeerSimpleDto>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.ABV))
                .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.IBU))
                 .ForMember(dto => dto.BeerStyle, conf => conf.MapFrom(rec => rec.BeerStyle))
                .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.SRM));


            Mapper.CreateMap<ABVDto, ABV>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.AbvId, conf => conf.MapFrom(rec => rec.AbvId));

            Mapper.CreateMap<IBUDto, IBU>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.Rager, conf => conf.MapFrom(rec => rec.Rager))
                .ForMember(dto => dto.Tinseth, conf => conf.MapFrom(rec => rec.Tinseth))
                .ForMember(dto => dto.IbuId, conf => conf.MapFrom(rec => rec.IbuId));

            Mapper.CreateMap<SRMDto, SRM>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.Mosher, conf => conf.MapFrom(rec => rec.Mosher))
                .ForMember(dto => dto.Daniels, conf => conf.MapFrom(rec => rec.Daniels))
                .ForMember(dto => dto.SrmId, conf => conf.MapFrom(rec => rec.SrmId));

            Mapper.CreateMap<DTO, Brewery>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.BreweryId, conf => conf.MapFrom(rec => rec.Id));

            Mapper.CreateMap<BrewerySimpleDto, Brewery>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.BreweryId, conf => conf.MapFrom(rec => rec.Id));

            Mapper.CreateMap<DTOUser, UserBeer>()
                .ForMember(dto => dto.Username, conf => conf.MapFrom(rec => rec.Username));
               //.ForMember(dto => dto.User.Gravatar, conf => conf.MapFrom(rec => rec.Gravatar));

            Mapper.CreateMap<UserBeer, DTOUser>()
               .ForMember(dto => dto.Username, conf => conf.MapFrom(rec => rec.Username))
               .ForMember(dto => dto.Gravatar, conf => conf.MapFrom(rec => rec.User.Gravatar));

        }
    }
}