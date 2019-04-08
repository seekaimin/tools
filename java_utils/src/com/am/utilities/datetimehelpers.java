package com.am.utilities;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.regex.Pattern;

/**
 * 时间相关帮助类
 * @author am
 *
 */
public class datetimehelpers {
	/**
	 * 全时间格式化字符串 带毫秒
	 */
	public final static String dateTimeLongFormatString = "yyyy-MM-dd HH:mm:ss SSS";
	/**
	 * 全时间格式化字符串
	 */
	public final static String dateTimeFormatString = "yyyy-MM-dd HH:mm:ss";
	/**
	 * 日期格式化字符串
	 */
	public final static String dateFormatString = "yyyy-MM-dd";
	/**
	 * 时间化字符串
	 */
	public final static String timeFormatString = "HH:mm:ss";
	/**
	 * 默认计算基准时间 <br/>
	 * 1970-01-01 00:00:00
	 */
	public final static String defaultData = "1970-01-01 00:00:00";

	/**
	 * 获取日期正则表达式
	 * 
	 * @param dateSplitChar
	 *            日期分割符
	 * @return 日期正则表达式
	 */
	public static Pattern getDatePattern(char dateSplitChar) {
		StringBuffer el = new StringBuffer();
		el.append("^((\\d{2}(([02468][048])|([13579][26]))[\\@DSC\\s]?((((0?");
		el.append("[13578])|(1[02]))[\\@DSC\\s]?((0?[1-9])|([1-2][0-9])|(3[01])))");
		el.append("|(((0?[469])|(11))[\\@DSC\\s]?((0?[1-9])|([1-2][0-9])|(30)))|");
		el.append("(0?2[\\@DSC\\s]?((0?[1-9])|([1-2][0-9])))))|(\\d{2}(([02468][12");
		el.append("35679])|([13579][01345789]))[\\@DSC\\s]?((((0?[13578])|(1[02]))");
		el.append("[\\@DSC\\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))");
		el.append("[\\@DSC\\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\\@DSC\\s]?((0?[");
		el.append("1-9])|(1[0-9])|(2[0-8]))))))");
		String dscVal = String.valueOf(dateSplitChar);
		String elstr = el.toString().replace("@DSC", dscVal);
		Pattern p = Pattern.compile(elstr);
		return p;
	}

	/**
	 * 获取日期时间正则表达式
	 * 
	 * @param dateSplitChar
	 *            日期分割符
	 * @param timeSplitChar
	 *            时间分割符
	 * @return 日期时间正则表达式
	 */
	public static Pattern getDateTimePattern(char dateSplitChar, char timeSplitChar) {
		// String s =
		// "^((\\d{2}(([02468][048])|([13579][26]))[\\-\\/\\s]?((((0?[13578])|(1[02]))[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])))))|(\\d{2}(([02468][1235679])|([13579][01345789]))[\\-\\/\\s]?((((0?[13578])|(1[02]))[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\\-\\/\\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\\-\\/\\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))(\\s(((0?[0-9])|([1-2][0-3]))\\:([0-5]?[0-9])((\\s)|(\\:([0-5]?[0-9])))))?$";
		String el = "^((\\d{2}(([02468][048])|([13579][26]))[\\@DSC\\s]?((((0?[13578])|(1[02]))[\\@DSC\\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\\@DSC\\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\\@DSC\\s]?((0?[1-9])|([1-2][0-9])))))|(\\d{2}(([02468][1235679])|([13579][01345789]))[\\@DSC\\s]?((((0?[13578])|(1[02]))[\\@DSC\\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\\@DSC\\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\\@DSC\\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))(\\s(((0?[0-9])|([1-2][0-3]))\\@TSC([0-5]?[0-9])((\\s)|(\\@TSC([0-5]?[0-9])))))?$";
		String dscVal = String.valueOf(dateSplitChar);
		String tscVal = String.valueOf(timeSplitChar);
		String elstr = el.replace("@DSC", dscVal).replace("@TSC", tscVal);
		Pattern p = Pattern.compile(elstr);
		return p;
	}

