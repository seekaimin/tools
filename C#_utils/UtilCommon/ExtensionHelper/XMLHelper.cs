using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Util.Common.ExtensionHelper
{
    /// <summary>
    /// XML保存接口
    /// </summary>
    public interface IXML<T>
    {
        /// <summary>
        /// 将XML转化为对象
        /// </summary>
        /// <param name="element">xml</param>
        /// <returns>转化的对象</returns>
        T GetItemFormXML(XContainer element);
        /// <summary>
        /// 将对象转化为XML
        /// </summary>
        /// <param name="item">对象</param>
        /// <returns>xml</returns>
        XContainer GetXMLFormItem(T item);
    }

    /// <summary>
    /// XMLHelper
    /// </summary>
    public static class XMLHelper
    {
        /// <summary>
        /// 创建一个新节点
        /// </summary>
        /// <param name="xname">节点名称</param>
        /// <param name="value">值</param>
        /// <returns>XElement</returns>
        public static XElement CreateXElement(this XName xname, object value)
        {
            return xname.CreateXElement(value, null);
        }
        /// <summary>
        /// 创建一个新节点
        /// </summary>
        /// <param name="xname">节点名称</param>
        /// <param name="value">值</param>
        /// <param name="attributes">Attributes</param>
        /// <returns>XElement</returns>
        public static XElement CreateXElement(this XName xname, object value, params KeyValuePair<string, string>[] attributes)
        {
            XElement xelement = new XElement(xname);
            xelement.SetValue(value);
            if (attributes != null && attributes.Length > 0)
            {
                foreach (KeyValuePair<string, string> item in attributes)
                {
                    xelement.SetAttributeValue(item.Key, item.Value);
                }
            }
            return xelement;
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="element">父节点</param>
        /// <param name="xname">子节点名称</param>
        /// <param name="value">子节点值</param>
        /// <param name="isCheckExists">是否检核已经存在</param>
        public static void AddChild(this XElement element, XName xname, object value, bool isCheckExists = false)
        {
            XElement item = null;
            if (isCheckExists)
            {
                item = element.Element(xname);
                if (item == null)
                {
                    item = xname.CreateXElement(value);
                    element.Add(item);
                }
                else
                {
                    item.SetValue(value);
                }
            }
            else
            {
                item = xname.CreateXElement(value);
                element.Add(item);
            }
        }
        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="element">父节点</param>
        /// <param name="xname">子节点名称</param>
        /// <param name="value">子节点值</param>
        /// <param name="isCheckExists">是否检核已经存在</param>
        /// <param name="attributes">attributes</param>
        public static void AddChild(this XElement element, XName xname, object value, bool isCheckExists = false, params KeyValuePair<string, string>[] attributes)
        {
            XElement item = null;
            if (isCheckExists)
            {
                item = element.Element(xname);
                if (item == null)
                {
                    item = xname.CreateXElement(value, attributes);
                    element.Add(item);
                }
                else
                {
                    item.SetValue(value);
                    foreach (KeyValuePair<string, string> attribute in attributes)
                    {
                        if (item.Attribute(attribute.Key) == null)
                        {
                            item.SetAttributeValue(attribute.Key, attribute.Value);
                        }
                        else
                        {
                            item.Attribute(attribute.Key).SetValue(attribute.Value);
                        }
                    }
                }
            }
            else
            {
                item = xname.CreateXElement(value, attributes);
                element.Add(item);
            }
        }





        /// <summary>
        /// 通过子节节点名称获取子节点
        /// </summary>
        /// <param name="element">当前节点</param>
        /// <param name="name">节点名称</param>
        /// <returns>XContainer</returns>
        public static XElement GetChildByName(this XContainer element, XName name)
        {
            return element.Elements().FirstOrDefault(p => p.Name == name);
        }

        /// <summary>
        /// 通过子节节点名称获取子节点
        /// </summary>
        /// <param name="element">当前节点</param>
        /// <param name="name">节点名称</param>
        /// <returns>string</returns>
        public static string GetChildValueByName(this XContainer element, XName name)
        {
            XElement x = element.GetChildByName(name);
            return x == null ? "" : x.Value;
        }


        /// <summary>
        /// 通过子节节点名称获取子节点
        /// </summary>
        /// <param name="element">当前节点</param>
        /// <param name="name">节点名称</param>
        /// <returns>XContainer</returns>
        public static IEnumerable<XElement> GetChildrenByName(this XContainer element, XName name)
        {
            return element.Elements().Where(p => p.Name == name);
        }
        /// <summary>
        /// 获取XAttribute Value
        /// </summary>
        /// <param name="element">当前节点</param>
        /// <param name="name">XAttribute name</param>
        /// <param name="isThrow">是否在XAttribute为空的时候抛出异常</param>
        /// <returns>string</returns>
        public static string GetValueByName(this XElement element, XName name, bool isThrow = false)
        {
            string result = string.Empty;
            XAttribute attr = element.Attributes().FirstOrDefault(p => p.Name == name);
            if (attr == null)
            {
                if (isThrow)
                    throw new ArgumentNullException("name");
            }
            else
            {
                result = attr.Value;
            }
            return result;
        }
        /// <summary>
        /// 递归获取第一批与name相匹配的节点
        /// </summary>
        /// <param name="element">当前节点</param>
        /// <param name="name">XAttribute name</param>
        /// <returns>IEnumerable</returns>
        public static IEnumerable<XElement> RecurrenceFistXContainersByName(this XContainer element, XName name)
        {
            var result = element.Elements().Where(p => p.Name == name);
            if (result.Count() == 0)
            {
                foreach (XContainer item in element.Elements().Where(p => p.Name != name))
                {
                    return item.RecurrenceFistXContainersByName(name);
                }
            }
            return result;
        }



        /// <summary>
        /// 映射XML
        /// </summary>
        /// <param name="demo">T demo</param>
        /// <param name="filePath">file path</param>
        /// <param name="encoding">字符集</param>
        public static void ToXML<T>(this T demo, string filePath, Encoding encoding)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
            //序列化这个对象            
            XmlSerializer serializer = new XmlSerializer(typeof(T));
#if DEBUG
            //将对象序列化输出到控制台            
            serializer.Serialize(Console.Out, demo);
#endif
            using (StreamWriter sw = new StreamWriter(filePath, false, encoding))
            {
                serializer.Serialize(sw, demo);
            }
        }
        /// <summary>
        /// XML转化为实体
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <param name="encoding">字符集</param>
        public static T ToEntity<T>(string filePath, Encoding encoding)
        {
            if (File.Exists(filePath))
                using (StreamReader sr = new StreamReader(filePath, encoding))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(sr);
                }
            else
            {
                return default(T);
            }
        }




        /// <summary>
        /// 获取节点Value
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="name">节点名称</param>
        /// <returns>Value   不存返回  ""</returns>
        public static string GetChildElementValueByName(this XElement root, XName name)
        {
            String result = "";
            XElement node = root.GetChildByName(name);
            if (node != null)
            {
                result = node.Value;
            }
            return result;
        }





    }

}
