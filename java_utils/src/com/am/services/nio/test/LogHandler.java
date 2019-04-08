package com.am.services.nio.test;

import java.util.Date;

import com.am.services.nio.*;
import com.am.services.nio.event.NioEventAdapter;

/**
 * 日志记录
 */
public class LogHandler extends NioEventAdapter {
    public LogHandler() {
    }

    public void onClosed(NioRequest request) {
        String log = new Date().toString() + " from " + request.getAddress().toString();
        System.out.println(log);
    }

    public void onError(Exception ex) {
        System.out.println("Error: " + ex.getMessage());
    }
}
