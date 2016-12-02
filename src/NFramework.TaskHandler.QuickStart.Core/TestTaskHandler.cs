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
                TestMessageData data = JsonObjectExtensions.ToObject<TestMessageData>(message.Data);
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
