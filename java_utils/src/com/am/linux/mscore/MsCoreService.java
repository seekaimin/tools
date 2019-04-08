package com.am.linux.mscore;

import java.net.DatagramSocket;
import java.net.SocketException;
import java.net.UnknownHostException;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;

import com.am.linux.SSH;
import com.am.services.ServiceBase;
import com.am.services.ServiceStates;
import com.am.utilities.Encodings;
import com.am.utilities.integerhelpers;
import com.am.utilities.sockethelpers;
import com.am.utilities.stringhelpers;
import com.am.utilities.tools;

/**
 * 高飞服务帮助类
 * 
 * @author Administrator
 *
 */
public class MsCoreService extends ServiceBase {

	public final static String service_name = "MultiCoreServer";
	public final static String mscore_name = "mscore";

	/**
	 * 指令超时时间
	 */
	protected int cmd_remove_timeout = 5 * 60 * 1000;
	/**
	 * 指令状态检测间隔
	 */
	protected int check_cmd_state_timespan = 10000;

	/**
	 * 接收指令缓存
	 */
	protected int get_gf_cmds_buffer_length = 200 * 1024;

	Lock current_lock = new ReentrantLock();
	public String mscore_service_state = "test";
	private SSH ssh = null;

	/**
	 * 测试模式
	 */
	public boolean isTest = false;

	/**
	 * 通讯端口
	 */
	private int port = 50000;
	/**
	 * 命令池
	 */
	List<MSCoreCmd> am_pool = new ArrayList<MSCoreCmd>();

	/**
	 * 正在运行的指令池
	 */
	List<MSCoreCmd> gf_pool = new ArrayList<MSCoreCmd>();

	MSCoreCmd get(List<MSCoreCmd> pool, String id) {

		for (MSCoreCmd cmd : pool) {
			if (cmd.ID.equals(id)) {
				return cmd;
			}
		}
		return null;
	}

	/**
	 * 空参构造
	 */
	public MsCoreService() {
	}

	/**
	 * 构造函数
	 * 
	 * @param remote_host
	 *            远程主机地址
	 * @param username
	 *            登录名
	 * @param password
	 *            登录密码
	 * @param port
	 *            服务通讯端口
	 */
	public MsCoreService(String remote_host, String username, String password, int port) {
		this.reset(remote_host, username, password, port);
	}

	/**
	 * 重置服务
	 * 
	 * @param remote_host
	 *            远程主机地址
	 * @param username
	 *            登录名
	 * @param password
	 *            登录密码
	 * @param port
	 *            服务通讯端口
	 */
	public void reset(String remote_host, String username, String password, int port) {
		this.current_lock.lock();
		this.am_pool.clear();
		this.current_lock.unlock();

		try {
			this.port = port;
			if (ssh != null) {
				ssh.close();
			}
			ssh = new SSH(remote_host, username, password);
		} catch (Exception e) {

		}
	}

	@Override
	protected String getServiceName() {
		return "MsCore Control service";
	}

	@Override
	protected boolean validate() {
		/*
		 * // 检核服务器是否能登录 try { if (false == this.ssh.login()) {
		 * print("登录流媒体转发服务器失败!"); return false; } } catch (IOException e) {
		 * print("登录流媒体转发服务器出现异常:", e); return false; }
		 */
		isclearcmds = true;
		return true;
	}

	/**
	 * 检查UDP套接字状态
	 * 
	 * @throws SocketException
	 * @throws UnknownHostException
	 */
	protected DatagramSocket checkSocket(DatagramSocket socket) throws SocketException, UnknownHostException {
		int timeout = 10000;
		if (socket != null) {
			// tools.println("isconnected:%s---------isclosed:%s",
			// socket.isConnected(), socket.isClosed());
		}
		if (socket == null || socket.isConnected() == false || socket.isClosed()) {
			tools.close(socket);
			socket = sockethelpers.getUdpClient(ssh.remote_host, port, timeout);
		}
		return socket;
	}

	@Override
	protected void begin() {
		this.ServiceState = ServiceStates.Started;
		// 检核服务状态
		this.checkStateThread();
		tools.sleep(1000);
		// 检核指令状态
		this.checkCmdsStateThread();
		// 移除超时的指令
		this.removeTimeOutCmdsThread();
	}

	@Override
	protected void end() {

	}

