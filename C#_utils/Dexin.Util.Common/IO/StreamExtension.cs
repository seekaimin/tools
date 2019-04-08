using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Dexin.Util.Common.ExtensionHelper;

namespace Dexin.Util.Common.IO
{
    /// <summary>
    /// Stream的扩展类
    /// </summary>
    public static class StreamExtension
    {
        /// <summary>
        /// 从流中读取一行数据，已byte=10为行结束符
        /// </summary>
        /// <param name="SourceStream"></param>
        /// <returns></returns>
        public static byte[] ReadLineAsBytes(this Stream SourceStream)
        {
            var resultStream = new MemoryStream();
            while (true)
            {
                int data = SourceStream.ReadByte();
                if (data == -1)
                    break;

                resultStream.WriteByte((byte)data);
                if (data == 10)
                    break;
            }

            return resultStream.ToArray();
        }

        /// <summary>
        /// 读取流信息中的所有数据
        /// </summary>
        /// <param name="SourceStream">要读取的流</param>
        /// <param name="receivebuffsize">读取时的缓冲区大小</param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(this Stream SourceStream, int receivebuffsize)
        {
            List<byte> bytes = new List<byte>();
            while (SourceStream.CanRead)
            {
                byte[] buffer = new byte[receivebuffsize];
                int num = SourceStream.Read(buffer, 0, receivebuffsize);
                if (num == 0)
                    break;
                if (num != buffer.Length)
                    buffer = buffer.GetBytes(num);

                bytes.AddRange(buffer);
            }

            return bytes.ToArray();
        }
    }
}
