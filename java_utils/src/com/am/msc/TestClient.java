package com.am.msc;

import java.io.IOException;

import com.am.utilities.Encodings;
import com.am.utilities.tools;

public class TestClient {

	public static void main(String args[]) throws IOException {
		// byte[] dd = stringhelpers.toBytes("23", Encodings.UTF8);
		// tools.println(dd);
		new Thread(() -> {
			String key = "23";
			try {
				MSClient.instance.Init("192.168.1.254", 7070, key);
			} catch (IOException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
			MSClient.instance.Receive(Encodings.UTF8, new IMessageAction() {
				@Override
				public void error(Exception e) {
					e.printStackTrace();
				}

				@Override
				public void call(String message) {
					tools.println(message);
				}
			});
		}).start();
		String key1 = "24";
		MSClient.instance.Init("192.168.1.254", 7070, key1);
		MSClient.instance.Receive(Encodings.UTF8, new IMessageAction() {
			@Override
			public void error(Exception e) {
				e.printStackTrace();
			}

			@Override
			public void call(String message) {
				tools.println(message);
			}
		});
	}
}
