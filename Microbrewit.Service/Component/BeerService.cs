using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using log4net;
using Microbrewit.Api.Service.Util;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch.Interface;
using Microbrewit.Service.Interface;
using Nest;
using Newtonsoft.Json;

namespace Microbrewit.Service.Component
{
    public class BeerService : IBeerService
    {
        private IUserService _userService;
        private IBeerElasticsearch _beerElasticsearch;
        private IBeerRepository _beerRepository;
        private IBreweryService _breweryService;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string[] _include = {
                "Recipe.SpargeStep",
                "Recipe.MashSteps.Hops",
                "Recipe.MashSteps.Fermentables",
                "Recipe.MashSteps.Others",
                "Recipe.SpargeStep.Hops",
                "Recipe.BoilSteps.Hops",
                "Recipe.BoilSteps.Fermentables",
                "Recipe.BoilSteps.Others",
                "Recipe.FermentationSteps.Hops",
                "Recipe.FermentationSteps.Fermentables",
                "Recipe.FermentationSteps.Others",
                "Recipe.FermentationSteps.Yeasts",
                "Forks.ABV",
                "Forks.BeerStyle",
                "Forks.IBU",
                "Forks.SRM",
                "ABV", "IBU", "SRM", "Brewers.User", "Breweries"};

        public BeerService(IBeerElasticsearch beerElasticsearch, IBeerRepository beerRepository)
        {
            _beerElasticsearch = beerElasticsearch;
            _beerRepository = beerRepository;
        }
        public BeerService(IBeerElasticsearch beerElasticsearch, IBeerRepository beerRepository, IUserService userService,IBreweryService breweryService)
        {
            _beerElasticsearch = beerElasticsearch;
            _beerRepository = beerRepository;
            _userService = userService;
            _breweryService = breweryService;
        }

        public async Task<IEnumerable<BeerDto>> GetAllAsync(int from, int size)
        {
            var beerDtos = await _beerElasticsearch.GetAllAsync();
            if (beerDtos.Any()) return beerDtos;
            var beers = await _beerRepository.GetAllAsync(_include);
            return Mapper.Map<IEnumerable<Beer>, IEnumerable<BeerDto>>(beers);
        }

        public async Task<BeerDto> GetSingleAsync(int id)
        {
            var beerDto = await _beerElasticsearch.GetSingleAsync(id);
            if (beerDto != null) return beerDto;
            var beer = await _beerRepository.GetSingleAsync(o => o.Id == id,_include);
            return Mapper.Map<Beer, BeerDto>(beer);
        }

        public async Task<BeerDto> AddAsync(BeerDto beerDto)
        {
            var beer = Mapper.Map<BeerDto, Beer>(beerDto);
            if (beerDto.Recipe != null)
            {
                BeerCalculations(beer);
            }
            beer.BeerStyle = null;
            beer.CreatedDate = DateTime.Now;
            beer.UpdatedDate = DateTime.Now;

            await _beerRepository.AddAsync(beer);
            var result = await _beerRepository.GetSingleAsync(o => o.Id == beer.Id,_include);
            var mappedResult = Mapper.Map<Beer, BeerDto>(result);
            await _beerElasticsearch.UpdateAsync(mappedResult);
            // elasticsearch relation managing
            if (mappedResult.ForkOfId != null)
                await ReIndexSingleElasticSearchAsync((int)mappedResult.ForkOfId);
            if(mappedResult.Brewers.Any())
                await _userService.ReIndexBeerRelationElasticSearch(beerDto);
            if(mappedResult.Breweries.Any())
                await _breweryService.ReIndexBeerRelationElasticSearch(beerDto);
            return mappedResult;

        }

        public async Task<BeerDto> AddAsync(BeerDto beerDto, string username)
        {
            if (beerDto.Brewers.All(b => b.Username != username))
            {
                if (beerDto.Breweries.Any())
                {
                    var breweryMemberships = _breweryService.GetMemberships(username);
                    if (beerDto.Breweries.Any(brewery => breweryMemberships.Any(b => b.BreweryId != brewery.Id)))
                        return null;
                }
                else
                {
                    beerDto.Brewers.Add(new DTOUser { Username = username});   
                }
                
            }
            var returnBeer  =await AddAsync(beerDto);
            await _userService.UpdateNotification(username, new NotificationDto {Id = returnBeer.Id,Type = "UserBeer",Value = true});
            return returnBeer;
        }

        public async Task<BeerDto> DeleteAsync(int id)
        {
            var beer = await _beerRepository.GetSingleAsync(o => o.Id == id);
            var beerDto = await _beerElasticsearch.GetSingleAsync(id);
            if(beer != null) await _beerRepository.RemoveAsync(beer);
            if (beerDto == null) return beerDto;
            await _beerElasticsearch.DeleteAsync(id);
            // elasticsearch relation managing
            await _userService.ReIndexBeerRelationElasticSearch(beerDto);
            await _breweryService.ReIndexBeerRelationElasticSearch(beerDto);
            return beerDto;
        }

        public async Task UpdateAsync(BeerDto beerDto)
        {
            if (beerDto.Recipe != null) beerDto.Recipe.Id = beerDto.Id;
            var beer = Mapper.Map<BeerDto, Beer>(beerDto);
            
            if (beer.Recipe != null)
            {
                BeerCalculations(beer);
            }
            //Log.Debug(JsonConvert.SerializeObject(beer));
            await _beerRepository.UpdateAsync(beer);
            var result = await _beerRepository.GetSingleAsync(o => o.Id == beerDto.Id,_include);
            var mappedResult = Mapper.Map<Beer, BeerDto>(result);
            await _beerElasticsearch.UpdateAsync(mappedResult);
            // elasticsearch relation managing
            if (mappedResult.Brewers.Any())
                await _userService.ReIndexBeerRelationElasticSearch(beerDto);
            if (mappedResult.Breweries.Any())
                await _breweryService.ReIndexBeerRelationElasticSearch(beerDto);
        }

