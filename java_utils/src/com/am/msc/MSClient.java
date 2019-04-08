package com.am.msc;

import java.io.Closeable;
import java.io.IOException;
import java.net.Socket;

public class MSClient implements Closeable {
	private static MSClient _instance = null;
	public static MSClient instance = _instance == null ? new MSClient() : _instance;

	private boolean running = false;

	public boolean isRunning() {
		return running;
	}

	Socket socket;

	public boolean Init(String server, int port, String key) throws IOException {
		if (this.isRunning()) {
			this.close();
		}
		socket = new Socket(server, port);
		byte[] keyBuffer = util.toBytes(key, "UTF-8");
		int size = 2 + 2 + keyBuffer.length;
		// 注册
		byte[] buffer = new byte[size];
		CopyIndex index = new CopyIndex();
		util.Copy(buffer, util.HEAD_CLIENT, index);
		util.Copy(buffer, (short) keyBuffer.length, index);
		util.Copy(buffer, keyBuffer, index);
		socket.getOutputStream().write(buffer);
		this.running = true;
		return true;
	}

	public void Receive(IBufferAction action) {
		synchronized (socket) {
			int size = 1024;
			byte[] buffer = new byte[size];
			try {
				while (this.isRunning()) {
					int len = socket.getInputStream().read(buffer);
					CopyIndex index = new CopyIndex();
					byte[] data = util.GetBytes(buffer, len, index);
					action.call(data);
				}
			} catch (Exception e) {
				action.error(e);
				try {
					this.close();
				} catch (IOException e1) {
				}
			}
		}
	}

	public void Receive(String encoding, IMessageAction action) {
		synchronized (socket) {
			int size = 1024;
			byte[] buffer = new byte[size];
			try {
				while (this.isRunning()) {
					int len = socket.getInputStream().read(buffer);
					CopyIndex index = new CopyIndex();
					byte[] data = util.GetBytes(buffer, len, index);
					action.call(util.toString(data, encoding));
				}
			} catch (Exception e) {
				action.error(e);
				try {
					this.close();
				} catch (IOException e1) {
				}
			}
		}
	}

	@Override
	public void close() throws IOException {
		if (false == this.isRunning()) {
			return;
		}
		this.running = false;
		socket.close();
	}
}
