package com.am.services.TCP;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

import com.am.utilities.stringhelpers;
import com.am.utilities.tools;

/**
 * 
 * @author Administrator
 *
 */
public class NewServer {

	public static void main(String args[]) {
		try {
			NewServer.ins().DEBUG = true;
			NewServer.ins().setServiceName("门禁");
			NewServer.ins().setPort(11001);
			NewServer.ins().setHandleSize(8);
			NewServer.ins().setMaxClientSize(0);
			NewServer.ins().setReceiveBufferSize(1024 * 4);

			NewServer.ins().setHandle(new ITcpClientHandle() {
				@Override
				public void handle(TcpSocket socket) throws TcpException {
					// tools.sleep(1);
					// tools.println(stringhelpers.bytesToHexString(socket.getData()));
					String s = "fe fd 01 00 00 00 00 00 02 00 00 32 06 06 10 10 00 00 00 00 00 00 0106 06 20 16 00 00 00 01 00 30 00 0b 00 00 04 59 dc f7 9c 00 00 00 006e 78 b2 cd fe fd 01 00 00 00 00 00 01 00 00 38 06 06 10 10 00 00 0000 00 00 01 06 06 10 10 00 00 00 00 00 12 00 11 01 f0 0e 05 ee ee eeee ee ee aa aa aa aa aa aa aa df 37 86 67";
					s = s.replace(" ", "");
					s = "00000000";
					// byte[] d = stringhelpers.hexStringToBytes(s);
					socket.setWriteBuffer(socket.getData());
				}

				@Override
				public void exception(Exception e) {
				}
			});
			NewServer.ins().start();
		} catch (Exception e) {
			e.printStackTrace();
		}

	}

	int port = 11001;
	private static NewServer _ins = null;
	private TcpSocketPool readPool = new TcpSocketPool();
	private ServerSocket server;
	private boolean running = false;
	private ITcpClientHandle handle = null;
	private int handleSize = 4;
	public boolean DEBUG = true;
	private String statemsg = "";
	private String serviceName = "system service";
	private int receiveBufferSize = 100;
	private int maxClientSize = 1000;
	private int clientSize = 0;

	public synchronized void AddClient() {
		clientSize++;
	}

	public synchronized void RemvoeClient(TcpSocket client) {
		tools.close(client);
		if (clientSize > 0) {
			clientSize--;
		}
	}

	/**
	 * 单件
	 * 
	 * @return
	 */
	public static NewServer ins() {
		if (_ins == null) {
			_ins = new NewServer();
		}
		return _ins;
	}

	public ITcpClientHandle getHandle() {
		return this.handle;
	}

	public void setHandle(ITcpClientHandle handle) {
		this.handle = handle;
	}

	public TcpSocket getReadPool() throws InterruptedException {
		return readPool.get();
	}

	public void setReadPool(TcpSocket item, boolean calcSize) {
		this.readPool.set(item);
		if (calcSize) {
			this.AddClient();
		}
	}

	public boolean isRunning() {
		return running;
	}

	public String getStatemsg() {
		return statemsg;
	}

	public int getPort() {
		return port;
	}

	public void setPort(int port) {
		this.port = port;
	}

	public int getHandleSize() {
		return handleSize;
	}

	public void setHandleSize(int handleSize) {
		this.handleSize = handleSize;
	}

	public int getMaxClientSize() {
		return maxClientSize;
	}

	public void setMaxClientSize(int maxClientSize) {
		this.maxClientSize = maxClientSize;
	}

	public int getClientSize() {
		return clientSize;
	}

	public String getServiceName() {
		return serviceName;
	}

	public void setServiceName(String serviceName) {
		this.serviceName = serviceName;
	}

	public int getReceiveBufferSize() {
		return receiveBufferSize;
	}

	public void setReceiveBufferSize(int receiveBufferSize) {
		this.receiveBufferSize = receiveBufferSize;
	}

	public boolean validate() {
		try {
			server = new ServerSocket(port);
		} catch (IOException e) {
			tools.println("端口被暂用");
			return false;
		}
		return true;
	}

	public void start() {
		if (running || this.validate() == false) {
			return;
		}
		running = true;
		// 创建reader线程
		for (int i = 0; i < this.handleSize; i++) {
			new Thread(new NewThreadPool(this)).start();
		}
		new Thread(() -> {
			while (running) {
				int size = readPool.size();
				statemsg = stringhelpers.fmt("读取池剩余数量:%d---客户端数量:%d", size, this.clientSize);
				if (this.DEBUG) {
					tools.println(statemsg);
				}
				tools.sleep(1000);
			}
		}).start();
		new Thread(() -> {
			while (running) {
				try {
					// tools.println("%d/%d",this.clientSize,this.maxClientSize);
					if (this.maxClientSize <= 0 || this.clientSize < this.maxClientSize) {
						Socket socket = server.accept();
						socket.setSoTimeout(1);
						TcpSocket temp = new TcpSocket(socket, this.receiveBufferSize);
						this.setReadPool(temp, true);
					} else {
						tools.sleep(10);
					}
				} catch (IOException e) {
					e.printStackTrace();
					break;
				}
			}
		}).start();

		tools.println("service port:[%d] [%s] is started", this.getPort(), this.getServiceName());
	}

	public void stop() {
		if (running == false) {
			return;
		}
		running = false;
		tools.close(server);
		tools.close(readPool);
	}

}
