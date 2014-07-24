using AutoMapper;
using log4net;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Microbrewit.Api.Redis
{
    public static class BreweryRedis
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public async static Task<IList<BreweryDto>> GetBreweries()
        {
            var breweriesDto = new List<BreweryDto>();
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    var breweriesJson = await redisClient.HashGetAllAsync("brewery");
                    foreach (var breweryJson in breweriesJson)
                    {
                        breweriesDto.Add(JsonConvert.DeserializeObject<BreweryDto>(breweryJson.Value));
                    }
                    return breweriesDto;
                }
            }
            catch (RedisConnectionException connectionException)
            {
                Log.Debug(connectionException.Message);
                return breweriesDto;
            }
        }

        public async static Task<BreweryDto> GetBrewery(int id)
        {
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    var breweryJson = await redisClient.HashGetAsync("brewery",id);
                    if (!breweryJson.IsNull)
                    {
                        return JsonConvert.DeserializeObject<BreweryDto>(breweryJson);
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (RedisConnectionException connectionException)
            {
                Log.Debug(connectionException.Message);
                return null;
            }
        }

        public async static Task UpdateRedisStore(IList<Brewery> breweries)
        {
            var breweriesDto = Mapper.Map<IList<Brewery>,IList<BreweryDto>>(breweries);
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    foreach (var brewery in breweriesDto)
                    {
                        await redisClient.HashSetAsync("brewery", brewery.Id, JsonConvert.SerializeObject(brewery), flags: CommandFlags.FireAndForget);
                    }
                }
            }
            catch (RedisConnectionException connectionException)
            {
                Log.Debug(connectionException.Message);
            }
        }
    }
}