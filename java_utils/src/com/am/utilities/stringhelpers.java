package com.am.utilities;

import java.io.UnsupportedEncodingException;

/**
 * String 工具类
 * 
 * @author am
 *
 */
public class stringhelpers {

	/**
	 * 判断字符串是否为null 或者 空字符串
	 * 
	 * @param src
	 *            源字符串
	 * @return
	 */
	public static boolean isNullOrEmpty(String src) {
		if (src == null)
			return true;
		src = src.trim();
		if (src.length() == 0)
			return true;
		return false;
	}

	/**
	 * 
	 * Convert byte[] to hex
	 * string.这里我们可以将byte转换成int，然后利用Integer.toHexString(int)来转换成16进制字符串。
	 * 
	 * @param src
	 *            byte[] data
	 * @return hex string
	 */
	public static String bytesToHexString(byte[] src) {
		return bytesToHexString(src, "");
	}

	/**
	 * 
	 * Convert byte[] to hex
	 * string.这里我们可以将byte转换成int，然后利用Integer.toHexString(int)来转换成16进制字符串。
	 * 
	 * @param src
	 *            byte[] data
	 * @return hex string
	 */
	public static String bytesToHexString(Byte[] src) {
		return bytesToHexString(src, "");
	}

	/**
	 * 
	 * Convert byte[] to hex
	 * string.这里我们可以将byte转换成int，然后利用Integer.toHexString(int)来转换成16进制字符串。
	 * 
	 * @param src
	 *            byte[] data
	 * @param src
	 *            int length 需要显示的长度
	 * @return hex string
	 */
	public static String bytesToHexString(byte[] src, int length) {
		if (src == null) {
			return "";
		}
		if (length < src.length) {
			byte[] temp = arrayhelpers.GetBytes(src, length, new CopyIndex());
			return bytesToHexString(temp, "");
		}
		return bytesToHexString(src, "");
	}

	/**
	 * 
	 * Convert byte[] to hex
	 * string.这里我们可以将byte转换成int，然后利用Integer.toHexString(int)来转换成16进制字符串。
	 * 
	 * @param src
	 *            byte[] data
	 * @param src
	 *            int length 需要显示的长度
	 * @return hex string
	 */
	public static String bytesToHexString(Byte[] src, int length) {
		if (src == null) {
			return "";
		}
		if (length < src.length) {
			Byte[] temp = arrayhelpers.GetBytes(src, length, new CopyIndex());
			return bytesToHexString(temp, "");
		}
		return bytesToHexString(src, "");
	}

	/**
	 * 带分隔符 Convert byte[] to hex
	 * string.这里我们可以将byte转换成int，然后利用Integer.toHexString(int)来转换成16进制字符串。
	 * 
	 * @param src
	 *            byte[] data
	 * @param splitChar
	 *            分隔符
	 * @return hex string
	 */
	public static String bytesToHexString(byte[] src, String splitChar) {
		return bytesToString(src, 16, splitChar);
	}

	/**
	 * 带分隔符 Convert byte[] to hex
	 * string.这里我们可以将byte转换成int，然后利用Integer.toHexString(int)来转换成16进制字符串。
	 * 
	 * @param src
	 *            byte[] data
	 * @param splitChar
	 *            分隔符
	 * @return hex string
	 */
	public static String bytesToHexString(Byte[] src, String splitChar) {
		return bytesToString(src, 16, splitChar);
	}

	/**
	 * Convert hex string to byte[]
	 * 
	 * @param hexString
	 *            the hex string
	 * @return byte[]
	 */
	public static byte[] hexStringToBytes(String hexString) {
		return stringToBytes(hexString, 16);
	}

	/**
	 * 将字符串转换为buff
	 * 
	 * @param src
	 *            字符串
	 * @param format
	 *            格式化类型 10 16
	 * @return
	 */
	public static byte[] stringToBytes(String src, int format) {
		if (stringhelpers.isNullOrEmpty(src)) {
			return new byte[0];
		}
		int size = src.length() / 2;
		byte[] result = new byte[size];

		for (int i = 0; i < src.length();) {
			String temp = src.substring(i, i + 2);
			byte b = (byte) Integer.parseInt(temp, format);
			result[i / 2] = b;
			i += 2;
		}
		return result;
	}

