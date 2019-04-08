using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Dexin.Util.Common.Reflection
{
    /// <summary>
    /// EventHandle相关帮助方法
    /// </summary>
    public class EventHandleHelper
    {
        /// <summary>
        /// 获取指定对象的指定事件的所有委托信息
        /// </summary>
        /// <param name="p_Object">检索对象</param>
        /// <param name="p_EventName">事件名称[EventClick]</param>
        /// <param name="p_EventType">类型</param>
        /// <returns></returns>
        public static Delegate[] GetObjectEventList(object p_Object, string p_EventName, Type p_EventType)
        {
            PropertyInfo _PropertyInfo = p_Object.GetType().GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
            if (_PropertyInfo != null)
            {
                object _EventList = _PropertyInfo.GetValue(p_Object, null);
                if (_EventList != null && _EventList is EventHandlerList)
                {
                    EventHandlerList _List = (EventHandlerList)_EventList;
                    FieldInfo _FieldInfo = p_EventType.GetField(p_EventName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                    if (_FieldInfo == null)
                        return null;
                    Delegate _ObjectDelegate = _List[_FieldInfo.GetValue(p_Object)];
                    if (_ObjectDelegate == null)
                        return null;
                    return _ObjectDelegate.GetInvocationList();
                }
            }
            return null;
        }
    }
}
