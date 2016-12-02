using NFramework.TaskHandler.Consts;
using NFramework.TaskHandler.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.Redis
{
    public class RedisTaskQueue<T> : RedisQueueClient<T>, ITaskQueue<T>
        where T : TaskMessageBase
    {
        private string _redisAppName;
        private string _taskQueueType;
        private int _taskQueueIndex;
        private ITaskResultContainer _taskResultContainer;
        public ITaskResultContainer TaskResultContainer { get { return _taskResultContainer; } }

        public RedisTaskQueue(string redisAppName, string taskQueueType, int taskQueueIndex)
        {
            if (string.IsNullOrWhiteSpace(redisAppName))
                throw new ArgumentNullException("redisAppName不能为空！");
            if (string.IsNullOrWhiteSpace(taskQueueType))
                throw new ArgumentNullException("taskQueueType不能为空！");

            this._redisAppName = redisAppName;
            this._taskQueueType = taskQueueType;
            this._taskQueueIndex = taskQueueIndex;
            this._taskResultContainer = new RedisTaskResultContainer(this._redisAppName);
        }

        public int TaskQueueIndex { get { return _taskQueueIndex; } }
        public string TaskQueueType { get { return _taskQueueType; } }
        protected override string RedisAppName { get { return this._redisAppName; } }

        protected override string QueueType
        {
            get { return Consts.Default_TaskQueue_Prefix + _taskQueueType + ":" + _taskQueueIndex.ToString(); }
        }

        public bool SetTaskResult(T message, TaskResult result)
        {
            //默认10分钟过期
            return TaskResultContainer.Set(message.Id, result, TimeSpan.FromMinutes(10));
        }

        public Task<bool> SetTaskResultAsync(T message, TaskResult result)
        {
            return TaskResultContainer.SetAsync(message.Id, result, TimeSpan.FromMinutes(10));
        }
    }
}
