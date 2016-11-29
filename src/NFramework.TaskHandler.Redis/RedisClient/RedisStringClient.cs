using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.Redis
{
    public abstract class RedisStringClient<T> : RedisClient
        where T : class,new()
    {
        public void Set(string key, T value)
        {
            this.Execute(database => database.StringSet(this.GetRealKey(key), value));
        }

        public Task<bool> SetAsync(string key, T value)
        {
            return this.Execute(database => database.StringSetAsync<T>(this.GetRealKey(key), value));
        }

        public bool Set(string key, T value, TimeSpan expiresIn)
        {
            return this.Execute(database => database.StringSet(this.GetRealKey(key), value, expiresIn));
        }

        public Task<bool> SetAsync(string key, T value, TimeSpan expiresIn)
        {
            return this.Execute(database => database.StringSetAsync<T>(this.GetRealKey(key), value, expiresIn));
        }

        public bool Set(string key, T value, DateTime expiresAt)
        {
            return this.Execute(database => database.StringSet(this.GetRealKey(key), value, expiresAt));
        }

        public Task<bool> SetAsync(string key, T value, DateTime expiresAt)
        {
            return this.Execute(database => database.StringSetAsync<T>(this.GetRealKey(key), value, expiresAt));
        }

        public T Get(string key)
        {
            return this.Execute<T>(database => database.StringGet<T>(this.GetRealKey(key)));
        }

        public Task<T> GetAsync(string key)
        {
            return this.Execute(database => database.StringGetAsync<T>(this.GetRealKey(key)));
        }

        public T GetAndRemove(string key)
        {
            return this.Execute<T>(database =>
            {
                string redisKey = this.GetRealKey(key);

                T value = database.StringGet<T>(redisKey);
                if (value != null)
                    database.KeyDelete(redisKey);

                return value;
            });
        }

        public Task<T> GetAndRemoveAsync(string key)
        {
            return Task.Run(() => GetAndRemove(key));
        }

        public bool Remove(string key)
        {
            return this.Execute(database => database.KeyDelete(this.GetRealKey(key)));
        }

        public Task<bool> RemoveAsync(string key)
        {
            return this.Execute(database => database.KeyDeleteAsync(this.GetRealKey(key)));
        }
    }
}
