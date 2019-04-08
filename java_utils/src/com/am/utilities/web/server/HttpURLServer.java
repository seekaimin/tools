package com.am.utilities.web.server;

import java.io.File;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.ServerSocket;
import java.net.Socket;

public class HttpURLServer {
	/**
	 * WEB_ROOT is the directory where our html and other files reside.
	 * For this package,WEB_ROOT is the "webroot" directory under the
	 * working directory.
	 * the working directory is the location in the file system
	 * from where the java command was invoke.
	 */
	public static final String WEB_ROOT=System.getProperty("user.dir")+File.separator+"webroot";
	
	//private static final String SHUTDOWN_COMMAND = "/SHUTDOWN";
	
	private boolean shutdown=false;
	
	public static void main(String[] args) {
		HttpURLServer server=new HttpURLServer();
		server.await();
	}
	
	public void await(){
		ServerSocket serverSocket=null;
		int port=8008;
		try {
			serverSocket=new ServerSocket(port,1);
		} catch (Exception e) {
			e.printStackTrace();
			System.exit(0);
		}
		while(!shutdown){
			InputStream input=null;
			OutputStream output=null;
			try {
				Socket socket=serverSocket.accept();
				input=socket.getInputStream();
				output=socket.getOutputStream();
				//create HttpURLRequset object and parse
				HttpURLRequset request=new HttpURLRequset(input);
				request.parse();
				
				//create HttpURLResponse object
				HttpURLResponse response=new HttpURLResponse(output);
				response.setHttpURLRequset(request);
				response.sendStaticResource();
			} catch (Exception e) {
				e.printStackTrace();
				continue;
			}
		}
	}
}
