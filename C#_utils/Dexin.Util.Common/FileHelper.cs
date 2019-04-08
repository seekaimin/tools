using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dexin.Util.Common
{
    public class FileHelper
    {
        #region 上传文件
        /// <summary>
        /// 上传文件到WEB服务器
        /// </summary>
        /// <param name="filePosted">来自WEB客户端上传的文件</param>
        /// <param name="MaxSize">限制上传文件的大小（单位字节）</param>
        /// <param name="LimitFileType">限制文件类型</param>
        /// <param name="savePath">保存到服务器路径（格式为AppDomain.CurrentDomain.BaseDirectory + "Uploads/Excel/"）</param>
        /// <param name="msg">返回的消息</param>
        /// <returns></returns>
        public static bool Upload(HttpPostedFileBase filePosted, int MaxSize, FileType LimitFileType, ref string savePathFileName, out string msg)
        {

            string FileName;
            if (filePosted == null || filePosted.ContentLength <= 0)
            {
                msg = string.Format(Resource.NotNull, Resource.File);// "文件不能为空";
                return false;
            }
            else
            {
                string filename = Path.GetFileName(filePosted.FileName);
                int filesize = filePosted.ContentLength;//获取上传文件的大小单位为字节byte
                string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名
                string NoFileName = System.IO.Path.GetFileNameWithoutExtension(filename);//获取无扩展名的文件名
                int Maxsize = MaxSize * 1024;//定义上传文件的最大空间大小为4M
                string strFileType = ".xls,.xlsx";
                string savePath = AppDomain.CurrentDomain.BaseDirectory + @"Uploads\Excel\";
                if (LimitFileType == FileType.EXCEL)
                {
                    strFileType = ".xls,.xlsx";//定义上传文件的类型字符串
                }
                else if (LimitFileType == FileType.XML)
                {
                    strFileType = ".xml";//定义上传文件的类型字符串
                    savePath = AppDomain.CurrentDomain.BaseDirectory + "Uploads/XML/";
                }
                else if (LimitFileType == FileType.TXT)
                {
                    strFileType = ".txt";//定义上传文件的类型字符串
                    savePath = AppDomain.CurrentDomain.BaseDirectory + "Uploads/TXT/";
                }
                FileName = NoFileName + DateTime.Now.ToString("yyyyMMddhhmmss") + fileEx;
                if (!strFileType.ToLower().Contains(fileEx))
                {
                    msg = Resource.FileTypeError;// "文件类型不对，只能导入xls和xlsx格式的文件";
                    return false;
                }
                if (filesize >= Maxsize)
                {
                    msg = Resource.FileSizeLimited;
                    return false;
                }
                savePathFileName = Path.Combine(savePath, FileName);
                filePosted.SaveAs(savePathFileName);
            }
            msg = Resource.FileUploadingSuccess;
            return true;

        }


        public static void Upload(HttpPostedFileBase postFile, string uploadFolder)
        {
            if (postFile == null || postFile.ContentLength <= 0)
                throw new SMSMessageException(string.Format(Resource.NotNull, Resource.File));
            string fileName = new FileInfo(postFile.FileName).Name;
            string fullFileName = System.IO.Path.Combine(uploadFolder, fileName);
            if (File.Exists(fullFileName))
                throw new SMSMessageException(Resource.FileExist);

            postFile.SaveAs(fullFileName);
        }
        #endregion
        #region 文件类型
        /// <summary>
        /// 文件类型
        /// </summary>
        public enum FileType
        {
            XML,
            EXCEL,
            TXT,
        }
        #endregion
        #region 下载数据模板
        public static void DownloadTemplate(string path, IList<string> stbHeaders)
        {
            DataTable dt = ExcelHelper.CreatDataTable(stbHeaders);
            dt.TableName = "tblname";
            ExcelHelper.SavedataToExcelOledb(path, dt);
        }
        #endregion
    }
}
