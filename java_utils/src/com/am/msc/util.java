package com.am.msc;

import java.io.UnsupportedEncodingException;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;

public class util {

	// 服务端注册
	public static final short HEAD_SERVER = 0x010F;
	// 客户端注册
	public static final short HEAD_CLIENT = 0x010E;

	// 服务端操作区域
	// 创建订阅/发送订阅数据
	public static final byte SERVER_OP_CREATE_OR_SEND = 0x01;
	// 取消订阅
	public static final byte SERVER_OP_CANCEL = 0x02;

	/**
	 * 数组拷贝方法扩展
	 * 
	 * @param des
	 *            目的数组
	 * @param src
	 *            源数组
	 * @param mode
	 *            传输对象
	 */
	public static void Copy(Object[] des, Object[] src, CopyIndex mode) {
		// arrayhelpersFan.getInstance().Copy(des, src, mode);
		System.arraycopy(src, 0, des, mode.getIndex(), src.length);
		mode.AddIndex(src.length);
	}

	/**
	 * 数组拷贝方法扩展
	 * 
	 * @param des
	 *            目的数组
	 * 
	 * @param desindex
	 *            目的数组保存的起始位置
	 * @param src
	 *            源数组
	 * @param length
	 *            拷贝长度
	 * @param mode
	 *            传输对象
	 */
	public static void Copy(Object[] des, int desindex, Object[] src, int length, CopyIndex mode) {
		// arrayhelpersFan.getInstance().Copy(des, src, mode);
		if (src.length < length) {
			length = src.length;
		}
		System.arraycopy(src, desindex, des, mode.getIndex(), length);
		mode.AddIndex(src.length);
	}

	/**
	 * 数组拷贝方法扩展
	 * 
	 * @param des
	 *            目的数组
	 * @param src
	 *            源数组
	 * @param mode
	 *            传输对象
	 */
	public static void Copy(String[] des, String[] src, CopyIndex mode) {
		System.arraycopy(src, 0, des, mode.getIndex(), src.length);
		mode.AddIndex(src.length);
	}

	/**
	 * 数组拷贝方法扩展
	 * 
	 * @param des
	 *            目的数组
	 * @param src
	 *            源数组
	 * @param mode
	 *            传输对象
	 */
	public static void Copy(byte[] des, byte[] src, CopyIndex mode) {
		System.arraycopy(src, 0, des, mode.getIndex(), src.length);
		mode.AddIndex(src.length);
	}

	/**
	 * 数组拷贝方法扩展
	 * 
	 * @param des
	 *            目的数组
	 * @param src
	 *            源数组
	 * @param length
	 *            拷贝长度
	 * @param mode
	 *            传输对象
	 */
	public static void Copy(byte[] des, byte[] src, int length, CopyIndex mode) {
		System.arraycopy(src, 0, des, mode.getIndex(), length);
		mode.AddIndex(src.length);
	}

	/**
	 * 拷贝数据到数组
	 * 
	 * @param des
	 *            目的数组
	 * @param src
	 *            源
	 * @param mode
	 *            传输对象
	 */
	public static void Copy(byte[] des, byte src, CopyIndex mode) {
		byte[] temp = new byte[] { src };
		System.arraycopy(temp, 0, des, mode.getIndex(), 1);
		mode.AddIndex(1);
	}

	/**
	 * 拷贝数据到数组
	 * 
	 * @param des
	 *            目的数组
	 * @param src
	 *            源
	 * @param mode
	 *            目的数组的起始位置
	 */
	public static void Copy(byte[] des, short src, CopyIndex mode) {
		int size = 2;
		ByteBuffer temp = createByteBuffer(size, des, mode);
		temp.asShortBuffer().put(src);
		mode.AddIndex(size);
	}

	/**
	 * 拷贝数据到数组
	 * 
	 * @param des
	 *            目的数组
	 * @param src
	 *            源
	 * @param mode
	 *            目的数组的起始位置
	 */
	public static void Copy(byte[] des, int src, CopyIndex mode) {
		int size = 4;
		ByteBuffer temp = createByteBuffer(size, des, mode);
		temp.asIntBuffer().put(src);
		mode.AddIndex(size);
	}

	/**
	 * 拷贝数据到数组
	 * 
	 * @param des
	 *            目的数组
	 * @param src
	 *            源
	 * @param mode
	 *            目的数组的起始位置
	 */
	public static void Copy(byte[] des, long src, CopyIndex mode) {
		int size = 8;
		ByteBuffer temp = createByteBuffer(size, des, mode);
		temp.asLongBuffer().put(src);
		mode.AddIndex(size);
	}

