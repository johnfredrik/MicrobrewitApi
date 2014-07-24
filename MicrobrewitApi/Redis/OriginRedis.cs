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
    public class OriginRedis
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async static Task<IList<Origin>> GetOriginsAsync()
        {
            var originsDto = new List<Origin>();
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    var originJson = await redisClient.HashGetAllAsync("origin");
                    foreach (var origin in originJson)
                    {
                        originsDto.Add(JsonConvert.DeserializeObject<Origin>(origin.Value));
                    }
                    return originsDto;
                }
            }
            catch (RedisConnectionException connectionException)
            {
                Log.Debug(connectionException.Message);
                return originsDto;
            }
        }

        public async static Task<Origin> GetOriginAsync(int id)
        {
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    var originJson = await redisClient.HashGetAsync("origin", id);
                    if (!originJson.IsNull)
                    {
                        return JsonConvert.DeserializeObject<Origin>(originJson);
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

        public async static Task UpdateRedisStoreAsync(IList<Origin> origins)
        {
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();

                    foreach (var origin in origins)
                    {
                        await redisClient.HashSetAsync("origin", origin.Id, JsonConvert.SerializeObject(origin), flags: CommandFlags.FireAndForget);
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