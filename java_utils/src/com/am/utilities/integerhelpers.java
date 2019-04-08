package com.am.utilities;

import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.text.DecimalFormat;

/**
 * 数字帮助类
 * 
 * @author am
 *
 */
public class integerhelpers {
	/**
	 * 转大端字节序数组
	 * 
	 * @param src
	 *            需要转化的数据源
	 * @param isBigEndian
	 *            true:大端网络序 false:小端字节序
	 * @return 字节序数组
	 */
	public static byte[] toBuffer(short src, boolean isBigEndian) {
		ByteBuffer temp = ByteBuffer.wrap(new byte[2]);
		if (isBigEndian) {
			temp.order(ByteOrder.BIG_ENDIAN);
		} else {
			temp.order(ByteOrder.LITTLE_ENDIAN);
		}
		temp.asShortBuffer().put(src);
		return temp.array();
	}

	/**
	 * 转大端字节序数组
	 * 
	 * @param src
	 *            需要转化的数据源
	 * @param isBigEndian
	 *            true:大端网络序 false:小端字节序
	 * @return 字节序数组
	 */
	public static byte[] toBuffer(int src, boolean isBigEndian) {
		ByteBuffer temp = ByteBuffer.wrap(new byte[4]);
		if (isBigEndian) {
			temp.order(ByteOrder.BIG_ENDIAN);
		} else {
			temp.order(ByteOrder.LITTLE_ENDIAN);
		}
		temp.asIntBuffer().put(src);
		return temp.array();
	}

	/**
	 * 转大端字节序数组
	 * 
	 * @param src
	 *            需要转化的数据源
	 * @param isBigEndian
	 *            true:大端网络序 false:小端字节序
	 * @return 字节序数组
	 */
	public static byte[] toBuffer(long src, boolean isBigEndian) {
		ByteBuffer temp = ByteBuffer.wrap(new byte[8]);
		if (isBigEndian) {
			temp.order(ByteOrder.BIG_ENDIAN);
		} else {
			temp.order(ByteOrder.LITTLE_ENDIAN);
		}
		temp.asLongBuffer().put(src);
		return temp.array();
	}

	/**
	 * buffer 2 数字
	 * 
	 * @param src
	 *            buffer
	 * @param isBigEndian
	 *            true:大端网络序 false:小端字节序
	 * @return value
	 */
	public static short toInt16(byte[] src, boolean isBigEndian) {
		ByteBuffer temp = ByteBuffer.wrap(src);
		if (isBigEndian) {
			temp.order(ByteOrder.BIG_ENDIAN);
		} else {
			temp.order(ByteOrder.LITTLE_ENDIAN);
		}
		return temp.getShort();
	}

	/**
	 * buffer 2 数字
	 * 
	 * @param src
	 *            buffer
	 * @param isBigEndian
	 *            true:大端网络序 false:小端字节序
	 * @return value
	 */
	public static int toInt32(byte[] src, boolean isBigEndian) {
		ByteBuffer temp = ByteBuffer.wrap(src);
		if (isBigEndian) {
			temp.order(ByteOrder.BIG_ENDIAN);
		} else {
			temp.order(ByteOrder.LITTLE_ENDIAN);
		}
		return temp.getInt();
	}

	/**
	 * buffer 2 数字
	 * 
	 * @param src
	 *            buffer
	 * @param isBigEndian
	 *            true:大端网络序 false:小端字节序
	 * @return value
	 */
	public static long toInt64(byte[] src, boolean isBigEndian) {
		ByteBuffer temp = ByteBuffer.wrap(src);
		if (isBigEndian) {
			temp.order(ByteOrder.BIG_ENDIAN);
		} else {
			temp.order(ByteOrder.LITTLE_ENDIAN);
		}
		return temp.getLong();
	}

