using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace Dexin.Util.Common.ConfigurationManager
{
    /// <summary>
    /// 管理一个<[root]><setting name="" value=""/></[root]>格式的xml文件
    /// </summary>
    public class XmlKeyValue
    {
        /// <summary>
        /// 配置文件信息.用于判断配置文件是否自上次读取后更改过
        /// </summary>
        private static FileInfo fileInfo;

        /// <summary>
        /// 配置文件路径
        /// </summary>
        private string _ConfigurationFilePaht, _DefaultConfigurationFilePath;
        private string _DefaultDocumentRootName;

        private XDocument _XDoc;

        private Dictionary<string, string> _XmlKeyValues;
        /// <summary>
        /// 获取所有配置信息
        /// </summary>
        public Dictionary<string, string> XmlKeyValues
        {
            get { return _XmlKeyValues; }
        }

        /// <summary>
        /// 初始化xml读取工具
        /// <[root]><setting name="" value=""/></[root]>
        /// </summary>
        /// <param name="configurationFilePath">文件路径</param>
        /// <param name="defaultConfigurationFilePath">缺省文件路径</param>
        /// <param name="defaultDocumentRootName">跟元素名称(默认值root)</param>
        public XmlKeyValue(string configurationFilePath, string defaultConfigurationFilePath, string defaultDocumentRootName = "root")
        {
            _ConfigurationFilePaht = configurationFilePath;
            _DefaultConfigurationFilePath = defaultConfigurationFilePath;
            _DefaultDocumentRootName = defaultDocumentRootName;

            if (!File.Exists(_DefaultConfigurationFilePath))
            {
                throw new FileNotFoundException("defaultConfigurationFilePath is not exists!");
            }

            Init();
        }

        /// <summary>
        /// 初始化配置信息到内存
        /// </summary>
        private void Init()
        {
            if (!File.Exists(_ConfigurationFilePaht))
            {
                File.Copy(_DefaultConfigurationFilePath, _ConfigurationFilePaht);
            }

            _XmlKeyValues = Load(_ConfigurationFilePaht, _DefaultDocumentRootName);
            _XDoc = XDocument.Load(_ConfigurationFilePaht);

            //记录读取的配置文件信息
            fileInfo = new FileInfo(_ConfigurationFilePaht);
        }

        /// <summary>
        /// 根据name获得Value;
        /// 如果没有找到给定的name会引发异常
        /// </summary>
        /// <param name="name">AttributeName</param>
        /// <returns></returns>
        public string GetXValue(string name)
        {
            try
            {
                FileInfo currFileInfo = new FileInfo(_ConfigurationFilePaht);
                if (!currFileInfo.Exists || fileInfo.LastWriteTimeUtc < currFileInfo.LastWriteTimeUtc)
                {
                    this.Init();
                }

                return _XmlKeyValues[name];
            }
            catch (KeyNotFoundException ex)
            {
                if (string.IsNullOrEmpty(_DefaultConfigurationFilePath))
                    throw ex;

                var defaultXDoc = XDocument.Load(_DefaultConfigurationFilePath);
                var targetElement = defaultXDoc.Elements(_DefaultDocumentRootName).Elements()
                    .SingleOrDefault(x => x.Attribute("name").Value == name);
                if (targetElement != null)
                {
                    SetOrAddXValue(name, targetElement.Attribute("value").Value);
                    SaveAll();

                    return GetXValue(name);
                }
                else
                {
                    try
                    {
                        var defaultXDocInner = XDocument.Load(System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream(string.Format("{0}{1}", System.Reflection.Assembly.GetEntryAssembly().GetName().Name, ".DefaultSetting.xml")));
                        var targetElementInner = defaultXDocInner.Elements(_DefaultDocumentRootName).Elements()
                        .SingleOrDefault(x => x.Attribute("name").Value == name);
                        if (targetElementInner != null)
                        {
                            SetOrAddXValue(name, targetElementInner.Attribute("value").Value);
                            SaveAll();

                            return GetXValue(name);
                        }
                        else
                        {
                            throw new InvalidConfigurationException("", ex);
                        }
                    }
                    catch (Exception innerEx)
                    {
                        throw new InvalidConfigurationException("", innerEx);
                    }
                }
            }
            catch (Exception exx)
            {
                throw new InvalidConfigurationException("", exx);

            }
        }

        /// <summary>
        /// 添加一个配置
        /// </summary>
        /// <param name="name">配置Name</param>
        /// <param name="value">配置Value</param>
        public void AddXValue(string name, string value)
        {
            XElement newel =
                      new XElement("setting", new XAttribute("name", name), new XAttribute("value", value));

            XElement x = _XDoc.Root;
            x.Add(newel);
        }

        /// <summary>
        /// 修改一个存在的配置
        /// 如果没有找到给定的name会引发异常
        /// </summary>
        /// <param name="name">配置Name</param>
        /// <param name="value">配置Value</param>
        public void SetXValue(string name, string value)
        {
            XElement element = _XDoc.Elements(_DefaultDocumentRootName).Elements().FirstOrDefault(p => p.Attribute("name").Value == name);
            if (element != null)
            {
                element.Attribute("value").Value = value;
            }
            else
            {
                throw new ArgumentException("the given name is not exist", "name");
            }
        }

        /// <summary>
        /// 当配置存在时修改配置, 否则新增该配置
        /// </summary>
        /// <param name="name">配置Name</param>
        /// <param name="value">配置Value</param>
        public void SetOrAddXValue(string name, string value)
        {
            XElement element = _XDoc.Elements(_DefaultDocumentRootName).Elements().FirstOrDefault(p => p.Attribute("name").Value == name);
            if (element != null)
            {
                element.Attribute("value").Value = value;
            }
            else
            {
                this.AddXValue(name, value);
            }
        }

        /// <summary>
        /// 保存到配置到文件
        /// </summary>
        public void SaveAll()
        {
            _XDoc.Save(_ConfigurationFilePaht);
            this.Init();
        }

        /// <summary>
        /// 加载配置信息
        /// </summary>
        /// <param name="file">配置文件路劲</param>
        /// <param name="defaultDocumentRootName">跟节点名称</param>
        /// <returns>配置信息</returns>
        public static Dictionary<string, string> Load(string file, string defaultDocumentRootName = "root")
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            XDocument doc = XDocument.Load(file);

            var k = from el in doc.Elements(defaultDocumentRootName).Elements()
                    select el;
            foreach (XElement xe in k)
            {
                res.Add(xe.Attribute("name").Value, xe.Attribute("value").Value);
            }

            return res;
        }

        /// <summary>
        /// 加载配置信息
        /// </summary>
        /// <param name="stream">配置信息流</param>
        /// <param name="defaultDocumentRootName">跟节点名称</param>
        /// <returns>配置信息</returns>
        public static Dictionary<string, string> Load(Stream stream, string defaultDocumentRootName = "root")
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            XDocument doc = XDocument.Load(stream);

            var k = from el in doc.Elements(defaultDocumentRootName).Elements()
                    select el;
            foreach (XElement xe in k)
            {
                res.Add(xe.Attribute("name").Value, xe.Attribute("value").Value);
            }

            return res;
        }
    }
}
