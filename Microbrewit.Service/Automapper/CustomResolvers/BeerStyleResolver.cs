using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class BeerStyleResolver : ValueResolver<Beer, DTO>
    {
        private IBeerStyleElasticsearch _beerStyleElasticsearch = new BeerStyleElasticsearch();
        private IBeerStyleRepository _beerstyleRespository = new BeerStyleRepository();

        protected override DTO ResolveCore(Beer beer)
        {
            var dto = new DTO();
            if (beer.BeerStyleId != null)
            {
                var beerStyle = _beerStyleElasticsearch.GetSingle((int)beer.BeerStyleId);
                if (beerStyle == null)
                {
                    beerStyle = Mapper.Map<BeerStyle, BeerStyleDto>(_beerstyleRespository.GetSingle(f => f.Id == beer.BeerStyleId));
                }
                dto.Id = beerStyle.Id;
                dto.Name = beerStyle.Name;

                return dto;
            } 
            else
	        {
                return null;
	        }
            

        }
    }
}