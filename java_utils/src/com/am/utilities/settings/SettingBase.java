package com.am.utilities.settings;

import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.util.Properties;

/**
 * 配置基类 提供配置信息的处理方法
 * 
 * @author Administrator
 *
 */
public abstract class SettingBase extends FileReadBase  {

	/**
	 * 配置文件对象
	 */
	protected Properties propertiy = null;


	/**
	 * 获取配置文件对象
	 * 
	 */
	public void Init() {
		propertiy = new Properties();
		// 方法二：通过类加载目录getClassLoader()加载属性文件
		try {
			String path = getPath();
			InputStream in = new FileInputStream(path);
			InputStreamReader inr = new InputStreamReader(in, getEncoding());
			// 从输入流中读取属性列表（键和元素对）
			propertiy.load(inr);
			in.close();
			Read();
		} catch (IOException e) {
			e.printStackTrace();
			propertiy = null;
		}
	}


	/**
	 * 通过key读取属性文件中的值
	 * 
	 * @param name
	 *            key
	 * @return key相对应的value
	 */
	protected String getPropertyByName(String name) {
		String result = "";
		try {
			result = propertiy.getProperty(name).trim();
		} catch (Exception e) {
			e.printStackTrace();
			if (result.length() == 0) {
				result = "null";
			}
		}
		return result;
	}
	/**
	 * 设置配置信息
	 * @param name key
	 * @param value value
	 */
	protected void setProperty(String name,String value)
	{
		propertiy.setProperty(name, value);
	}

	/**
	 * 保存
	 * 
	 * @param prop
	 *            item
	 */
	public void Save(String comments) {
		try {
			String path = getPath();
			Write();
			FileOutputStream out = new FileOutputStream(path);
			OutputStreamWriter osw = new OutputStreamWriter(out,getEncoding());
			propertiy.store(osw, comments);
			out.close();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

}
