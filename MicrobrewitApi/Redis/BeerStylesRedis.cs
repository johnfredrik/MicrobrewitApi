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
    public class BeerStylesRedis
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        public async static Task<IList<BeerStyleDto>> GetBeerStyles()
        {
            var beerStyles = new List<BeerStyleDto>();
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {

                    var redisClient = redis.GetDatabase();
                    var json = await redisClient.HashValuesAsync("beerstyles");
                    foreach (var item in json)
                    {
                        beerStyles.Add(JsonConvert.DeserializeObject<BeerStyleDto>(item));
                    }
                    return beerStyles;
                }
            }
            catch (RedisConnectionException connectionException)
            {
                Log.Debug(connectionException.Message);
                return beerStyles;
            }
            
        }

        public async static Task<BeerStyleDto> GetBeerStyle(int id)
        {
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    var beerstyleJson = await redisClient.HashGetAsync("beerstyle", id);
                    if (!beerstyleJson.IsNull)
                    {
                        return JsonConvert.DeserializeObject<BeerStyleDto>(beerstyleJson);
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

        public async static Task UpdateRedisStore(IList<BeerStyle> beerStyles)
        {
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var beerStylesDto = Mapper.Map<IList<BeerStyle>, IList<BeerStyleDto>>(beerStyles);
                    var redisClient = redis.GetDatabase();
                    foreach (var beerStyle in beerStylesDto)
                    {
                        await redisClient.HashSetAsync("beerstyle",beerStyle.Id, JsonConvert.SerializeObject(beerStyle), flags: CommandFlags.FireAndForget);
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