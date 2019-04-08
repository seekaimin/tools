package com.am.reflect;

import java.lang.reflect.Method;

/**
 * uri 映射类
 * @author am
 *
 */
public class UriMapping {
	/**
	 * 所在类
	 */
	private Class<?> clazz;
	/**
	 * uri attr
	 */
	private UriAttribute uriAttr;
	/**
	 * 绑定的方法
	 */
	private Method method;
	
	public Class<?> getClazz() {
		return clazz;
	}
	public Method getMethod() {
		return method;
	}
	public UriAttribute getUriAttr() {
		return uriAttr;
	}
	public void setClazz(Class<?> clazz) {
		this.clazz = clazz;
	}
	public void setMethod(Method method) {
		this.method = method;
	}
	public void setUriAttr(UriAttribute uriAttr) {
		this.uriAttr = uriAttr;
	}
	
}