	/**
	 * 数组转进制字符串
	 * 
	 * @param src
	 *            需要转的数组
	 * @param format
	 *            2 8 10 16
	 * @param splitChar
	 *            分隔符
	 * @return
	 */
	@SuppressWarnings("null")
	public static String bytesToString(byte[] src, int format, String splitChar) {

		if (stringhelpers.isNullOrEmpty(splitChar)) {
			splitChar = "";
		}
		StringBuilder result = new StringBuilder();
		// 十进制转成十六进制：
		// Integer.toHexString(int i)
		// 十进制转成八进制
		// Integer.toOctalString(int i)
		// 十进制转成二进制
		// Integer.toBinaryString(int i)
		// 十六进制转成十进制
		// Integer.valueOf("FFFF",16).toString()
		// 八进制转成十进制
		// Integer.valueOf("876",8).toString()
		// 二进制转十进制
		// Integer.valueOf("0101",2).toString()
		if (src != null || src.length > 0) {
			if (format == 2) {
				for (byte i : src) {
					String temp = stringhelpers.padLeft(Integer.toBinaryString(i), 8, "00");
					result.append(temp);
					result.append(splitChar);
				}
			} else if (format == 8) {
				for (byte i : src) {
					String temp = stringhelpers.padLeft(Integer.toOctalString(i), 3, "00");
					result.append(temp);
					result.append(splitChar);
				}
			} else if (format == 10) {
				for (byte i : src) {
					String temp = stringhelpers.padLeft(i + "", 3, "00");
					result.append(temp);
					result.append(splitChar);
				}
			} else if (format == 16) {
				for (byte i : src) {
					String temp = stringhelpers.padLeft(Integer.toHexString(i), 2, "00");
					result.append(temp);
					result.append(splitChar);
				}
			}
		}
		return result.toString();
	}

	/**
	 * 数组转进制字符串
	 * 
	 * @param src
	 *            需要转的数组
	 * @param format
	 *            2 8 10 16
	 * @param splitChar
	 *            分隔符
	 * @return
	 */
	@SuppressWarnings("null")
	public static String bytesToString(Byte[] src, int format, String splitChar) {

		if (stringhelpers.isNullOrEmpty(splitChar)) {
			splitChar = "";
		}
		StringBuilder result = new StringBuilder();
		// 十进制转成十六进制：
		// Integer.toHexString(int i)
		// 十进制转成八进制
		// Integer.toOctalString(int i)
		// 十进制转成二进制
		// Integer.toBinaryString(int i)
		// 十六进制转成十进制
		// Integer.valueOf("FFFF",16).toString()
		// 八进制转成十进制
		// Integer.valueOf("876",8).toString()
		// 二进制转十进制
		// Integer.valueOf("0101",2).toString()
		if (src != null || src.length > 0) {
			if (format == 2) {
				for (byte i : src) {
					String temp = stringhelpers.padLeft(Integer.toBinaryString(i), 8, "00");
					result.append(temp);
					result.append(splitChar);
				}
			} else if (format == 8) {
				for (byte i : src) {
					String temp = stringhelpers.padLeft(Integer.toOctalString(i), 3, "00");
					result.append(temp);
					result.append(splitChar);
				}
			} else if (format == 10) {
				for (byte i : src) {
					String temp = stringhelpers.padLeft(i + "", 3, "00");
					result.append(temp);
					result.append(splitChar);
				}
			} else if (format == 16) {
				for (byte i : src) {
					String temp = stringhelpers.padLeft(Integer.toHexString(i), 2, "00");
					result.append(temp);
					result.append(splitChar);
				}
			}
		}
		return result.toString();
	}

	/**
	 * Convert char to byte
	 * 
	 * @param c
	 *            char
	 * @return byte
	 */
	public static byte charToByte(char c) {

		return (byte) "0123456789ABCDEF".indexOf(c);
	}

	/**
	 * 字符串转数组
	 * 
	 * @param src
	 *            源字符串
	 * @param encoding
	 *            编码方式
	 * @return 数组
	 */
	public static byte[] toBytes(String src, String encoding) {
		byte[] result = new byte[0];
		try {
			result = src.getBytes(encoding);
		} catch (UnsupportedEncodingException e) {
			e.printStackTrace();
		}
		return result;
	}

	/**
	 * 数组转字符串
	 * 
	 * @param src
	 *            源数组
	 * @param encoding
	 *            编码方式
	 * @return 字符串
	 */
	public static String toString(byte[] src, String encoding) {
		String result = "";
		try {
			result = new String(src, encoding);
		} catch (UnsupportedEncodingException e) {
			e.printStackTrace();
		}
		return result;
	}

