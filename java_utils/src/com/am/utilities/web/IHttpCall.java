package com.am.utilities.web;

/**
 * http回调
 * @author am
 *
 */
public interface IHttpCall {
	void success(String message);
	void error(Exception ex);
	void complete();
}

