using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.QuickStart.Core
{
    public class TestTaskThread : TaskThreadBase<TestTaskMessage, TestTaskHandler>
    {
        public TestTaskThread(ITaskQueue<TestTaskMessage> queue, ILog log)
            : base(queue, log)
        {
        }

        public static TestTaskThread Start(ITaskQueue<TestTaskMessage> queue, ILog log)
        {
            TestTaskThread obj = new TestTaskThread(queue, log);
            Thread thread = new Thread(obj.HandleTask);
            thread.IsBackground = true;
            obj.SetThread(thread);
            thread.Start();

            log.InfoFormat("【{0}_{1}】任务处理线程已开启...", queue.TaskQueueType, queue.TaskQueueIndex.ToString());
            return obj;
        }

        protected override TestTaskHandler CreateHandler()
        {
            return new TestTaskHandler(this.taskQueue);
        }
    }
}
