package com.am.test;

import java.io.IOException;

import com.am.utilities.Encodings;
import com.am.utilities.tools;
import com.am.utilities.web.ContentTypes;
import com.am.utilities.web.HttpURLClient;
import com.am.utilities.web.Methods;

public class httptest {

	public static void main(String[] args) {
		a();
	}

	static void  a() {
		// TODO Auto-generated method stub
		String url = "http://127.0.0.1:8007/eduServer/api/testJson";
		// url = "http://192.168.0.86:8007/eduServer/api/testJson";
		HttpURLClient client = new HttpURLClient(url);
		client.setMethod(Methods.POST);
		client.setContentType(ContentTypes.applicationUrlencoded);
		client.add("values", "{\"id\":123456789,\"name\":\"小明\"}");

		client.setEncoding(Encodings.UTF8);
		client.setReadTimeout(5000);
		client.setConnectTimeout(5000);
		try {
			String respose = client.send();
			tools.println(respose);
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

}
