package com.am.utilities.web;

import java.io.Serializable;

import com.am.utilities.stringhelpers;

/**
 * web响应结构
 * 
 * @author Administrator
 *
 */
public class WebResponseResult implements Serializable {

	private static final long serialVersionUID = 1L;

	private long code = 0;
	private long total = 0;
	private long index = 0;
	private long size = 0;
	private String message = "";
	private Object data = null;

	public WebResponseResult() {

	}

	public WebResponseResult(long _code, String _message) {
		setCode(_code);
		setMessage(_message);
	}

	@Override
	public String toString() {
		return stringhelpers.fmt("code:%d-msg:%s", getCode(), getMessage());
	}

	/**
	 * 返回代码
	 * 
	 * @return
	 */
	public long getCode() {
		return code;
	}

	/**
	 * 设置反回代码
	 * 
	 * @param code
	 */
	public void setCode(long code) {
		this.code = code;
	}

	/**
	 * 获取返回消息
	 * 
	 * @return 返回消息
	 */
	public String getMessage() {
		return message;
	}

	/**
	 * 设置返回消息
	 * 
	 * @param message
	 *            消息
	 */
	public void setMessage(String message) {
		this.message = message;
	}

	/**
	 * 获取其他数据
	 * 
	 * @return 其他数据
	 */
	public Object getData() {
		return data;
	}

	/**
	 * 设置其他数据
	 * 
	 * @param data
	 *            其他数据
	 */
	public void setData(Object data) {
		this.data = data;
	}

	/**
	 * 总记录数
	 * 
	 * @return
	 */
	public long getTotal() {
		return total;
	}

	/**
	 * 页码
	 * 
	 * @return
	 */
	public long getIndex() {
		return index;
	}

	/**
	 * 总记录数
	 * 
	 * @param total
	 */
	public void setTotal(long total) {
		this.total = total;
	}

	/**
	 * 页码
	 * 
	 * @param index
	 */
	public void setIndex(long index) {
		this.index = index;
	}

	/**
	 * 每页显示记录数
	 * 
	 * @return
	 */
	public long getSize() {
		return size;
	}

	/**
	 * 每页显示记录数
	 * 
	 * @param size
	 */
	public void setSize(long size) {
		this.size = size;
	}

	/**
	 * 总页码
	 */
	public long getPages() {
		long pages = 1;
		if (this.getSize() > 0) {
			pages = (this.getTotal() - 1) / this.getSize() + 1;
		}
		return pages;
	}
}
