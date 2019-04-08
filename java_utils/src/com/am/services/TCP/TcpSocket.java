package com.am.services.TCP;

import java.io.Closeable;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.util.Date;

import com.am.io.BytePool;
import com.am.utilities.datetimehelpers;
import com.am.utilities.tools;

/**
 * 通讯套接字封装
 * 
 * @author am
 *
 */
public class TcpSocket implements Closeable {
	private Socket socket;
	private Date create;
	private Date lastRead;
	private Date lastWrite;
	private byte[] readBuffer = null;
	private byte[] writeBuffer = null;
	private BytePool data = null;

	public TcpSocket(Socket socket, int bufferSize) {
		this.socket = socket;
		create = datetimehelpers.getNow();
		readBuffer = new byte[bufferSize];
		data = new BytePool(bufferSize);
	}

	/**
	 * 通讯套接字
	 * 
	 * @return
	 */
	public Socket getSocket() {
		return socket;
	}

	@Override
	public void close() throws IOException {
		tools.close(socket);
	}

	public void setWriteBuffer(byte[] buffer) {
		this.writeBuffer = buffer;

	}
	public void read() throws IOException, TcpException {
		lastRead = datetimehelpers.getNow();
		InputStream stream = this.getSocket().getInputStream();
		int readSize = stream.read(this.readBuffer);
		if (readSize < 0) {
			throw new TcpException(true, "接收到数据长度为:-1");
		}
		if (readSize == 0) {
			throw new TcpException(false, "接收到数据长度为:0");
		}
		this.data.clear();
		this.data.add(readBuffer, readSize);

		// Arrays.fill(this.readBuffer, (byte) 0);
		// readSize = stream.read(this.readBuffer);
		// if (readSize < 0) {
		// throw new TcpException(true, "接收到数据长度为:-1");
		// } else if (readSize == 0) {
		// throw new TcpException(false, "接收到数据长度为:0");
		// } else {
		// this.data.clear();
		// this.data.add(readBuffer, readSize);
		// }
	}

	public void write() throws IOException {
		OutputStream stream = this.getSocket().getOutputStream();
		if (this.writeBuffer != null && this.readBuffer.length > 0) {
			lastWrite = datetimehelpers.getNow();
			stream.write(this.writeBuffer);
		}
		stream.flush();

	}

	public Date getCreate() {
		return create;
	}

	public Date getLastRead() {
		return lastRead;
	}

	public Date getLastWrite() {
		return this.lastWrite;
	}

	public byte[] getData() {
		return this.data.getActive();
	}

}
