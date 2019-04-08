package com.am.msc;

import java.io.IOException;

import com.am.utilities.Encodings;
import com.am.utilities.stringhelpers;
import com.am.utilities.tools;

public class TestServer {
	public static void main(String args[]) throws IOException {
		// byte[] dd = stringhelpers.toBytes("23", Encodings.UTF8);
		// tools.println(dd);

		boolean flag = MSServer.instance.Init("192.168.1.254", 7070);
		tools.println("注册:", flag);
		int count = 0;
		String key = "23";
		//String key1 = "24";

		new Thread(() -> {
			int count1 = 0;
			//boolean f = true;
			while (true) {
				count1++;
				byte[] buffer = stringhelpers.toBytes("我是測試消息_____a: " + count1, Encodings.UTF8);
				try {
					MSServer.instance.Send(key, buffer);
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				tools.sleep(500);
			}
		}).start();

		boolean f = true;
		while (true) {
			count++;
			String str = "我是測試消息_____b: " + count;
			//tools.println("sssssssssssssssss:%s",count);
			byte[] buffer = stringhelpers.toBytes(str, Encodings.UTF8);
			f = !f;
			MSServer.instance.Send(key, buffer);
			tools.sleep(500);
			//tools.println("key:%s;发送数据:%s", key, count);
		}
		//tools.close(MSServer.instance);

	}
}
