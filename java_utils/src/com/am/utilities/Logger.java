package com.am.utilities;

import java.io.Closeable;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.util.Calendar;
import java.util.Date;

import com.am.io.pathhelpers;

public class Logger implements Closeable {
	/**
	 * 日志级别 = 1
	 */
	public final static int CONSOLE = 1;
	/**
	 * 日志级别 = 2
	 */
	public final static int INFO = 2;
	/**
	 * 日志级别 = 4
	 */
	public final static int ERROR = 4;
	/**
	 * 日志级别 = 8
	 */
	public final static int WARN = 8;
	/**
	 * 日志级别 = 16
	 */
	public final static int DEBUG = 16;
	/**
	 * 日志级别 = 0xFFFFFFFF
	 */
	public final static int ALL = 0xFFFFFFFF;

	/**
	 * 时间 等级 线程名称 消息
	 */
	private final String foramat = "[%s][%s][%s]-%s" + pathhelpers.newLine();
	private String dateForamt = "yyyy-MM-dd HH:mm:ss.SSS";
	private int count = 0;
	private String root = "";
	private int level = 1;
	private boolean disposed = true;
	private FileWriter fileWriter;

	public boolean isDisposed() {
		return this.disposed;
	}

	public Logger() {
		this.disposed = true;
	}

	public void init(String root, int level) {
		if (false == this.isDisposed()) {
			return;
		}
		this.root = root;
		this.level = level;
		try {
			this.ckeck();
		} catch (Exception ex) {

		}
	}

	/**
	 * 初始化
	 * 
	 * @param root
	 *            根目录
	 * @param levels
	 *            日志级别 以逗号gekai info,error,warn,debug,console,all
	 */
	public void init(String root, String levels) {
		if (false == this.isDisposed()) {
			return;
		}
		if (stringhelpers.isNullOrEmpty(levels)) {
			levels = "info";
		}

		String[] ls = levels.split(",");
		this.level = 0;
		for (String l : ls) {
			String t = l.trim().toUpperCase();
			if (t.equals("INFO")) {
				this.level |= Logger.INFO;
			} else if (t.equals("ERROR")) {
				this.level |= Logger.ERROR;
			} else if (t.equals("WARN")) {
				this.level |= Logger.WARN;
			} else if (t.equals("DEBUG")) {
				this.level |= Logger.DEBUG;
			} else if (t.equals("CONSOLE")) {
				this.level |= Logger.CONSOLE;
			} else if (t.equals("ALL")) {
				this.level |= Logger.ALL;
			}
		}
		this.root = root;
		try {
			this.ckeck();
		} catch (Exception ex) {

		}
	}

	private String tempPath = null;

	private void ckeck() throws IOException {
		Date dtnow = datetimehelpers.getNow();
		int year = datetimehelpers.getFieldValue(dtnow, Calendar.YEAR);
		int month = datetimehelpers.getFieldValue(dtnow, Calendar.MONTH);
		String folder = pathhelpers.combine(root, "logs", year + "", month + "");

		File file = new File(folder);
		if (file.exists() == false) {
			file.mkdirs();
		}

		int day = datetimehelpers.getFieldValue(dtnow, Calendar.DAY_OF_MONTH);
		String path = stringhelpers.fmt("%s%s.log", folder, day);
		if (tempPath == null) {
			this.close();
			tempPath = path;
			create(path);
		}else if (false == path.equals(tempPath)) {
			this.close();
			tempPath = path;
			create(path);
		}
	}

	private void create(String path) throws IOException {
		if (this.isDisposed()) {
			File file = new File(path);
			if (file.exists() == false) {
				file.createNewFile();
			}
			fileWriter = new FileWriter(path, true);
			this.disposed = false;
		}
	}

	/**
	 * 日志记录
	 * 
	 * @param level
	 *            日志级别
	 * @param msg
	 *            消息
	 */
	private synchronized void write(int level, String msg) {
		if (this.isDisposed()) {
			return;
		}
		int le = this.getLevel(level);
		if (this.level != Logger.ALL && le > this.level) {
			return;
		}
		try {
			String levelName = this.getLevelName(level);
			if (levelName == null) {
				levelName = "null";
			}
			this.ckeck();
			if (fileWriter != null) {
				String s = stringhelpers.fmt(foramat, datetimehelpers.getNowString(dateForamt), levelName, Thread.currentThread().getName(), msg);
				fileWriter.write(s);
				fileWriter.flush();
			}
		} catch (Exception e) {
			if (count == 0) {
				count++;
				e.printStackTrace();
			}
		} finally {

		}
	}

	/**
	 * 日志记录
	 * 
	 * @param level
	 *            日志级别
	 * @param msg
	 *            消息
	 */
	public synchronized void log(int level, String msg) {
		if (this.isDisposed()) {
			return;
		}
		this.console(msg);
		this.write(level, msg);
	}

