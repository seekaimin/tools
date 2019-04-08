package com.am.services.TCP;

import java.io.Closeable;
import java.io.IOException;
import java.net.SocketException;
import java.net.SocketTimeoutException;

import com.am.utilities.tools;

public class TcpWriter implements Runnable, Closeable {
	private boolean running = false;
	ITcpClientHandle handle;
	TcpServer server;

	public TcpWriter(TcpServer server) {
		running = true;
		this.server = server;
		this.handle = server.getHandle();
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
		while (this.isRunning()) {
			TcpSocket socket = null;
			try {
				socket = this.server.getWriteTcpSocket();
			} catch (Exception e) {
				continue;
			}
			if (socket == null) {
				continue;
			}
			try {
				socket.write();
				this.server.setReadTcpSocket(socket);
				// tools.println("执行耗费时间(ms):%d",socket.getLastWrite().getTime()-socket.getLastRead().getTime());
				if (this.server.DEBUG) {
					//tools.println("执行耗时:%d",datetimehelpers.getNow().getTime()-							 socket.getLastRead() .getTime());
				}
			} catch (SocketTimeoutException e) {
				this.server.setReadTcpSocket(socket);
			} catch (SocketException e) {
				tools.close(socket);
				this.handle.exception(e);
			} catch (Exception e) {
				tools.close(socket);
				this.handle.exception(e);
			} finally {
			}
		}

	}

	public boolean isRunning() {
		return running && this.server.isRunning();
	}

}