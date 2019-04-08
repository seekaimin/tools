package com.am.services.nio.event;

/**
 * 
 * @author Administrator
 *
 */
public class NioException extends Exception {

	/**
	 * 
	 */
	private static final long serialVersionUID = -1800171813824371236L;
	
	/**
	 * 是否关闭通道
	 */
	private boolean close = false;
	
	public NioException(boolean close,String message){
		super(message);
		this.setClose(close);
	}

	/**
	 * 是否关闭通道
	 */
	public boolean isClose() {
		return close;
	}

	/**
	 * 是否关闭通道
	 */
	public void setClose(boolean close) {
		this.close = close;
	}

}
