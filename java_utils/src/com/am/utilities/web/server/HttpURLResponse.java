package com.am.utilities.web.server;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.OutputStream;

import com.am.utilities.tools;

/**
 * HTTP HttpURLResponse = Status-Line *(( general-header |
 * HttpURLResponse-header | entity-header ) CRLF) CRLF [message-body]
 * Status-Line=Http-Version SP Status-Code SP Reason-Phrase CRLF
 *
 */
public class HttpURLResponse {
	private static final int BUFFER_SIZE = 1024;
	HttpURLRequset request;
	OutputStream output;

	public HttpURLResponse(OutputStream output) {
		this.output = output;
	}

	public void setHttpURLRequset(HttpURLRequset HttpURLRequset) {
		this.request = HttpURLRequset;
	}

	public void sendStaticResource() throws IOException {
		byte[] bytes = new byte[BUFFER_SIZE];
		FileInputStream fis = null;
		try {
			File file = new File(HttpURLServer.WEB_ROOT, this.request.getUri());
			if (file.exists()) {
				fis = new FileInputStream(file);
				int ch = fis.read(bytes, 0, BUFFER_SIZE);
				while (ch != -1) {
					output.write(bytes, 0, BUFFER_SIZE);
					ch = fis.read(bytes, 0, BUFFER_SIZE);
				}
			} else {
				// file not found
				String errorMessage = "HTTP/1.1 404 File Not Found\r\n" + "Content-Type:text/html\r\n"
						+ "Content-Length:23\r\n" + "\r\n" + "<h1>File Not Found</h1>";
				output.write(errorMessage.getBytes());
			}
		} catch (Exception e) {
			System.out.println(e.toString());
		} finally {
			tools.close(fis);
			tools.close(output);
		}
	}
}