package com.am.utilities;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.Socket;
import java.net.SocketException;
import java.net.UnknownHostException;
import java.util.ArrayList;
import java.util.List;


/**
 * socket套接字帮助类
 * @author am
 *
 */
public class sockethelpers {

	/*********************************************************************/
	/************************* UDP编程 开始 ******************************/
	/*********************************************************************/
	/**
	 * 获取一个UDP客户端套接字
	 * 
	 * @param remote
	 *            通讯远程地址
	 * @param port
	 *            通讯远程端口
	 * @param timeout
	 *            超时时间
	 * @return udp客户端套接字
	 * @throws SocketException
	 * @throws UnknownHostException
	 */
	public static DatagramSocket getUdpClient(String remote, int port, int timeout)
			throws SocketException, UnknownHostException {
		DatagramSocket udp = new DatagramSocket();
		InetAddress address = InetAddress.getByName(remote);
		udp.setSoTimeout(timeout);
		udp.connect(address, port);
		return udp;
	}

	/**
	 * 发送数据
	 * 
	 * @param socket
	 *            套接字
	 * @param buffer
	 *            需要发送的数据
	 * @param receive_buffer_length
	 *            数据接收缓冲区大小 若为 <=0，则不需要接收数据
	 * @return 接收的数据
	 * @throws IOException
	 *             IOException
	 */
	public static byte[] send(DatagramSocket socket, byte[] buffer, int receive_buffer_length) throws IOException {
		byte[] result = new byte[0];
		DatagramPacket sendPacket = new DatagramPacket(buffer, buffer.length);
		socket.send(sendPacket);
		if (receive_buffer_length > 0) {
			result = receive(socket, receive_buffer_length);
		}
		return result;
	}

	/**
	 * 接收数据
	 * 
	 * @param socket
	 *            套接字
	 * @param receive_buffer_length
	 *            接收缓存大小
	 * @return 接收的数据
	 * @throws IOException
	 */
	public static byte[] receive(DatagramSocket socket, int receive_buffer_length) throws IOException {
		if (socket.getReceiveBufferSize() < receive_buffer_length) {
			socket.setReceiveBufferSize(receive_buffer_length);
		}
		// 缓存大小
		byte[] temp = new byte[receive_buffer_length];
		DatagramPacket receivePacket = new DatagramPacket(temp, receive_buffer_length);
		socket.receive(receivePacket);
		byte[] result = new byte[receivePacket.getLength()];
		System.arraycopy(receivePacket.getData(), 0, result, 0, result.length);
		return result;
	}

	/*********************************************************************/
	/************************* UDP编程 结束 ******************************/
	/*********************************************************************/

	/**
	 * 发送数据
	 * 
	 * @param socket
	 *            套接字
	 * @param buffer
	 *            需要发送的数据
	 * @param isreceivedata
	 *            是否接收数据
	 * @return 接收的数据
	 * @throws IOException
	 *             IOException
	 */
	public static byte[] send(Socket socket, byte[] buffer, boolean isreceivedata) throws IOException {
		byte[] result = new byte[0];
		OutputStream out = null;
		out = socket.getOutputStream();
		int index = 0;
		// 缓存大小
		int send_buffer_size = 1024;
		// 分段发送数据
		while (true) {
			int temp_size = send_buffer_size;
			if (buffer.length - index < send_buffer_size) {
				temp_size = buffer.length - index;
			}
			byte[] temp = new byte[temp_size];

			System.arraycopy(buffer, index, temp, 0, temp_size);
			out.write(temp);
			out.flush();
			index += temp_size;
			if (index >= buffer.length) {
				break;
			}
		}
		if (isreceivedata) {
			result = receive(socket);
		}
		return result;
	}

	/**
	 * 接收数据
	 * 
	 * @param socket
	 *            套接字
	 * @return 接收的数据
	 * @throws IOException
	 */
	public static byte[] receive(Socket socket) throws IOException {
		byte[] result = new byte[0];
		// 缓存大小
		int send_buffer_size = 1024;
		InputStream in = socket.getInputStream();
		List<byte[]> list = new ArrayList<byte[]>();
		int recv_length = 0;
		do {
			byte[] temp = new byte[send_buffer_size];
			int size = in.read(temp);
			if (size < 0) {
				break;
			}
			if (size < send_buffer_size) {
				byte[] d = new byte[size];
				System.arraycopy(temp, 0, d, 0, size);
				list.add(d);
			} else {
				list.add(temp);
			}
			recv_length += size;
		} while (in.available() > 0);
		if (recv_length > 0) {
			int copysize = 0;
			result = new byte[recv_length];
			for (byte[] t : list) {
				System.arraycopy(t, 0, result, copysize, t.length);
				copysize += t.length;
			}
		}
		return result;
	}

	public static void dispose() {

	}

}
