using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.Exceptions
{
    public class TaskQueueException : Exception
    {
        public TaskQueueException()
            : base()
        {
        }
        public TaskQueueException(string message)
            : base(message)
        {
        }

        public TaskQueueException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
