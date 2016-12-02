using NFramework.TaskHandler.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.QuickStart.Core
{
    public class TestTaskMessage : RedisTaskMessageBase
    {
        public TestTaskMessage()
            : base()
        {
        }

        public TestTaskMessage(string routingKey)
            : base(routingKey)
        {
        }

        ///// <summary>
        ///// 这里重写RedisAppName
        ///// </summary>
        //public override string RedisAppName
        //{
        //    get
        //    {
        //        return "CustomRedisAppName";
        //    }
        //}

        /// <summary>
        /// 这里通过重写 配置 任务队列数量
        /// </summary>
        public override int TaskQueueCount
        {
            get
            {
                return Consts.TestTaskQueueCount;
            }
        }

        /// <summary>
        /// 任务类型：不能重复
        /// </summary>
        public override string TaskType
        {
            get { return Consts.TestTaskType; }
        }
    }
}
