using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;
using Util.Common.ExtensionHelper;

namespace Util.Common.Nets.Sync
{
    /// <summary>
    /// http server
    /// </summary>
    public class HTTPServer : NetServerBase
    {
        /// <summary>
        /// 监听端口
        /// </summary>
        public int Port { get; protected set; }
        /// <summary>
        /// 构造
        /// </summary>
        public HTTPServer(int port)
        {
            this.Port = port;
        }

        /// <summary>
        /// 客户端处理程序
        /// </summary>
        public event HTTPServerExecuteHandle Handle;

        /// <summary>
        /// 监听对象
        /// </summary>
        public HttpListener Listener { get; protected set; }
        /// <summary>
        /// This example requires the System and System.Net namespaces.
        /// </summary>
        /// <param name="prefixes"></param>
        void SimpleListenerExample(params string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("Listening...");
            // Note: The GetContext method blocks while waiting for a request. 
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;

            Console.WriteLine(request);

            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            // Construct a response.
            string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();
            listener.Stop();
        }
        /// <summary>
        /// 启动htp服务
        /// </summary>
        /// <param name="port">监听端口</param>
        /// <param name="prefixes">服务器名称</param>
        public override void Start()
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            if (this.Running == true)
            {
                return;

            }
            Listener = new HttpListener();
            Listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;//指定身份验证 Anonymous匿名访问
            string templete = "http://{0}:{1}/";
            string uri = templete.Fmt("*", this.Port);
            Listener.Prefixes.Add(uri);
            this.Running = true;
            Listener.Start();
            //异步处理连接客户端
            Listener.BeginGetContext(new AsyncCallback(AcceptHttpClient), this);
        }
        /// <summary>
        /// 异步处理客户端连接
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptHttpClient(IAsyncResult ar)
        {
            HTTPServer server = (HTTPServer)ar.AsyncState;
            if (server.Running)
            {
                try
                {
                    server.Listener.BeginGetContext(new AsyncCallback(server.AcceptHttpClient), ar.AsyncState);
                }
                catch (Exception ex) { }
            }
            HttpListenerContext context = null;
            try
            {
                context = server.Listener.EndGetContext(ar);
                context.Response.StatusCode = 200;//设置返回给客服端http状态代码
                if (server.Handle != null)
                {
                    server.Handle(context);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (context != null)
                    {
                        context.Response.StatusCode = 200;
                        byte[] o = Encoding.UTF8.GetBytes(ex.Message);
                        context.Response.OutputStream.Write(o, 0, o.Length);
                    }
                }
                catch (Exception e) { }
            }
        }

        private void TaskProc(object o)
        {
            //HttpListenerContext ctx = (HttpListenerContext)o;
            //ctx.Response.StatusCode = 200;//设置返回给客服端http状态代码
            //if (DoHttpListenerContext != null)
            //{
            //    DoHttpListenerContext(ctx);
            //}
            //string type = ctx.Request.QueryString["type"];
            //string userId = ctx.Request.QueryString["userId"];
            ////string password = ctx.Request.QueryString["password"];
            ////string filename = Path.GetFileName(ctx.Request.RawUrl);
            ////string userName = HttpUtility.ParseQueryString(filename).Get("userName");//避免中文乱码

            ////进行处理

            ////使用Writer输出http响应代码
            //using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream))
            //{
            //    writer.Write("ok");
            //    writer.Close();
            //    ctx.Response.Close();
            //}
        }


        /// <summary>
        /// 停止服务
        /// </summary>
        public override void Stop()
        {
            this.DoDispose();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            if (this.Running)
            {

                this.Running = false;
                Listener.DoDispose();
            }
        }
    }
    /// <summary>
    /// 定义HTTP客户端操作句柄
    /// </summary>
    /// <param name="context">请求上下文对象</param>
    public delegate void HTTPServerExecuteHandle(HttpListenerContext context);


}











