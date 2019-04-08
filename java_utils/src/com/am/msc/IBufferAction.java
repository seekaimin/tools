package com.am.msc;

/**
 * 消息回调接口
 * 
 * @author am
 *
 */
public interface IBufferAction {
	void call(byte[] buffer);

	void error(Exception e);
}
