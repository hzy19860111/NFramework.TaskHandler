using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler
{
    public class TaskResult
    {
        public TaskResult()
        {
            this.Success = true;
        }

        public TaskResult(bool success, string message = null, object data = null)
        {
            this.Success = success;
            this.Message = message;
            this.Data = data;
        }

        public static readonly TaskResult NotSend = new TaskResult(false, "消息未发送至队列！");
        public static readonly TaskResult TimeOut = new TaskResult(false, "操作超时，请稍后查看处理结果！");

        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
