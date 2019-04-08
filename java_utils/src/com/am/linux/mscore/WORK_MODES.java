package com.am.linux.mscore;

/**
 * 工作模式
 * 
 * @author Administrator
 *
 */
public enum WORK_MODES {
	/**
	 * 无
	 */
	NONE,
	/**
	 * 普通的录制功能
	 */
	dump_nor,
	/**
	 * 紧急广播录制功能
	 */
	dump_broadcast_exigence,
	/**
	 * 日常广播录制功能
	 */
	dump_broadcast_common,
	/**
	 * 紧急日常广播录制功能
	 */
	dump_broadcast_all,
	/**
	 * 时移录制功能，该模式按半小时自动进行分片录制
	 */
	dump_timeshift_auto,
	/**
	 * 根据用户设置的时间片段进行录制
	 */
	dump_timeshift_timelist,
	/**
	 * 协议转发
	 */
	transmit,
	/**
	 * 带重复用的协议转发
	 */
	remux,
	/**
	 * 带转码流的协议转发
	 */
	transcode,
	/**
	 * 实现http和rtsp流的推送，同时提供httpserver以及mscore网页查询功能
	 */
	httpserver
}