	/**
	 * 判断是否是一个正确的日期字符串
	 * 
	 * @param src
	 *            需要检核的字符串
	 * @param dateSplitChar
	 *            日期分隔符
	 * @return 是否正确
	 */
	public static boolean isDate(String src, char dateSplitChar) {
		return getDatePattern(dateSplitChar).matcher(src).matches();
	}

	/**
	 * 判断是否是一个正确的日期时间字符串
	 * 
	 * @param src
	 *            需要检核的字符串
	 * @param dateSplitChar
	 *            日期分隔符
	 * @param timeSplitChar
	 *            时间分隔符
	 * @return 是否正确
	 */
	public static boolean isDateTime(String src, char dateSplitChar, char timeSplitChar) {
		return getDateTimePattern(dateSplitChar, timeSplitChar).matcher(src).matches();
	}

	/**
	 * 获取基准时间 <br/>
	 * 1970-01-01 00:00:00
	 * 
	 * @return
	 */
	public static Date getDefaulatDate() {
		return toDate(defaultData, dateTimeFormatString);
	}

	/**
	 * 获取当前时间
	 * 
	 * @return 时间
	 */
	public static Date getNow() {
		Date date = Calendar.getInstance().getTime();
		return date;
	}

	/**
	 * 获取当前时间字符串 <br />
	 * foramtstring:yyyy-MM-dd HH:mm:ss
	 * 
	 * @return 时间字符串
	 */
	public static String getNowString() {
		return getNowString(dateTimeFormatString);
	}

	/**
	 * 获取当前时间字符串 带毫秒 <br />
	 * foramtstring:yyyy-MM-dd HH:mm:ss SSS
	 * 
	 * @return 时间字符串
	 */
	public static String getNowLongString() {
		return getNowString(dateTimeLongFormatString);
	}

	/**
	 * 获取当前时间字符串
	 * 
	 * @param famatString
	 *            yyyy-MM-dd HH:mm:ss
	 * @return 时间字符串
	 */
	public static String getNowString(String famatString) {
		Date date = getNow();
		return format(date, famatString);
	}

	/**
	 * 默认格式化字符串yyyy-MM-dd HH:mm:ss
	 * 
	 * @param dateStr
	 * @param format
	 * @return
	 */
	public static Date toDate(String dateStr, String format) {
		SimpleDateFormat sdf = new SimpleDateFormat(format);
		try {
			Date date = sdf.parse(dateStr);
			return date;
		} catch (ParseException e) {
			e.printStackTrace();
			return null;
		}
	}

	/**
	 * 格式化时间
	 * 
	 * @param date
	 *            时间
	 * @param format
	 *            格式化字符串
	 * @return
	 */
	public static String format(Date date, String format) {
		SimpleDateFormat sdf = new SimpleDateFormat(format);
		return sdf.format(date);
	}

	/**
	 * 计算距1970-1-1的总毫秒数
	 * 
	 * @param dateStr
	 *            日期字符串
	 * @return 计算总毫秒数
	 */
	public static long getTotalMilliseconds(String dateStr) {
		Date date1 = getDefaulatDate();
		Date date2 = toDate(dateStr, dateTimeFormatString);
		return date2.getTime() - date1.getTime();
	}

	/**
	 * 计算距1970-1-1的总毫秒数
	 * 
	 * @param dateStr
	 *            日期字符串
	 * @return 计算总毫秒数
	 */
	public static long getTotalMilliseconds(Date date) {
		Date date1 = getDefaulatDate();
		return date.getTime() - date1.getTime();
	}

	static void test() {
		Calendar calendar1 = Calendar.getInstance();
		calendar1.set(1970, 1, 1, 0, 0, 0);
		Calendar calendar2 = Calendar.getInstance();
		calendar2.set(2016, 7, 7, 0, 0, 0);

		long milliseconds = (calendar2.getTimeInMillis() - calendar1.getTimeInMillis()) / 1000;

		System.out.println("相差秒：\t" + milliseconds);
		// System.out.println("加1秒后的时间：\t" + addSecond(calendar1.getTime(),
		// (int)(milliseconds + 3600)));
	}

	/**
	 * 1970-1-1 为基准时间
	 * 
	 * @param type
	 *            Calendar.SECOND
	 * @param value
	 *            需要添加的值
	 * @return Date
	 */
	public static Date addTime(int type, int value) {
		return addTime(getDefaulatDate(), type, value);
	}

