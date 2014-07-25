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
    public class FermentableRedis
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async static Task<IList<FermentableDto>> GetFermentablesAsync()
        {
            var fermentablesDto = new List<FermentableDto>();
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    var fermentableJson = await redisClient.HashGetAllAsync("fermentable");
                    foreach (var fermentable in fermentableJson)
                    {
                        fermentablesDto.Add(JsonConvert.DeserializeObject<FermentableDto>(fermentable.Value));
                    }
                    return fermentablesDto;
                }
            }
            catch (RedisConnectionException connectionException)
            {
                Log.Debug(connectionException.Message);
                return fermentablesDto;
            }
        }

        public async static Task<FermentableDto> GetFermentableAsync(int id)
        {
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    var fermentableJson = await redisClient.HashGetAsync("fermentable", id);
                    if (!fermentableJson.IsNull)
                    {
                        return JsonConvert.DeserializeObject<FermentableDto>(fermentableJson);
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

        public async static Task UpdateRedisStoreAsync(IList<FermentableDto> fermenetables)
        {
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    
                    var redisClient = redis.GetDatabase();

                    foreach (var fermentable in fermenetables)
                    {
                        await redisClient.HashSetAsync("fermentable", fermentable.Id, JsonConvert.SerializeObject(fermentable), flags: CommandFlags.FireAndForget);
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