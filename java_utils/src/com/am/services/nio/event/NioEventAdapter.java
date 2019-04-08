package com.am.services.nio.event;

import com.am.services.nio.NioRequest;
import com.am.services.nio.NioResponse;
import com.am.services.nio.event.NioServerListener;

/**
 * <p>
 * Title: 事件适配器
 * </p>
 * 
 * @author starboy
 * @version 1.0
 */

public abstract class NioEventAdapter implements NioServerListener {
	public NioEventAdapter() {
	}

	public void onError(Exception e) {
	}

	public void onAccept() throws Exception {
	}

	public void onAccepted(NioRequest request) throws Exception {
	}

	public void onRead(NioRequest request) throws Exception {
	}

	public void onWrite(NioRequest request, NioResponse response) throws Exception {
	}

	public void onClosed(NioRequest request) {
	}
}