	/**
	 * 从数组中获取相对应的数据
	 * 
	 * @param src
	 *            源数组
	 * @param mode
	 *            目的数组的起始位置
	 * @return 值
	 */
	public static byte[] GetBytes(byte[] src, int size, CopyIndex mode) {
		byte[] result = new byte[size];
		System.arraycopy(src, mode.getIndex(), result, 0, size);
		mode.AddIndex(size);
		return result;
	}

	/**
	 * 从数组中获取相对应的数据
	 * 
	 * @param src
	 *            源数组
	 * @param mode
	 *            目的数组的起始位置
	 * @return 值
	 */
	public static Byte[] GetBytes(Byte[] src, int size, CopyIndex mode) {
		Byte[] result = new Byte[size];
		System.arraycopy(src, mode.getIndex(), result, 0, size);
		mode.AddIndex(size);
		return result;
	}

	/**
	 * 从数组中获取相对应的数据
	 * 
	 * @param src
	 *            源数组
	 * @param mode
	 *            目的数组的起始位置
	 * @return 值
	 */
	public static byte GetInt8(byte[] src, CopyIndex mode) {
		int size = 1;
		byte result = src[mode.getIndex()];
		mode.AddIndex(size);
		return result;
	}

	/**
	 * 从数组中获取相对应的数据
	 * 
	 * @param src
	 *            源数组
	 * @param mode
	 *            目的数组的起始位置
	 * @return 值
	 */
	public static short GetUInt8(byte[] src, CopyIndex mode) {
		int size = 1;
		byte result = src[mode.getIndex()];
		mode.AddIndex(size);
		return (short) (result & 0xFF);
	}

	/**
	 * 从数组中获取相对应的数据
	 * 
	 * @param src
	 *            源数组
	 * @param mode
	 *            目的数组的起始位置
	 * @return 值
	 */
	public static short GetInt16(byte[] src, CopyIndex mode) {
		int size = 2;
		ByteBuffer temp = createByteBuffer(size, src, mode);
		return temp.getShort();
	}

	/**
	 * 从数组中获取相对应的数据
	 * 
	 * @param src
	 *            源数组
	 * @param mode
	 *            目的数组的起始位置
	 * @return 值
	 */
	public static int GetUInt16(byte[] src, CopyIndex mode) {
		short temp = GetInt16(src, mode);
		return temp & 0xFFFF;
	}

	/**
	 * 从数组中获取相对应的数据
	 * 
	 * @param src
	 *            源数组
	 * @param mode
	 *            目的数组的起始位置
	 * @return 值
	 */
	public static int GetInt32(byte[] src, CopyIndex mode) {
		int size = 4;
		ByteBuffer temp = createByteBuffer(size, src, mode);
		return temp.getInt();
	}

	/**
	 * 从数组中获取相对应的数据
	 * 
	 * @param src
	 *            源数组
	 * @param mode
	 *            目的数组的起始位置
	 * @return 值
	 */
	public static long GetUInt32(byte[] src, CopyIndex mode) {
		int temp = GetInt32(src, mode);
		return temp & 0xFFFFFFFF;
	}

	/**
	 * 从数组中获取相对应的数据
	 * 
	 * @param src
	 *            源数组
	 * @param mode
	 *            目的数组的起始位置
	 * @return 值
	 */
	public static long GetInt64(byte[] src, CopyIndex mode) {
		int size = 8;
		ByteBuffer temp = createByteBuffer(size, src, mode);
		return temp.getInt();
	}

	public static byte[] short2Buffer(short src, boolean isBigEndian) {
		ByteBuffer temp = ByteBuffer.wrap(new byte[2]);
		if (isBigEndian) {
			temp.order(ByteOrder.BIG_ENDIAN);
		} else {
			temp.order(ByteOrder.LITTLE_ENDIAN);
		}
		temp.asShortBuffer().put(src);
		return temp.array();
	}

	private static ByteBuffer createByteBuffer(int size, byte[] buffer, CopyIndex mode) {
		ByteBuffer temp = ByteBuffer.wrap(buffer, mode.getIndex(), size);
		if (mode.isBigEndian()) {
			temp.order(ByteOrder.BIG_ENDIAN);
		} else {
			temp.order(ByteOrder.LITTLE_ENDIAN);
		}
		return temp;
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
}
