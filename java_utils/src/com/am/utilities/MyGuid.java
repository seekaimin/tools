package com.am.utilities;

import java.util.Random;

/**
 * 自定义guid
 * @author am
 *
 */
public class MyGuid {

	/**
	 * empty guid
	 * 
	 * @return empty
	 */
	public static byte[] empyt() {
		return new byte[16];
	}

	private byte[] a = new byte[4];
	private byte[] b = new byte[2];
	private byte[] c = new byte[2];
	private byte[] d = new byte[2];
	private byte[] e = new byte[6];

	public MyGuid() {

	}

	/**
	 * 生成一个新GUID
	 * 
	 * @return
	 */
	public void builder() {
		Random random = new Random();
		random.nextBytes(a);
		random.nextBytes(b);
		random.nextBytes(c);
		random.nextBytes(d);
		random.nextBytes(e);
	}

	public static MyGuid newId() {
		MyGuid temp = new MyGuid();
		temp.builder();
		return temp;
	}

	public byte[] toBytes() {
		byte[] result = new byte[16];
		CopyIndex mode = new CopyIndex();
		arrayhelpers.Copy(result, a, mode);
		arrayhelpers.Copy(result, b, mode);
		arrayhelpers.Copy(result, c, mode);
		arrayhelpers.Copy(result, d, mode);
		arrayhelpers.Copy(result, e, mode);
		return result;
	}

	@Override
	public String toString() {
		return this.toString(Format.D);
	}

	public String toString(Format format) {

		if (format != Format.N) {
			format = Format.D;
		}

		StringBuffer result = new StringBuffer();
		byte[] _a = arrayhelpers.desc(a);
		byte[] _b = arrayhelpers.desc(b);
		byte[] _c = arrayhelpers.desc(c);

		String a_string = stringhelpers.bytesToHexString(_a);
		String b_string = stringhelpers.bytesToHexString(_b);
		String c_string = stringhelpers.bytesToHexString(_c);
		String d_string = stringhelpers.bytesToHexString(d);
		String e_string = stringhelpers.bytesToHexString(e);

		String split = "";
		if (format == Format.D) {
			result.append("-");
		}
		result.append(a_string);
		result.append(split);
		result.append(b_string);
		result.append(split);
		result.append(c_string);
		result.append(split);
		result.append(d_string);
		result.append(split);
		result.append(e_string);

		return result.toString();
	}

	public void parse(String src) {
		if (src == null) {
			src = "";
		}
		String temp = src.replace("-", "").trim();
		temp = stringhelpers.padRight(temp, 32, "0");
		String a_string = temp.substring(0, 8);
		String b_string = temp.substring(8, 12);
		String c_string = temp.substring(12, 16);
		String d_string = temp.substring(16, 20);
		String e_string = temp.substring(20, 32);

		byte[] _a = stringhelpers.hexStringToBytes(a_string);
		byte[] _b = stringhelpers.hexStringToBytes(b_string);
		byte[] _c = stringhelpers.hexStringToBytes(c_string);

		a = arrayhelpers.desc(_a);
		c = arrayhelpers.desc(_b);
		b = arrayhelpers.desc(_c);
		d = stringhelpers.hexStringToBytes(d_string);
		e = stringhelpers.hexStringToBytes(e_string);
	}

	public void parse(byte[] src) {
		if (src.length == 16) {
			CopyIndex mode = new CopyIndex();
			a = arrayhelpers.GetBytes(src, 4, mode);
			b = arrayhelpers.GetBytes(src, 2, mode);
			c = arrayhelpers.GetBytes(src, 2, mode);
			d = arrayhelpers.GetBytes(src, 2, mode);
			e = arrayhelpers.GetBytes(src, 6, mode);
		} else {
			this.clear();
		}
	}

	public void clear() {
		this.parse(MyGuid.empyt());
	}

	/**
	 * 格式化方式
	 * 
	 * @author Administrator
	 *
	 */
	public enum Format {

		/**
		 * N 模式格式化 debd78679d6bff75b4e719ab3bc8b7a7
		 */
		N,
		/**
		 * D 模式格式化(默认格式化方式) debd7867-9d6b-ff75-b4e7-19ab3bc8b7a7
		 */
		D,
	}
}
