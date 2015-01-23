using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class BeerStyleResolver : ValueResolver<Beer, DTO>
    {
        private ElasticSearch _elasticsearch = new ElasticSearch();
        private IBeerStyleRepository _beerstyleRespository = new BeerStyleRepository();

        protected override DTO ResolveCore(Beer beer)
        {
            var dto = new DTO();
            if (beer.BeerStyleId != null)
            {
                var beerStyle = _elasticsearch.GetBeerStyle((int)beer.BeerStyleId).Result;
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