	/**
	 * 记录带格式化的error日志
	 * 
	 * @param format
	 *            需要格式化的字符串
	 * @param args
	 *            参数
	 */
	public void log(int level, String format, Object... args) {
		String msg = stringhelpers.fmt(format, args);
		this.log(level, msg);
	}

	private String getLevelName(int level) {
		String result = "";
		if ((level & Logger.ERROR) == Logger.ERROR) {
			result = "ERROR";
		} else if ((level & Logger.INFO) == Logger.INFO) {
			result = "INFO";
		} else if ((level & Logger.WARN) == Logger.WARN) {
			result = "WARN";
		} else if ((level & Logger.DEBUG) == Logger.DEBUG) {
			result = "DEBUG";
		} else {
			result = level + "";
		}
		return result.toString();
	}

	/**
	 * 获取定义的日志级别 重最小级别开始
	 * 
	 * @param level
	 * @return
	 */
	private int getLevel(int level) {
		int result = level;
		if ((level & Logger.INFO) == Logger.INFO) {
			result = Logger.INFO;
		} else if ((level & Logger.ERROR) == Logger.ERROR) {
			result = Logger.ERROR;
		} else if ((level & Logger.WARN) == Logger.WARN) {
			result = Logger.WARN;
		} else if ((level & Logger.DEBUG) == Logger.DEBUG) {
			result = Logger.DEBUG;
		} else if ((level & Logger.CONSOLE) == Logger.CONSOLE) {
			result = Logger.CONSOLE;
			int temp = level & 0xFFFFFFE;
			if (temp > 0) {
				result = temp;
			}
		}
		return result;
	}

	/**
	 * 记录info消息
	 * 
	 * @param msg
	 *            消息
	 */
	public void info(String msg) {
		this.write(Logger.INFO, msg);
	}

	/**
	 * 记录带格式化的info日志
	 * 
	 * @param format
	 *            需要格式化的字符串
	 * @param args
	 *            参数
	 */
	public void info(String format, Object... args) {
		String msg = stringhelpers.fmt(format, args);
		this.info(msg);
	}

	/**
	 * 记录debug消息
	 * 
	 * @param msg
	 *            消息
	 */
	public void debug(String msg) {
		this.write(Logger.DEBUG, msg);
	}

	/**
	 * 记录带格式化的debug日志
	 * 
	 * @param format
	 *            需要格式化的字符串
	 * @param args
	 *            参数
	 */
	public void debug(String format, Object... args) {
		String msg = stringhelpers.fmt(format, args);
		this.debug(msg);
	}

	/**
	 * 记录warn消息
	 * 
	 * @param msg
	 *            消息
	 */
	public void warn(String msg) {
		this.write(Logger.WARN, msg);
	}

	/**
	 * 记录带格式化的warn日志
	 * 
	 * @param format
	 *            需要格式化的字符串
	 * @param args
	 *            参数
	 */
	public void warn(String format, Object... args) {
		String msg = stringhelpers.fmt(format, args);
		this.warn(msg);
	}

	/**
	 * 记录error消息
	 * 
	 * @param msg
	 *            消息
	 */
	public void error(String msg) {
		this.write(Logger.ERROR, msg);
	}

	/**
	 * 记录带格式化的error日志
	 * 
	 * @param format
	 *            需要格式化的字符串
	 * @param args
	 *            参数
	 */
	public void error(String format, Object... args) {
		String msg = stringhelpers.fmt(format, args);
		this.error(msg);
	}

	/**
	 * 记录error消息
	 * 
	 * @param msg
	 *            消息
	 */
	public void console(String msg) {
		if ((level & Logger.CONSOLE) == Logger.CONSOLE) {
			tools.println(msg);
		}
	}

	/**
	 * 记录带格式化的error日志
	 * 
	 * @param format
	 *            需要格式化的字符串
	 * @param args
	 *            参数
	 */
	public void console(String format, Object... args) {
		String msg = stringhelpers.fmt(format, args);
		this.console(msg);
	}

	/**
	 * 关闭资源
	 */
	@Override
	public void close() throws IOException {
		if (this.isDisposed()) {
			return;
		}
		this.disposed = true;
		tools.close(fileWriter);
		fileWriter = null;
		tempPath = null;
	}

	public static void main(String args[]) {
		Logger l = new Logger();
		// int a = l.getLevel(Logger.CONSOLE | 4);
		// tools.println(a + "");

		l.init("D:\\testroot", "debug,console");// |Logger.CONSOLE);

		l.info("info:%s", 1);
		l.error("error:%s", 2);
		tools.sleep(5000);
		l.warn("warn:%s", 3);
		l.debug("debug:%s", 4);
		l.console("console:%s", 5);
		l.log(Logger.ERROR | Logger.CONSOLE, "测试 %s", "fffffff");

		tools.close(l);
	}
}
