package com.am.services;

import com.am.utilities.tools;

/**
 * 服务基类
 * 
 * @author Administrator
 *
 */
public abstract class ServiceBase {
	/**
	 * 服务运行状态
	 */
	public ServiceStates ServiceState = ServiceStates.Stoped;

	/**
	 * 服务名称
	 */
	protected abstract String getServiceName();

	/**
	 * 是否启用debug模式
	 * 
	 * @return
	 */
	protected boolean DEBUG = false;
	/**
	 * 信息模式
	 * 
	 * @return
	 */
	protected boolean INFO = false;

	/**
	 * 服务主线程
	 */
	private Thread mainthread = null;

	/**
	 * 启动服务
	 */
	public void start() {
		if (this.ServiceState == ServiceStates.Starting || ServiceState == ServiceStates.Started) {
			// 已经启动
			return;
		}
		this.disposeThread(this.mainthread);
		this.ServiceState = ServiceStates.Starting;
		try {
			if (this.validate()) {
				info("validate() callback true!");
				ServiceState = ServiceStates.Started;
				new Thread(() -> {
					try {
						begin();
					} catch (Exception e) {
						info("begin() throw exception:", e);
						stop();
					}
				}).start();
				tools.println("%s:is started!", getServiceName());
			} else {
				info("validate() callback false!");
				this.stop();
			}
		} catch (Exception e) {
			info("start() throw exception:", e);
			this.stop();
		}
	}

	/**
	 * 停止服务
	 */
	public void stop() {
		if (this.ServiceState == ServiceStates.Stoping || this.ServiceState == ServiceStates.Stoped) {
			return;
		}
		tools.println("%s:is stoping!", getServiceName());
		this.ServiceState = ServiceStates.Stoping;
		try {
			this.end();
		} catch (Exception e) {
			info("stop() throw exception:", e);
		} finally {
			this.ServiceState = ServiceStates.Stoped;
			this.disposeThread(this.mainthread);
			tools.println("%s:is stoped!", getServiceName());
		}
	}

	// virtual abstract
	/**
	 * 启动检核
	 * 
	 * @return
	 */
	protected abstract boolean validate();

	/**
	 * 开始
	 */
	protected abstract void begin();

	/**
	 * 停止
	 */
	protected abstract void end();

	/**
	 * 停止线程
	 */
	protected void disposeThread(Thread thread) {
		try {
			if (thread != null) {
				thread.interrupt();
			}
		} catch (Exception e) {

		}
	}

	/**
	 * 打印异常信息
	 * 
	 * @param e
	 *            异常信息
	 * @param title
	 *            消息头
	 */
	public void debug(String title, Exception e) {
		if (this.DEBUG) {
			tools.println(title);
			e.printStackTrace();
		}
	}

	/**
	 * 打印信息
	 * 
	 * @param message
	 */
	public void debug(String message) {
		if (this.DEBUG) {
			tools.println(message);
		}
	}

	/**
	 * 打印信息
	 * 
	 * @param message
	 */
	public void info(String message) {
		if (this.INFO) {
			tools.println(message);
		}
	}

	/**
	 * 打印异常信息
	 * 
	 * @param e
	 *            异常信息
	 * @param title
	 *            消息头
	 */
	public void info(String title, Exception e) {
		if (this.INFO) {
			tools.println(title);
			e.printStackTrace();
		}
	}

}
