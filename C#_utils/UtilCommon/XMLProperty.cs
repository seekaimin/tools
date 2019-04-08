using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using Util.Common.ExtensionHelper;

namespace Util.Common
{
    /// <summary>
    /// 配置信息
    /// </summary>
    [Serializable]
    public abstract class XMLProperty
    {
        /// <summary>
        /// 属性节点 名称 : default = Property
        /// </summary>
        public const String NodeName = "Property";
        /// <summary>
        /// Key
        /// </summary>
        public const String KeyName = "Key";
        /// <summary>
        /// Value
        /// </summary>
        public const String ValueName = "Value";
        /// <summary>
        /// Comment
        /// </summary>
        public const String CommentName = "Comment";




        /// <summary>
        /// 文档
        /// </summary>
        public XDocument Doc { get; set; }
        /// <summary>
        /// 根节点
        /// </summary>
        public XElement Root { get; set; }


        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (Validate())
            {
                Doc = XDocument.Load(FilePath);
                Root = Doc.Root;
                Read();
            }
        }
        /// <summary>
        /// Read
        /// </summary>
        protected abstract void Read();
        /// <summary>
        /// Write
        /// </summary>
        protected abstract void Write();
        /// <summary>
        /// 检核
        /// </summary>
        /// <returns></returns>
        public abstract bool Validate();


        /// <summary>
        /// 文件路径
        /// </summary>
        public virtual string FilePath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "properties.xml"); }
        }


        /// <summary>
        /// 获取第一级属性
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public string Get(string key)
        {
            var item = Root.GetNodeByKey(key);
            if (item == null) return null;
            var attr = item.GetAttribute(XMLProperty.ValueName);
            if (attr == null) return null;
            return attr.Value;
        }

        /// <summary>
        /// 设置属性信息
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="comment">comment</param>
        public void Set(string key, string value, string comment, Dictionary<string, string> attrs)
        {
            lock (Root)
            {
                var item = Root.GetNodeByKey(key);
                if (item == null)
                {
                    item = new XElement(XMLProperty.NodeName);
                    item.SetAttributeValue(XMLProperty.KeyName, key);
                    Root.Add(item);
                }
                item.InitElement(key, value, comment, attrs);
            }
        }
        /// <summary>
        /// 设置属性信息
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="comment">comment</param>
        public void Set(string key, string value)
        {
            lock (Root)
            {
                var item = Root.GetNodeByKey(key);
                if (item == null)
                {
                    item = new XElement(XMLProperty.NodeName);
                    item.SetAttributeValue(XMLProperty.KeyName, key);
                    item.SetAttributeValue(XMLProperty.CommentName, "");
                    Root.Add(item);
                }
                item.SetAttributeValue(XMLProperty.ValueName, value);
            }
        }

        /// <summary>
        /// 保存属性信息
        /// </summary>
        public void Save()
        {
            Write();
            Doc.Save(FilePath);
        }
        /// <summary>
        /// 检测配置文件信息是否存在
        /// </summary>
        /// <param name="dllpath">dll库路径</param>
        /// <param name="fileapath">文件相对库路径</param>
        public virtual void CheckPropertiesFile(string dllpath, string fileapath)
        {
            if (!File.Exists(FilePath))
            {
                Assembly assembly = Assembly.LoadFrom(dllpath);
                //恢复默认配置
                using (Stream stream = assembly.GetManifestResourceStream(fileapath))
                {
                    byte[] buff = new byte[stream.Length];
                    stream.Read(buff, 0, buff.Length);
                    using (StreamWriter sw = new StreamWriter(FilePath, false))
                    {
                        sw.Write(Encoding.UTF8.GetString(buff));
                    }
                }
            }
        }
    }

    /// <summary>
    /// 帮助类
    /// </summary>
    public static class XmlPropertyHelper
    {

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="item">节点对象</param>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <param name="comment">描述</param>
        /// <param name="attrs">其他属性</param>
        public static void InitElement(this XElement item, String key, object value, string comment, Dictionary<string, string> attrs)
        {
            item.SetAttributeValue(XMLProperty.KeyName, key);
            item.SetAttributeValue(XMLProperty.ValueName, value.ToStr());
            if (false == comment.IsNullOrEmpty())
            {
                item.SetAttributeValue(XMLProperty.CommentName, comment);
            }
            if (attrs != null)
            {
                foreach (string k in attrs.Keys)
                {
                    item.SetAttributeValue(k, attrs[k]);
                }
            }
        }
        /// <summary>
        /// 获取当前节点下的所有节点
        /// </summary>
        /// <param name="_key">key</param>
        /// <param name="parent">父级节点</param>
        /// <returns>节点信息</returns>
        public static List<XElement> GetNodesByKey(this XElement parent, string key)
        {
            List<XElement> result = new List<XElement>();
            #region 获取子节点信息
            foreach (XElement element in parent.Elements())
            {
                if (element.IsContainKey(key))
                {
                    result.Add(element);
                }
            }
            #endregion
            return result;
        }
        /// <summary>
        /// 获取当前节点下的所有节点
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="parent">父级节点</param>
        /// <returns>节点信息</returns>
        public static XElement GetNodeByKey(this XElement parent, string key)
        {
            foreach (XElement element in parent.Elements())
            {
                if (element.IsContainKey(key))
                {
                    return element;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取 XAttribute
        /// </summary>
        /// <param name="node">节点信息</param>
        /// <param name="name">name</param>
        /// <returns>XAttribute</returns>
        public static XAttribute GetAttribute(this XElement node, string name)
        {
            return node.Attribute(name);
        }
        /// <summary>
        /// 获取 XAttribute  Value 节点对应的值
        /// </summary>
        /// <param name="node">节点信息</param>
        /// <param name="name">name</param>
        /// <returns>value</returns>
        public static string GetAttributeValue(this XElement node)
        {
            var attr = node.GetAttribute(XMLProperty.ValueName);
            return attr == null ? "" : attr.Value;
        }
        /// <summary>
        /// 是否包含 XAttribute
        /// </summary>
        /// <param name="node">节点信息</param>
        /// <param name="name">name</param>
        /// <param name="value">value</param>
        /// <returns>bool</returns>
        public static bool IsContainKey(this XElement node, string value)
        {
            XAttribute attr = node.GetAttribute(XMLProperty.KeyName);
            if (attr == null)
                return false;
            return attr.Value.Equals(value);
        }
    }

}