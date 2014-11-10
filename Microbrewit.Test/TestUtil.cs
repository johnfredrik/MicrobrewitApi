using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using System.Configuration;
using StackExchange.Redis;
using log4net;

namespace Microbrewit.Test
{
    public static class TestUtil
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void DeleteDataInDatabase()
        {
            using(var context = new MicrobrewitContext())
	        {
                var delete = System.IO.File.ReadAllText(@"..\..\JSON\delete.sql");
                context.Database.ExecuteSqlCommand(delete);
	        }
        }

        public static void InsertDataDatabase()
        {
            using (var context = new MicrobrewitContext())
            {
                var insert = System.IO.File.ReadAllText(@"..\..\JSON\insert.sql");
                context.Database.ExecuteSqlCommand(insert);
            }
        }
        //using (var conn = new RedisConnection(server, port, -1, password,
        //  allowAdmin: true)) 
        public static void FlushRedisStore()
        {
            try
            {
                using (var redis = ConnectionMultiplexer.Connect(redisStore + ",allowAdmin=true"))
                {
                    var redisServer = redis.GetServer(redisStore, 6379);
                    redisServer.FlushAllDatabases();
                }
            }
            catch (RedisConnectionException)
            {
                Log.Debug("Redis connection not found");
            } 
        }
    }
}
