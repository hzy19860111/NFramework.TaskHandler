using NFramework.TaskHandler.Consts;
using NFramework.TaskHandler.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.Redis
{
    public class RedisTaskMessageContainer<T> : ITaskMessageContainer<T> where T : TaskMessageBase
    {
        private IRouting _iRouting;

        public IRouting Routing
        {
            get { return this._iRouting; }
        }

        public RedisTaskMessageContainer()
        {
            _iRouting = new HashRouting();
        }

        /// <summary>
        /// 可通过构造函数注入自定义路由规则
        /// </summary>
        /// <param name="iRouting"></param>
        public RedisTaskMessageContainer(IRouting iRouting)
        {
            this._iRouting = iRouting;
        }

        public virtual string RedisAppName { get { return Consts.Default_TaskHandler_RedisAppName; } }

        /// <summary>
        /// 根据队列类型、路由Key 推送到队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routingKey"></param>
        /// <param name="queueType"></param>
        /// <param name="message"></param>
        /// <param name="taskQueueCount"></param>
        public void Send(T message)
        {
            var queue = GetTaskQueue(message.RoutingKey, message.TaskType, message.TaskQueueCount);
            queue.Push(message);

            message.SendAfter(this.RedisAppName);
        }

        public IEnumerable<ITaskQueue<T>> GetTaskQueues(string queueType, int queueCount)
        {
            for (int queueIndex = 0; queueIndex < queueCount; queueIndex++)
            {
                yield return GetTaskQueue(queueType, queueIndex);
            }
        }

        public ITaskQueue<T> GetTaskQueue(string routingKey, string taskQueueType, int taskQueueCount)
        {
            //todo:缓存RedisTaskQueue
            return new RedisTaskQueue<T>(this.RedisAppName, taskQueueType, this.Routing.Route(routingKey, taskQueueCount));
        }

        public ITaskQueue<T> GetTaskQueue(string taskQueueType, int taskQueueIndex)
        {
            //todo:缓存RedisTaskQueue
            return new RedisTaskQueue<T>(this.RedisAppName, taskQueueType, taskQueueIndex);
        }
    }

    public class RedisTaskMessageContainer : RedisTaskMessageContainer<TaskMessageBase>
    {
    }
}
