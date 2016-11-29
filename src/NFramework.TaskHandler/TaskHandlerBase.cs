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
            //TaskResult为NULL 为失败重试，不需要设置结果
            if (taskResult != null)
                taskQueue.SetTaskResult(message, taskResult);
        }

        protected abstract TaskResult InternalHandle(T message);
    }
}
