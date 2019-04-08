package com.am.services.nio;

import java.io.Closeable;
import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.nio.channels.SelectionKey;
import java.nio.channels.Selector;
import java.nio.channels.ServerSocketChannel;
import java.nio.channels.SocketChannel;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;
import java.util.Set;

import com.am.services.nio.event.NioServerListener;
import com.am.utilities.tools;

/**
 * <p>
 * Title: 主控服务线程
 * </p>
 * 
 * @author starboy
 * @version 1.0
 */

public class NioServer implements Runnable, Closeable {
	private NioSelectionKeyPool readPool = new NioSelectionKeyPool(this); // 回应池
	private NioSelectionKeyPool writePool = new NioSelectionKeyPool(this); // 回应池

	private NioSelectionKeyPool canReadPool = new NioSelectionKeyPool(this); // 可以处理的读池
	private NioSelectionKeyPool canWritePool = new NioSelectionKeyPool(this); // 可以处理的写池子

	public NioSelectionKeyPool getReadePool() {
		return this.readPool;
	}

	public NioSelectionKeyPool getWritePool() {
		return this.writePool;
	}

	public NioSelectionKeyPool getCanReadPool() {
		return this.canReadPool;
	}

	public NioSelectionKeyPool getCanWritePool() {
		return this.canWritePool;
	}

	private Selector selector;
	private NioNotifier nioNotifier;
	private List<Integer> ports = new ArrayList<Integer>();
	public boolean DEBUG = false;
	private boolean running = false;
	private String runningState = "";

	public NioServer() {
		// 获取事件触发器
		nioNotifier = new NioNotifier();
	}

	/**
	 * 创建主控服务线程
	 * 
	 * @param port
	 *            服务端口
	 * @throws java.lang.Exception
	 */
	private int maxReadThread = 1;
	private int maxWriteThread = 4;
	/**
	 * 服务名称
	 */
	private String serviceName = "system service";

	// private NioSelectionKeyPool readPool = new NioSelectionKeyPool(); // 回应池
	// private NioSelectionKeyPool writePool = new NioSelectionKeyPool(); // 回应池
	//
	// private NioSelectionKeyPool canReadPool = new NioSelectionKeyPool();
	// //可以处理的读池
	// private NioSelectionKeyPool canWritePool = new NioSelectionKeyPool();
	// //可以处理的写池子

	public SelectionKey getWriterRequest() throws InterruptedException {
		return writePool.get();
	}

	public void setWriterRequest(SelectionKey key) {
		writePool.set(key);
		selector.wakeup();
	}

	public SelectionKey getReaderRequest() throws InterruptedException {
		return readPool.get();
	}

	public void setReaderRequest(SelectionKey key) {
		readPool.set(key);
		selector.wakeup();
	}

	public SelectionKey getCanReaderRequest() throws InterruptedException {
		return canReadPool.get();
	}

	public void setCanReaderRequest(SelectionKey key) {
		canReadPool.set(key);
	}

	public SelectionKey getCanWriterRequest() throws InterruptedException {
		return canWritePool.get();
	}

	public void setCanWriterRequest(SelectionKey key) {
		canWritePool.set(key);
	}

	/**
	 * 验证
	 * 
	 * @return
	 */
	protected boolean validate() {
		try {
			if (ports.size() == 0) {
				throw new Exception("没有需要监听的端口!");
			}
			selector = Selector.open();
			for (int port : ports) {
				tools.println("Server listening on port: " + port);
				// 实例化一个信道
				ServerSocketChannel listnChannel = ServerSocketChannel.open();
				// 将该信道绑定到指定端口
				listnChannel.socket().bind(new InetSocketAddress(port));
				// 配置信道为非阻塞模式
				listnChannel.configureBlocking(false);
				// 将选择器注册到各个信道
				listnChannel.register(selector, SelectionKey.OP_ACCEPT);
				
				
			}
		} catch (Exception e) {
			tools.println(e.getMessage());
			e.printStackTrace();
			return false;
		}
		return true;
	}

