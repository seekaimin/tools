package com.am.utilities.settings;

import com.am.utilities.Encodings;

public abstract class FileReadBase {

	/**
	 * 默认编码方式
	 */
	public String Encoding = Encodings.UTF8; 

	/**
	 * 获取配置文件名
	 * 
	 * @return 配置文件名
	 */
	public abstract String getPropertyFileName();
	/**
	 * 初始化属性
	 */
	protected abstract void Read();

	/**
	 * 将属性写入配置文件
	 */
	protected abstract void Write();
	
	
	
	

	/**
	 * 获取编码方式
	 * @return 编码方式  默认 UTF8
	 */
	public String getEncoding() {		
		return Encoding;
	}


	/**
	 * 设置编码方式
	 * @param encoding 编码方式名称  参考:com.dexin.utilities.Encodings
	 */
	public void setEncoding(String encoding) {
		Encoding = encoding;
	}
	/**
	 * 获取配置文件路径
	 * 
	 * @return
	 */
	public String getPath() {
		return this.getClass().getClassLoader().getResource(getPropertyFileName()).getPath();
	}
}
