using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.Redis
{
    public class Consts
    {
        /// <summary>
        /// TaskHandler 默认RedisAppName
        /// </summary>
        public const string Default_TaskHandler_RedisAppName = "NF_TaskHandler";

        /// <summary>
        /// TaskResult 默认Key前缀
        /// </summary>
        public const string Default_TaskResult_Prefix = "NF:TaskHandler:TaskResult:";

        /// <summary>
        /// TaskQueue默认 前缀
        /// </summary>
        public const string Default_TaskQueue_Prefix = "NF:TaskHandler:TaskQueue:";
    }
}
