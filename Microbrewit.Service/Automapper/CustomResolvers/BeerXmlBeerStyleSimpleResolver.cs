using System.Linq;
using AutoMapper;
using Microbrewit.Model.DTOs;
using Microbrewit.Model.BeerXml;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class BeerXmlBeerStyleSimpleResolver : ValueResolver<Recipe, BeerStyleSimpleDto>
    {
        private IBeerStyleElasticsearch _beerStyleElasticsearch = new BeerStyleElasticsearch();
        private IBeerStyleRepository _beerstyleRespository = new BeerStyleRepository();

        protected override BeerStyleSimpleDto ResolveCore(Recipe recipe)
        {
            var beerStyleSimpleDto = new BeerStyleSimpleDto();
            if (recipe.Style != null)
            {
                var beerStyle = _beerStyleElasticsearch.Search(recipe.Style.Name,0,1).FirstOrDefault();
                if (beerStyle == null) return null;
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