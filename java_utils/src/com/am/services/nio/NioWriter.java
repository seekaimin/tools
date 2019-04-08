package com.am.services.nio;

import java.io.IOException;
import java.nio.channels.SelectionKey;
import java.nio.channels.SocketChannel;

import com.am.services.nio.event.NioException;

/**
 * <p>
 * Title: 回应线程
 * </p>
 * <p>
 * Description: 用于向客户端发送信息
 * </p>
 * 
 * @author starboy
 * @version 1.0
 */

public class NioWriter extends Thread {
	public NioWriter(NioServer server) {
		this.server = server;
	}

	private NioServer server = null;

	/**
	 * SMS发送线程主控服务方法,负责调度整个处理过程
	 */
	public void run() {
		while (server.isRunning()) {
			try {
				SelectionKey key = this.server.getCanWriterRequest();
				// NioRequest request = (NioRequest) key.attachment();
				// long ms = 500 - (new Date().getTime() -
				// request.getRequestTime().getTime());
				// if (ms > 0) {
				// this.server.setReaderRequest(key);
				// Thread.sleep(1);
				// continue;
				// }
				// 处理写事件
				write(key);
			} catch (Exception e) {
				continue;
			}
		}
	}

	/**
	 * 处理向客户发送数据
	 * 
	 * @param key
	 *            SelectionKey
	 * @throws Exception
	 */
	public void write(SelectionKey key) throws Exception {
		SocketChannel sc = null;
		try {
			sc = (SocketChannel) key.channel();
			NioResponse response = new NioResponse(sc);
			// 触发onWrite事件
			NioRequest request = (NioRequest) key.attachment();
			server.getNotifier().fireOnWrite(request, response);

			key.attach(null);
			server.setReaderRequest(key);
		} catch (IOException e) {
			server.disposeKey(key);
		} catch (NioException e) {
			if (e.isClose()) {
				server.disposeKey(key);
			} else {
				server.getNotifier().fireOnError(e);

				key.attach(null);
				server.setReaderRequest(key);
			}
		} catch (Exception e) {
			server.getNotifier().fireOnError(e);
			key.attach(null);
			server.setReaderRequest(key);
		}
	}

}
