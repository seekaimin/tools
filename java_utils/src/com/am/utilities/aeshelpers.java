package com.am.utilities;

import java.math.BigInteger;
import java.security.MessageDigest;

import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;

import sun.misc.BASE64Decoder;
import sun.misc.BASE64Encoder;

/**
 * aeshelpers
 * 
 * @author am
 *
 */
public class aeshelpers {

	public static void main(String[] args) throws Exception {
		
		tools.println((2>>>1)+"");
		String content = "00";
		byte[] data = stringhelpers.stringToBytes(content, 16);
		data = new byte[16];
		for (int i = 0; i < data.length; i++) {
			data[i] = (byte)i;
		}
		byte[] key = new byte[16];
		for (int i = 0; i < key.length; i++) {
			key[i] = (byte)i;
		}
		System.out.println("密钥:" + stringhelpers.bytesToHexString(key));
		System.out.println("size:" + data.length + " 加密数据：" + stringhelpers.bytesToHexString(data));
		byte[] encrypt = aeshelpers.encrypt(data, key);
		System.out.println("size:" + encrypt.length + " 加密后数据：" + stringhelpers.bytesToHexString(encrypt));

		byte[] decrypt = aeshelpers.decrypt(encrypt, key);
		content = new String(decrypt);
		System.out.println("size:" + decrypt.length + " 加密数据：" +
		stringhelpers.bytesToHexString(decrypt));
		
		
		
		
		
		
		
		
		byte[] d = stringhelpers.toBytes("123456", Encodings.ASCII);
		tools.println(d);
		d =  aeshelpers.md5(d);
		tools.println(d);
	}

	/**
	 * 将byte[]转为各种进制的字符串
	 * 
	 * @param bytes
	 *            byte[]
	 * @param radix
	 *            可以转换进制的范围，从Character.MIN_RADIX到Character.MAX_RADIX，超出范围后变为10进制
	 * @return 转换后的字符串
	 */
	public static String binary(byte[] bytes, int radix) {
		return new BigInteger(1, bytes).toString(radix);// 这里的1代表正数
	}

	/**
	 * base 64 encode
	 * 
	 * @param bytes
	 *            待编码的byte[]
	 * @return 编码后的base 64 code
	 */
	public static String base64Encode(byte[] bytes) {
		return new BASE64Encoder().encode(bytes);
	}

	/**
	 * base 64 decode
	 * 
	 * @param base64Code
	 *            待解码的base 64 code
	 * @return 解码后的byte[]
	 * @throws Exception
	 */
	public static byte[] base64Decode(String base64Code) throws Exception {

		return stringhelpers.isNullOrEmpty(base64Code) ? null : new BASE64Decoder().decodeBuffer(base64Code);
	}

	/**
	 * 获取byte[]的md5值
	 * 
	 * @param bytes
	 *            byte[]
	 * @return md5
	 * @throws Exception
	 */
	public static byte[] md5(byte[] bytes) throws Exception {
		MessageDigest md = MessageDigest.getInstance("MD5");
		md.update(bytes);
		return md.digest();
	}

	/**
	 * 获取字符串md5值
	 * 
	 * @param msg
	 * @return md5
	 * @throws Exception
	 */
	public static byte[] md5(String msg) throws Exception {
		return stringhelpers.isNullOrEmpty(msg) ? null : md5(msg.getBytes());
	}

	/**
	 * 结合base64实现md5加密
	 * 
	 * @param msg
	 *            待加密字符串
	 * @return 获取md5后转为base64
	 * @throws Exception
	 */
	public static String md5Encrypt(String msg) throws Exception {
		return stringhelpers.isNullOrEmpty(msg) ? null : base64Encode(md5(msg));
	}
	/**
	 * 加密
	 * 
	 * @param content
	 *            需要加密的内容
	 * @param key
	 *            加密密钥
	 * @return
	 */
	public static byte[] encrypt2(byte[] content, byte[] key) {

		byte[] data = content;
		byte[] pwd = key;
		int size = content.length % 16;
		if (size > 0) {
			data = new byte[content.length - size];
			System.arraycopy(content, 0, data, 0, data.length);
		}
		if (key.length > 16) {
			pwd = new byte[16];
			System.arraycopy(key, 0, pwd, 0, 16);
		} else if (key.length < 16) {
			pwd = new byte[16];
			System.arraycopy(key, 0, pwd, 0, key.length);
		}

		byte[] temp = doaes(data, pwd, Cipher.ENCRYPT_MODE);
		if (size > 0) {
			byte[] result = new byte[temp.length + size];
			System.arraycopy(temp, 0, result, 0, data.length);
			System.arraycopy(content, data.length, result, data.length, size);
			return result;
		}
		// SecretKeySpec scretkey = new SecretKeySpec(pwd, "AES");
		// Cipher cipher = Cipher.getInstance("AES/ECB/NoPadding");
		// cipher.init(Cipher.ENCRYPT_MODE, scretkey);// 初始化
		// byte[] result = cipher.doFinal(data);
		// return result; // 加密
		return temp;
	}

