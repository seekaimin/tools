package com.am.io;

import java.io.File;

public class WebUpLoadItem {
	/**
	 * 字段名称
	 */
	public final static String FIELD_NAME = "FIELDNAME";
	/**
	 * 文件名称 仅支持UTF-8
	 */
	public final static String FILE_NAME = "FILENAME";
	/**
	 * 文件字总节数
	 */
	public final static String FILE_SIZE = "FILESIZE";
	/**
	 * 当前上传文件起始位置
	 */
	public final static String FILE_START = "FILESTART";
	/**
	 * BLOB大小
	 */
	public final static String FILE_BLOB = "FILE_BLOB";
	/**
	 * 服务端保存的文件标识 用于断点上传
	 */
	public final static String FILE_SERVERID = "FILESERVERID";
	/**
	 * 文件数据
	 */
	public final static String BLOB = "BLOB";
	/**
	 * 签名信息
	 */
	public final static String SIGN = "SIGN";
	
	/**
	 * 开辟临时文件保存大小(M)  默认20M
	 */
	public int TempFileSize = 20;

	private String fieldName = "";
	private String fileName = "";
	private long fileSize = 0;
	private long filePositon = 0;
	private long fileBlob = 0;
	private String fileNewName = "";
	private String fileNewFullName = "";
	private String sign = "";

	/**
	 * 文件名称
	 */
	public String getFileName() {
		return fileName;
	}
	
	
	/**
	 * 获取文件扩展名
	 * @return
	 */
	public String getExtend()
	{
		return pathhelpers.getFileExtension(this.getFileName());
	}

	/**
	 * 文件名称
	 */
	public void setFileName(String fileName) {
		this.fileName = fileName;
	}

	/**
	 * 字段名称
	 */
	public String getFieldName() {
		return fieldName;
	}

	/**
	 * 字段名称
	 */
	public void setFieldName(String fieldName) {
		this.fieldName = fieldName;
	}

	/**
	 * 新文件名称
	 */
	public String getFileNewName() {
		return fileNewName;
	}

	/**
	 * 新文件名称
	 */
	public void setFileNewName(String fileNewName) {
		this.fileNewName = fileNewName;
	}

	/**
	 * 新全路径名称
	 */
	public String getFileNewFullName() {
		return fileNewFullName;
	}

	/**
	 * 新全路径名称
	 */
	public void setFileNewFullName(String fileNewFullName) {
		this.fileNewFullName = fileNewFullName;
	}

	/**
	 * 文件大小
	 */
	public long getFileSize() {
		return fileSize;
	}

	/**
	 * 文件大小
	 */
	public void setFileSize(long fileSize) {
		this.fileSize = fileSize;
	}

	/**
	 * 文件位置
	 */
	public long getFilePositon() {
		return filePositon;
	}

	/**
	 * 文件位置
	 */
	public void setFilePositon(long filePositon) {
		this.filePositon = filePositon;
	}

	/**
	 * 文件块大小
	 */
	public long getFileBlob() {
		return fileBlob;
	}

	/**
	 * 文件块大小
	 */
	public void setFileBlob(long fileBlob) {
		this.fileBlob = fileBlob;
	}

	@Override
	public String toString() {
		return fieldName + "----------" + fileNewName;
	}

	/**
	 * 获取文件全路径
	 * 
	 * @param directory
	 *            保存的文件夹
	 * @return
	 */
	public String getFileFullPath(String directory) {
		String result = directory + File.separator + this.getFileNewName();
		return result;
	}


	/**
	 * 签名信息
	 * @return
	 */
	public String getSign() {
		return sign;
	}


	/**
	 * 签名信息
	 * @param sign 签名信息
	 */
	public void setSign(String sign) {
		this.sign = sign;
	}

}
