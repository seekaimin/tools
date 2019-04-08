package com.am.delegate;

/**
 * 通用代理
 * 
 * @author am
 *
 */
public interface IAction<T> {
	/**
	 * 代理执行方法 无参数
	 */
	public void call();

	/**
	 * 代理执行方法 单参数
	 * 
	 * @param obj
	 *            参数
	 */
	public void call(T obj);

	/**
	 * 代理执行方法 数组参数
	 * 
	 * @param obj
	 */
	@SuppressWarnings("unchecked")
	public void call(T... obj);
}
