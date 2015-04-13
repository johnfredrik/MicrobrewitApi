using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class ForkOfResolver : ValueResolver<Beer, BeerSimpleDto>
    {
        private readonly IBeerElasticsearch _beerElasticsearch = new BeerElasticsearch();
        private readonly IBeerRepository _beerRespository = new BeerRepository();

        protected override BeerSimpleDto ResolveCore(Beer beer)
        {
            BeerSimpleDto beerSimpleDto = null;
            if (beer.ForkeOfId != null)
            {
                var beerDto = _beerElasticsearch.GetSingle((int)beer.ForkeOfId);
                if (beerDto == null)
                {
                    beerSimpleDto = Mapper.Map<Beer, BeerSimpleDto>(_beerRespository.GetSingle(f => f.Id == beer.ForkeOfId));
                }
                else
                {
                    beerSimpleDto = Mapper.Map<BeerDto, BeerSimpleDto>(beerDto);
                }
               
            }
            return beerSimpleDto;

        }
    }
}