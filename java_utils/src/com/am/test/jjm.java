package com.am.test;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.math.BigInteger;
import java.security.KeyFactory;
import java.security.KeyPair;
import java.security.KeyPairGenerator;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.security.PrivateKey;
import java.security.PublicKey;
import java.security.Signature;
import java.security.interfaces.RSAPrivateKey;
import java.security.interfaces.RSAPublicKey;
import java.security.spec.RSAPrivateKeySpec;
import java.security.spec.RSAPublicKeySpec;

import com.am.utilities.CopyIndex;
import com.am.utilities.aeshelpers;
import com.am.utilities.arrayhelpers;
import com.am.utilities.crchelpers;
import com.am.utilities.stringhelpers;
import com.am.utilities.tools;

import sun.misc.BASE64Decoder;
import sun.misc.BASE64Encoder;

public class jjm {
	/**
	 * 得到产生的私钥/公钥对
	 * 
	 * @return RSA秘钥对
	 */
	public static KeyPair getKeypair() {
		// 产生RSA密钥对(myKeyPair)
		KeyPairGenerator myKeyGen = null;
		try {
			myKeyGen = KeyPairGenerator.getInstance("RSA");
			myKeyGen.initialize(512);
		} catch (NoSuchAlgorithmException e) {
			e.printStackTrace();
		}
		KeyPair myKeyPair = myKeyGen.generateKeyPair();
		return myKeyPair;
	}

	/**
	 * 
	 * @param mySig
	 *            签名信息
	 * @param myKeyPair
	 *            秘钥对
	 * @param infomation
	 *            签名结果
	 * @return
	 */
	public static byte[] getpublicByKeypair(Signature mySig, KeyPair myKeyPair, byte[] infomation) {
		byte[] publicInfo = null;
		try {
			mySig.initSign(myKeyPair.getPrivate()); // 用私钥初始化签名对象
			mySig.update(infomation); // 将待签名的数据传送给签名对象
			publicInfo = mySig.sign(); // 返回签名结果字节数组
		} catch (Exception e) {
			e.printStackTrace();
		}
		return publicInfo;
	}

	/**
	 * 公钥验证签名
	 * 
	 * @param mySig
	 * @param myKeyPair
	 * @param infomation
	 * @param publicInfo
	 * @return
	 * @author hym
	 */
	public static boolean decryptBypublic(Signature mySig, KeyPair myKeyPair, String infomation, byte[] publicInfo) {
		boolean verify = false;
		try {
			mySig.initVerify(myKeyPair.getPublic()); // 使用公钥初始化签名对象,用于验证签名
			mySig.update(infomation.getBytes()); // 更新签名内容
			verify = mySig.verify(publicInfo); // 得到验证结果
		} catch (Exception e) {
			e.printStackTrace();
		}
		return verify;
	}

	// 将byte数组变成RSAPublicKey

	public static PrivateKey bytes2PrivateKeyK(byte[] b1, byte[] b2) {
		BigInteger modulus = new BigInteger(b1);
		BigInteger publicExponent = new BigInteger(b2);
		RSAPrivateKeySpec spec = new RSAPrivateKeySpec(modulus, publicExponent);// 存储的就是这两个大整形数
		PrivateKey pk = null;
		try {
			KeyFactory keyFactory = KeyFactory.getInstance("RSA");
			pk = keyFactory.generatePrivate(spec);
		} catch (Exception e) {
			e.printStackTrace();
		}
		return pk;
	}

	public static PublicKey bytes2PublicKey(byte[] b1, byte[] b2) {
		BigInteger modulus = new BigInteger(b1);
		BigInteger publicExponent = new BigInteger(b2);
		RSAPublicKeySpec spec = new RSAPublicKeySpec(modulus, publicExponent);// 存储的就是这两个大整形数
		PublicKey pk = null;
		try {
			KeyFactory keyFactory = KeyFactory.getInstance("RSA");
			pk = keyFactory.generatePublic(spec);
		} catch (Exception e) {
			e.printStackTrace();
		}
		return pk;
	}

	// 编码返回字符串
	public static String encodeBASE64(byte[] key) {
		return (new BASE64Encoder()).encodeBuffer(key);
	}

