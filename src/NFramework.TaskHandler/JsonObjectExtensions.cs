using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework.TaskHandler
{
    public static class JsonObjectExtensions
    {
        public static T ToObject<T>(this JObject jObj)
        {
            return jObj.ToObject<T>();
        }

        public static T[] ToArray<T>(this JArray jArray)
        {
            return jArray.ToArray<T>();
        }
    }
}
