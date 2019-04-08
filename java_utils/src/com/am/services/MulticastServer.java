package com.am.services;

import java.net.DatagramPacket;
import java.net.InetAddress;
import java.net.MulticastSocket;
import java.util.Arrays;

import com.am.utilities.nethelpers;
import com.am.utilities.stringhelpers;
import com.am.utilities.tools;

/**
 * udp 多播播服务程序
 * 
 * @author am
 *
 */
public class MulticastServer {

	/**
	 * udp多播回调处理接口
	 * 
	 * @author am
	 *
	 */
	public interface IMulticastCallback {
		/**
		 * 接收数据处理程序
		 * 
		 * @param channel
		 */
		public void doReceive(byte[] receive);

		/**
		 * 出现异常处理程序
		 * 
		 * @param e
		 */
		public void doException(Exception e);

	}

	public static void main(String args[]) {
		try {
			MulticastServer.ins().setReceiveBufferSize(1316 + 100);
			MulticastServer.ins().setAction(new IMulticastCallback() {
				@Override
				public void doException(Exception e) {
					e.printStackTrace();
					tools.println(e.getMessage());
				}

				@Override
				public void doReceive(byte[] receive) {
					tools.println(stringhelpers.bytesToHexString(receive));
				}

			});
			MulticastServer.ins().start();
		} catch (Exception e) {
			e.printStackTrace();
		}

	}

	private static MulticastServer _ins = null;

	/**
	 * 单件
	 * 
	 * @return
	 */
	public static MulticastServer ins() {
		if (_ins == null) {
			_ins = new MulticastServer();
		}
		return _ins;
	}

	boolean running = false;
	String server = "224.2.2.2";
	int port = 10001;
	int receiveBufferSize = 1024;

	MulticastSocket socket;
	InetAddress addr;
	IMulticastCallback action;

	// 读取
	// InetSocketAddress address =
	// (InetSocketAddress)channel.receive(byteBuffer);
	/**
	 * work 异步接收数据
	 * 
	 * @param success
	 *            成功处理代理
	 * @param error
	 *            出现异常处理代理
	 * @throws Exception
	 */
	void doWork() {
		byte[] data = new byte[this.getReceiveBufferSize()];
		while (running) {
			try {
				Arrays.fill(data, (byte) 0); // 数组至0
				DatagramPacket pack = new DatagramPacket(data, data.length, addr, port);
				socket.receive(pack);
				this.action.doReceive(pack.getData());
			} catch (Exception e) {
				this.action.doException(e);
			}
		}
	}

	/**
	 * 验证
	 * 
	 * @return
	 */
	protected boolean validate() {

		try {
			if (port < 0) {
				return false;
			}
			if (this.action == null) {
				return false;
			}
			if (nethelpers.isIPAddress(server) == false) {
				return false;
			}
			addr = InetAddress.getByName(server);
			socket = new MulticastSocket(port);
			socket.joinGroup(addr);
		} catch (Exception e) {
			e.printStackTrace();
			return false;
		}
		return true;
	}

	protected String getServiceName() {
		return "service";
	}

	/**
	 * 启动服务
	 * 
	 * @throws Exception
	 */
	public void start() throws Exception {
		if (running) {
			return;
		}
		if (validate() == false) {
			tools.println("service : %s is validate error!", this.getServiceName());
			return;
		}
		running = true;
		new Thread(() -> {
			doWork();
		}).start();
		if (running) {
			tools.println("service : %s is started!", this.getServiceName());
		}
	}

	/**
	 * 停止服务
	 */
	public void stop() {
		if (running == false)
			return;
		running = false;
		tools.close(socket);
		try {
			socket.leaveGroup(addr);
		} catch (Exception e) {
		}
		tools.println("service : %s is stoped!", this.getServiceName());
	}

	/**
	 * 运行状态
	 * 
	 * @return
	 */
	public boolean isRunning() {
		return running;
	}

	/**
	 * 接收数据地址
	 * 
	 * @return
	 */
	public String getServer() {
		return server;
	}

	/**
	 * 接收数据地址
	 * 
	 * @param server
	 */
	public void setServer(String server) {
		this.server = server;
	}

	/**
	 * 监听端口
	 * 
	 * @return
	 */

	public int getPort() {
		return port;
	}

	/**
	 * 监听端口
	 * 
	 * @param port
	 */

	public void setPort(int port) {
		this.port = port;
	}

	/**
	 * 接收数据缓存
	 * 
	 * @return 接收数据缓存大小
	 */
	public int getReceiveBufferSize() {
		return receiveBufferSize;
	}

	/**
	 * 接收数据缓存
	 * 
	 * @param size
	 */

	public void setReceiveBufferSize(int size) {
		this.receiveBufferSize = size;
	}

	/**
	 * 设置异常处理程序
	 * 
	 * @param success
	 */
	public void setAction(IMulticastCallback action) {
		this.action = action;
	}

}
