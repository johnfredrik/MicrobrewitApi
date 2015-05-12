using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Repository.Repository;
using Microbrewit.Service.Elasticsearch;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class BeerStyleResolver : ValueResolver<Beer, BeerStyleSimpleDto>
    {
        private IBeerStyleElasticsearch _beerStyleElasticsearch = new BeerStyleElasticsearch();
        private IBeerStyleRepository _beerstyleRespository = new BeerStyleDapperRepository();

        protected override BeerStyleSimpleDto ResolveCore(Beer beer)
        {
            var beerStyleSimpleDto = new BeerStyleSimpleDto();
            if (beer.BeerStyleId != null)
            {
                var beerStyle = _beerStyleElasticsearch.GetSingle((int)beer.BeerStyleId);
                if (beerStyle == null)
                {
                    beerStyle = Mapper.Map<BeerStyle, BeerStyleDto>(_beerstyleRespository.GetSingle((int)beer.BeerStyleId));
                }
                beerStyleSimpleDto.Id = beerStyle.Id;
                beerStyleSimpleDto.Name = beerStyle.Name;

                return beerStyleSimpleDto;
            } 
            else
	        {
                return null;
	        }
            

        }
    }
}