	/**
	 * @param basedata
	 *            需要计算的基准时间
	 * @param type
	 *            Calendar.SECOND
	 * @param value
	 *            需要添加的值
	 * @return Date
	 */
	public static Date addTime(Date basedate, int type, int value) {

		Calendar calendar = toCalendar(basedate);
		calendar.add(type, value);
		return calendar.getTime();
	}

	/**
	 * date ot Calendar
	 * 
	 * @param date
	 *            date
	 * @return Calendar
	 */
	public static Calendar toCalendar(Date date) {

		Calendar calendar = Calendar.getInstance();
		calendar.setTime(date);
		return calendar;
	}

	/**
	 * 获取自定义时间
	 * 
	 * @param year
	 *            年
	 * @param month
	 *            月
	 * @param day
	 *            日
	 * @return 时间
	 */
	public static Date toDate(int year, int month, int day) {
		Calendar calendar = Calendar.getInstance();
		calendar.set(year, month - 1, day, 0, 0, 0);
		return calendar.getTime();
	}

	/**
	 * 获取自定义时间
	 * 
	 * @param year
	 *            年
	 * @param month
	 *            月
	 * @param day
	 *            日
	 * @param hour
	 *            时
	 * @param mintue
	 *            分
	 * @param second
	 *            秒
	 * @return 时间
	 */
	public static Date toDate(int year, int month, int day, int hour, int mintue, int second) {
		Calendar calendar = Calendar.getInstance();
		calendar.set(year, month - 1, day, hour, mintue, second);
		return calendar.getTime();
	}

	/**
	 * 根据字段获取相对应的值
	 * 
	 * @param date
	 *            时间
	 * @param field
	 *            字段 Calendar.YEAR
	 * @return 值
	 */
	public static int getFieldValue(Date date, int field) {
		Calendar calendar = toCalendar(date);
		int result = calendar.get(field);
		if (field == Calendar.MONTH) {
			result = result + 1;
		}
		return result;
	}
	
	

	/**
	 * 获取年份
	 * @param date
	 * @return
	 */
	public static int getYear(Date date) {
		return getFieldValue(date,Calendar.YEAR);
	}

	/**
	 * 获取月份
	 * @param date
	 * @return
	 */
	public static int getMonth(Date date) {
		return getFieldValue(date,Calendar.MONTH);
	}
	

	/**
	 * 获取日
	 * @param date
	 * @return
	 */
	public static int getDay(Date date) {
		return getFieldValue(date,Calendar.DAY_OF_MONTH);
	}
	

	/**
	 * 获取时
	 * @param date
	 * @return
	 */
	public static int getHour(Date date) {
		return getFieldValue(date,Calendar.HOUR_OF_DAY);
	}
	

	/**
	 * 获取分
	 * @param date
	 * @return
	 */
	public static int getMinute(Date date) {
		return getFieldValue(date,Calendar.MINUTE);
	}

	/**
	 * 获取秒
	 * @param date
	 * @return
	 */
	public static int getSecond(Date date) {
		return getFieldValue(date,Calendar.SECOND);
	}

	/**
	 * 获取日期部分
	 * 
	 * @param date
	 *            时间
	 * @return 日期
	 */
	public static Date getDate(Date date) {
		String temp = format(date, dateFormatString);
		return toDate(temp, dateFormatString);
	}

	/**
	 * 获取时间部分
	 * 
	 * @param date
	 *            时间
	 * @return 时间
	 */
	public static Date getTime(Date date) {
		String temp = format(date, timeFormatString);
		return toDate(temp, timeFormatString);
	}

	/**
	 * 日期转UTC时间(ms)
	 * @param src 日期
	 * @return
	 */
	public static long toUTC(Date src) {
		return src.getTime();
	}

	/**
	 * utc to date
	 * @param utc utc ms
	 * @return
	 */
	public static Date toDate(long utc) {
		Date date = getNow();
		try {
			SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
			String d = format.format(utc);
			date = format.parse(d);
		} catch (Exception e) {

		}
		return date;
	}
}
