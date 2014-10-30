using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using StackExchange.Redis;
using System.Configuration;
using Microbrewit.Model.DTOs;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections;
using Microbrewit.Model;
using AutoMapper;
using Microbrewit.Repository;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class BeerStyleResolver : ValueResolver<Beer, DTO>
    {
        private Elasticsearch.ElasticSearch _elasticsearch = new Elasticsearch.ElasticSearch();
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