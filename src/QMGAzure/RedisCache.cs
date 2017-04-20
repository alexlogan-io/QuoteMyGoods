/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace QMGAzure
{
    public class RedisCache
    {
        private IDatabase _cache;
        private static string _connectionString;
        public static ConnectionMultiplexer _connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        public RedisCache(string connectionString)
        {
            _connectionString = connectionString;
            _cache = _connection.GetDatabase();
        }

        public RedisCache() { }

        public void SetString(string key, string value)
        {
            _cache.StringSetAsync(key, value);
        }

        public async Task<string> GetString(string key)
        {
            return await _cache.StringGetAsync(key);
        }

        public void SetObject(string key, object value)
        {
            _cache.StringSetAsync(key, JsonConvert.SerializeObject(value));
        }

        public async Task<T> GetObject<T>(string key)
        {
            var obj = await _cache.StringGetAsync(key);
            if(!obj.IsNull)
            {
                return JsonConvert.DeserializeObject<T>(obj);
            }else
            {
                return default(T);
            }            
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(_connectionString);
        });
    }
}
*/
