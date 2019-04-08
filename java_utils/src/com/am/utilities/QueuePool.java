package com.am.utilities;

import java.io.Closeable;
import java.io.IOException;
import java.util.LinkedList;
import java.util.List;

import com.am.delegate.IAction;

public class QueuePool<T> implements Closeable {

	private boolean disposed = false;
	/**
	 * 缓存池
	 */
	private List<T> pool = new LinkedList<T>();

	public QueuePool() {
		disposed = false;
	}

	/**
	 * 获取第一个
	 * 
	 * @return
	 * @throws InterruptedException
	 */
	public T get() throws InterruptedException {
		synchronized (pool) {
			while (pool.isEmpty()) {
				if (this.isDisposed()) {
					return null;
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
	public void add(T item) {
		synchronized (pool) {
			if (this.isDisposed()) {
				return;
			}
			pool.add(item);
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
			pool.clear();
			// while (!pool.isEmpty()) {
			// tools.close(pool.remove(0));
			// }
		}
	}

	public void close(IAction<T> action) {
		if (this.isDisposed()) {
			return;
		}
		this.disposed = true;
		synchronized (pool) {
			pool.notifyAll();
			while (pool.size() > 0) {
				action.call(pool.remove(0));
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