	/**
	 * 数字转16进制字符串
	 * 
	 * @param val
	 *            数组
	 * @return 16进制字符串
	 */
	public static String toHexString(byte val) {
		StringBuilder sb = new StringBuilder();
		int size = 2;
		for (int i = 0; i < size; i++) {
			sb.append("0");
		}
		String hv = Integer.toHexString(val);
		String temp = sb.append(hv).toString();
		String result = temp.substring(temp.length() - size, temp.length());
		return result;
	}

	/**
	 * 数字转16进制字符串
	 * 
	 * @param val
	 *            数组
	 * @return 16进制字符串
	 */
	public static String toHexString(short val) {
		StringBuilder sb = new StringBuilder();
		int size = 4;
		for (int i = 0; i < size; i++) {
			sb.append("0");
		}
		String hv = Integer.toHexString(val);
		String temp = sb.append(hv).toString();
		String result = temp.substring(temp.length() - size, temp.length());
		return result;
	}

	/**
	 * 数字转16进制字符串
	 * 
	 * @param val
	 *            数组
	 * @return 16进制字符串
	 */
	public static String toHexString(int val) {
		StringBuilder sb = new StringBuilder();
		int size = 8;
		for (int i = 0; i < size; i++) {
			sb.append("0");
		}
		String hv = Integer.toHexString(val);
		String temp = sb.append(hv).toString();
		String result = temp.substring(temp.length() - size, temp.length());
		return result;
	}

	/**
	 * 数字转16进制字符串
	 * 
	 * @param val
	 *            数组
	 * @return 16进制字符串
	 */
	public static String toHexString(long val) {
		StringBuilder sb = new StringBuilder();
		int size = 16;
		for (int i = 0; i < size; i++) {
			sb.append("0");
		}
		String hv = Long.toHexString(val);
		String temp = sb.append(hv).toString();
		String result = temp.substring(temp.length() - size, temp.length());
		return result;
	}

	/**
	 * 字符串转数字 支持16进制输入(0x开始)
	 * 
	 * @param val
	 *            需要转换的字符串
	 * @return 数字
	 */
	public static byte toInt8(String val) {
		byte reuslt = 0;
		String value_str = stringhelpers.toNumberString(val, "0").toUpperCase();
		if (value_str.startsWith("0X")) {
			reuslt = Integer.valueOf(value_str.replace("0X", ""), 16).byteValue();
		} else {
			reuslt = Integer.valueOf(value_str).byteValue();
		}
		return reuslt;
	}

	/**
	 * 字符串转数字 支持16进制输入(0x开始)
	 * 
	 * @param val
	 *            需要转换的字符串
	 * @return 数字
	 */
	public static short toInt16(String val) {
		short reuslt = 0;
		String value_str = stringhelpers.toNumberString(val, "0").toUpperCase();
		if (value_str.startsWith("0X")) {
			reuslt = Integer.valueOf(value_str.replace("0X", ""), 16).shortValue();
		} else {
			reuslt = Integer.valueOf(value_str).shortValue();
		}
		return reuslt;
	}

	/**
	 * 字符串转数字 支持16进制输入(0x开始)
	 * 
	 * @param val
	 *            需要转换的字符串
	 * @return 数字
	 */
	public static int toInt32(String val) {
		int reuslt = 0;
		String value_str = stringhelpers.toNumberString(val, "0").toUpperCase();
		if (value_str.startsWith("0X")) {
			reuslt = Long.valueOf(value_str.replace("0X", ""), 16).intValue();
		} else {
			reuslt = Long.valueOf(value_str).intValue();
		}
		return reuslt;
	}

	/**
	 * 字符串转数字 支持16进制输入(0x开始)
	 * 
	 * @param val
	 *            需要转换的字符串
	 * @return 数字
	 */
	public static long toInt64(String val) {
		long reuslt = 0;
		String value_str = stringhelpers.toNumberString(val, "0").toUpperCase();
		if (value_str.startsWith("0X")) {
			java.math.BigInteger bigInteger = new java.math.BigInteger(value_str.replace("0X", ""), 16);
			reuslt = bigInteger.longValue();
		} else {
			java.math.BigInteger bigInteger = new java.math.BigInteger(value_str);
			reuslt = bigInteger.longValue();

		}
		return reuslt;
	}

