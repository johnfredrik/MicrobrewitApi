using System.Configuration;
using System.IO;
using System.Reflection;
using log4net;
using Microbrewit.Model;
using StackExchange.Redis;

namespace Microbrewit.Test
{
    public static class TestUtil
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void DeleteDataInDatabase()
        {
            using(var context = new MicrobrewitContext())
	        {
                var delete = File.ReadAllText(@"..\..\JSON\delete.sql");
                context.Database.ExecuteSqlCommand(delete);
	        }
        }

        public static void InsertDataDatabase()
        {
            using (var context = new MicrobrewitContext())
            {
                var insert = File.ReadAllText(@"..\..\JSON\insert.sql");
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
