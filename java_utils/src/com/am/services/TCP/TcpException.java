package com.am.services.TCP;

/**
 * 自定义TCP异常
 * 
 * @author am
 *
 */
public class TcpException extends Exception {

	/**
	 * 
	 */
	private static final long serialVersionUID = 7842529195200247040L;

	private boolean close = false;

	/**
	 * 自定义TCP异常
	 * 
	 * @param close
	 *            是否主动关闭客户端
	 * @param message
	 *            异常消息
	 */
	public TcpException(boolean close, String message) {
		super(message);
		this.setClose(close);
	}

	public TcpException(boolean close, Exception ex) {
		super(ex);
		this.setClose(close);
	}
	/**
	 * 是否关闭客户端
	 * 
	 * @return
	 */
	public boolean isClose() {
		return close;
	}

	/**
	 * 是否关闭客户端
	 * 
	 * @param close
	 */
	public void setClose(boolean close) {
		this.close = close;
	}

}
