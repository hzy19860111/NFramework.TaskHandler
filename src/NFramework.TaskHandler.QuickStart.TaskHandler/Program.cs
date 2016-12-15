using NFramework.TaskHandler.QuickStart.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.QuickStart.TaskHandler
{
    class Program
    {
        public delegate bool ControlCtrlDelegate(int CtrlType);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);
        private static ControlCtrlDelegate cancelHandler = new ControlCtrlDelegate(HandlerRoutine);

        public static bool HandlerRoutine(int CtrlType)
        {
            if (testTaskProcesser != null)
            {
                testTaskProcesser.EnsureStop();
            }

            return false;
        }

        private static TestTaskProcessor testTaskProcesser;

        static void Main(string[] args)
        {
            SetConsoleCtrlHandler(cancelHandler, true);

            FileInfo file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4Net.config"));
            log4net.Config.XmlConfigurator.Configure(file);

            testTaskProcesser = new TestTaskProcessor();
            testTaskProcesser.StartThread();

            Console.ReadLine();
        }
    }
}
