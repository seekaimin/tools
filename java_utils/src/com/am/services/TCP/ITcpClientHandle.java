package com.am.services.TCP;

/**
 * 处理客户端句柄
 * @author am
 */
public interface ITcpClientHandle {

	/**
	 * 处理句柄
	 * @param socket 通讯套接字
	 * @param size 接收数据长度
	 * @param buffer 接收缓存
	 * @throws TcpException TCP异常
	 */
	void handle(TcpSocket socket) throws TcpException;
	
	/**
	 * 异常处理
	 * @param e
	 */
	void exception(Exception e);
}
