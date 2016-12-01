using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler
{
    public interface ITaskQueue<T> where T : TaskMessageBase
    {
        ITaskResultContainer TaskResultContainer { get; }

        string TaskQueueType { get; }

        int TaskQueueIndex { get; }

        T Pop();

        Task<T> PopAsync();

        long Push(T message);

        Task<long> PushAsync(T message);

        long GetQueueLength();

        Task<long> GetQueueLengthAsync();

        bool SetTaskResult(T message, TaskResult result);

        Task<bool> SetTaskResultAsync(T message, TaskResult result);
    }
}
