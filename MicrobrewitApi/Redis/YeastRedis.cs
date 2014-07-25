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
    public class YeastRedis
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async static Task<IList<YeastDto>> GetYeastsAsync()
        {
            var yeastsDto = new List<YeastDto>();
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    var yeastJson = await redisClient.HashGetAllAsync("yeast");
                    foreach (var yeast in yeastJson)
                    {
                        yeastsDto.Add(JsonConvert.DeserializeObject<YeastDto>(yeast.Value));
                    }
                    return yeastsDto;
                }
            }
            catch (RedisConnectionException connectionException)
            {
                Log.Debug(connectionException.Message);
                return yeastsDto;
            }
        }

        public async static Task<IList<YeastDto>> GetYeastsAsync(string search)
        {
            var yeastsDto = new List<YeastDto>();
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    var yeastJson = await redisClient.HashGetAllAsync("yeast");
                    foreach (var yeast in yeastJson)
                    {
                        yeastsDto.Add(JsonConvert.DeserializeObject<YeastDto>(yeast.Value));
                    }
                    return yeastsDto;
                }
            }
            catch (RedisConnectionException connectionException)
            {
                Log.Debug(connectionException.Message);
                return yeastsDto;
            }
        }

        public async static Task<YeastDto> GetYeastAsync(int id)
        {
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    var yeastJson = await redisClient.HashGetAsync("yeast", id);
                    if (!yeastJson.IsNull)
                    {
                        return JsonConvert.DeserializeObject<YeastDto>(yeastJson);
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

        public async static Task UpdateRedisStoreAsync(IList<YeastDto> yeasts)
        {
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    
                    var redisClient = redis.GetDatabase();

                    foreach (var yeast in yeasts)
                    {
                        await redisClient.HashSetAsync("yeast", yeast.Id, JsonConvert.SerializeObject(yeast), flags: CommandFlags.FireAndForget);
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