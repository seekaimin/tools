package com.am.io;

import java.io.File;

import javax.servlet.ServletRequest;

public class pathhelpers {

	/**
	 * 获取服务端绝对路径
	 * 
	 * @param path
	 *            服务端路径
	 * @param request
	 *            request请求
	 * @return 服务端绝对路径
	 */
	public static String getServerPath(String path, ServletRequest request) {
		return request.getServletContext().getRealPath(path);
	}

	/**
	 * 获取文件名称
	 * 
	 * @param fullname
	 *            文件全路径名称
	 * @return
	 */
	public static String getFileName(String fullname) {
		return fullname.substring(fullname.lastIndexOf(separator()) + 1);
	}

	/**
	 * 获取文件扩展名
	 * 
	 * @param filename
	 *            文件名称
	 * @return
	 */
	public static String getFileExtension(String filename) {
		return filename.substring(filename.lastIndexOf('.') + 1);
	}

	/**
	 * 路径合并
	 * 
	 * @param paths
	 * @return
	 */
	public static String combine(String... paths) {
		StringBuffer result = new StringBuffer();
		if (paths != null && paths.length > 0) {
			for (String path : paths) {
				result.append(path);
				if (path.endsWith(separator()) == false) {
					result.append(separator());
				}
			}
		}
		return result.toString();
	}

	/**
	 * 合并全路径文件名
	 * 
	 * @param filename
	 *            文件名
	 * @param paths
	 *            文件路径
	 * @return
	 */
	public static String combineFullName(String filename, String... paths) {
		StringBuffer result = new StringBuffer();
		if (paths != null && paths.length > 0) {
			for (String path : paths) {
				result.append(path);
				if (path.endsWith(separator()) == false) {
					result.append(separator());
				}
			}
		}
		String name = pathhelpers.getFileName(filename);
		result.append(name);
		return result.toString();
	}


	/**
	 * 获取通用换行符
	 * @return
	 */
	public static String newLine() {
		return System.getProperty("line.separator");
	}

	/**
	 * 获取通用文件夹分割字符
	 * @return
	 */
	public static String separator() {
		return File.separator;
	}
}
