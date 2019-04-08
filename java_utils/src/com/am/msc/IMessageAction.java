package com.am.msc;

/**
 * 消息回调接口
 * 
 * @author am
 *
 */
public interface IMessageAction {
	void call(String message);

	void error(Exception e);
}
