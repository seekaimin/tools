package com.am.delegate;

public interface IFunc<T> {

	/**
	 * 代理执行方法
	 * @param obj
	 * @return T
	 */
	public T call();
	/**
	 * 代理执行方法
	 * @param obj
	 * @return T
	 */
	public T call(Object... obj);
}
