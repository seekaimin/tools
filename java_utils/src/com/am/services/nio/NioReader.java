package com.am.services.nio;

import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.channels.SelectionKey;
import java.nio.channels.SocketChannel;
import java.util.Date;

import com.am.services.nio.event.NioException;

/**
 * <p>
 * Title: 读线程
 * </p>
 * <p>
 * Description: 该线程用于读取客户端数据
 * </p>
 * 
 * @author starboy
 * @version 1.0
 */

public class NioReader extends Thread {
	private NioServer server = null;

	public NioReader(NioServer server) {
		this.server = server;
	}

	public void run() {
		while (this.server.isRunning()) {
			try {
				SelectionKey key = this.server.getCanReaderRequest();
				// 读取数据
				read(key);
			} catch (Exception e) {
				continue;
			}
		}
	}

	/**
	 * 读取客户端发出请求数据
	 * 
	 * @param sc
	 *            套接通道
	 */
	private static int BUFFER_SIZE = 1024;

	public static byte[] readRequest(SocketChannel sc) throws IOException {
		ByteBuffer buffer = ByteBuffer.allocate(BUFFER_SIZE);
		int off = 0;
		int r = 0;
		byte[] data = new byte[BUFFER_SIZE * 10];

		while (true) {
			buffer.clear();
			r = sc.read(buffer);
			// System.out.println("rec len:" + r);
			if (r == -1)
				throw new IOException("远程连接已断开");
			if (r == 0)
				break;
			if (r > 0) {
				if ((off + r) > data.length) {
					data = grow(data, BUFFER_SIZE * 10);
				}
				byte[] buf = buffer.array();
				System.arraycopy(buf, 0, data, off, r);
				off += r;
			}
		}
		byte[] req = new byte[off];
		System.arraycopy(data, 0, req, 0, off);
		return req;
	}

	/**
	 * 处理连接数据读取
	 * 
	 * @param key
	 *            SelectionKey
	 * @throws Exception
	 */
	public void read(SelectionKey key) throws Exception {
		SocketChannel sc = null;
		try {
			// 读取客户端数据
			sc = (SocketChannel) key.channel();
			byte[] clientData = readRequest(sc);
			if (clientData.length > 0) {
				NioRequest request = (NioRequest) key.attachment();
				request.setDataInput(clientData);
				request.setRequestTime(new Date());
				// 触发onRead
				server.getNotifier().fireOnRead(request);
				// 提交主控线程进行写处理
				server.setWriterRequest(key);
			}
		} catch (IOException e) {
			server.disposeKey(key);
		} catch (NioException e) {
			if (e.isClose()) {
				server.disposeKey(key);
			} else {
				server.getNotifier().fireOnError(e);
			}
		} catch (Exception e) {
			server.getNotifier().fireOnError(e);
		}
	}

	/**
	 * 数组扩容
	 * 
	 * @param src
	 *            byte[] 源数组数据
	 * @param size
	 *            int 扩容的增加量
	 * @return byte[] 扩容后的数组
	 */
	public static byte[] grow(byte[] src, int size) {
		byte[] tmp = new byte[src.length + size];
		System.arraycopy(src, 0, tmp, 0, src.length);
		return tmp;
	}
}