	// 编码返回字符串
	public static byte[] decodeBASE64(String key) {
		try {
			return (new BASE64Decoder()).decodeBuffer(key);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return new byte[0];
	}

	public static void main(String[] args) {
		aesTest();
	}

	void base64test() {
		MyBASE64Encoder encoder = new MyBASE64Encoder();
		String str = "2AE1456B0B0F3C7913A5D7AC069E5DCEFF1A55472819F52A1E90896E2CFEEE76A0767EF1637F8CAAA8EA1515F42E0AFB55BD404FEFF4DB9B72553AF809569D76775B69B7D6F1B03ED639D3AE7C1360C5F4D01761CE0ED265747CC4193EB40547";
		// str = "01020304";
		byte[] buf = stringhelpers.stringToBytes(str, 16);
		ByteArrayOutputStream os = new ByteArrayOutputStream();
		try {
			encoder.encode(buf, os);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		tools.println(os.toString());
	}

	static void aesTest() {
		byte t0 = 24;
		int t1 = t0<<3;
		tools.println(t1);
		// AES测试
		// 密钥 : 0x000102030405060708090a0b0c0d0e0f
		// 原文 : 0x010203040506
		// 密文 : 0xFC480106C439B1E7615896242A9E7285
		try {
			byte[] key = stringhelpers.stringToBytes("000102030405060708090a0b0c0d0e0f", 16);
			// byte[] in =
			// stringhelpers.stringToBytes("010203040506010203040506010203040506010203040506010203040506010203040506",
			// 16);// 010203040506
			byte[] in = stringhelpers.stringToBytes("000102030405060708090a0b0c0d0e0f000102030405060708090a0b0c0d0e0f000102030405060708090a0b0c0d0e0f000102030405060708090a0b", 16);// 010203040506

			byte[] out = aeshelpers.encrypt(in, key);
			tools.println("加密后:%s", stringhelpers.bytesToHexString(out));
			byte[] t = aeshelpers.decrypt(out, key);
			tools.println("解密后:%s", stringhelpers.bytesToHexString(t));

		} catch (Exception e) {
			tools.println(e.getMessage());
		}
	}

	void aaa() {
		byte[] key = new byte[16];

		for (int i = 0; i < 16; i++) {
			key[i] = (byte) i;
		}

		tools.println(stringhelpers.bytesToHexString(key));

		byte[] data = "123456789".getBytes();
		tools.println(data);

		try {
			MessageDigest md = MessageDigest.getInstance("MD5");
			md.update(data);
			byte[] dd = md.digest();

			tools.println("fffffffffffffffffffff");
			tools.println(dd);
		} catch (NoSuchAlgorithmException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}

		try {
			// a94be986cf870679db2bf53a5ff1047f
			byte[] miwwen = aeshelpers.encrypt(data, key);
			tools.println(stringhelpers.bytesToHexString(miwwen));

			int crc = crchelpers.crc32(miwwen);
			byte[] crcs = new byte[4];
			arrayhelpers.Copy(crcs, crc, new CopyIndex());
			tools.println("crc:%s---%s", crc, stringhelpers.bytesToHexString(crcs));
			byte[] mingwen = aeshelpers.decrypt(miwwen, key);
			tools.println(new String(mingwen));
		} catch (Exception e) {
			e.printStackTrace();
		}

		int crc = -1240217751;
		byte[] crcs = new byte[4];
		arrayhelpers.Copy(crcs, crc, new CopyIndex());
		tools.println("crc:%s---%s", crc, stringhelpers.bytesToHexString(crcs));

	}
	// 2018-09-29 15:13:37 029-main : 00 0x01 0x02 0x03 0x04 0x05 0x06 0x07 0x08
	// 0x09 0x0a 0x0b 0x0c 0x0d 0x0e 0x0f 0x
	// 2018-09-29 15:13:37 048-main : 313233343536373839
	// 2018-09-29 15:13:37 308-main : b0 0x79 0x46 0x4c 0xd7 0x41 0x01 0xdd 0xa1
	// 0x88 0xfa 0x9e 0x53 0x88 0xda 0x44 0x
	// 2018-09-29 15:13:37 310-main : crc:-1240217751---b6 0x13 0xc7 0x69 0x
	// 2018-09-29 15:13:37 311-main : 123456789
	// 2018-09-29 15:13:37 311-main : crc:1240217751---49ec3897

	static void a() {
		try {
			KeyPair keyPair = getKeypair();

			RSAPrivateKey privateKey = (RSAPrivateKey) keyPair.getPrivate();// 获取私钥
			RSAPublicKey publicKey = (RSAPublicKey) keyPair.getPublic();// 获取公钥

			byte[] p1 = privateKey.getModulus().toByteArray();
			byte[] p2 = privateKey.getPrivateExponent().toByteArray();
			byte[] b1 = publicKey.getModulus().toByteArray();
			byte[] b2 = publicKey.getPublicExponent().toByteArray();

			tools.println("=====================private start============================");
			tools.println("modulus:%s", stringhelpers.bytesToHexString(p1));
			tools.println("publicExponent:%s", stringhelpers.bytesToHexString(p2));
			tools.println("=====================private end============================");

			tools.println("=====================public start============================");
			tools.println("modulus:%s", stringhelpers.bytesToHexString(b1));
			tools.println("publicExponent:%s", stringhelpers.bytesToHexString(b2));
			tools.println("=====================public end============================");

			keyPair = new KeyPair(bytes2PublicKey(b1, b2), bytes2PrivateKeyK(p1, p2));

			Signature mySig = Signature.getInstance("MD5WithRSA");// 用指定算法产生签名对象
			byte[] publicinfo = getpublicByKeypair(mySig, keyPair, "验证我".getBytes());
			tools.println("pk:%s", stringhelpers.bytesToHexString(publicinfo));

			boolean verify = decryptBypublic(mySig, keyPair, "验证我", publicinfo);
			System.out.println("验证签名的结果是：" + verify);
		} catch (NoSuchAlgorithmException e) {
			e.printStackTrace();
		}
	}
}