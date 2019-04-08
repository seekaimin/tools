package com.am.utilities;

import java.io.Closeable;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.net.URLDecoder;
import java.net.URLEncoder;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Date;

/**
 * 工具类
 * 
 * @author am
 *
 */
public class tools {
	/**
	 * <p>
	 * 将字符串转化为数字 可自动匹配16进制字符串
	 * </p>
	 * <p>
	 * 并且检核数据大小
	 * </p>
	 * 
	 * @param title
	 *            错误时提示标题
	 * @param val
	 *            传输的字符串
	 * @param min
	 *            最小值
	 * @param max
	 *            最大值
	 * @param isthrowexception
	 *            是否抛出异常
	 * @return 数字
	 * @throws Exception
	 */
	public static long checkNumberValue(String val, String title, long min, long max, boolean isthrowexception)
			throws Exception {
		long result_value = checkNumberValue(val, title, isthrowexception);
		if (result_value < min || result_value > max) {
			if (isthrowexception) {
				throw new Exception(stringhelpers.fmt("%s:取值范围为 %d~%d", title, min, max));
			}
		}
		return result_value;
	}

	/**
	 * 将字符串转化为数字 可自动匹配16进制字符串
	 * 
	 * @param title
	 *            错误时提示标题
	 * @param val
	 *            传输的字符串
	 * @param isthrowexception
	 *            是否抛出异常
	 * @return 数字
	 * @throws Exception
	 */
	public static long checkNumberValue(String val, String title, boolean isthrowexception) throws Exception {
		long result_value = 0;
		try {
			result_value = integerhelpers.toInt64(val);
		} catch (Exception e) {
			if (isthrowexception)
				throw new Exception(title + ":参数错误");
		}
		return result_value;
	}

	/**
	 * 计算MD5
	 * 
	 * @param src
	 *            需要计算的字符串
	 * @return
	 */
	public final static String calcMD5(String src) {
		// char hexDigits[] = { '0', '1', '2', '3', '4', '5', '6', '7', '8',
		// '9', 'a', 'b', 'c', 'd', 'e', 'f' };
		try {
			byte[] data = src.getBytes();

			byte[] md = calcMD5(data);
			// 把密文转换成十六进制的字符串形式
			/*
			 * int j = md.length; char str[] = new char[j * 2]; int k = 0; for
			 * (int i = 0; i < j; i++) { byte byte0 = md[i]; str[k++] =
			 * hexDigits[byte0 >>> 4 & 0xf]; str[k++] = hexDigits[byte0 & 0xf];
			 * } return new String(str);
			 */
			return stringhelpers.bytesToHexString(md);
		} catch (Exception e) {
			e.printStackTrace();
			return "";
		}
	}

	/**
	 * 计算MD5
	 * 
	 * @param src
	 *            源数组
	 * @return MD5
	 */
	public static byte[] calcMD5(byte[] src) {
		byte[] result = new byte[0];
		// 获得MD5摘要算法的 MessageDigest 对象
		MessageDigest mdInst = null;
		try {
			mdInst = MessageDigest.getInstance("MD5");
			// 使用指定的字节更新摘要
			mdInst.update(src);
			// 获得密文
			result = mdInst.digest();
		} catch (NoSuchAlgorithmException e) {
			e.printStackTrace();
		}
		return result;
	}

	/**
	 * 将文件读取到数组
	 * 
	 * @param filePath
	 *            文件路径
	 * @return buffer
	 * @throws IOException
	 */
	public static byte[] readFile2Array(String filePath) throws IOException {
		File file = new File(filePath);
		int file_length = (int) file.length();
		FileInputStream fi = new FileInputStream(file);
		byte[] result = new byte[file_length];
		fi.read(result, 0, file_length);
		fi.close();
		return result;
	}

	/**
	 * 打印信息
	 * 
	 * @param src
	 *            需要打印的信息
	 */
	public static void print(String src) {
		System.out.print(src);
	}

	/**
	 * 打印信息 带格式化
	 * 
	 * @param foramtstr
	 *            需要格式化的信息
	 * @param args
	 *            格式化需要的参数
	 */
	public static void print(String foramtstr, Object... args) {
		print(stringhelpers.fmt(foramtstr, args));
	}

	/**
	 * 打印信息
	 * 
	 * @param src
	 *            需要打印的信息
	 */
	public static void println(String src) {
		String msg = datetimehelpers.getNowLongString() + "-" + Thread.currentThread().getName() + " : " + src;
		System.out.println(msg);
	}

