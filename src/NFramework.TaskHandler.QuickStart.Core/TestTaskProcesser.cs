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
