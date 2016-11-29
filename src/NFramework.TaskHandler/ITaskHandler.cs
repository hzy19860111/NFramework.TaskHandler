using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler
{
    public interface ITaskHandler<T> where T : TaskMessageBase
    {
        void Handle(T message);
    }
}
