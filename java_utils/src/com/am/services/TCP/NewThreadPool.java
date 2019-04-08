package com.am.services.TCP;

import java.io.Closeable;
import java.io.IOException;
import java.net.SocketTimeoutException;

import com.am.utilities.tools;

public class NewThreadPool implements Runnable, Closeable {
	private NewServer server;
	private boolean running = false;
	private TcpSocketPool readPool = new TcpSocketPool();
	private TcpSocketPool writePool = new TcpSocketPool();
	private int maxClient = 10;

	public NewThreadPool(NewServer server) {
		running = true;
		this.server = server;
		this.maxClient = server.getMaxClientSize();
	}

	public boolean isRunning() {
		return running && this.server.isRunning();
	}

	@Override
	public void close() throws IOException {
		if (!this.running) {
			return;
		}
		this.running = false;
	}

	@Override
	public void run() {
		// get client
		new Thread(() -> {
			pull();
		}).start();
		// read
		new Thread(() -> {
			read();
		}).start();
		// write
		for (int i = 0; i < 1; i++) {
			new Thread(() -> {
				write();
			}).start();
		}
	}

	private void pull() {
		while (this.isRunning()) {
			if (this.maxClient > this.readPool.size() + this.writePool.size()) {
				try {
					TcpSocket item = this.server.getReadPool();
					if (item != null) {
						this.setReadPool(item);
					}
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
			} else {
				tools.sleep(1);
			}
		}
	}

	private void read() {
		while (this.isRunning()) {

			TcpSocket client = null;
			try {
				client = this.getReadPool();
				if (client != null) {
					client.read();
					this.setWritePool(client);
				}
			} catch (SocketTimeoutException e) {
				this.server.setReadPool(client, false);
			} catch (TcpException e) {
				if (e.isClose()) {
					this.server.RemvoeClient(client);
				} else {
					this.server.setReadPool(client, false);
				}
			} catch (Exception e) {
				this.server.RemvoeClient(client);
			}

		}
	}

	private void write() {
		while (this.isRunning()) {
			TcpSocket client = null;
			try {
				client = this.getWritePool();
				if (client != null) {
					this.server.getHandle().handle(client);
					client.write();
					this.server.setReadPool(client, false);
				}
			} catch (TcpException e) {
				if (e.isClose()) {
					this.server.RemvoeClient(client);
				} else {
					this.server.setReadPool(client, false);
				}
			} catch (Exception e) {
				this.server.RemvoeClient(client);
			}

		}
	}

	public TcpSocket getReadPool() throws InterruptedException {
		return readPool.get();
	}

	public void setReadPool(TcpSocket item) {
		this.readPool.set(item);
	}

	public TcpSocket getWritePool() throws InterruptedException {
		return writePool.get();
	}

	public void setWritePool(TcpSocket item) {
		this.writePool.set(item);
	}

}