	/******** small to big start ********/
	/**
	 * small to big
	 * 
	 * @param src
	 *            small
	 * @return big val
	 */
	public static long toInt64(byte src) {
		String temp = "0x" + toHexString(src);
		return toInt64(temp);
	}

	/**
	 * small to big
	 * 
	 * @param src
	 *            small
	 * @return big val
	 */
	public static long toInt64(short src) {
		String temp = "0x" + toHexString(src);
		return toInt64(temp);
	}

	/**
	 * small to big
	 * 
	 * @param src
	 *            small
	 * @return big val
	 */
	public static long toInt64(int src) {
		String temp = "0x" + toHexString(src);
		return toInt64(temp);
	}

	/**
	 * small to big
	 * 
	 * @param src
	 *            small
	 * @return big val
	 */
	public static int toInt32(byte src) {
		String temp = "0x" + toHexString(src);
		return toInt32(temp);
	}

	/**
	 * small to big
	 * 
	 * @param src
	 *            small
	 * @return big val
	 */
	public static int toInt32(short src) {
		String temp = "0x" + toHexString(src);
		return toInt32(temp);
	}

	/**
	 * small to big
	 * 
	 * @param src
	 *            small
	 * @return big val
	 */
	public static short toInt16(byte src) {
		String temp = "0x" + toHexString(src);
		return toInt16(temp);
	}

	/******** small to big end ********/

	/******** big to small start ********/
	/**
	 * big to byte
	 * 
	 * @param src
	 *            big val
	 * @return byte
	 */
	public static byte toInt8(long src) {
		byte result = (byte) (src & 0xFF);
		return result;
	}

	/**
	 * big to short
	 * 
	 * @param src
	 *            big val
	 * @return short
	 */
	public static short toInt16(long src) {
		short result = (short) (src & 0xFFFF);
		return result;
	}

	/**
	 * big to int
	 * 
	 * @param src
	 *            big val
	 * @return int
	 */
	public static int toInt32(long src) {
		int result = (int) (src & 0xFFFFFFFF);
		return result;
	}

	/******** big to small end ********/

	/******** number to string start ********/
	/**
	 * to string
	 * 
	 * @param src
	 *            val
	 * @return string
	 */
	public static String toString(byte src) {
		return String.valueOf(src);
	}

	/**
	 * to string
	 * 
	 * @param src
	 *            val
	 * @return string
	 */
	public static String toString(short src) {
		return String.valueOf(src);
	}

	/**
	 * to string
	 * 
	 * @param src
	 *            val
	 * @return string
	 */
	public static String toString(int src) {
		return String.valueOf(src);
	}

	/**
	 * to string
	 * 
	 * @param src
	 *            val
	 * @return string
	 */
	public static String toString(long src) {
		return String.valueOf(src);
	}

	/**
	 * to string
	 * 
	 * @param src
	 *            val
	 * @return string
	 */
	public static String toString(boolean src) {
		return String.valueOf(src);
	}

	/******** number to string end ********/
	/**
	 * double format
	 * 
	 * @param src
	 *            需要格式化的浮点数
	 * @param length
	 *            保留小数位长度 如果为0,则四舍五入
	 * @return
	 */
	public static String format(Double src, int length) {
		// DecimalFormat df = new DecimalFormat("######0.00");
		StringBuilder f = new StringBuilder("0");
		if (length > 0) {
			f.append(".");
			for (int i = 0; i < length; i++) {
				f.append("0");
			}
		}
		DecimalFormat df = new DecimalFormat(f.toString());
		return df.format(src);
	}

	public static void main(String[] args) {
	}
}
