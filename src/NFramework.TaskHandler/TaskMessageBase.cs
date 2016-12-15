using log4net;
using Newtonsoft.Json;
using NFramework.TaskHandler.Consts;
using NFramework.TaskHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler
{
    public abstract class TaskMessageBase
    {
        #region 静态成员

        private static Lazy<ILog> lazyLog = new Lazy<ILog>(() => LogManager.GetLogger(typeof(TaskMessageBase).Name));
        protected static ILog Log { get { return lazyLog.Value; } }

        #endregion

        #region 构造函数

        public TaskMessageBase()
        {
            this.Id = Guid.NewGuid().ToString();
            this.RoutingKey = this.Id;//默认路由key 为消息Id
        }

        public TaskMessageBase(string routingKey)
            : this()
        {
            this.RoutingKey = routingKey;
        }

        #endregion

        /// <summary>
        /// 任务类型
        /// </summary>
        public abstract string TaskType { get; }

        /// <summary>
        /// 消息队列数量
        /// </summary>
        public virtual int TaskQueueCount { get { return TaskQueueConsts.Default_TaskQueue_Count; } }

        #region Required Properties

        /// <summary>
        /// 消息Id，应为不重复的
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 路由Key
        /// </summary>
        public string RoutingKey { get; set; }

        #endregion

        #region CustomData

        /// <summary>
        /// 消息数据
        /// </summary>
        public object Data { get; set; }

        public TaskMessageBase SetData(object data)
        {
            this.Data = data;
            return this;
        }

        #endregion

        #region Retry

        /// <summary>
        /// 是否重试
        /// </summary>
        public bool Retried { get { return RetryCount > 0; } }
        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// 设置重试次数
        /// </summary>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        public TaskMessageBase SetRetry(int retryCount)
        {
            if (retryCount > 0)
            {
                RetryCount = retryCount;
            }

            return this;
        }


        #endregion

        #region Send

        protected virtual bool LogSuccessMessage
        {
            get { return false; }
        }

        public virtual bool SetTaskResult
        {
            get
            {
                return true;
            }
        }

        public void Send()
        {
            try
            {
                InternalSend();
                if (LogSuccessMessage)
                    Log.InfoFormat("消息发送成功,{0}", this.ToString());
            }
            catch (Exception ex)
            {
                string message = string.Format("消息发送失败,{0}", this.ToString());
                throw new TaskQueueException(message, ex);
            }
        }

        public void AfterSend()
        {
            this._hasSent = true;
        }

        protected abstract void InternalSend();

        public void Retry()
        {
            if (this.RetryCount > 0)
            {
                this.RetryCount--;
                this.Send();
            }
        }

        #endregion

        #region TaskResult

        /// <summary>
        /// 是否已发送
        /// </summary>
        private bool _hasSent = false;

        protected abstract ITaskResultContainer TaskResultContainer { get; }

        protected TaskResult GetTaskResult()
        {
            return TaskResultContainer.GetAndRemove(this.Id);
        }

        /// <summary>
        /// 等待任务处理结果，默认超时时间：2分钟
        /// </summary>
        /// <returns></returns>
        public TaskResult AwaitTaskResult()
        {
            return AwaitTaskResult(2 * 60 * 1000);
        }

        /// <summary>
        /// 等待任务处理结果
        /// </summary>
        /// <param name="timeOut">超时时间，单位：毫秒</param>
        /// <returns></returns>
        public TaskResult AwaitTaskResult(int timeOut)
        {
            //设置 不回写TaskResult，直接返回成功的Result
            if (!this.SetTaskResult)
                return new TaskResult();
            //如果未发送消息或未设置redisApp
            if (!this._hasSent)
                return TaskResult.NotSend;

            //最大超时时间5分钟
            if (timeOut > 5 * 60 * 1000)
                timeOut = 5 * 60 * 1000;

            System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource(timeOut);
            //.net framework 4.0
            //System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource();
            //var timer = new System.Timers.Timer(timeOut) { AutoReset = false };
            //timer.Elapsed += (sender, eventArgs) => { cts.Cancel(); };
            //timer.Start();

            return Task.Factory.StartNew<TaskResult>(() =>
            {
                while (!cts.IsCancellationRequested)
                {
                    var result = GetTaskResult();
                    if (result != null)
                        return result;

                    System.Threading.Thread.Sleep(200);
                }
                return TaskResult.TimeOut;
            }).Result;
        }

        #endregion

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