	/**
	 * 封装字符串格式化函数
	 * 
	 * @param foramtstr
	 *            需要格式化的字符串
	 * @param args
	 *            参数
	 * @return
	 */
	public static String fmt(String foramtstr, Object... args) {
		if (foramtstr == null)
			foramtstr = "";
		String result = String.format(foramtstr, args);
		return result;
	}

	/**
	 * 格式化浮点型数据 带四舍五入
	 * 
	 * @param src
	 *            源
	 * @param format
	 *            格式化字符串 ps: #.##
	 * @return 格式化后的字符串
	 */
	public static String fmt(double src, String format) {
		java.text.DecimalFormat df = new java.text.DecimalFormat(format);
		return df.format(src);
	}

	/**
	 * 格式化浮点型数据 带四舍五入
	 * 
	 * @param src
	 *            源
	 * @param format
	 *            格式化字符串 ps: #.##
	 * @return 格式化后的字符串
	 */
	public static String fmt(float src, String format) {
		java.text.DecimalFormat df = new java.text.DecimalFormat(format);
		return df.format(src);
	}

	/**
	 * 格式化浮点型数据 带四舍五入
	 * 
	 * @param src
	 *            源
	 * @param format
	 *            格式化字符串 ps: #.##
	 * @return 格式化后的字符串
	 */
	public static String fmtPercentage(double src, String format) {
		java.text.DecimalFormat df = new java.text.DecimalFormat(format);
		double temp = src * 100.0;
		return df.format(temp) + "%";
	}

	/**
	 * 格式化浮点型数据 带四舍五入
	 * 
	 * @param src
	 *            源
	 * @param format
	 *            格式化字符串 ps: #.##
	 * @return 格式化后的字符串
	 */
	public static String fmtPercentage(float src, String format) {
		java.text.DecimalFormat df = new java.text.DecimalFormat(format);
		float temp = src * 100.0f;
		return df.format(temp) + "%";
	}

	/**
	 * toString
	 * 
	 * @param src
	 *            源
	 * @param defaultValue
	 *            默认值
	 * @return 字符串
	 */
	public static String toStr(Object src, String defaultValue) {
		String result = defaultValue;
		try {
			result = src == null ? defaultValue : src.toString();
		} catch (Exception e) {
			result = defaultValue;
		}
		return result;
	}

	/**
	 * 数字字符串转化
	 * 
	 * @param src
	 *            源
	 * @param defaultValue
	 *            默认值
	 * @return
	 */
	public static String toNumberString(Object src, String defaultValue) {
		String result = defaultValue;
		if (src != null && stringhelpers.toStr(src, defaultValue).length()>0) {
			try {
				result = src == null ? defaultValue
						: src.toString().replace("\r", "").replace("\n", "").replace(" ", "").trim();
			} catch (Exception e) {
				result = defaultValue;
			}
		}
		return result;
	}

	/**
	 * 获取boolean参数
	 * 
	 * @param src
	 *            参数名称
	 * @return
	 */
	public static boolean toBoolean(String src) {
		return Boolean.valueOf(src);
	}

	/**
	 * 左边补全
	 * 
	 * @param src
	 *            源字符串
	 * @param totalWidth
	 *            总长度
	 * @param paddingChars
	 *            补全字符串
	 * @return 补全后的字符串
	 */
	public static String padLeft(String src, int totalWidth, String paddingChars) {
		StringBuilder temp = new StringBuilder();
		if (src == null) {
			src = "";
		}
		for (int i = src.length(); i < totalWidth; i++) {
			temp.append(paddingChars);
		}
		temp.append(src);
		String result = temp.substring(temp.length() - totalWidth, temp.length());
		return result;
	}

	/**
	 * 右边补全
	 * 
	 * @param src
	 *            源字符串
	 * @param totalWidth
	 *            总长度
	 * @param paddingChars
	 *            补全字符串
	 * @return 补全后的字符串
	 */
	public static String padRight(String src, int totalWidth, String paddingChar) {
		StringBuilder temp = new StringBuilder();
		if (src == null) {
			src = "";
		}
		temp.append(src);
		for (int i = src.length(); i < totalWidth; i++) {
			temp.append(paddingChar);
		}
		String result = temp.substring(0, totalWidth);
		return result;
	}

}
