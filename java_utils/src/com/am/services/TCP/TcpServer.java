package com.am.services.TCP;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

import com.am.utilities.stringhelpers;
import com.am.utilities.tools;

public class TcpServer {

	public static void main(String args[]) {
		try {
			TcpServer.ins().setServiceName("门禁");
			TcpServer.ins().setReceiveBufferSize(200);
			TcpServer.ins().setTimeout(1);
			TcpServer.ins().setReadThreadSize(10);
			TcpServer.ins().setWriteThreadSize(10);
			TcpServer.ins().setExecuteThreadSize(10);
			TcpServer.ins().setTimeout(1);
			TcpServer.ins().setPort(11000);
			TcpServer.ins().DEBUG = true;
			TcpServer.ins().setHandle(new ITcpClientHandle() {
				@Override
				public void handle(TcpSocket socket) throws TcpException {
					tools.sleep(100);
					// tools.println(stringhelpers.bytesToHexString(socket.getData()));
					String s = "fe fd 01 00 00 00 00 00 02 00 00 32 06 06 10 10 00 00 00 00 00 00 0106 06 20 16 00 00 00 01 00 30 00 0b 00 00 04 59 dc f7 9c 00 00 00 006e 78 b2 cd fe fd 01 00 00 00 00 00 01 00 00 38 06 06 10 10 00 00 0000 00 00 01 06 06 10 10 00 00 00 00 00 12 00 11 01 f0 0e 05 ee ee eeee ee ee aa aa aa aa aa aa aa df 37 86 67";
					s = s.replace(" ", "");
					s="00000000";
					byte[] d = stringhelpers.hexStringToBytes(s);
					socket.setWriteBuffer(d);
				}

				@Override
				public void exception(Exception e) {
					// e.printStackTrace();
				}
			});
			TcpServer.ins().start();
		} catch (Exception e) {
			e.printStackTrace();
		}
		
		
		tools.sleep(20000);
		TcpServer.ins().stop();

	}

	private static TcpServer _ins = null;
	private TcpSocketPool readPool = new TcpSocketPool();
	private TcpSocketPool writePool = new TcpSocketPool();
	private TcpSocketPool executePool = new TcpSocketPool();

	// ExecutorService ExecutePool = Executors.newCachedThreadPool();

	/**
	 * 单件
	 * 
	 * @return
	 */
	public static TcpServer ins() {
		if (_ins == null) {
			_ins = new TcpServer();
		}
		return _ins;
	}

	private int readThreadSize = 1;
	private int writeThreadSize = 1;
	private int executeThreadSize = 8;
	private int doMaxClientSize = 100;

	private boolean running = false;
	private String serviceName = "system service";
	private int receiveBufferSize = 1024;
	private int timeout = 1;
	private int port = 11001;
	private ServerSocket server = null;
	public boolean DEBUG = true;
	private ITcpClientHandle handle = null;

	public ITcpClientHandle getHandle() {
		return this.handle;
	}

	public void setHandle(ITcpClientHandle handle) {
		this.handle = handle;
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
		for (int i = 0; i < readThreadSize; i++) {
			new Thread(new TcpReader(this)).start();
		}
		// 创建writer线程
		for (int i = 0; i < this.writeThreadSize; i++) {
			new Thread(new TcpWriter(this)).start();
		}
		// 创建writer线程
		for (int i = 0; i < this.executeThreadSize; i++) {
			new Thread(new TcpExecuter(this)).start();
		}
		this.check();
		new Thread(() -> {
			while (running) {
				try {
					Socket socket = server.accept();
					socket.setSoTimeout(this.getTimeout());
					TcpSocket temp = new TcpSocket(socket, this.getReceiveBufferSize());
					this.setReadTcpSocket(temp);
				} catch (IOException e) {
					e.printStackTrace();
					break;
				}
			}
		}).start();
		tools.println("service [%s] is started", serviceName);
	}

	public void stop() {
		if (running == false) {
			return;
		}
		tools.close(this.readPool);
		tools.close(this.writePool);
		tools.close(this.executePool);
		running = false;
		tools.close(server);
	}

	String statemsg = "";

	public String getRunningState() {
		return statemsg.toString();
	}

	private void check() {
		new Thread(() -> {
			while (this.isRunning()) {
				int size = readPool.size() + writePool.size() + executePool.size();
				statemsg = stringhelpers.fmt("读取池:%d---执行池:%d---写入池:%d---客户端数量:%d", readPool.size(), executePool.size(),
						writePool.size(), size);
				if (this.DEBUG) {
					tools.println(statemsg);
				}
				tools.sleep(1000);
			}
		}).start();
	}

	public TcpSocket getReadTcpSocket() throws InterruptedException {
		return this.readPool.get();
	}

	public TcpSocket getWriteTcpSocket() throws InterruptedException {
		return this.writePool.get();
	}

	public TcpSocket getExecuteTcpSocket() throws InterruptedException {
		return this.executePool.get();
	}

	public void setReadTcpSocket(TcpSocket socket) {
		this.readPool.set(socket);
	}

	public void setWriteTcpSocket(TcpSocket socket) {
		this.writePool.set(socket);
	}

	public void setExecuteTcpSocket(TcpSocket socket) {
		this.executePool.set(socket);
	}

	public int getReceiveBufferSize() {
		return receiveBufferSize;
	}

	public void setReceiveBufferSize(int receiveBufferSize) {
		this.receiveBufferSize = receiveBufferSize;
	}

	public int getTimeout() {
		return timeout;
	}

	public void setTimeout(int timeout) {
		this.timeout = timeout;
	}

	public boolean isRunning() {
		return running;
	}

	public String getServiceName() {
		return serviceName;
	}

	public void setServiceName(String serviceName) {
		this.serviceName = serviceName;
	}

	public int getPort() {
		return port;
	}

	public void setPort(int port) {
		this.port = port;
	}

	public int getReadThreadSize() {
		return readThreadSize;
	}

	public void setReadThreadSize(int readThreadSize) {
		this.readThreadSize = readThreadSize;
	}

	public int getWriteThreadSize() {
		return writeThreadSize;
	}

	public void setWriteThreadSize(int writeThreadSize) {
		this.writeThreadSize = writeThreadSize;
	}

	public int getExecuteThreadSize() {
		return executeThreadSize;
	}

	public void setExecuteThreadSize(int executeThreadSize) {
		this.executeThreadSize = executeThreadSize;
	}

	public int getDoMaxClientSize() {
		return doMaxClientSize;
	}

	public void setDoMaxClientSize(int doMaxClientSize) {
		this.doMaxClientSize = doMaxClientSize;
	}

}