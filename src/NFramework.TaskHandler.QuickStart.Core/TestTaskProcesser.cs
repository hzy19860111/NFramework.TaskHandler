using NFramework.TaskHandler.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.QuickStart.Core
{
    public class TestTaskProcesser : TaskProcesserBase<TestTaskMessage, TestTaskHandler, TestTaskThread>
    {
        protected override string TaskQueueType
        {
            get { return Consts.TestTaskType; }
        }

        /// <summary>
        /// 这里可通过重写 配置 任务队列数量
        /// </summary>
        protected override int TaskQueueCount
        {
            get
            {
                return Consts.TestTaskQueueCount;
            }
        }

        protected override ITaskMessageContainer<TestTaskMessage> CreateTaskMessageContainer()
        {
            return new RedisTaskMessageContainer<TestTaskMessage>();
        }

        protected override TestTaskThread CreateTaskThread(ITaskQueue<TestTaskMessage> queue, log4net.ILog log)
        {
            return new TestTaskThread(queue, log);
        }
    }
}
