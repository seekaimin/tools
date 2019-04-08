package com.am.linux;

import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.List;

import com.am.linux.mscore.MsCoreService;
import com.am.utilities.CopyIndex;
import com.am.utilities.Encodings;
import com.am.utilities.arrayhelpers;
import com.am.utilities.stringhelpers;

import ch.ethz.ssh2.Connection;
import ch.ethz.ssh2.Session;

/**
 * SSH帮助类
 * 
 * @author Administrator
 *
 */
public class SSH {

	public static final String CMD_IFCONFIG = "ifconfig";

	public static final String CMD_TEMPLATE_START = "service %s start";
	public static final String CMD_TEMPLATE_STOP = "service %s stop";
	public static final String CMD_TEMPLATE_RESTART = "service %s restart";
	public static final String CMD_TEMPLATE_PGREP = "pgrep %s";

	public static final String CMD_PPPOE_STATUS = "service rp_pppoe status";
	public static final String CMD_PPPOE_SETUP = "service rp_pppoe setup ";
	public static final String CMD_PPPOE_RESTARAT = "service rp_pppoe restart";
	public static final String CMD_PPPOE_STOP = "service rp_pppoe stop";

	public static final String KEYWORD_IFCONFIG_PPPOE = "Point-to-Point Protocol";
	public static final String KEYWORD_IFCONFIG_PTP = "P-t-P:";

	public static final String CMD_GET_NETWORK_CARDS = "cat /proc/net/dev | awk '{if(NR > 2) print substr($1, 0, index($1,\":\"))}'";

	public static final int MSCORE_START_DELAY_TIMEMILLIS = 10 * 1000;

	// virtual

	private Connection conn;
	private Session session;

	/**
	 * 服务器地址
	 */
	public String remote_host;
	/**
	 * 终端登录字符集 默认 utf-8
	 */
	protected String ssh_charset = Encodings.UTF8;
	/**
	 * 登录用户名
	 */
	protected String username;
	/**
	 * 登录用户密码
	 */
	protected String password;

	public SSH() {
	}

	/**
	 * 构建SSH终端
	 * 
	 * @param remote_host
	 *            需要登录的主机(名称或IP)
	 * @param username
	 *            登录名称 最好使用root
	 * @param password
	 *            登录密码
	 */
	public SSH(String remote_host, String username, String password) {
		this.remote_host = remote_host;
		this.username = username;
		this.password = password;
	}

	/**
	 * 开启连接
	 */
	public boolean open() throws IOException {
		if (login()) {
			session = conn.openSession();
			return true;
		}		
		session = null;
		return false;
	}

	/**
	 * 关闭连接
	 */
	public void close() {

		try {
			session.close();
		} catch (Exception e) {
		}
		try {
			conn.close();
		} catch (Exception e) {
		}
	}

	/**
	 * 登录
	 * 
	 * @return 是否登录成功
	 * @throws IOException
	 */
	public boolean login() throws IOException {
		boolean result = false;
		conn = new Connection(this.remote_host);
		conn.connect();
		conn.setTCPNoDelay(true);
		result = conn.authenticateWithPassword(username, password);
		return result;
	}	
	/**
	 * 执行指令  不带返回值
	 *  
	 * @param cmd 指令
	 * @throws IOException
	 */
	public void exec(String cmd) throws IOException {
		try {
			if (this.open()) {
				this.doCmd(cmd);
			}
		} catch (IOException e) {
			e.printStackTrace();
		} finally {
			this.close();
		}
	}

	/**
	 * 处理指令 不带返回值
	 * 
	 * @param cmd
	 *            指令
	 * @throws IOException
	 */
	public void doCmd(String cmd) throws IOException {
		session.execCommand(cmd);
	}

	/**
	 * 获取响应输出流
	 * 
	 * @param cmd
	 *            指令信息
	 * @return 返回输出流
	 * @throws IOException
	 */
	public byte[] doCmdReturn(String cmd) throws IOException {
		session.execCommand(cmd);
		InputStream stream = session.getStdout();
		List<byte[]> data = new ArrayList<byte[]>();
		int length = 0;
		try {
			int size = 0;
			do {
				byte[] buf = new byte[1024];
				size = stream.read(buf);
				if (size > 0) {
					byte[] temp = new byte[size];
					for (int i = 0; i < size; i++) {
						temp[i] = buf[i];
					}
					data.add(temp);
					length += size;
				}
			} while (size > 0);
		} catch (IOException e) {
			e.printStackTrace();
		}

		byte[] result = new byte[length];
		if (length > 0) {
			CopyIndex mode = new CopyIndex();
			for (byte[] srs : data) {
				arrayhelpers.Copy(result, srs, mode);
			}
		}
		return result;
	}

	/**
	 * 执行指令
	 * 
	 * @param cmd
	 *            指令信息
	 * @return buffer
	 */
	public byte[] execReturn(String cmd) {
		byte[] result = new byte[0];
		try {
			if (this.open()) {
				result = this.doCmdReturn(cmd);
			}
		} catch (IOException e) {
			e.printStackTrace();
		} finally {
			this.close();
		}
		return result;
	}

	/**
	 * 执行指令
	 * 
	 * @param cmd
	 *            指令信息
	 * @param callbacl_encoding
	 *            返回字符集设置
	 * @return 返回字符串
	 */
	public String execReturn(String cmd, String callbacl_encoding) {
		byte[] src = execReturn(cmd);
		return stringhelpers.toString(src, callbacl_encoding);
	}

	public static void main(String args[]) {
		SSH cmd = new SSH("192.168.58.233", "root", "123456");
		try {
			if (cmd.login()) {
				String c = stringhelpers.fmt(CMD_TEMPLATE_STOP, MsCoreService.mscore_name);
				String out = cmd.execReturn(c, Encodings.UTF8);
				System.out.println(out);
				System.out.println(out);
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

}
