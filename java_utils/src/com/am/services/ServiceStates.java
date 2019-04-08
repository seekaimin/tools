package com.am.services;

/**
 * 服务运行状态
 * 
 * @author Administrator
 *
 */
public enum ServiceStates {
	/**
	 * 启动中
	 */
	Starting,
	/**
	 * 启动
	 */
	Started,
	/**
	 * 挂起
	 */
	Suspend,
	/**
	 * 停止中
	 */
	Stoping,
	/**
	 * 停止
	 */
	Stoped,
}
