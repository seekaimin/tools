package com.am.linux.mscore;

import java.util.Date;

import com.am.utilities.datetimehelpers;
import com.am.utilities.stringhelpers;
import com.am.utilities.tools;

public class MSCoreCmd {

	public final static String field_listurlin = "listurlin";
	public final static String field_netio_interface_in = "netio_interface_in";
	public final static String field_netio_interface_out = "netio_interface_out";
	public final static String field_listurlout = "listurlout";
	public final static String field_runstate = "runstate";
	public final static String field_bitrate = "bitrate";
	/**
	 * 标识
	 */
	public String ID = "";

	/**
	 * 计算ID
	 */
	public void calcID() {
		String id = this.URL_IN + this.URL_OUT;
		ID = tools.calcMD5(id);
	}

	/**
	 * 操作类型
	 */
	public CMD_TYPES CMD_TYPE = CMD_TYPES.ADD;
	/**
	 * 工作模式
	 */
	public WORK_MODES WORK_MODE = WORK_MODES.NONE;
	/**
	 * 源地址
	 */
	public String URL_IN = "";
	/**
	 * 源地址
	 */
	public String URL_OUT = "";
	/**
	 * 绑定本地输入地址
	 */
	public String LOCAL_IP_IN = "";
	/**
	 * 绑定本地输输出地址
	 */
	public String LOCAL_IP_OUT = "";

	/**
	 * 运行状态
	 */
	public String runstate = "";
	/**
	 * 码率
	 */
	public String bitrate = "";

	/**
	 * 最后一次请求时间
	 */
	public Date last_request_time = null;

	/**
	 * 空参构造
	 */
	public MSCoreCmd() {
	}

	public MSCoreCmd(String line) {
		String[] attrs = line.split(";");
		for (String attr : attrs) {
			if (attr.contains("=")) {
				int start_index = attr.indexOf('=') + 1;
				String value = attr.substring(start_index);
				if (attr.startsWith(MSCoreCmd.field_listurlin)) {
					this.URL_IN = value;
				} else if (attr.startsWith(MSCoreCmd.field_listurlout)) {
					this.URL_OUT = value;
				} else if (attr.startsWith(MSCoreCmd.field_netio_interface_in)) {
					this.LOCAL_IP_IN = value;
				} else if (attr.startsWith(MSCoreCmd.field_netio_interface_out)) {
					this.LOCAL_IP_OUT = value;
				} else if (attr.startsWith(MSCoreCmd.field_runstate)) {
					this.runstate = value;
				} else if (attr.startsWith(MSCoreCmd.field_bitrate)) {
					this.bitrate = value;
				}
			} else if (attr.equals("remux")) {
				this.WORK_MODE = WORK_MODES.remux;
			} else if (attr.equals("transcode")) {
				this.WORK_MODE = WORK_MODES.transcode;
			}
		}
		this.calcID();
	}

	/**
	 * 指令构造函数
	 * 
	 * @param cmd_type
	 *            指令类型 CMD_TYPES
	 * @param work_mode
	 *            工作模式 WORK_MODES
	 * @param url_in
	 *            输入url
	 * @param url_out
	 *            输出url
	 */
	public MSCoreCmd(CMD_TYPES cmd_type, WORK_MODES work_mode, String url_in, String url_out) {
		this.CMD_TYPE = cmd_type;
		this.WORK_MODE = work_mode;
		this.URL_IN = url_in;
		this.URL_OUT = url_out;
	}

	/**
	 * 重置请求时间
	 */
	public void resetLastRequestTime() {
		this.last_request_time = datetimehelpers.getNow();
	}

	/**
	 * toString
	 */
	@Override
	public String toString() {
		return this.ID;
	}

	/**
	 * 转化称输出指令行字符串
	 * 
	 * @return 指令字符串
	 */
	public StringBuilder toCmd_String() {
		StringBuilder sb = new StringBuilder();
		String workmode = this.WORK_MODE == WORK_MODES.NONE ? "" : this.WORK_MODE.toString();
		sb.append(stringhelpers.fmt("%s;", workmode));

		if (false == stringhelpers.isNullOrEmpty(this.URL_IN)) {
			sb.append(stringhelpers.fmt("%s=%s;", MSCoreCmd.field_listurlin, this.URL_IN));
		}
		if (false == stringhelpers.isNullOrEmpty(this.LOCAL_IP_IN)) {
			sb.append(stringhelpers.fmt("%s=%s;", MSCoreCmd.field_netio_interface_in, this.LOCAL_IP_IN));
		}
		if (false == stringhelpers.isNullOrEmpty(this.LOCAL_IP_IN)) {
			sb.append(stringhelpers.fmt("%s=%s;", MSCoreCmd.field_netio_interface_out, this.LOCAL_IP_OUT));
		}
		if (false == stringhelpers.isNullOrEmpty(this.URL_OUT)) {
			sb.append(stringhelpers.fmt("%s=%s;", MSCoreCmd.field_listurlout, this.URL_OUT));
		}
		return sb;
	}

	/**
	 * 转化添加指令行字符串
	 * 
	 * @return 指令字符串
	 */
	public String toCmd_ADD_String() {
		StringBuilder sb = new StringBuilder();
		sb.append(stringhelpers.fmt("%s;", CMD_TYPES.ADD.toString()));
		sb.append(this.toCmd_String());
		return sb.toString();
	}

	/**
	 * 转化删除指令行字符串
	 * 
	 * @return 指令字符串
	 */
	public String toCmd_DELETE_String() {
		StringBuilder sb = new StringBuilder();
		sb.append(stringhelpers.fmt("%s;", CMD_TYPES.DELETE.toString()));
		sb.append(this.toCmd_String());
		return sb.toString();
	}

	/**
	 * 构建单条命令
	 * 
	 * @param cmd
	 * @return
	 */
	public String toCmd_EMPTY_String(CMD_TYPES cmd) {
		StringBuilder sb = new StringBuilder();
		sb.append(stringhelpers.fmt("%s;", cmd.toString()));
		sb.append(this.toCmd_String());
		return sb.toString();
	}

	/**
	 * 检查是否超时
	 * 
	 * @param time
	 *            超时时间(ms)
	 * @return 是否超时
	 */
	public boolean isTimeOut(long time) {
		long dtnow = datetimehelpers.getNow().getTime();
		if (this.last_request_time == null) {
			tools.println("时间为空~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~!");
			this.last_request_time = datetimehelpers.getNow();
		}
		long dt = this.last_request_time.getTime();

		long temp = dtnow - dt;
		return temp > time;
	}
}
