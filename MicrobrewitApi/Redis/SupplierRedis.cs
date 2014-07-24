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
    public class SupplierRedis
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async static Task<IList<SupplierDto>> GetSuppliersAsync()
        {
            var suppliersDto = new List<SupplierDto>();
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    var supplierJson = await redisClient.HashGetAllAsync("supplier");
                    foreach (var supplier in supplierJson)
                    {
                        suppliersDto.Add(JsonConvert.DeserializeObject<SupplierDto>(supplier.Value));
                    }
                    return suppliersDto;
                }
            }
            catch (RedisConnectionException connectionException)
            {
                Log.Debug(connectionException.Message);
                return suppliersDto;
            }
        }

        public async static Task<SupplierDto> GetSupplierAsync(int id)
        {
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var redisClient = redis.GetDatabase();
                    var supplierJson = await redisClient.HashGetAsync("supplier", id);
                    if (!supplierJson.IsNull)
                    {
                        return JsonConvert.DeserializeObject<SupplierDto>(supplierJson);
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

        public async static Task UpdateRedisStoreAsync(IList<Supplier> fermenetables)
        {
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {
                    var suppliersDto = Mapper.Map<IList<Supplier>, IList<SupplierDto>>(fermenetables);
                    var redisClient = redis.GetDatabase();

                    foreach (var supplier in suppliersDto)
                    {
                        await redisClient.HashSetAsync("supplier", supplier.Id, JsonConvert.SerializeObject(supplier), flags: CommandFlags.FireAndForget);
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