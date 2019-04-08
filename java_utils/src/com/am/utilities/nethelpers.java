package com.am.utilities;

/**
 * 网络相关帮助累
 * @author Administrator
 *
 */
public class nethelpers {

	/**
	 * ip地址转化 192.168.000.123 to 192.168.0.123
	 * 
	 * @param IP
	 *            需要转化的IP地址
	 * @return
	 */
	public static String toIpaddress(String IP) {
		String result = "";
		if (nethelpers.isIPAddress(IP)) {
			String[] items = IP.split("\\.");
			int num0 = integerhelpers.toInt32(items[0]);
			int num1 = integerhelpers.toInt32(items[1]);
			int num2 = integerhelpers.toInt32(items[2]);
			int num3 = integerhelpers.toInt32(items[3]);
			result = num0 + "." + num1 + "." + num2 + "." + num3;
		}
		return result;
	}

	/**
	 * 检查字符串是否为ip地址
	 * 
	 * @param IP
	 *            传入的字符串
	 * @return
	 */
	public static boolean isIPAddress(String IP) {
		if (stringhelpers.isNullOrEmpty(IP)) {
			return false;
		}
		String[] items = IP.split("\\.");
		if (items.length != 4)
			return false;
		for (int i = 0; i < items.length; i++) {
			int num = -1;
			try {
				num = Integer.parseInt(items[i]);
			} catch (Exception e) {
				return false;
			}
			if ((num < 0) || (num > 255))
				return false;
			if ((num == 0) && (i == 0))
				return false;
		}
		return true;
	}

	/**
	 * 获取IP地址 的Address
	 * 
	 * @param IP
	 *            传入的字符串
	 * @return
	 */
	public static long getAddress(String IP) {
		if (nethelpers.isIPAddress(IP) == false)
			return 0;
		IP = nethelpers.toIpaddress(IP);
		byte[] datas = nethelpers.ipToBuff(IP);
		long result = (datas[0] << 24) | (datas[1] << 16) | (datas[2] << 8) | (datas[3]);
		return result;
	}

	/**
	 * 检查字符串是否为ip地址
	 * 
	 * @param IP
	 *            传入的字符串
	 * @return
	 */
	public static boolean IsMulticaseAddress(String IP) {
		if (nethelpers.isIPAddress(IP) == false)
			return false;
		long address = nethelpers.getAddress(IP);
		String min = "224.0.0.255";
		String max = "239.255.255.255";
		long min_val = nethelpers.getAddress(min);
		long max_val = nethelpers.getAddress(max);
		return address > min_val && address <= max_val;
	}

	/**
	 * 检查字符串是否为掩码
	 * 
	 * @param msk
	 *            传入的子网掩码
	 * @return
	 */
	public static boolean IsMask(String msk) {
		if (stringhelpers.isNullOrEmpty(msk))
			return false;
		String[] items = msk.split("\\.");
		if (items.length != 4)
			return false;

		boolean vZero = false; // 出现0
		for (int i = 0; i < items.length; i++) {
			int num = -1;
			try {
				num = Integer.parseInt(items[i]);
			} catch (Exception e) {
				return false;
			}
			if ((num < 0) || (num > 255))
				return false;
			if (vZero) {
				if (num != 0)
					return false;
			} else {
				for (int k = 7; k >= 0; k--)
					if (((num >> k) & 1) == 0) // 出现0
						vZero = true;
					else if (vZero)
						return false; // 不为0
			}
		}
		return true;
	}

	/**
	 * 字符串地址比较
	 * 
	 * @param ip0
	 *            地址0
	 * @param ip1
	 *            地址1
	 * @return
	 */
	public static boolean Compare(String ip0, String ip1) {
		if (!nethelpers.isIPAddress(ip0)) {
			return false;
		}
		if (!nethelpers.isIPAddress(ip1)) {
			return false;
		}
		ip0 = nethelpers.toIpaddress(ip0);
		ip1 = nethelpers.toIpaddress(ip1);
		return ip0.equals(ip1);
	}

	/**
	 * 将IP地址转化为4字节数组
	 * 
	 * @param IP
	 *            ip地址
	 * @return 4字节数组
	 */
	public static byte[] ipToBuff(String IP) {
		byte[] result = new byte[4];
		if (!nethelpers.isIPAddress(IP)) {
			return result;
		}
		IP = nethelpers.toIpaddress(IP);
		String[] items = IP.split("\\.");
		for (int i = 0; i < 4; i++) {
			result[i] = integerhelpers.toInt8(items[i]);
		}
		return result;
	}
}
