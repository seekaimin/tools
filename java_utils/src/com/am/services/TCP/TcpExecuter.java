package com.am.services.TCP;

import java.io.Closeable;
import java.io.IOException;

import com.am.utilities.tools;

public class TcpExecuter implements Runnable, Closeable {
	private boolean running = false;
	ITcpClientHandle handle;
	TcpServer server;

	public TcpExecuter(TcpServer server) {
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
				socket = this.server.getExecuteTcpSocket();
			} catch (Exception e) {
				continue;
			}
			if (socket == null) {
				continue;
			}
			try {
				//Date s = datetimehelpers.getNow();tools.println("aaaaaaaaaaaaaaaaaaaaaaaaa执行耗时:%d", datetimehelpers.getNow().getTime() - socket.getLastRead().getTime());
				this.handle.handle(socket);
				this.server.setWriteTcpSocket(socket);
			} catch (TcpException e) {
				if (e.isClose()) {
					tools.close(socket);
				} else {
					this.server.setReadTcpSocket(socket);
				}
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