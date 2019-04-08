using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using Dexin.Util.Common.IO;
using System.Drawing;
using System.Web;
using Dexin.Util.Common.ExtensionHelper;

namespace Dexin.Util.Common.Net
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// HTTP请求的默认编码
        /// 默认值为UTF8
        /// </summary>
        public static Encoding DEFAULTENCODE = Encoding.UTF8;

        /// <summary>
        /// HttpRequest
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="getResponseBufferSize"></param>
        /// <returns></returns>
        public static HttpResponseData HttpRequest(string url, NameValueCollection data, int getResponseBufferSize = 1024)
        {
            return HttpRequest(url, data, DEFAULTENCODE, getResponseBufferSize);
        }

        /// <summary>
        /// HttpRequest
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <param name="getResponseBufferSize"></param>
        /// <returns></returns>
        public static HttpResponseData HttpRequest(string url, NameValueCollection data, Encoding encoding, int getResponseBufferSize = 1024)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = bs.Length;

            //1.HttpWebRequest
            using (Stream stream = request.GetRequestStream())
            {
                if (data != null)
                {
                    //1.1 key/value
                    StringBuilder sbParam = new StringBuilder();
                    foreach (string key in data.Keys)
                    {
                        sbParam.Append(string.Format("{0}={1}&", key, data[key]));
                    }

                    byte[] paramData = encoding.GetBytes(sbParam.ToString());
                    stream.Write(paramData, 0, paramData.Length);
                }
            }


            //2.WebResponse
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Encoding resEncoding = encoding;
            if (!string.IsNullOrEmpty(response.ContentEncoding))
                resEncoding = Encoding.GetEncoding(response.ContentEncoding);

            return new HttpResponseData(resEncoding, response.GetResponseStream().ReadAllBytes(getResponseBufferSize));
        }

        /// <summary>
        /// HttpRequest
        /// </summary>
        /// <param name="url"></param>
        /// <param name="files"></param>
        /// <param name="data"></param>
        /// <param name="getResponseBufferSize"></param>
        /// <returns></returns>
        public static HttpResponseData HttpRequest(string url, NameValueCollection data, NameValueCollection files, int getResponseBufferSize = 1024)
        {
            return HttpRequest(url, data, files, DEFAULTENCODE, getResponseBufferSize);
        }

        /// <summary>
        /// HttpRequest
        /// </summary>
        /// <param name="url"></param>
        /// <param name="files"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <param name="getResponseBufferSize"></param>
        /// <returns></returns>
        public static HttpResponseData HttpRequest(string url, NameValueCollection data, NameValueCollection files, Encoding encoding, int getResponseBufferSize = 1024)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endbytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            //1.HttpWebRequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;

            using (Stream stream = request.GetRequestStream())
            {
                //1.1 key/value
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                if (data != null)
                {
                    foreach (string key in data.Keys)
                    {
                        stream.Write(boundarybytes, 0, boundarybytes.Length);
                        string formitem = string.Format(formdataTemplate, key, data[key]);
                        byte[] formitembytes = encoding.GetBytes(formitem);
                        stream.Write(formitembytes, 0, formitembytes.Length);
                    }
                }

                //1.2 file
                if (files != null)
                {
                    string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    foreach (string name in files.Keys)
                    {
                        stream.Write(boundarybytes, 0, boundarybytes.Length);
                        string header = string.Format(headerTemplate, "file", Path.GetFileName(files[name]));
                        byte[] headerbytes = encoding.GetBytes(header);
                        stream.Write(headerbytes, 0, headerbytes.Length);
                        using (FileStream fileStream = new FileStream(files[name], FileMode.Open, FileAccess.Read))
                        {
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                stream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                }

                //1.3 form end
                stream.Write(endbytes, 0, endbytes.Length);
            }
            //2.WebResponse
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Encoding resEncoding = encoding;
            if (!string.IsNullOrEmpty(response.ContentEncoding))
                resEncoding = Encoding.GetEncoding(response.ContentEncoding);

            return new HttpResponseData(resEncoding, response.GetResponseStream().ReadAllBytes(getResponseBufferSize));
        }

        /// <summary>
        /// 从HttpListenerRequest中解析request数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="receivebuffsize">读取时的缓冲区大小</param>
        /// <returns></returns>
        public static HttpRequestData ReceiveHttpRequest(HttpListenerRequest request, int receivebuffsize)
        {
            HttpRequestData requestData = new HttpRequestData();
            requestData.RemoteAddress = request.RemoteEndPoint.Address;

            if (string.Compare("GET", request.HttpMethod, true) == 0)
            {
                foreach (var item in request.QueryString.AllKeys)
                {
                    requestData.Params.Add(item, request.QueryString[item]);
                }
            }
            else if (string.Compare("POST", request.HttpMethod, true) == 0)
            {
                #region 处理enctype="multipart/form-data"
                if (request.ContentType.Length > 20 && string.Compare(request.ContentType.Substring(0, 20), "multipart/form-data;", true) == 0)
                {
                    Encoding Encoding = DEFAULTENCODE;
                    string[] values = request.ContentType.Split(';').Skip(1).ToArray();
                    string boundary = string.Join(";", values).Replace("boundary=", "").Trim();//取Head中的分隔符
                    byte[] ChunkBoundary = Encoding.GetBytes("--" + boundary + "\r\n");//变量分隔符
                    byte[] EndBoundary = Encoding.GetBytes("--" + boundary + "--\r\n");//结束符

                    //将直接操作reque.inputstream，改为读取所有信息到内存，操作内存信息，大幅提高执行效率
                    byte[] allBytes = request.InputStream.ReadAllBytes(receivebuffsize);//读取所有数据
                    Stream SourceStream = new MemoryStream(allBytes);
                    //Stream SourceStream = request.InputStream;


                    MemoryStream resultStream = new MemoryStream();
                    bool CanMoveNext = true;
                    Values data = null;
                    int content_type_line_num = 0;
                    while (CanMoveNext)
                    {
                        byte[] currentChunk = SourceStream.ReadLineAsBytes();//读一行数据
                        if (currentChunk.Length == 0)//流已经读完
                            break;
                        if (Encoding.GetString(currentChunk).Equals("\r\n"))
                            content_type_line_num++;//计算换行数量
                        if (!Encoding.GetString(currentChunk).Equals("\r\n") || content_type_line_num >= 2)
                        {
                            resultStream.Write(currentChunk, 0, currentChunk.Length);
                        }


                        if (ArrayHelper.CompareBytes(ChunkBoundary, currentChunk))//如果是变量分隔符表示变量开始或变量结束
                        {
                            content_type_line_num = 0;

                            byte[] result = new byte[resultStream.Length - ChunkBoundary.Length];
                            resultStream.Position = 0;
                            resultStream.Read(result, 0, result.Length);
                            CanMoveNext = true;
                            if (result.Length > 0)
                            {
                                data.datas = result;
                                ParseValue(requestData, Encoding, data);
                            }

                            data = new Values();
                            resultStream.Dispose();
                            resultStream = new MemoryStream();

                        }
                        else if (Encoding.GetString(currentChunk).Contains("Content-Disposition"))//获取参数中的变量名称
                        {
                            byte[] result = new byte[resultStream.Length - 2];
                            resultStream.Position = 0;
                            resultStream.Read(result, 0, result.Length);
                            CanMoveNext = true;
                            string line = Encoding.GetString(result);
                            data.name = line.Substring(line.IndexOf("name")).Split('=')[1].Replace("\"", "");
                            data.file_name = line.Substring(line.IndexOf("filename")).Split('=')[1].Replace("\"", "");
                            resultStream.Dispose();
                            resultStream = new MemoryStream();
                        }
                        else if (Encoding.GetString(currentChunk).Contains("Content-Type"))//判断参数中是否指示为文件
                        {
                            content_type_line_num = 0;

                            CanMoveNext = true;
                            data.type = ValueType.File;
                            resultStream.Dispose();
                            resultStream = new MemoryStream();
                        }
                        else if (ArrayHelper.CompareBytes(EndBoundary, currentChunk))//判断是否为结束符
                        {
                            byte[] result = new byte[resultStream.Length - EndBoundary.Length - 2];
                            resultStream.Position = 0;
                            resultStream.Read(result, 0, result.Length);
                            data.datas = result;
                            ParseValue(requestData, request.ContentEncoding, data);

                            resultStream.Dispose();
                            CanMoveNext = false;
                        }
                    }
                }
                #endregion
                #region 其他
                else
                {
                    string strPara = "";
                    using (StreamReader sr = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        strPara = sr.ReadToEnd();
                    }

                    foreach (string item in strPara.Split('&'))
                    {
                        string[] keyvalue = item.Split('=');
                        requestData.Params.Add(keyvalue[0], keyvalue[1]);
                    }
                }
                #endregion
            }

            return requestData;
        }

        private static void ParseValue(HttpRequestData postData, Encoding Encoding, Values data)
        {
            if (data != null)
            {
                if (data.type == ValueType.Normal)
                {
                    postData.Params.Add(data.name, Encoding.GetString(data.datas));
                }
                else if (data.type == ValueType.File)
                {
                    postData.Files.Add(new HttpFile(data.name, data.file_name, data.datas));
                }
            }
        }

        private class Values
        {
            public ValueType type = ValueType.Normal;
            public string name;
            public string file_name;
            public byte[] datas;
        }

        private enum ValueType
        {
            /// <summary>
            /// 普通参数
            /// </summary>
            Normal = 0,
            /// <summary>
            /// 文件
            /// </summary>
            File = 1,
        }

        public static string HttpRequest(string url, string referUrl, IDictionary<string, string> parameters, IDictionary<string, string> headers, MethodType method)
        {
            if (method.ToString().ToLower() == "post")
            {
                HttpWebRequest req = null;
                HttpWebResponse rsp = null;
                System.IO.Stream reqStream = null;
                try
                {
                    req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = method.ToString();
                    req.KeepAlive = false;
                    req.ProtocolVersion = HttpVersion.Version10;
                    req.Timeout = 5000;
                    req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    req.Referer = referUrl;
                    if (headers != null)
                    {
                        foreach (var v in headers)
                        {
                            req.Headers.Add(v.Key, v.Value);
                        }
                    }
                    byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters, "utf8"));

                    reqStream = req.GetRequestStream();
                    reqStream.Write(postData, 0, postData.Length);
                    rsp = (HttpWebResponse)req.GetResponse();
                    Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                    return GetResponseAsString(rsp, encoding);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (reqStream != null) reqStream.Close();
                    if (rsp != null) rsp.Close();
                }
            }
            else
            {
                //创建请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?" + BuildQuery(parameters, "utf8"));

                //GET请求
                request.Method = "GET";
                request.ReadWriteTimeout = 5000;
                request.ContentType = "text/html;charset=UTF-8";
                if (headers != null)
                {
                    foreach (var v in headers)
                    {
                        request.Headers.Add(v.Key, v.Value);
                    }
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

                //返回内容
                string retString = myStreamReader.ReadToEnd();
                return retString;
            }
        }
        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        private static string BuildQuery(IDictionary<string, string> parameters, string encode)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;
            if (parameters == null)
            {
                return "";
            }
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name))//&& !string.IsNullOrEmpty(value)
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(name);
                    postData.Append("=");
                    if (encode == "gb2312")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.GetEncoding("gb2312")));
                    }
                    else if (encode == "utf8")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    }
                    else
                    {
                        postData.Append(value);
                    }
                    hasParam = true;
                }
            }
            return postData.ToString();
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        private static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            System.IO.Stream stream = null;
            StreamReader reader = null;
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }
        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        private static Image GetResponseAsImage(HttpWebResponse rsp)
        {
            System.IO.Stream stream = null;
            StreamReader reader = null;
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                Image bitmap = Image.FromStream(stream);
                return bitmap;
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }
    }
    public enum MethodType
    {
        Post,
        Get,
    }
    /// <summary>
    /// 表示HTTP请求中附加的参数和文件信息
    /// </summary>
    public class HttpRequestData
    {
        /// <summary>
        /// 摘要:
        /// 在派生类中重写时，获取 System.Web.HttpRequest.QueryString、System.Web.HttpRequest.Form、System.Web.HttpRequest.ServerVariables
        /// 和 System.Web.HttpRequest.Cookies 项的组合集合。
        /// </summary>
        public NameValueCollection Params { get; private set; }

        /// <summary>
        ///  客户端上载的文件。System.Web.HttpFileCollection 对象中的项的类型为 System.Web.HttpPostedFile。
        /// </summary>
        public IList<HttpFile> Files { get; private set; }

        /// <summary>
        /// 请求的远程地址
        /// </summary>
        public IPAddress RemoteAddress { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public HttpRequestData()
        {
            Params = new NameValueCollection();
            Files = new List<HttpFile>();
        }
    }

    /// <summary>
    /// HTTP上传的文件对象
    /// </summary>
    public class HttpFile
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">文件上传的键值</param>
        /// <param name="fileFullName">文件可能的全路径</param>
        /// <param name="fileContent">文件内容</param>
        internal HttpFile(string name, string fileFullName, byte[] fileContent)
        {
            this.Name = name;
            this.FullFileName = fileFullName;
            this.FileContent = fileContent;
        }

        /// <summary>
        /// 文件上传的键值
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 文件可能的全路径
        /// </summary>
        public string FullFileName { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get
            {
                return new FileInfo(FullFileName).Name;
            }
        }
        /// <summary>
        /// 文件内容
        /// </summary>
        public byte[] FileContent { get; set; }
    }

    /// <summary>
    /// HTTP的响应数据
    /// </summary>
    public class HttpResponseData
    {
        /// <summary>
        /// 响应内容编码
        /// </summary>
        public Encoding ContentEncoding { get; set; }

        /// <summary>
        /// 响应内容
        /// </summary>
        public byte[] ResponseData { get; set; }

        /// <summary>
        /// Response的数据
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="data"></param>
        public HttpResponseData(Encoding encoding, byte[] data)
        {
            ContentEncoding = encoding;
            ResponseData = data;
        }

        /// <summary>
        /// 获取Response字符串
        /// </summary>
        /// <returns></returns>
        public string GetEncodingString()
        {
            return ContentEncoding.GetString(ResponseData);
        }
    }
}
