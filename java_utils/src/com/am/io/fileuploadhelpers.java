package com.am.io;

import java.io.File;
import java.io.IOException;
import java.io.RandomAccessFile;
import java.util.List;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;

import org.apache.commons.fileupload.FileItem;
import org.apache.commons.fileupload.disk.DiskFileItemFactory;
import org.apache.commons.fileupload.servlet.ServletFileUpload;

import com.am.utilities.Encodings;
import com.am.utilities.MyGuid;
import com.am.utilities.integerhelpers;
import com.am.utilities.stringhelpers;
import com.am.utilities.tools;
import com.am.utilities.MyGuid.Format;

public class fileuploadhelpers {

	/**
	 * 处理上传
	 * 
	 * @param directory
	 *            需要保存的路径
	 * @param request
	 *            请求
	 * @return
	 * @throws ServletException
	 * @throws IOException
	 */
	@SuppressWarnings("unchecked")
	public static boolean doUpLoad(String directory, HttpServletRequest request, WebUpLoadItem data)
			throws ServletException, IOException {
		boolean flag = true;
		DiskFileItemFactory factory = new DiskFileItemFactory();
		// 最大缓存
		factory.setSizeThreshold(data.TempFileSize * 1024 * 1024);
		try {
			// 检查目录是否存在
			File file = new File(directory);
			// 判断文件夹是否存在,如果不存在则创建文件夹
			if (!file.exists()) {
				file.mkdir();
			}
			// 设置临时文件目录
			factory.setRepository(file);
		} catch (Exception e) {
			e.printStackTrace();
		}
		ServletFileUpload upload = new ServletFileUpload(factory);
		upload.setHeaderEncoding(Encodings.UTF8);
		try {
			// 获取所有文件列表
			List<FileItem> items = upload.parseRequest(request);
			FileItem fileItem = null;
			for (FileItem item : items) {
				// 字段名称
				String key = item.getFieldName().toUpperCase();
				if (item.isFormField()) {
					String value = item.getString();
					if (key.equals(WebUpLoadItem.FIELD_NAME)) {
						data.setFieldName(item.getString());
					} else if (key.equals(WebUpLoadItem.FILE_NAME)) {
						value = item.getString(Encodings.UTF8);
						data.setFileName(value);
					} else if (key.equals(WebUpLoadItem.FILE_SIZE)) {
						long filesize = integerhelpers.toInt64(value);
						data.setFileSize(filesize);
					} else if (key.equals(WebUpLoadItem.FILE_START)) {
						long filestart = integerhelpers.toInt64(value);
						data.setFilePositon(filestart);
					} else if (key.equals(WebUpLoadItem.FILE_SERVERID)) {
						value = value.replace("#", File.separator);
						data.setFileNewFullName(value);
					} else if (key.equals(WebUpLoadItem.SIGN)) {
						data.setSign(value);
						tools.println("sign : [%s]",data.getSign());
					}
				} else {
					// 文件名
					String fileName = item.getName();
					if (stringhelpers.isNullOrEmpty(fileName)) {
						// 无需要上传的文件
						continue;
					}
					if (fileItem == null) {
						fileItem = item;
					}
				} // end of if
			} // end of for
			if (fileItem != null) {
				if (stringhelpers.isNullOrEmpty(data.getFileNewFullName())) {

					String newname = MyGuid.newId().toString(Format.N);
					String ext = data.getExtend();
					newname = newname + "." + ext;
					data.setFileNewName(newname);
					String newfullname = pathhelpers.combineFullName(newname, directory);
					data.setFileNewFullName(newfullname);
				} else {
					// 设置文件名
					String filename = pathhelpers.getFileName(data.getFileNewFullName());
					data.setFileName(filename);
				}
				RandomAccessFile randomFile = null;
				// 写入文件
				File wf = new File(data.getFileNewFullName());
				if (wf.exists()) {
					do {
						try {
							randomFile = new RandomAccessFile(data.getFileNewFullName(), "rw");
							// 将写文件指针移到文件上传位置。
							randomFile.seek(data.getFilePositon());
							randomFile.write(fileItem.get());
							break;
						} catch (Exception e) {
							tools.println("文件被暂用!");
							tools.sleep(1000);
						} finally {
							tools.close(randomFile);
						}
					} while (true);
				} else {
					// 写入文件
					fileItem.write(wf);
				}
			} else {
				flag = false;
			}
		} catch (Exception e) {
			e.printStackTrace();
			flag = false;
		}
		return flag;
	}
}
