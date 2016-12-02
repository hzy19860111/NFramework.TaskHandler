using log4net;
using NFramework.TaskHandler.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NFramework.TaskHandler
{
    public abstract class TaskProcesserBase<TMessage, TTaskHandler, TTaskThread>
        where TMessage : TaskMessageBase
        where TTaskHandler : TaskHandlerBase<TMessage>
        where TTaskThread : TaskThreadBase<TMessage, TTaskHandler>
    {
        protected List<TTaskThread> Threads { get; private set; }
        protected bool Stopped { get; private set; }
        protected ITaskMessageContainer<TMessage> MessageContainer { get; private set; }
        protected static readonly ILog Log = LogManager.GetLogger("BaseTaskProcess");
        protected abstract string TaskQueueType { get; }
        protected virtual int TaskQueueCount { get { return TaskQueueConsts.Default_TaskQueue_Count; } }
        private System.Timers.Timer timer;

        public TaskProcesserBase()
        {
            this.MessageContainer = this.CreateTaskMessageContainer();
            this.Threads = new List<TTaskThread>();
        }

        protected abstract ITaskMessageContainer<TMessage> CreateTaskMessageContainer();

        public void StartThread()
        {
            Log.InfoFormat("开始启动任务处理【队列类型:{0} 队列数量:{1}】", this.TaskQueueType, TaskQueueCount.ToString());
            var queues = MessageContainer.GetTaskQueues(TaskQueueType, TaskQueueCount);
            foreach (var queue in queues)
            {
                Threads.Add(StartThread(queue, Log));
            }

            //守护线程，确保启动的线程处于工作状态(每60秒检查一次）
            this.timer = new System.Timers.Timer(1000 * 60) { AutoReset = true };
            timer.Elapsed += (sender, evertArgs) =>
            {
                if (this.Stopped)
                    return;
                this.EnsureAlive();
            };
            timer.Start();
        }

        protected TTaskThread StartThread(ITaskQueue<TMessage> queue, ILog log)
        {
            TTaskThread obj = CreateTaskThread(queue, log);
            Thread thread = new Thread(obj.HandleTask);
            thread.IsBackground = true;
            obj.SetThread(thread);

            log.InfoFormat("【{0}_{1}】任务处理线程已开启...", queue.TaskQueueType, queue.TaskQueueIndex.ToString());

            thread.Start();
            return obj;
        }

        protected abstract TTaskThread CreateTaskThread(ITaskQueue<TMessage> queue, ILog log);

        public void EnsureAlive()
        {
            Threads.ForEach(thread => thread.EnsureAlive());
        }

        public void Stop()
        {
            Threads.ForEach(thread => thread.Stop());
            this.Stopped = true;
        }

        public void Restart()
        {
            this.Stopped = false;
            Threads.ForEach(thread => thread.EnsureAlive());
        }

        public void EnsureStop()
        {
            if (!this.Stopped)
                Stop();

            while (Threads.Count(thread => thread.IsAlive()) >= 0)
            {

            }
        }

        public int AliveThreadCount()
        {
            return Threads.Count(thread => thread.IsAlive());
        }
    }
}
