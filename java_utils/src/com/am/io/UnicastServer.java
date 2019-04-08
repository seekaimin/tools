package com.am.io;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.nio.ByteBuffer;
import java.nio.channels.DatagramChannel;
import java.nio.channels.SelectionKey;
import java.nio.channels.Selector;
import java.util.Iterator;

import com.am.utilities.stringhelpers;
import com.am.utilities.tools;

/**
 * udp 单播服务程序
 * 
 * @author am
 *
 */
public class UnicastServer {

	/**
	 * udp单播回调处理接口
	 * @author am
	 *
	 */
	public interface IUnicastCallback {
		/**
		 * 接收数据处理程序
		 * @param channel
		 */
		public void doReceive(DatagramChannel channel);

		/**
		 * 出现异常处理程序
		 * @param e
		 */
		public void doException(Exception e);

	}

	public static void main(String args[]) {
		try {
			UnicastServer.ins().setReceiveBufferSize(1316 + 100);
			UnicastServer.ins().setAction(new IUnicastCallback() {
				@Override
				public void doException(Exception e) {
					e.printStackTrace();
					tools.println(e.getMessage());
				}

				@Override
				public void doReceive(DatagramChannel channel) {
					ByteBuffer byteBuffer = ByteBuffer.allocate(UnicastServer.ins().getReceiveBufferSize());
					try {
						channel.receive(byteBuffer);
						tools.println(stringhelpers.bytesToHexString(byteBuffer.array()));
					} catch (IOException e) {
						e.printStackTrace();
					}
				}

			});
			UnicastServer.ins().start();
		} catch (Exception e) {
			e.printStackTrace();
		}

	}

	private static UnicastServer _ins = null;

	/**
	 * 单件
	 * 
	 * @return
	 */
	public static UnicastServer ins() {
		if (_ins == null) {
			_ins = new UnicastServer();
		}
		return _ins;
	}

	boolean running = false;
	int port = 10001;
	int receiveBufferSize = 1024;

	DatagramChannel channel;
	Selector selector;
	IUnicastCallback action;

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
		while (running) {
			try {
				int selectCount = selector.select();
				if (selectCount > 0) {
					Iterator<SelectionKey> iterator = selector.selectedKeys().iterator();
					while (iterator.hasNext()) {
						SelectionKey key = iterator.next();
						iterator.remove();
						DatagramChannel c = (DatagramChannel) key.channel();
						if (key.isValid() && key.isReadable()) {
							this.action.doReceive(c);
						}
					}
				}
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
			channel = DatagramChannel.open();
			channel.configureBlocking(false);
			channel.socket().bind(new InetSocketAddress(port));
			selector = Selector.open();
			channel.register(selector, SelectionKey.OP_READ);
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
		tools.close(channel);
		tools.close(selector);
		tools.println("service : %s is stoped!", this.getServiceName());
	}

	public boolean isRunning() {
		return running;
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
	public void setAction(IUnicastCallback action) {
		this.action = action;
	}

}
