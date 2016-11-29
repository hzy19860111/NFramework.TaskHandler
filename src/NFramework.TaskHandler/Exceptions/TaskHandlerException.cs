using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.Exceptions
{
    public class TaskHandlerException : Exception
    {
        public TaskHandlerException()
            : base()
        {
        }
        public TaskHandlerException(string message)
            : base(message)
        {
        }

        public TaskHandlerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
