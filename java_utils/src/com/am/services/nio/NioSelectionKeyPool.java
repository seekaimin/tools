package com.am.services.nio;

import java.io.Closeable;
import java.io.IOException;
import java.nio.channels.SelectionKey;
import java.nio.channels.SocketChannel;
import java.util.LinkedList;
import java.util.List;

/**
 * 处理key的线程池
 * 
 * @author Administrator
 *
 */
public class NioSelectionKeyPool implements Closeable {

	public NioServer server;

	public NioSelectionKeyPool(NioServer server) {
		this.server = server;
	}

	private List<SelectionKey> pool = new LinkedList<SelectionKey>();

	/**
	 * 获取第一个
	 * 
	 * @return
	 * @throws InterruptedException
	 */
	public SelectionKey get() throws InterruptedException {
		synchronized (pool) {
			while (pool.isEmpty()) {
				pool.wait();
			}
			SelectionKey key = pool.remove(0);
			return key;
		}
	}

	/**
	 * 添加一个
	 */
	public void set(SelectionKey key) {
		synchronized (pool) {
			pool.add(key);
			pool.notifyAll();
		}
	}

	/**
	 * key池长度
	 * 
	 * @return
	 */
	public int size() {
		return pool.size();
	}

	/**
	 * 清空
	 */
	@Override
	public void close() throws IOException {
		synchronized (pool) {
			pool.clear();
		}
	}

	/**
	 * 事件注册
	 * 
	 * @param selector
	 * @param nioNotifier
	 */
	public boolean register(int opts) {
		synchronized (pool) {
			boolean flag = false;
			while (!pool.isEmpty()) {
				flag = true;
				SelectionKey key = pool.remove(0);
				SocketChannel schannel = (SocketChannel) key.channel();
				try {
					if (opts == SelectionKey.OP_READ) {
						schannel.register(server.getSelector(), opts, new NioRequest(schannel));
					} else {
						schannel.register(server.getSelector(), opts, key.attachment());
					}
				} catch (Exception e) {
					this.set(key);
					server.getNotifier().fireOnError(e);
				}
			}
			return flag;
		}
	}

}
