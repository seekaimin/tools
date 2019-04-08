package com.am.msc;

public class CopyIndex {
	private int Index = 0;
	private boolean bigEndian = true;

	public boolean isBigEndian() {
		return this.bigEndian;
	}

	/**
	 * 构造
	 */
	public CopyIndex() {
	}

	/**
	 * 构造
	 * 
	 * @param bigEndian
	 *            true:大端网络序 false:小端字节序
	 */
	public CopyIndex(boolean bigEndian) {
		this.bigEndian = bigEndian;
	}

	/**
	 * 构造
	 * 
	 * @param index
	 *            起始下标
	 */
	public CopyIndex(int index) {
		setIndex(index);
	}

	/**
	 * 将下标增加相对应的长度
	 * 
	 * @param length
	 *            增加的值
	 */
	public void AddIndex(int length) {
		setIndex(getIndex() + length);
	}

	/**
	 * 重置下标
	 */
	public void Reset() {
		setIndex(0);
	}

	/**
	 * 获取下标
	 * 
	 * @return 下标
	 */
	public int getIndex() {
		return Index;
	}

	/**
	 * 设置下标
	 * 
	 * @param Index
	 *            当前下标的值
	 */
	public void setIndex(int Index) {
		this.Index = Index;
	}
}
