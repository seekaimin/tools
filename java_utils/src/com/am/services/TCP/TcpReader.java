package com.am.services.TCP;

import java.io.Closeable;
import java.io.IOException;
import java.net.SocketException;
import java.net.SocketTimeoutException;

import com.am.utilities.tools;

public class TcpReader implements Runnable, Closeable {
	private boolean running = false;
	ITcpClientHandle handle;
	TcpServer server;

	public TcpReader(TcpServer server) {
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
				socket = this.server.getReadTcpSocket();
			} catch (Exception e) {
				e.printStackTrace();
				continue;
			}
			if (socket == null) {
				//tools.println("sssssssssssssss");
				continue;
			}
			try {
				socket.read();
				this.server.setExecuteTcpSocket(socket);
			} catch (SocketTimeoutException e) {
				//tools.println("read timeout");
				this.server.setReadTcpSocket(socket);
			} catch (SocketException e) {
				tools.close(socket);
				this.handle.exception(e);
			} catch (TcpException e) {
				if (e.isClose()) {
					tools.close(socket);
				} else {
					//tools.println("read ex");
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