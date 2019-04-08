using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace UtilWEB
{
    /// <summary>
    /// web helper
    /// </summary>
    public static class WEBHelper
    {
        /// <summary>
        /// 执行序列化
        /// </summary>
        /// <typeparam name="T">需要序列化的类型</typeparam>
        /// <param name="item">序列化对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeJosn<T>(this T item)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //执行序列化
            return jsonSerializer.Serialize(item);
        }
        /// <summary>
        /// 执行反序列化
        /// </summary>
        /// <typeparam name="T">需要反序列化的类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns>返回反序列化对象</returns>
        public static Object DeserializeJosn<T>(this String json)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //执行反序列化
            return jsonSerializer.Deserialize<T>(json);
        }


        /// <summary>
        /// 将对象转换为 JSON 字符串
        /// </summary>
        /// <param name="item">需要序列化的对象</param>
        /// <returns></returns>
        public static String Serialize(this Object item)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(item);
        }
        /// <summary>
        /// 将指定的 JSON 字符串转换为对象图
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static Object Deserialize(this String json)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.DeserializeObject(json);
        }


    }
}