	/**
	 * 检核请求命令
	 * 
	 * @param cmds
	 *            指令集
	 */
	public void checkCmd(MSCoreCmd... cmds) {
		current_lock.lock();
		try {
			DatagramSocket socket = null;
			List<MSCoreCmd> need_add_items = new ArrayList<MSCoreCmd>();
			for (MSCoreCmd cmd : cmds) {
				cmd.calcID();
				MSCoreCmd t = get(am_pool, cmd.ID);
				if (t != null) {
					t.resetLastRequestTime();
				} else {
					cmd.resetLastRequestTime();
					am_pool.add(cmd);
					need_add_items.add(cmd);
				}
			}

			if (stringhelpers.isNullOrEmpty(mscore_service_state) == false) {
				socket = this.checkSocket(socket);
				add(socket, need_add_items);
			}
		} catch (Exception e) {

		} finally {
			current_lock.unlock();
		}
	}

	boolean isclearcmds = false;

	public void checkStateThread() {
		new Thread(new Runnable() {
			public void run() {
				DatagramSocket socket = null;
				do {
					// 检核流媒体服务是否运行
					checkServiceState(socket);
					if (stringhelpers.isNullOrEmpty(mscore_service_state)) {
						tools.println("mscore_service_state is null!  length：%s", mscore_service_state.length());
					} else {
						tools.println("mscore_service_state is %s!", mscore_service_state);
					}
					if (stringhelpers.isNullOrEmpty(mscore_service_state) == false) {
						// 运行 检查间隔 5000ms
						if (isclearcmds) {
							try {
								socket = checkSocket(socket);
								clearAllCmds(socket);
								isclearcmds = false;
							} catch (Exception e) {
							}
						}
						tools.sleep(5000);
						continue;
					}
					// 检核高飞服务是否启动
					if (gf_service_state()) {
						tools.sleep(2000);
						// 已经启动
						continue;
					}
					info("流媒体服务器尚未启动!");
					// 重启高飞服务
					restart_gf_service();
					tools.sleep(10000);
				} while (ServiceState == ServiceStates.Started);
			}
		}).start();
	}

	public void checkServiceState(DatagramSocket socket) {
		try {
			socket = this.checkSocket(socket);
			MSCoreCmd cmd = new MSCoreCmd();
			String order = cmd.toCmd_EMPTY_String(CMD_TYPES.GET_STATE);
			mscore_service_state = stringhelpers.toNumberString(sendCmd(socket, order, 50), "");
		} catch (Exception e) {
			debug("getMSCoreServiceState error:", e);
			mscore_service_state = "";
		}
		info("mscore state:" + mscore_service_state);
	}

	boolean gf_service_state() {
		long id = 0;
		try {
			if (ssh.open()) {
				String pgrep = stringhelpers.fmt(SSH.CMD_TEMPLATE_PGREP, MsCoreService.mscore_name);
				byte[] pgrepdata = ssh.doCmdReturn(pgrep);
				String val = stringhelpers.toString(pgrepdata, Encodings.UTF8);
				if (false == stringhelpers.isNullOrEmpty(val)) {
					id = integerhelpers.toInt64(val);
				}
			}
		} catch (Exception e) {
			id = 0;
		} finally {
			ssh.close();
		}
		return id > 0;
	}

	void restart_gf_service() {
		try {
			if (ssh.open()) {
				// 启动服务
				String resstart = stringhelpers.fmt(SSH.CMD_TEMPLATE_RESTART, MsCoreService.service_name);
				ssh.doCmd(resstart);
				info("重新启动MSCORE");
			}
		} catch (Exception e) {
			debug("getMSCoreServiceState error:", e);
		} finally {
			ssh.close();
		}
	}

	int send_buffer_length = 1024;

	String sendCmd(DatagramSocket socket, String cmd, int receive_buffer_length) {
		String result = "";
		String show_result = "";
		current_lock.lock();
		byte[] receive = new byte[0];
		try {
			byte[] data = stringhelpers.toBytes(cmd, Encodings.ASCII);
			debug("send data : " + cmd);
			receive = sockethelpers.send(socket, data, receive_buffer_length);
			String temp = new String(receive, 0, receive.length, Encodings.GB2312).trim();
			show_result = temp;
			int start_index = 0;
			int head_length = 15;
			if (temp.length() >= head_length) {
				String code = temp.substring(0, head_length);
				// info("send cmd code:" + code);
				if (code.equals("statecode=0000;")) {
					start_index += head_length;
					result = temp.substring(start_index);
				}
			} else {
				// result = temp;
				info("receive_result:" + show_result);
			}
			// info("send cmd result : " + result);
		} catch (Exception e) {
			// result = e.getMessage();
			debug("send cmd error : ", e);
		} finally {
			current_lock.unlock();
		}
		debug("###################################start############################################");
		debug("receive result : ");
		debug("size:" + receive.length + "---data:" + stringhelpers.bytesToHexString(receive));
		debug(show_result);
		debug("####################################end#############################################");
		return result;
	}