        public async Task<IEnumerable<BeerDto>> SearchAsync(string query, int @from, int size)
        {
            return await _beerElasticsearch.SearchAsync(query,from,size);
        }

        public async Task<IEnumerable<BeerDto>> GetUserBeersAsync(string username)
        {
            var beersDto = await _beerElasticsearch.GetUserBeersAsync(username);
            if (beersDto != null) return beersDto;
            var beers = await _beerRepository.GetAllUserBeerAsync(username,
                //"Recipe.MashSteps",
                "Recipe.MashSteps.Hops",
                "Recipe.MashSteps.Fermentables",
                "Recipe.MashSteps.Others",
                // "Recipe.BoilSteps",
                "Recipe.BoilSteps.Hops",
                "Recipe.BoilSteps.Fermentables",
                "Recipe.BoilSteps.Others",
                // "Recipe.FermentationSteps",
                "Recipe.FermentationSteps.Hops",
                "Recipe.FermentationSteps.Fermentables",
                "Recipe.FermentationSteps.Others",
                "Recipe.FermentationSteps.Yeasts",
                // "Forks"
                "Forks.ABV",
                "Forks.BeerStyle",
                "Forks.IBU",
                "Forks.SRM",
                "ABV", "IBU", "SRM", "Brewers.User", "Breweries");
             return Mapper.Map<IEnumerable<Beer>, IEnumerable<BeerDto>>(beers);
        }

        public async Task ReIndexElasticSearch()
        {
            var beers = await _beerRepository.GetAllAsync(_include);
            try
            {
                var beerDtos = Mapper.Map<IList<Beer>, IEnumerable<BeerDto>>(beers);
                await _beerElasticsearch.UpdateAllAsync(beerDtos);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        public async Task ReIndexSingleElasticSearchAsync(int beerId)
        {
            var beer = await _beerRepository.GetSingleAsync(b => b.Id == beerId, _include);
            if (beer == null) return;
            var beerDto = Mapper.Map<Beer, BeerDto>(beer);
            await _beerElasticsearch.UpdateAsync(beerDto);

        }

        public async Task<IEnumerable<BeerDto>> GetLastAsync(int @from, int size)
        {
            var lastBeersDto = await _beerElasticsearch.GetLastAsync(from, size);
            if (lastBeersDto != null) return lastBeersDto;
            var lastBeers = await _beerRepository.GetLastAsync(from,size,
                //"Recipe.MashSteps",
                "Recipe.MashSteps.Hops",
                "Recipe.MashSteps.Fermentables",
                "Recipe.MashSteps.Others",
                // "Recipe.BoilSteps",
                "Recipe.BoilSteps.Hops",
                "Recipe.BoilSteps.Fermentables",
                "Recipe.BoilSteps.Others",
                // "Recipe.FermentationSteps",
                "Recipe.FermentationSteps.Hops",
                "Recipe.FermentationSteps.Fermentables",
                "Recipe.FermentationSteps.Others",
                "Recipe.FermentationSteps.Yeasts",
                // "Forks"
                "Forks.ABV",
                "Forks.BeerStyle",
                "Forks.IBU",
                "Forks.SRM",
                "ABV", "IBU", "SRM", "Brewers.User", "Breweries");
            return Mapper.Map<IList<Beer>, IEnumerable<BeerDto>>(lastBeers);
        }

        public IEnumerable<BeerDto> GetAllUserBeer(string username)
        {
            var userBeersDto = _beerElasticsearch.GetUserBeers(username);
            if (userBeersDto != null) return userBeersDto;
            var userBeers = _beerRepository.GetAllUserBeer(username);
            return Mapper.Map<IEnumerable<Beer>, IEnumerable<BeerDto>>(userBeers);
        }

        public IEnumerable<BeerDto> GetAllBreweryBeers(int breweryId)
        {
            var beersDto = _beerElasticsearch.GetAllBreweryBeers(breweryId);
            if (beersDto != null) return beersDto;
            var beers = _beerRepository.GetAllBreweryBeers(breweryId);
            return Mapper.Map<IEnumerable<Beer>, IEnumerable<BeerDto>>(beers);
        }

        public BeerDto GetSingle(int beerId)
        {
            var beerDto = _beerElasticsearch.GetSingle(beerId);
            if (beerDto != null) return beerDto;
            var beer = _beerRepository.GetSingle(o => o.Id == beerId,_include);
           
                return Mapper.Map<Beer, BeerDto>(beer);
        }

        private static void BeerCalculations(Beer beer)
        {
            if (beer.Recipe.FG <= 0) beer.Recipe.FG = 1.015;

            beer.Recipe.OG = Calculation.CalculateOG(beer.Recipe);
            var abv = Calculation.CalculateABV(beer.Recipe);
            if (beer.ABV != null)
            {
                abv.Id = beer.ABV.Id;
            }
            beer.ABV = abv;
            var srm = Calculation.CalculateSRM(beer.Recipe);
            if (beer.SRM != null)
            {
                srm.Id = beer.SRM.Id;
            }
            beer.SRM = srm;
            var ibu = Calculation.CalculateIBU(beer.Recipe);
            if (beer.IBU != null)
            {
                ibu.Id = beer.IBU.Id;
            }
            beer.IBU = ibu;
        }

        
    }
}
