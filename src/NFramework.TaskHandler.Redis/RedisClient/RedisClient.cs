using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.Redis
{
    public abstract class RedisClient
    {
        private static readonly Lazy<RedisConnectionManager> redisManager = new Lazy<RedisConnectionManager>(() => new RedisConnectionManager());
        private static RedisConnectionManager RedisManager { get { return redisManager.Value; } }

        protected abstract string RedisAppName { get; }

        protected virtual string GetRealKey(string key)
        {
            return key;
        }

        protected void Execute(Action<IDatabase> action)
        {
            action(RedisManager.GetDatabase(this.RedisAppName));
        }

        protected T Execute<T>(Func<IDatabase, T> func)
        {
            return func(RedisManager.GetDatabase(this.RedisAppName));
        }
    }
}
