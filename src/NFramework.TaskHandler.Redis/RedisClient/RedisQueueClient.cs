using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.Redis
{
    public abstract class RedisQueueClient<T> : RedisClient
        where T : class
    {
        protected abstract string QueueType { get; }

        public T Pop()
        {
            return this.Execute<T>(database =>
            {
                string value = database.ListRightPop(this.QueueType);
                if (string.IsNullOrWhiteSpace(value))
                    return default(T);

                return JsonConvert.DeserializeObject<T>(value);
            });
        }

        public Task<T> PopAsync()
        {
            return this.Execute<Task<T>>(database =>
                {
                    return Task.Run<T>(() =>
                    {
                        string value = database.ListRightPop(this.QueueType);
                        if (string.IsNullOrWhiteSpace(value))
                            return default(T);

                        return JsonConvert.DeserializeObject<T>(value);
                    });
                });
        }


        public long Push(T message)
        {
            return this.Execute(database =>
             {
                 string value = JsonConvert.SerializeObject(message);
                 return database.ListLeftPush(this.QueueType, value);
             });
        }

        public Task<long> PushAsync(T message)
        {
            return this.Execute(database =>
            {
                return Task.Run(() =>
                 {
                     string value = JsonConvert.SerializeObject(message);
                     return database.ListLeftPushAsync(this.QueueType, value);
                 });
            });
        }

        public long GetQueueLength()
        {
            return this.Execute(database => database.ListLength(this.QueueType));
        }

        public Task<long> GetQueueLengthAsync()
        {
            return this.Execute(database => database.ListLengthAsync(this.QueueType));
        }
    }
}