	public void clearAllCmds(DatagramSocket socket) {
		MSCoreCmd cmd = new MSCoreCmd();
		this.sendCmd(socket, cmd.toCmd_EMPTY_String(CMD_TYPES.CLEAR_DATA), 50);
		info("clearAllCmds");
	}

	void checkCmdsStateThread() {
		new Thread(new Runnable() {
			public void run() {
				DatagramSocket socket = null;
				do {
					if (stringhelpers.isNullOrEmpty(mscore_service_state)) {
						tools.sleep(2000);
					} else {
						try {
							current_lock.lock();
							socket = checkSocket(socket);
							// 获取状态
							if (getCmdsState(socket)) {
								socket = checkSocket(socket);
								// 检核状态
								checkCmdState(socket);
							}
						} catch (Exception e) {
							debug("checkCmdsStateThread error : ", e);
						} finally {
							current_lock.unlock();
							// 每隔10秒检查一次状态
							tools.sleep(check_cmd_state_timespan);
						}
					}
				} while (ServiceState == ServiceStates.Started);
			}
		}).start();
	}

	void testCheckCmdsStateThread() {
		new Thread(new Runnable() {
			public void run() {
				DatagramSocket socket = null;
				do {
					try {
						if (stringhelpers.isNullOrEmpty(mscore_service_state)) {
							tools.sleep(2000);
						} else {
							socket = checkSocket(socket);
							// 获取状态
							getCmdsState(socket);
						}
					} catch (Exception e) {
						// 每隔10秒获取一次状态
						tools.sleep(check_cmd_state_timespan);
					} finally {
						// 每隔10秒检查一次状态
						tools.sleep(check_cmd_state_timespan);
					}
				} while (ServiceState == ServiceStates.Started);
			}
		}).start();
	}

	public boolean getCmdsState(DatagramSocket socket) {
		int count = -1;
		List<MSCoreCmd> temp_cmds = new ArrayList<MSCoreCmd>();
		try {
			MSCoreCmd getstatecmd = new MSCoreCmd();
			String result = sendCmd(socket, getstatecmd.toCmd_EMPTY_String(CMD_TYPES.INFO), get_gf_cmds_buffer_length);
			info("CMD INFO:\r\n" + result);
			// 解析命令状态
			String[] lines = result.split("\r\n");
			for (String line : lines) {
				String temp_line = line.trim();
				if (temp_line.toUpperCase().startsWith("CMD") && temp_line.contains("ADD;")) {
					MSCoreCmd cmd = new MSCoreCmd(temp_line);
					temp_cmds.add(cmd);
				} else if (temp_line.toUpperCase().startsWith("TOTALNUM")) {
					int start = temp_line.indexOf('=') + 1;
					int end = temp_line.indexOf(';');
					String count_value = temp_line.substring(start, end);
					if (false == stringhelpers.isNullOrEmpty(count_value)) {
						count = integerhelpers.toInt32(count_value);
					}
				}
			}
			return true;
		} catch (Exception e) {
			this.debug("getCmdsState error : ", e);
			return false;
		} finally {
			if (count >= 0) {
				gf_pool.clear();
				gf_pool.addAll(temp_cmds);
			}
		}
	}

	public void checkCmdState(DatagramSocket socket) {
		List<MSCoreCmd> need_remove_items = new ArrayList<MSCoreCmd>();
		List<MSCoreCmd> need_add_items = new ArrayList<MSCoreCmd>();
		int die_cmd_count = 0;
		try {
			int gf_index = 0;
			// 检核高飞指令池中需要移除的指令
			while (gf_index < gf_pool.size()) {
				MSCoreCmd gf_cmd = gf_pool.get(gf_index);
				if (gf_cmd.runstate.equals(MSCoreCmdStates.die)) {
					// 死掉的指令需要移除
					need_remove_items.add(gf_cmd);
					die_cmd_count++;
				} else {
					// am指令池没有的也需要移除
					MSCoreCmd temp_cmd = this.get(am_pool, gf_cmd.ID);
					if (temp_cmd == null) {
						need_remove_items.add(gf_cmd);
					}
				}
				gf_index++;
			}
			tools.sleep(2000);
			// 检核需要添加到高飞指令此的指令
			int am_index = 0;
			while (am_index < am_pool.size()) {
				MSCoreCmd am_cmd = am_pool.get(am_index);
				MSCoreCmd gf_cmd = this.get(gf_pool, am_cmd.ID);
				if (gf_cmd == null) {
					need_add_items.add(am_cmd);
				} else {
					MSCoreCmd temp_cmd = this.get(need_remove_items, am_cmd.ID);
					if (temp_cmd != null) {
						need_add_items.add(am_cmd);
					}
				}
				am_index++;
			}
			// 发送移除命令
			remove(socket, need_remove_items);
			// 发送添加命令
			add(socket, need_add_items);
		} catch (Exception e) {
			debug("checkCmdState error : ", e);
		} finally {
			String msg = stringhelpers.fmt("指令数:%s--转发数:%s--无效指令数:%s", am_pool.size(), gf_pool.size(), die_cmd_count);
			info(msg);
		}
	}

