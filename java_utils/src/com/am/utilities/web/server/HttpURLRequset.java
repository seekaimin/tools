package com.am.utilities.web.server;

import java.io.InputStream;

import com.am.utilities.stringhelpers;
import com.am.utilities.tools;

public class HttpURLRequset {
	private InputStream input;

	private String uri;

	public HttpURLRequset(InputStream input) {
		this.input = input;
	}

	public void parse() {
		// Read a set of characters from the socket
		StringBuffer request = new StringBuffer(2048);
		int i;
		byte[] buffer = new byte[2048];
		try {
			i = input.read(buffer);
		} catch (Exception e) {
			e.printStackTrace();
			i = -1;
		}
		tools.println(stringhelpers.bytesToHexString(buffer));
		for (int j = 0; j < i; j++) {
			request.append((char) buffer[j]);
		}
		System.out.print(request.toString());
		uri = parseUri(request.toString());
	}

	public String parseUri(String requestString) {
		int index1, index2;
		index1 = requestString.indexOf(" ");
		if (index1 != -1) {
			index2 = requestString.indexOf(" ", index1 + 1);
			if (index2 > index1) {
				return requestString.substring(index1 + 1, index2);
			}
		}
		return null;
	}

	public String getUri() {
		return this.uri;
	}
}
