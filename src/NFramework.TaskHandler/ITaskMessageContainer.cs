using NFramework.TaskHandler.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler
{
    public interface ITaskMessageContainer<T> where T : TaskMessageBase
    {
        IRouting Routing { get; }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="taskQueueCount"></param>
        void Send(T message);

        IEnumerable<ITaskQueue<T>> GetTaskQueues(string queueType, int queueCount);

        ITaskQueue<T> GetTaskQueue(string routingKey, string taskQueueType, int taskQueueCount);

        ITaskQueue<T> GetTaskQueue(string taskQueueType, int taskQueueIndex);
    }
}
