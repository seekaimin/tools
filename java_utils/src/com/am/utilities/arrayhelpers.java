package com.am.utilities;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.Calendar;
import java.util.Date;

/**
 * 数组工具类 提供关于数组的操作方法
 * 
 * @author am
 *
 */
public class arrayhelpers {

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
	 * 拷贝数据到数组 数组占位4B 距1970-1-1的总秒数
	 * 
	 * @param des
	 *            目的数组
	 * 
	 * @param src
	 *            源
	 * 
	 * @param mode
	 *            目的数组的起始位置
	 */
	public static void CopyDateTime4(byte[] des, Date src, CopyIndex mode) {
		int seconds = (int) (datetimehelpers.getTotalMilliseconds(src) / 1000);
		Copy(des, seconds, mode);
	}

	/**
	 * 拷贝数据到数组 数组占位4B 距1970-1-1的总秒数
	 * 
	 * @param des
	 *            目的数组
	 * 
	 * @param src
	 *            源
	 * 
	 * @param mode
	 *            目的数组的起始位置
	 */
	public static void CopyDateTime4(byte[] des, String src, CopyIndex mode) {
		Date date = datetimehelpers.toDate(src, datetimehelpers.dateTimeFormatString);
		CopyDateTime4(des, date, mode);
	}

	/**
	 * 拷贝数据到数组 数组占位 7B year(2B) + month(1B) + day(1B) + hour(1B) + minute(1B) +
	 * second(1B)
	 * 
	 * @param des
	 *            目的数组
	 * 
	 * @param src
	 *            源
	 * 
	 * @param mode
	 *            目的数组的起始位置
	 */
	public static void CopyDateTime7(byte[] des, Date src, CopyIndex mode) {
		short year = (short) datetimehelpers.getYear(src);
		byte month = (byte) datetimehelpers.getMonth(src);
		byte day = (byte) datetimehelpers.getDay(src);
		byte hour = (byte) datetimehelpers.getHour(src);
		byte minute = (byte) datetimehelpers.getMinute(src);
		byte second = (byte) datetimehelpers.getSecond(src);

		arrayhelpers.Copy(des, year, mode);
		arrayhelpers.Copy(des, month, mode);
		arrayhelpers.Copy(des, day, mode);
		arrayhelpers.Copy(des, hour, mode);
		arrayhelpers.Copy(des, minute, mode);
		arrayhelpers.Copy(des, second, mode);

	}

	/**
	 * 拷贝数据到数组 数组占位 7B year(2B) + month(1B) + day(1B) + hour(1B) + minute(1B) +
	 * second(1B)
	 * 
	 * @param des
	 *            目的数组
	 * 
	 * @param src
	 *            源
	 * 
	 * @param mode
	 *            目的数组的起始位置
	 */
	public static void CopyDateTime7(byte[] des, String src, CopyIndex mode) {
		Date date = datetimehelpers.toDate(src, datetimehelpers.dateTimeFormatString);
		CopyDateTime7(des, date, mode);
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

	/**
	 * 从数组中获取相对应的数据 占位4B
	 * 
	 * @param src
	 *            源数组
	 * @param mode
	 *            目的数组的起始位置
	 * @return 值
	 */
	public static Date GetDateTime(byte[] src, CopyIndex mode) {
		int seconds = GetInt32(src, mode);
		return datetimehelpers.addTime(Calendar.SECOND, seconds);
	}

	/**
	 * 输入流转byte[]
	 * 
	 * @param stream
	 *            输入流
	 * @param isCloseStream
	 *            是否关闭输入流
	 * @return byte[]
	 */
	public static byte[] inputStreamToByteArray(InputStream stream, boolean isCloseStream) {
		ByteArrayOutputStream bytestream = new ByteArrayOutputStream();
		int size = 0;
		byte[] result = new byte[0];
		try {
			byte[] data = new byte[1024];
			while ((size = stream.read(data)) != -1) {
				bytestream.write(data, 0, size);
			}
			result = bytestream.toByteArray();
		} catch (IOException e) {

		} finally {
			tools.close(bytestream);
			if (isCloseStream) {
				tools.close(stream);
			}
		}
		return result;
	}

	/**
	 * 将数组倒序
	 * 
	 * @param src
	 *            源数组
	 * @return
	 */
	public static byte[] desc(byte[] src) {
		if (src == null) {
			src = new byte[0];
		}
		byte[] result = new byte[src.length];
		int i = src.length - 1;
		int j = 0;
		for (; j < src.length;) {
			result[j] = src[i];
			i--;
			j++;
		}
		return result;
	}

	/**
	 * 从start开始到结束 开始搜索标记
	 * 
	 * @param start
	 *            搜索的起始位置
	 * @param src
	 * @param flags
	 *            目标标记
	 * @return 返回目标标记的起始下标 如果为-1 表示不存在
	 */
	public static int indexOf(int start, byte[] src, byte... des) {

		return indexOf(start, -1, src, des);
	}

	/**
	 * 从start开始到end结束 开始搜索标记
	 * 
	 * @param start
	 *            搜索的起始位置
	 * @param end
	 *            搜索的结束位置
	 * @param src
	 * @param flags
	 *            目标标记
	 * @return 返回目标标记的起始下标 如果为-1 表示不存在
	 */
	public static int indexOf(int start, int end, byte[] src, byte... des) {
		if (end == -1 || end > src.length) {
			end = src.length;
		}
		boolean flag = false;
		for (int i = start; i < end; i++) {
			if (i <= end - des.length) {
				flag = true;
				for (int j = 0; j < des.length; j++) {
					if (src[i + j] == des[j]) {
						flag = false;
						break;
					}
				}
			} else {
				flag = false;
			}
			if (flag) {
				return i;
			}
		}
		return -1;
	}
}