	public void run() {
		if (this.running) {
			return;
		}
		if (this.validate() == false) {
			tools.close(selector);
			return;
		}
		this.running = true;

		// 创建读写线程池
		for (int i = 0; i < maxReadThread; i++) {
			Thread r = new NioReader(this);
			r.start();
		}
		for (int i = 0; i < maxWriteThread; i++) {
			Thread w = new NioWriter(this);
			w.start();
		}

		tools.println("Server [%s] started ...", this.serviceName);
		// 监听
		while (this.running) {
			try {
				int num = 0;
				num = selector.select();
				int remotes = (selector.keys().size() + readPool.size() + writePool.size() + canReadPool.size() + canWritePool.size() - this.getPorts().size());
				runningState = "remote number:" + remotes + ", receive size:" + canReadPool.size() + ", write size:"
						+ canWritePool.size();
				if (this.DEBUG) {
					tools.println(runningState);
				}
				if (num > 0) {
					Set<SelectionKey> selectedKeys = selector.selectedKeys();
					Iterator<SelectionKey> it = selectedKeys.iterator();
					while (it.hasNext()) {
						SelectionKey key = (SelectionKey) it.next();
						it.remove();

						// 处理IO事件
						if (key.isAcceptable()) {
							// Accept the new connection
							nioNotifier.fireOnAccept();

							ServerSocketChannel ssc = (ServerSocketChannel) key.channel();
							SocketChannel sc = ssc.accept();
							sc.configureBlocking(false);

							// 触发接受连接事件
							NioRequest request = new NioRequest(sc);
							nioNotifier.fireOnAccepted(request);

							// 注册读操作,以进行下一步的读操作
							sc.register(selector, SelectionKey.OP_READ, request);
							selector.wakeup();
						} else if (key.isValid() && key.isReadable()) {
							this.setCanReaderRequest(key);// 提交读服务线程读取客户端数据
							key.cancel();
						} else if (key.isValid() && key.isWritable()) {
							this.setCanWriterRequest(key);// 提交写服务线程向客户端发送回应数据
							key.cancel();
						}
					}
				} else {
					addRegister(); // 在Selector中注册新的写通道
				}

			} catch (Exception e) {
				nioNotifier.fireOnError(e);
				continue;
			}
		}
	}

	/**
	 * 关闭
	 */
	@Override
	public void close() throws IOException {
		if (!this.isRunning()) {
			return;
		}
		this.running = false;
		tools.close(selector);
	}

	/**
	 * 添加新的通道注册
	 */
	private void addRegister() {
		writePool.register(SelectionKey.OP_WRITE);
		readPool.register(SelectionKey.OP_READ);
	}

	public void disposeKey(SelectionKey key) {
		key.cancel();
		try {
			SocketChannel sc = (SocketChannel) key.channel();
			if (sc != null) {
				Socket socket = sc.socket();
				if (socket != null) {
					try {
						socket.close();
					} catch (IOException e) {
					}
				}
				// sc.finishConnect();
				sc.close();
			}
		} catch (Exception e1) {
		} finally {
			try {
				this.getNotifier().fireOnClosed((NioRequest) key.attachment());
			} catch (Exception e) {
				//e.printStackTrace();
			}
			selector.wakeup();
		}
	}

	public int getMaxReadThread() {
		return this.maxReadThread;
	}

	public void setMaxReadThread(int maxReadThread) {
		this.maxReadThread = maxReadThread;
	}

	public int getMaxWriteThread() {
		return this.maxWriteThread;
	}

	public void setMaxWriteThread(int maxWriteThread) {
		this.maxWriteThread = maxWriteThread;
	}

	public List<Integer> getPorts() {
		return ports;
	}

	public void addPort(Integer port) {
		this.ports.add(port);
	}

	/**
	 * 添加事件处理句柄
	 * 
	 * @param l
	 */
	public void addListener(NioServerListener l) {
		this.getNotifier().addListener(l);
	}

	/**
	 * 添加事件处理句柄
	 * 
	 * @param l
	 */
	protected NioNotifier getNotifier() {
		return this.nioNotifier;
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

	/**
	 * 获取当前selector
	 * 
	 * @return
	 */
	public Selector getSelector() {
		return this.selector;
	}

	public String getRunningState() {
		return runningState;
	}	
}
