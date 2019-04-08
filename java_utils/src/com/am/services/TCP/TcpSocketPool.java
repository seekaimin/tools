package com.am.services.TCP;

import java.io.Closeable;
import java.io.IOException;
import java.util.LinkedList;
import java.util.List;

import com.am.utilities.tools;

public class TcpSocketPool implements Closeable {

	private boolean disposed = false;
	/**
	 * 缓存池
	 */
	private List<TcpSocket> pool = new LinkedList<TcpSocket>();

	public TcpSocketPool() {
		disposed = false;
	}

	/**
	 * 获取第一个
	 * 
	 * @return
	 * @throws InterruptedException
	 */
	public TcpSocket get() throws InterruptedException {
		synchronized (pool) {
			while (pool.isEmpty()) {
				if (this.isDisposed()) {
					break;
				}
				pool.wait();
			}
			if (pool.isEmpty()) {
				return null;
			}
			return pool.remove(0);
		}
	}

	/**
	 * 添加一个
	 */
	public void set(TcpSocket key) {
		synchronized (pool) {
			if (this.isDisposed()) {
				return;
			}
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
		if (this.isDisposed()) {
			return;
		}
		this.disposed = true;
		synchronized (pool) {
			pool.notifyAll();
			while (!pool.isEmpty()) {
				tools.close(pool.remove(0));
			}
		}
	}


	public boolean isDisposed() {
		return this.disposed;
	}

	// public List<TcpSocket> getPool() {
	// return pool;
	// }
}
