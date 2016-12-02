using NFramework.TaskHandler.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.Redis
{
    public abstract class RedisTaskMessageBase : TaskMessageBase
    {
        public RedisTaskMessageBase()
            : base()
        {
        }

        public RedisTaskMessageBase(string routingKey)
            : base(routingKey)
        {
        }

        public virtual string RedisAppName { get { return Consts.Default_TaskHandler_RedisAppName; } }

        protected override void InternalSend()
        {
            RedisTaskMessageContainer container = new RedisTaskMessageContainer();
            container.Send(this);
        }

        private ITaskResultContainer _taskResultContainer;
        protected override ITaskResultContainer TaskResultContainer
        {
            get
            {
                if (_taskResultContainer == null)
                    _taskResultContainer = new RedisTaskResultContainer(this.RedisAppName);
                return _taskResultContainer;
            }
        }
    }
}
