package com.am.io;

import java.lang.reflect.Array;
import java.util.Arrays;

/**
 * 处理数据
 * 
 * @author am
 *
 */
public abstract class ObjectArray<T> {

	protected abstract Class<T> type();

	protected abstract T defvalue();

	public static void main(String args[]) {

	}

	protected T[] dataPool;
	protected int length;
	protected int position;
	protected int maxLength;

	/**
	 * 构造
	 * 
	 * @param size
	 */
	@SuppressWarnings("unchecked")
	public ObjectArray(int size) {

		maxLength = size;
		dataPool = (T[]) Array.newInstance(type(), maxLength);
	}

	/**
	 * 获取数据池
	 * 
	 * @return
	 */
	public T[] getDataPool() {
		return dataPool;
	}

	/**
	 * 添加数据到数据池
	 * 
	 * @param buffer
	 *            需要添加的数据
	 * @return
	 */
	public boolean add(T[] buffer) {
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
	@SuppressWarnings("unchecked")
	public void reset() {
		synchronized (dataPool) {
			int size = length - position;
			if (size > 0) {
				T[] temp = (T[]) Array.newInstance(type(), size);
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
			Arrays.fill(dataPool, this.defvalue());
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
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < dataPool.length; i++) {
				sb.append(dataPool[i]);
			}
			return sb.toString();
		}
	}

	/**
	 * 获取数据 position-length
	 * 
	 * @return
	 */
	@SuppressWarnings("unchecked")
	public T[] getActive() {
		synchronized (dataPool) {
			int size = length - position;
			if (size == 0) {
				T[] temp = (T[]) Array.newInstance(type(), 0);
				return temp;
			} else {
				T[] temp = (T[]) Array.newInstance(type(), size);
				System.arraycopy(dataPool, position, temp, 0, size);
				return temp;
			}
		}
	}

	public int getLength() {
		return length;
	}

	public int getPosition() {
		return position;
	}

	public int getMaxLength() {
		return maxLength;
	}

	
	

	/**
	 * 添加buffer
	 * 
	 * @param buffer
	 * @return
	 */
	public boolean add(T[] buffer, int len) {
		synchronized (dataPool) {
			if (length + len > maxLength) {
				return false;
			}
			System.arraycopy(buffer, 0, dataPool, length, len);
			length += len;
			return true;
		}
	}
}
