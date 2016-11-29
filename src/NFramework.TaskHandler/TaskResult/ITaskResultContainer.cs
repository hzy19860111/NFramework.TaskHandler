using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler
{
    public interface ITaskResultContainer
    {
        void Set(string key, TaskResult value);

        Task<bool> SetAsync(string key, TaskResult value);

        bool Set(string key, TaskResult value, TimeSpan expiresIn);

        Task<bool> SetAsync(string key, TaskResult value, TimeSpan expiresIn);

        bool Set(string key, TaskResult value, DateTime expiresAt);

        Task<bool> SetAsync(string key, TaskResult value, DateTime expiresAt);

        TaskResult Get(string key);

        Task<TaskResult> GetAsync(string key);

        TaskResult GetAndRemove(string key);

        Task<TaskResult> GetAndRemoveAsync(string key);

        bool Remove(string key);

        Task<bool> RemoveAsync(string key);
    }
}
