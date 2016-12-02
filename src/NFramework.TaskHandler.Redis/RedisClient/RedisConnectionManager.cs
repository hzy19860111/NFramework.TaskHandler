using NFramework.TaskHandler.Exceptions;
using NFramework.TaskHandler.Redis.Configurations;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.Redis
{
    public class RedisConnectionManager
    {
        private RedisSection _redisSeciton;
        private ConcurrentDictionary<string, ConnectionMultiplexer> _redisConnection;
        private object _syncRoot;

        public RedisConnectionManager()
        {
            this._redisSeciton = ConfigurationManager.GetSection("RedisSection") as RedisSection;
            if (this._redisSeciton == null)
                throw new TaskHandlerException("未找到RedisSection配置信息!");

            this.InitRedisConnection();

            this._syncRoot = new object();
        }

        private void InitRedisConnection()
        {
            this._redisConnection = new ConcurrentDictionary<string, ConnectionMultiplexer>();
            foreach (RedisDB redisDB in this._redisSeciton.RedisDBs)
            {
                this._redisConnection.TryAdd(redisDB.Name, ConnectionMultiplexer.Connect(redisDB.ConnectionString));
            }
        }

        public ConnectionMultiplexer GetConnection(RedisDB redisDB)
        {
            ConnectionMultiplexer connection;

            if (_redisConnection.TryGetValue(redisDB.Name, out connection))
            {
                if (connection.IsConnected)
                    return connection;
                //连接已断开，释放当前连接
                connection.Dispose();
                _redisConnection.TryRemove(redisDB.Name, out connection);
            }

            connection = ConnectionMultiplexer.Connect(redisDB.ConnectionString);
            _redisConnection.TryAdd(redisDB.Name, connection);
            return connection;
        }
        public ConnectionMultiplexer GetConnection(string appName)
        {
            var redisDb = _redisSeciton.GetRedisDBByAppName(appName);
            if (redisDb == null) throw new TaskHandlerException(string.Format("未找到appName:{0} 的RedisDB信息!", appName));

            return GetConnection(redisDb);
        }

        public IDatabase GetDatabase(string appName)
        {
            var redisApp = GetRedisApp(appName);
            var connection = GetConnection(appName);
            return connection.GetDatabase(redisApp.DatabaseIndex);
        }

        private RedisDB GetRedisDBByAppName(string appName)
        {
            var redisDb = _redisSeciton.GetRedisDBByAppName(appName);
            if (redisDb == null) throw new TaskHandlerException(string.Format("未找到appName:{0} 的RedisDB信息!", appName));
            return redisDb;
        }
        private RedisDB GetRedisDB(string dbName)
        {
            var redisDb = _redisSeciton.GetRedisDB(dbName);
            if (redisDb == null) throw new TaskHandlerException(string.Format("未找到Name:{0} 的RedisDB信息!", dbName));
            return redisDb;
        }
        private RedisApp GetRedisApp(string appName)
        {
            var redisApp = _redisSeciton.GetRedisApp(appName);
            if (redisApp == null) throw new TaskHandlerException(string.Format("未找到appName:{0} 的RedisApp信息!", appName));
            return redisApp;
        }
    }
}
