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
    /// <summary>
    /// Helper class to handle redis calls for hops.
    /// </summary>
    public static class HopsRedis
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Gets all hops from redis store.
        /// </summary>
        /// <param name="hopsDto"></param>
        /// <returns>List of Hops as HopDto</returns>
        public static async Task<IList<HopDto>> GetHopsRedis()
        {
            var hopsDto = new List<HopDto>(); 
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    var hopsJson = await redisClient.HashGetAllAsync("hops");
                    foreach (var hopJson in hopsJson.ToList())
                    {
                        hopsDto.Add(JsonConvert.DeserializeObject<HopDto>(hopJson.Value));
                    }
                    return hopsDto;
                }
            }
            catch (RedisConnectionException connectionException)
            {
                Log.Debug(connectionException.Message);
                return hopsDto;     
            }
           
        }
        /// <summary>
        /// Gets single hops from the redis store.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Single Hop as HopDto</returns>
        public async static Task<HopDto> GetHopRedis(int id)
        {
            
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    var hopJson = await redisClient.HashGetAsync("hops", id);
                    if (!hopJson.IsNull)
                    {
                        return JsonConvert.DeserializeObject<HopDto>(hopJson);
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
        
        public async static Task UpdateRedisStore(IList<HopDto> hops)
        {
          
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    

                    var redisClient = redis.GetDatabase();

                    foreach (var hop in hops)
                    {
                        await redisClient.HashSetAsync("hops", hop.Id, JsonConvert.SerializeObject(hop), flags: CommandFlags.FireAndForget);
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