	public void removeTimeOutCmdsThread() {
		new Thread(new Runnable() {
			public void run() {
				do {
					if (stringhelpers.isNullOrEmpty(mscore_service_state)) {
						tools.sleep(2000);
					} else {
						removeTimeOutCmds();
					}
				} while (ServiceState == ServiceStates.Started);
			}
		}).start();
	}

	/**
	 * 移除超时的指令
	 */
	void removeTimeOutCmds() {
		current_lock.lock();
		do {
			MSCoreCmd cmd = null;
			for (MSCoreCmd c : am_pool) {
				if (c.isTimeOut(cmd_remove_timeout)) {
					cmd = c;
					break;
				}
			}

			if (cmd == null) {
				break;
			}
			am_pool.remove(cmd);

		} while (true);
		current_lock.unlock();
		// 每隔30秒检查一次
		tools.sleep(30000);
	}

	void add(DatagramSocket socket, List<MSCoreCmd> cmds) {
		// 发送添加命令
		for (MSCoreCmd cmd : cmds) {
			if (stringhelpers.isNullOrEmpty(mscore_service_state)) {
				break;
			}
			String order = cmd.toCmd_ADD_String();
			info("添加命令:" + order);
			sendCmd(socket, order, 50);
			tools.sleep(1000);
		}
	}

	void remove(DatagramSocket socket, List<MSCoreCmd> cmds) {
		// 发送移除命令
		for (MSCoreCmd cmd : cmds) {
			if (stringhelpers.isNullOrEmpty(mscore_service_state)) {
				break;
			}
			String order = cmd.toCmd_DELETE_String();
			info("移除命令:" + order);
			sendCmd(socket, order, 50);
			tools.sleep(1000);
		}
	}

	public static void main(String args[]) {
		MsCoreService ms = new MsCoreService("192.168.58.244", "root", "123456", 50000);
		ms.DEBUG = false;
		ms.INFO = true;
		ms.ServiceState = ServiceStates.Started;
		ms.mscore_service_state = "ssssssssss";
		ms.check_cmd_state_timespan = 5000;
		ms.isTest = true;

		ms.testCheckCmdsStateThread();
		// tools.sleep(1000);
		// ms.checkCmdStateThread();

		// DatagramSocket socket = null;
		// try {
		// socket = ms.checkSocket(socket);
		//
		// ms.getCmdsState(socket);
		//
		// } catch (Exception e) {
		// e.printStackTrace();
		// }
		// tools.println(ms.mscore_service_state);

		/*
		 * tools.sleep(5000);
		 * 
		 * String urlin =
		 * "rtsp://192.168.58.234:554/pag://192.168.58.234:7302:51010702001310011368:0:MAIN:TCP";
		 * String urlout = "rtmp://192.168.58.233/live/6666"; MSCoreCmd cmd =
		 * new MSCoreCmd(CMD_TYPES.ADD, WORK_MODES.remux, urlin, urlout); String
		 * urlin1 =
		 * "rtsp://192.168.58.234:554/pag://192.168.58.234:7302:51010702001310011361:0:MAIN:TCP";
		 * String urlout1 = "rtmp://192.168.58.233/live/6661"; MSCoreCmd cmd1 =
		 * new MSCoreCmd(CMD_TYPES.ADD, WORK_MODES.remux, urlin1, urlout1);
		 * 
		 * MSCoreCmd[] cmds = new MSCoreCmd[2]; cmds[0] = cmd; cmds[1] = cmd1;
		 * ms.checkCmd(cmds);
		 */
	}
}
