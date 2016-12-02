using NFramework.TaskHandler.Consts;
using NFramework.TaskHandler.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.Redis
{
    public class RedisTaskResultContainer : RedisStringClient<TaskResult>, ITaskResultContainer
    {
        private string _redisAppName;

        public RedisTaskResultContainer(string redisAppName)
        {
            this._redisAppName = redisAppName;
        }

        protected override string GetRealKey(string key)
        {
            return TaskHandlerConsts.Default_TaskResult_Prefix + key;
        }

        protected override string RedisAppName { get { return this._redisAppName; } }
    }
}
