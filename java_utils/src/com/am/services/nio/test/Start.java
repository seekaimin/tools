package com.am.services.nio.test;

import com.am.services.nio.NioServer;

/**
 * <p>Title: 启动类</p>
 * @author starboy
 * @version 1.0
 */

public class Start {


	public static void main(String[] args) {
		try {
			TimeHandler timer = new TimeHandler();
			System.out.println("Server starting ...");
			NioServer server = new NioServer();
			server.addListener(timer);
			server.setServiceName("测试服务");
			server.setMaxReadThread(10);
			server.setMaxWriteThread(10);
			server.DEBUG = true;
			server.addPort(11001);
			server.addPort(5200);
			Thread tServer = new Thread(server);
			tServer.start();
		} catch (Exception e) {
			System.out.println("Server error: " + e.getMessage());
			System.exit(-1);
		}
	}
}
