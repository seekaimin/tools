package com.am.utilities.web.server;

import java.net.Socket;

public class HttpThread {
	private Thread thread;
	private Socket socket;
	
	public HttpThread(Socket soc)
	{
		
		this.setSocket(soc);
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	public Thread getThread() {
		return thread;
	}
	public void setThread(Thread thread) {
		this.thread = thread;
	}
	public Socket getSocket() {
		return socket;
	}
	public void setSocket(Socket socket) {
		this.socket = socket;
	}
	
}
