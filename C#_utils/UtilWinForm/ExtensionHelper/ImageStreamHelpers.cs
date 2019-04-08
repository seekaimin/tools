using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace Util.WinForm.ExtensionHelper
{
    /// <summary>
    /// 图片 流 扩展
    /// </summary>
    public static class ImageStreamHelpers
    {
        /// <summary>
        /// 将图片转化为buff
        /// </summary>
        /// <param name="image">图片信息</param>
        /// <param name="imgformat">转换格式</param>
        /// <returns></returns>
        public static byte[] ImageToBuff(this Image image, System.Drawing.Imaging.ImageFormat imgformat)
        {            
            List<byte> buff = new List<byte>();
            using (MemoryStream stream = image.ImageToStream(imgformat))
            {
                buff.AddRange(stream.ToArray());
            }
            return buff.ToArray();
        }
        /// <summary>
        /// 将buff转化为图片信息
        /// </summary>
        /// <param name="buff">buff信息</param>
        /// <returns></returns>
        public static Image BuffToImage(this byte[] buff)
        {
            Image image = null;
            using (Stream stream = new MemoryStream(buff))
            {
                image = stream.StreamToImage();
            }
            return image;
        }
        /// <summary>
        /// 将流转化为图片信息
        /// </summary>
        /// <param name="stream">stream信息</param>
        /// <param name="isclosestream">是否关闭流</param>
        /// <returns></returns>
        public static Image StreamToImage(this Stream stream, bool isclosestream = false)
        {
            Image image = Image.FromStream(stream);
            if (isclosestream)
            {
                stream.Close();
            }
            return image;
        }
        /// <summary>
        /// 将图片转化为流  使用完后必须关闭流
        /// </summary>
        /// <param name="image">图片信息</param>
        /// <param name="imgformat">转换格式</param>
        /// <returns></returns>
        public static MemoryStream ImageToStream(this Image image, System.Drawing.Imaging.ImageFormat imgformat)
        {
            MemoryStream stream = new MemoryStream();
            image.Save(stream, imgformat);
            return stream;
        }
    }
}
