using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler
{
    public abstract class TaskHandlerBase<T> : ITaskHandler<T> where T : TaskMessageBase
    {
        private ITaskQueue<T> taskQueue;
        protected ILog log;
        public TaskHandlerBase(ITaskQueue<T> taskQueue)
        {
            this.taskQueue = taskQueue;
            this.log = LogManager.GetLogger(typeof(TaskHandlerBase<T>).Name);
        }

        public void Handle(T message)
        {
            TaskResult taskResult = InternalHandle(message);
            //处理成功 或 处理失败并且未设置重试的消息，回写TaskResult
            if (taskResult.Success || (!taskResult.Success && !message.Retried))
            {
                if (message.SetTaskResult)
                    taskQueue.SetTaskResult(message, taskResult);
            }
            else
            {
                message.Retry();
                log.InfoFormat("【{0}_{1}】 消息开始重试，消息Id：{2}，当前RetryCount：{3}", taskQueue.TaskQueueType, taskQueue.TaskQueueIndex.ToString(), message.Id, message.RetryCount);
            }
        }

        protected abstract TaskResult InternalHandle(T message);
    }
}