	/**
	 * 打印信息 带格式化
	 * 
	 * @param foramtstr
	 *            需要格式化的信息
	 * @param args
	 *            格式化需要的参数
	 */
	public static void println(String foramtstr, Object... args) {
		println(stringhelpers.fmt(foramtstr, args));
	}

	public static void println(boolean src) {
		println(integerhelpers.toString(src));
	}

	public static void println(byte src) {
		println(integerhelpers.toString(src));
	}

	public static void println(short src) {
		println(integerhelpers.toString(src));
	}

	public static void println(long src) {
		println(integerhelpers.toString(src));
	}

	public static void println(byte[] src) {
		println(stringhelpers.bytesToHexString(src));
	}

	/**
	 * 打印信息
	 * 
	 * @param src
	 *            需要打印的信息
	 * @param splitChar
	 *            分隔符
	 */
	public static void println(byte[] src, String splitChar) {
		println(stringhelpers.bytesToHexString(src, splitChar));
	}

	public static void print(boolean src) {
		print(integerhelpers.toString(src));
	}

	public static void print(byte src) {
		print(integerhelpers.toString(src));
	}

	public static void print(short src) {
		print(integerhelpers.toString(src));
	}

	public static void print(long src) {
		print(integerhelpers.toString(src));
	}

	public static void print(byte[] src) {
		print(stringhelpers.bytesToHexString(src));
	}

	public static void println(Date dt) {
		tools.println(datetimehelpers.format(dt, datetimehelpers.dateTimeFormatString));
	}

	public static void println(Date dt, String fmt) {
		tools.println(datetimehelpers.format(dt, fmt));
	}

	/**
	 * 将对象转化为json字符串
	 * 
	 * @param obj
	 * @return json字符串
	 */
	public static String javaBeanToJson(Object obj) {
		String json = jsonhelpers.javaBeanToJson(obj);
		// System.out.println(json);
		return json;
	}

	/**
	 * 当前线程sleep
	 * 
	 * @param millis
	 *            sleep时间(ms)
	 */
	public static void sleep(long millis) {

		try {
			Thread.sleep(millis);
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

	}

	/**
	 * 关闭java.io.Closeable
	 * 
	 * @param closeable
	 *            java.io.Closeable
	 */
	public static void close(Closeable closeable) {
		try {
			if (closeable != null)
				closeable.close();
		} catch (IOException e) {
		}
	}

	/**
	 * 编码
	 * 
	 * @param msg
	 *            需要编码的字符串
	 * @param encoding
	 *            字符集
	 * @return
	 */
	public static String encode(String msg, String encoding) {
		try {
			return URLEncoder.encode(msg, encoding);
		} catch (Exception e) {
			return msg;
		}
	}

	/**
	 * 解码
	 * 
	 * @param msg
	 *            需要解码的字符串
	 * @param encoding
	 *            字符集
	 * @return
	 */
	public static String decode(String msg, String encoding) {
		try {
			return URLDecoder.decode(msg, "utf-8");
		} catch (Exception e) {
			return msg;
		}
	}

	/**
	 * 获取根路径
	 * 
	 * @return
	 */
	public static String getApplicationRootPath() {
		String dir = System.getProperty("user.dir");
		;
		if (dir.startsWith("file:/")) {
			dir = dir.substring(6);
		}
		String RootPath = dir;
		if (RootPath.lastIndexOf('\\') > 0) {
			if (!RootPath.endsWith("\\")) {
				RootPath += "\\";
			}
		} else if (RootPath.lastIndexOf('/') > 0) {
			if (!RootPath.endsWith("/")) {
				RootPath += "/";
			}
		}
		return RootPath;
	}

	/**
	 * 线程等待
	 *
	 * @param timeout
	 */
	public static void threadWait(Thread thread, int timeout) {
		try {
			synchronized (thread) {
				thread.wait(timeout);
			}
		} catch (Exception e) {
			// e.printStackTrace();
		}
	}

	/**
	 * 当前线程等待
	 *
	 * @param timeout
	 */
	public static void threadWait(int timeout) {
		threadWait(Thread.currentThread(), timeout);
	}

	/**
	 * 唤醒线程
	 */
	public static void threadNotifyAll(Thread thread) {
		try {
			synchronized (thread) {
				thread.notifyAll();
			}
		} catch (Exception e) {
			// e.printStackTrace();
		}
	}

	/**
	 * 唤醒当前线程
	 */
	public static void threadNotifyAll() {
		threadNotifyAll(Thread.currentThread());
	}

}
