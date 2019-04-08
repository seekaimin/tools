package com.am.services.nio.test;

import java.text.DateFormat;
import java.util.Date;
import java.util.Locale;

import com.am.services.nio.NioRequest;
import com.am.services.nio.NioResponse;
import com.am.services.nio.event.NioEventAdapter;

/**
 * 时间查询服务器
 */
public class TimeHandler extends NioEventAdapter {
	public TimeHandler() {
	}

    public void onRead(NioRequest request)  throws Exception {
    	Thread.sleep(50);
    	//tools.println("read"+stringhelpers.bytesToHexString(request.getDataInput()));
    }
	public void onWrite(NioRequest request, NioResponse response) throws Exception {
    	Thread.sleep(50);
		String command = new String(request.getDataInput());
		String time = null;
		Date date = new Date();
		// 判断查询命令
		if (command.equals("GB")) {
			// 中文格式
			DateFormat cnDate = DateFormat.getDateTimeInstance(DateFormat.FULL, DateFormat.FULL, Locale.CHINA);
			time = cnDate.format(date);
		} else {
			// 英文格式
			DateFormat enDate = DateFormat.getDateTimeInstance(DateFormat.FULL, DateFormat.FULL, Locale.US);
			time = enDate.format(date);
		}

		response.send(time.getBytes());
	}

	public void onAccepted(NioRequest request) throws Exception {
		// Server.remoteNumber++;
		// System.out.println(String.format("+++++++++++++new connection:%d++++++++++++++", Server.remoteNumber));
	}

	public void onClosed(NioRequest request) {
		// Server.remoteNumber--;
		// System.out.println(String.format("-------------lost connection:%d-------------", Server.remoteNumber));
	}
}
