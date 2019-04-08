package com.am.msc;

import java.io.Closeable;
import java.io.IOException;
import java.net.Socket;

public class MSServer implements Closeable {
	private static MSServer _instance = null;
	public static MSServer instance = _instance == null ? new MSServer() : _instance;

	private boolean running = false;

	public boolean isRunning() {
		return running;
	}

	Socket socket;

	public boolean Init(String server, int port) throws IOException {
		if (this.isRunning()) {
			this.close();
		}
		try {
			socket = new Socket(server, port);
			// 注册
			byte[] buffer = new byte[2];
			CopyIndex index = new CopyIndex();
			util.Copy(buffer, util.HEAD_SERVER, index);
			socket.getOutputStream().write(buffer);
		} catch (Exception ex) {
			ex.printStackTrace();
			return false;
		}
		this.running = true;
		return true;
	}

	public void Send(String key, byte[] valueBuffer) throws IOException {
		synchronized (socket) {
			byte[] keyBuffer = util.toBytes(key, "UTF-8");
			int keyLength = keyBuffer.length;
			int valueLength = valueBuffer.length;
			int len = 2 + keyLength + 1 + 2 + valueLength;
			int size = 4 + len;
			byte[] buffer = new byte[size];
			CopyIndex index = new CopyIndex();
			util.Copy(buffer, len, index);
			util.Copy(buffer, (short) keyLength, index);
			util.Copy(buffer, keyBuffer, index);
			util.Copy(buffer, util.SERVER_OP_CREATE_OR_SEND, index);
			util.Copy(buffer, (short) valueLength, index);
			util.Copy(buffer, valueBuffer, index);
			socket.getOutputStream().write(buffer);
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