	/**
	 * 解密
	 * 
	 * @param content
	 *            需要加密的内容
	 * @param key
	 *            解密密钥
	 * @return
	 */
	public static byte[] decrypt2(byte[] content, byte[] key) {

		byte[] data = content;
		byte[] pwd = key;
		int size = content.length % 16;
		if (size > 0) {
			data = new byte[content.length - size];
			System.arraycopy(content, 0, data, 0, data.length);
		}
		if (key.length > 16) {
			pwd = new byte[16];
			System.arraycopy(key, 0, pwd, 0, 16);
		} else if (key.length < 16) {
			pwd = new byte[16];
			System.arraycopy(key, 0, pwd, 0, key.length);
		}

		byte[] temp = doaes(data, pwd, Cipher.DECRYPT_MODE);
		if (size > 0) {
			byte[] result = new byte[temp.length + size];
			System.arraycopy(temp, 0, result, 0, data.length);
			System.arraycopy(content, data.length, result, data.length, size);
			return result;
		}
		// SecretKeySpec scretkey = new SecretKeySpec(key, "AES");
		// Cipher cipher = Cipher.getInstance("AES/ECB/NoPadding");
		// cipher.init(Cipher.DECRYPT_MODE, scretkey);// 初始化
		// byte[] result = cipher.doFinal(content);
		// return result; // 加密
		return temp;
	}

	/**
	 * 加密
	 * 
	 * @param content
	 *            需要加密的内容
	 * @param key
	 *            加密密钥
	 * @return
	 * @throws Exception
	 */
	public static byte[] encrypt(byte[] content, byte[] key) throws Exception {

		int unit = 16;
		int count = (content.length - 1) / unit + 1;
		byte[] data = new byte[count * unit];
		arrayhelpers.Copy(data, content, new CopyIndex(0));
		SecretKeySpec scretkey = new SecretKeySpec(key, "AES");
		Cipher cipher = Cipher.getInstance("AES");
		cipher.init(Cipher.ENCRYPT_MODE, scretkey);// 初始化
		byte[] result = cipher.doFinal(content);
		return result; // 加密
	}
	/**
	 * 加密
	 * 
	 * @param content
	 *            需要加密的内容
	 * @param key
	 *            加密密钥
	 * @return
	 * @throws Exception
	 */
	public static byte[] decrypt(byte[] content, byte[] key) throws Exception {

		int unit = 16;
		int count = (content.length - 1) / unit + 1;
		byte[] data = new byte[count * unit];
		arrayhelpers.Copy(data, content, new CopyIndex(0));
		SecretKeySpec scretkey = new SecretKeySpec(key, "AES");
		Cipher cipher = Cipher.getInstance("AES");
		cipher.init(Cipher.DECRYPT_MODE, scretkey);// 初始化
		byte[] result = cipher.doFinal(content);
		return result; // 加密
	}

	/**
	 * aes加解密
	 * 
	 * @param data
	 *            需要加解密的数据
	 * @param pwd
	 *            加解密密钥
	 * @param mode
	 *            操作类型 Cipher.DECRYPT_MODE
	 * @return
	 */
	private static byte[] doaes(byte[] data, byte[] pwd, int mode) {
		try {
			SecretKeySpec scretkey = new SecretKeySpec(pwd, "AES");
			Cipher cipher = Cipher.getInstance("AES/ECB/NoPadding");
			cipher.init(mode, scretkey);// 初始化
			byte[] result = cipher.doFinal(data);
			return result; // 加密
		} catch (Exception e) {
			e.printStackTrace();
		}
		return new byte[0];
	}
}
