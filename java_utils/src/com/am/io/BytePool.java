package com.am.io;

import java.util.Arrays;

import com.am.utilities.stringhelpers;
import com.am.utilities.tools;

/**
 * 处理数据
 * 
 * @author am
 *
 */
public class BytePool {

	public static void main(String args[]) {

		byte[] d = new byte[10];

		for (int i = 0; i < d.length; i++) {
			d[i] = (byte) i;
		}
		BytePool p = new BytePool(100);
		p.add(d);
		tools.println(p.toString());
		p.add(d);
		tools.println(p.toString());
		p.movePos(19);
		p.reset();
		tools.println(p.toString());
		p.add(d);
		tools.println(p.toString());
		p.movePos(9);
		p.reset();
		tools.println(p.toString());
		p.add(d);
		tools.println(p.toString());
		p.add(d);
		tools.println(p.toString());

	}

	private byte[] dataPool;
	private int length;
	private int position;
	private int maxLength;

	/**
	 * 构造
	 * 
	 * @param size
	 */
	public BytePool(int size) {
		maxLength = size;
		dataPool = new byte[maxLength];
	}

	/**
	 * 获取数据池
	 * 
	 * @return
	 */
	public byte[] getDataPool() {
		return dataPool;
	}

	/**
	 * 添加数据到数据池
	 * 
	 * @param buffer
	 *            需要添加的数据
	 * @return
	 */
	public boolean add(byte[] buffer) {
		synchronized (dataPool) {
			if (length + buffer.length > maxLength) {
				return false;
			}
			System.arraycopy(buffer, 0, dataPool, length, buffer.length);
			length += buffer.length;
			return true;
		}
	}

	/**
	 * 添加buffer
	 * 
	 * @param buffer
	 * @return
	 */
	public boolean add(byte[] buffer, int len) {
		synchronized (dataPool) {
			if (length + len > maxLength) {
				return false;
			}
			System.arraycopy(buffer, 0, dataPool, length, len);
			length += len;
			return true;
		}
	}

	/**
	 * 移动下标到pos位置
	 * 
	 * @param pos
	 * @return 是否移动成功
	 */
	public boolean movePos(int pos) {
		synchronized (dataPool) {
			if (pos >= 0) {
				if (pos <= length) {
					position = pos;
					return true;
				}
			}
			return false;
		}
	}

	/**
	 * position增加
	 * 
	 * @param pos
	 *            需要增加的下标
	 * @return
	 */
	public boolean addPos(int pos) {
		synchronized (dataPool) {
			if (pos >= 0) {
				if (pos + position <= length) {
					position += pos;
					return true;
				}
			}
			return false;
		}
	}

	/**
	 * 重置position 以及数据
	 */
	public void reset() {
		synchronized (dataPool) {
			int size = length - position;
			if (size > 0) {
				byte[] temp = new byte[size];
				System.arraycopy(dataPool, position, temp, 0, size);
				clear();
				System.arraycopy(temp, 0, dataPool, 0, size);
				length += size;
			} else {
				clear();
			}
		}
	}

	/**
	 * 清空buffer
	 */
	public void clear() {
		synchronized (dataPool) {
			Arrays.fill(dataPool, (byte) 0);
			position = 0;
			length = 0;
		}
	}

	/**
	 * toString 显示position - end 以十六进制的形式显示
	 */
	@Override
	public String toString() {
		synchronized (dataPool) {
			return stringhelpers.bytesToHexString(dataPool);
		}
	}

	/**
	 * 获取数据 position-length
	 * 
	 * @return
	 */
	public byte[] getActive() {
		synchronized (dataPool) {
			int size = length - position;
			if (size == 0) {
				return new byte[0];
			}
			byte[] temp = new byte[size];
			System.arraycopy(dataPool, position, temp, 0, size);
			return temp;
		}
	}
}
