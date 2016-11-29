using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.Routing
{
    /// <summary>
    /// 路由接口
    /// </summary>
    public interface IRouting
    {
        /// <summary>
        /// 给定路由Key，路由具体的serverIndex（从0开始）
        /// </summary>
        /// <param name="routingKey">路由Key</param>
        /// <param name="serverCount">服务器数量</param>
        /// <returns></returns>
        int Route(string routingKey, int serverCount);
    }
}
