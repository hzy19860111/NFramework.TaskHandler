using log4net;
using NFramework.TaskHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NFramework.TaskHandler
{
    public abstract class TaskThreadBase<TMessage, TTaskHandler>
        where TMessage : TaskMessageBase
        where TTaskHandler : TaskHandlerBase<TMessage>
    {
        protected ITaskQueue<TMessage> taskQueue { get; private set; }
        protected ILog log { get; private set; }
        protected bool Running { get; private set; }
        public Thread CurrentThread { get; private set; }

        public TaskThreadBase(ITaskQueue<TMessage> queue, ILog log)
        {
            this.Running = true;
            this.taskQueue = queue;
            this.log = log;
        }

        protected abstract TTaskHandler CreateHandler();

        public void HandleTask(object obj)
        {
            TMessage message = null;
            TTaskHandler handler = CreateHandler();
            while (Running)
            {
                try
                {
                    message = taskQueue.Pop();

                    if (message != null)
                    {
                        log.InfoFormat("【{0}_{1}】 开始处理消息，消息Id：{2}", taskQueue.TaskQueueType, taskQueue.TaskQueueIndex.ToString(), message.Id);
                        handler.Handle(message);
                        log.InfoFormat("【{0}_{1}】 消息处理成功，消息Id：{2}", taskQueue.TaskQueueType, taskQueue.TaskQueueIndex.ToString(), message.Id);
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
                catch (TaskHandlerException ex)
                {
                    if (message != null && message.Retried)
                    {
                        message.Retry();
                    }
                    log.Error(string.Format("【{0}_{1}】消息处理失败，原因：{2}，消息：{3}", taskQueue.TaskQueueType, taskQueue.TaskQueueIndex.ToString(), ex.Message, message == null ? "" : message.ToString()), ex);
                }
                catch (ThreadAbortException ex)
                {
                    Thread.ResetAbort();
                    log.Error(ex.Message, ex);
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("【{0}_{1}】消息处理失败，原因：{2}，消息：{3}", taskQueue.TaskQueueType, taskQueue.TaskQueueIndex.ToString(), ex.Message, message == null ? "" : message.ToString()), ex);
                }
            }
            log.InfoFormat("【{0}_{1}】停止运行", taskQueue.TaskQueueType, taskQueue.TaskQueueIndex.ToString());
        }

        public void SetThread(Thread thread)
        {
            CurrentThread = thread;
        }

        public virtual bool IsAlive()
        {
            if (CurrentThread != null)
                return CurrentThread.IsAlive;

            return false;
        }

        public virtual void EnsureAlive()
        {
            this.Running = true;

            if (IsAlive())
                return;
            try
            {
                if (this.CurrentThread != null)
                    CurrentThread.Abort();
            }
            catch
            {
            }

            Thread thread = new Thread(this.HandleTask);
            thread.IsBackground = true;
            this.SetThread(thread);
            thread.Start();
        }

        public void Stop()
        {
            Running = false;
        }
    }
}
