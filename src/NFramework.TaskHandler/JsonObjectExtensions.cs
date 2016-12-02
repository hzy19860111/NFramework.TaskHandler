using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler
{
    public class JsonObjectExtensions
    {
        public static T ToObject<T>(object obj)
        {
            return ((JObject)obj).ToObject<T>();
        }

        public static T[] ToArray<T>(object obj)
        {
            return ((JArray)obj).ToObject<T[]>();
        }
    }
}
