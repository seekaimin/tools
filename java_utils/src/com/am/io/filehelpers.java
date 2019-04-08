package com.am.io;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import com.am.utilities.CopyIndex;
import com.am.utilities.arrayhelpers;

/**
 * 文件帮助类
 * 
 * @author Administrator
 *
 */
public class filehelpers {

	/**
	 * 文件读取
	 * 
	 * @param filename
	 *            全路径文件名
	 * @return
	 * @throws IOException
	 */
	public byte[] read(String filename) throws IOException {
		byte[] result = null;
		List<byte[]> datas = read(filename, 8192);
		int length = 0;
		for (byte[] data : datas) {
			length += data.length;
		}
		result = new byte[length];
		CopyIndex index = new CopyIndex();
		for (byte[] data : datas) {
			arrayhelpers.Copy(result, data, index);
		}
		return result;
	}

	/**
	 * 文件读取
	 * 
	 * @param filename
	 *            全路径文件名
	 * @param size
	 *            缓存大小
	 * @return
	 * @throws IOException
	 */
	public List<byte[]> read(String filename, int size) throws IOException {

		List<byte[]> datas = new ArrayList<byte[]>();
		// 读取
		FileInputStream stream = null;
		try {
			stream = new FileInputStream(filename);
			int length = 0;
			int total = 0;
			do {
				length = size;
				if (total + size > stream.available()) {
					length = stream.available() - total;
				}
				byte[] data = new byte[length];
				length = stream.read(data);
				datas.add(data);
			} while (stream.available() > 0);
			return datas;
		} catch (IOException e) {
			throw e;
		} finally {
			if (stream != null) {
				stream.close();
			}
		}
	}

	/**
	 * 读取文件到字符串数组
	 * 
	 * @param filename
	 *            文件名
	 * @return 字符串数组
	 * @throws IOException
	 */
	public static List<String> readToList(String filename) throws IOException {
		List<String> lines = new ArrayList<String>();
		BufferedReader reader = null;
		try {
			reader = new BufferedReader(new FileReader(filename));
			String line = null;
			while ((line = reader.readLine()) != null) {
				lines.add(line);
			}
		} catch (IOException e) {
			throw e;
		} finally {
			if (reader != null)
				reader.close();
		}
		return lines;
	}

	/**
	 * 读取文件到字符串
	 * 
	 * @param filename
	 *            文件名
	 * @return 字符串
	 * @throws IOException
	 */
	public static StringBuffer readToEnd(String filename) throws IOException {
		StringBuffer sb = new StringBuffer();
		List<String> lines = readToList(filename);
		for (String line : lines) {
			sb.append(line);
		}
		return sb;
	}

	/**
	 * 将字符串写入文件
	 * 
	 * @param filename
	 *            文件名
	 * @param lines
	 *            字符串集合
	 * @throws IOException
	 */
	public static void writeText(String filename, String... lines) throws IOException {
		if (lines == null || lines.length == 0)
			return;
		// 写入
		BufferedWriter writer = null;
		try {
			writer = new BufferedWriter(new FileWriter(filename));
			for (String line : lines) {
				writer.write(line);
				writer.flush(); // 刷新缓冲区的数据到文件
			}
		} catch (Exception e) {

		} finally {
			if (writer != null)
				writer.close();
		}
	}

	/**
	 * 将字符串写入文件
	 * 
	 * @param filename
	 *            文件名
	 * @param append
	 *            是否追加文件
	 * @param lines
	 *            字符串集合
	 * @throws IOException
	 */
	public static void writeText(String filename, boolean append, List<String> lines) throws IOException {
		if (lines == null || lines.size() == 0)
			return;
		// 写入
		BufferedWriter writer = null;
		try {
			writer = new BufferedWriter(new FileWriter(filename, append));
			for (String line : lines) {
				writer.write(line);
				writer.flush(); // 刷新缓冲区的数据到文件
			}
		} catch (IOException e) {
			throw e;
		} finally {
			if (writer != null)
				writer.close();
		}
	}

	/**
	 * 写入文件
	 * 
	 * @param filename
	 *            文件名称
	 * @param append
	 *            是否追加
	 * @param datas
	 *            需要写入的数据
	 * @throws IOException
	 */
	public static void writeBuffer(String filename, boolean append, byte[]... datas) throws IOException {
		if (datas == null || datas.length == 0)
			return;

		FileOutputStream stream = null;
		try {
			File file = new File(filename);
			stream = new FileOutputStream(file);
			// if file doesnt exists, then create it
			if (!file.exists()) {
				file.createNewFile();
			}
			// get the content in bytes
			for (byte[] data : datas) {
				stream.write(data);
				stream.flush();
			}
		} catch (IOException e) {
			throw e;
		} finally {

			if (stream != null) {
				stream.close();
			}
		}
	}

	/**
	 * 写入文件
	 * 
	 * @param filename
	 *            文件名称
	 * @param append
	 *            是否追加
	 * @param datas
	 *            需要写入的数据
	 * @throws IOException
	 */
	public static void writeBuffer(String filename, boolean append, List<byte[]> datas) throws IOException {
		if (datas == null || datas.size() == 0)
			return;

		FileOutputStream stream = null;
		try {
			File file = new File(filename);
			stream = new FileOutputStream(file);
			// if file doesnt exists, then create it
			if (!file.exists()) {
				file.createNewFile();
			}
			// get the content in bytes
			for (byte[] data : datas) {
				stream.write(data);
				stream.flush();
			}
		} catch (IOException e) {
			throw e;
		} finally {

			if (stream != null) {
				stream.close();
			}
		}
	}

}
