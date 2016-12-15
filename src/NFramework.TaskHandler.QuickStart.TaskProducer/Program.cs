using NFramework.TaskHandler.QuickStart.Core;
using NFramework.TaskHandler.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.QuickStart.TaskProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfo file = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            log4net.Config.XmlConfigurator.Configure(file);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            int totalCount = 10000, successCount = 0, failCount = 0;

            Random random = new Random();

            //循环中使用Task https://github.com/StackExchange/StackExchange.Redis/issues/532 有这个问题，这里初始化下RedisClient中的延迟表达式
            //当然也可以不使用Lazy
            RedisTaskResultContainer container = new RedisTaskResultContainer("NF_TaskHandler");
            container.Get("1");


            for (int i = 0; i < totalCount; i++)
            {
                Task.Run(() =>
                {
                    TestTaskMessage message = new TestTaskMessage();
                    TestMessageData data = new TestMessageData();

                    data.Number1 = random.Next(1, 1000);
                    data.Number2 = random.Next(1, 1000);

                    //设置重试次数3
                    message.SetData(data).SetRetry(3).Send();

                    TaskResult result = message.AwaitTaskResult();

                    if (result.Success)
                    {
                        System.Threading.Interlocked.Add(ref successCount, 1);
                        Console.WriteLine("任务处理成功，{0}+{1}={2}", data.Number1, data.Number2, result.Data);
                    }
                    else
                    {
                        System.Threading.Interlocked.Add(ref failCount, 1);
                        Console.WriteLine("任务处理失败，原因：{0}", result.Message);
                    }
                });
            }

            Task.Run(() =>
            {
                while (true)
                {
                    if (successCount + failCount == totalCount)
                    {
                        Console.WriteLine("TotalCount:{0} SuccessCount:{1} FailCount:{2}  耗时：{3}", totalCount, successCount, failCount, watch.Elapsed);
                        break;
                    }
                    Thread.Sleep(1000);
                }
            });

            Console.ReadLine();
        }
    }
}
