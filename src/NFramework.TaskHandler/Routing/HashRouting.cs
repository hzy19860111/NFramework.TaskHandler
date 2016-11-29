using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler.Routing
{
    /// <summary>
    /// 根据哈希值路由
    /// </summary>
    public class HashRouting : IRouting
    {
        public int Route(string routingKey, int serverCount)
        {
            if (string.IsNullOrWhiteSpace(routingKey))
                return 0;

            if (serverCount < 2)
                return 0;

            int hashCode = routingKey.GetHashCode();
            //当hashCode为-2147483648时，调用Math.Abs()会溢出，这里处理下
            if (hashCode == int.MinValue)
                hashCode += serverCount;
            return Math.Abs(hashCode) % serverCount;
        }
    }
}
