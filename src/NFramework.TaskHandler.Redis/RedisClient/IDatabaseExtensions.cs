using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.Redis
{
    public static class IDatabaseExtensions
    {
        public static T StringGet<T>(this IDatabase database, string key)
        {
            string value = database.StringGet(key);
            if (string.IsNullOrWhiteSpace(value)) return default(T);

            return JsonConvert.DeserializeObject<T>(value);
        }

        public static Task<T> StringGetAsync<T>(this IDatabase database, string key)
        {
            return Task.Run(() => StringGet<T>(database, key));
        }

        public static bool StringSet<T>(this IDatabase database, string key, T value)
        {
            if (value == null) return false;
            string redisValue = JsonConvert.SerializeObject(value);

            return database.StringSet(key, redisValue);
        }

        public static bool StringSet<T>(this IDatabase database, string key, T value, TimeSpan expiresIn)
        {
            if (value == null) return false;
            string redisValue = JsonConvert.SerializeObject(value);

            return database.StringSet(key, redisValue, expiry: expiresIn);
        }

        public static bool StringSet<T>(this IDatabase database, string key, T value, DateTime expiresAt)
        {
            return StringSet(database, key, value, expiresAt - DateTime.Now);
        }

        public static Task<bool> StringSetAsync<T>(this IDatabase database, string key, T value)
        {
            return Task.Run(() => StringSet<T>(database, key, value));
        }

        public static Task<bool> StringSetAsync<T>(this IDatabase database, string key, T value, TimeSpan expiresIn)
        {
            return Task.Run(() => StringSet<T>(database, key, value, expiresIn));
        }

        public static Task<bool> StringSetAsync<T>(this IDatabase database, string key, T value, DateTime expiresAt)
        {
            return Task.Run(() => StringSet<T>(database, key, value, expiresAt - DateTime.Now));
        }
    }
}
