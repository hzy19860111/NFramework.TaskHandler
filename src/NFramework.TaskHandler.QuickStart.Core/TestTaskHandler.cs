using Newtonsoft.Json.Linq;
using NFramework.TaskHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.QuickStart.Core
{
    public class TestTaskHandler : TaskHandlerBase<TestTaskMessage>
    {
        public TestTaskHandler(ITaskQueue<TestTaskMessage> taskQueue)
            : base(taskQueue)
        {
        }

        protected override TaskResult InternalHandle(TestTaskMessage message)
        {
            try
            {
                //由于使用Newtonsoft.Json序列化，Message中的Object会反序列化为JObject对象，需要特殊处理
                TestMessageData data = JsonObjectExtensions.ToObject<TestMessageData>((JObject)message.Data);
                return new TaskResult(true, data: (data.Number1 + data.Number2).ToString());
            }
            catch (TaskHandlerException ex)
            {
                log.Error(ex.Message, ex);
                return new TaskResult(false, ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new TaskResult(false, "服务器内部错误，请与技术人员联系！");
            }
        }
    }
}
