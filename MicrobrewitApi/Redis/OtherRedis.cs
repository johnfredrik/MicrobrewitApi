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
    public class OtherRedis
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async static Task<IList<OtherDto>> GetOthersAsync()
        {
            var othersDto = new List<OtherDto>();
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    var otherJson = await redisClient.HashGetAllAsync("other");
                    foreach (var other in otherJson)
                    {
                        othersDto.Add(JsonConvert.DeserializeObject<OtherDto>(other.Value));
                    }
                    return othersDto;
                }
            }
            catch (RedisConnectionException connectionException)
            {
                Log.Debug(connectionException.Message);
                return othersDto;
            }
        }

        public async static Task<OtherDto> GetOtherAsync(int id)
        {
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    var otherJson = await redisClient.HashGetAsync("other", id);
                    if (!otherJson.IsNull)
                    {
                        return JsonConvert.DeserializeObject<OtherDto>(otherJson);
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

        public async static Task UpdateRedisStoreAsync(IList<Other> fermenetables)
        {
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var othersDto = Mapper.Map<IList<Other>, IList<OtherDto>>(fermenetables);
                    var redisClient = redis.GetDatabase();

                    foreach (var other in othersDto)
                    {
                        await redisClient.HashSetAsync("other", other.Id, JsonConvert.SerializeObject(other), flags: CommandFlags.FireAndForget);
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