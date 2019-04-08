package com.am.test;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;

import com.am.utilities.CopyIndex;
import com.am.utilities.Encodings;
import com.am.utilities.MyGuid;
import com.am.utilities.arrayhelpers;
import com.am.utilities.crchelpers;
import com.am.utilities.integerhelpers;
import com.am.utilities.jsonhelpers;
import com.am.utilities.stringhelpers;
import com.am.utilities.tools;
import com.am.utilities.web.ContentTypes;
import com.am.utilities.web.HttpCall;
import com.am.utilities.web.HttpURLClient;
import com.am.utilities.web.IHttpCall;
import com.am.utilities.web.Methods;

public class test {
	private int id = 0;
	private String Tame_FF = "";
	private String name = "";

	public test() {
	}

	public test(String name) {
		this.name = name;
		this.Tame_FF = name;
	}

	public int getId() {
		return id;
	}

	public String getTame_FF() {
		return Tame_FF;
	}

	public String getTtt() {
		return name;
	}

	public void setId(int id) {
		this.id = id;
	}

	public String toString() {
		return this.name;
	}

	public static void main(String[] args) {

	}

	void testtest() {
		test aa = new test();
		aa.a(new HttpCall() {
			public void success(String msg) {
				tools.println(msg);
			}

			public void error(Exception e) {
				if (e != null) {
					e.printStackTrace();
				}
			}

			public void complete() {

				tools.println("complete");
			}
		});
	}

	void a(final IHttpCall c) {

		c.success("123");
		c.error(new Exception("fffffffffff"));
		c.complete();
	}

	void aa() {
		String url = "http://localhost:11101/";
		HttpURLClient client = new HttpURLClient(url);
		client.setMethod(Methods.GET);
		byte[] data = new byte[8];
		CopyIndex index = new CopyIndex();
		arrayhelpers.Copy(data, 1, index);
		arrayhelpers.Copy(data, 1, index);
		int crc = crchelpers.crc32(data);
		index.Reset();

		data = new byte[12];
		arrayhelpers.Copy(data, 1, index);
		arrayhelpers.Copy(data, 1, index);
		arrayhelpers.Copy(data, crc, index);

		client.add("values", stringhelpers.bytesToHexString(data));
		client.setContentType(ContentTypes.applicationUrlencoded);
		client.setReadTimeout(50000);
		client.setConnectTimeout(50000);
		try {
			String respose = client.send();
			tools.println(respose);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

	}

	ByteBuffer buffer = ByteBuffer.allocate(10);

	void testBuffer() {
		byte[] d = new byte[1];
		for (int i = 0; i < 10; i++) {
			d[0] = (byte) i;
			buffer.put(d);
			buffer.flip();
			buffer.clear();
			tools.println("sss:%s", buffer.limit());
			buffer.put(d);
			buffer.flip();
			tools.println("sss:%s", buffer.limit());
		}
		tools.println("size:%s", buffer.limit());
	}

	public static void Copy(byte[] des, short src, CopyIndex mode) {
		int size = 4;
		byte[] temp = host2Net(src);
		System.arraycopy(temp, 0, des, mode.getIndex(), size);
		mode.AddIndex(size);
	}

	public static void Copy(byte[] des, int src, CopyIndex mode) {
		int size = 4;
		byte[] temp = host2Net(src);
		System.arraycopy(temp, 0, des, mode.getIndex(), size);
		mode.AddIndex(size);
	}

	/**
	 * 转网络字节序数组
	 * 
	 * @param host
	 *            需要转化的数据源
	 * @return 网络字节序数组
	 */
	public static byte[] host2Net(short src) {
		ByteBuffer temp = ByteBuffer.wrap(new byte[2]);
		temp.order(ByteOrder.BIG_ENDIAN);
		temp.asShortBuffer().put(src);
		return temp.array();
	}

	/**
	 * 转网络字节序数组
	 * 
	 * @param host
	 *            需要转化的数据源
	 * @return 网络字节序数组
	 */
	public static byte[] host2Net(int src) {
		ByteBuffer temp = ByteBuffer.wrap(new byte[4]);
		temp.order(ByteOrder.BIG_ENDIAN);
		temp.asIntBuffer().put(src);
		return temp.array();
	}

	/**
	 * 数组拷贝方法扩展
	 * 
	 * @param des
	 *            目的数组
	 * @param src
	 *            源数组
	 * @param mode
	 *            传输对象
	 */
	public static void Copy(byte[] des, byte[] src, CopyIndex mode) {
		System.arraycopy(src, 0, des, mode.getIndex(), src.length);
		mode.AddIndex(src.length);
	}

	void guidtest() {
		String temp = "7271e106-7b6f-40fe-8844-e7c7d0297805";

		MyGuid guid = new MyGuid();
		guid.parse(temp);
		byte[] d = guid.toBytes();

		MyGuid t = new MyGuid();
		t.parse(d);

		String b = stringhelpers.bytesToHexString(d);
		tools.println(guid.toString(MyGuid.Format.N));
		tools.println(b);
		tools.println(t.toString(MyGuid.Format.N));
	}

	public static void p(String... s) {
		if (s != null) {
			for (String ss : s) {
				tools.println("line:%s", ss);
			}
		}
	}

	public static void p(List<String> s) {
		if (s != null) {
			for (String ss : s) {
				tools.println("line:%s", ss);
			}
		}
	}

	static void mac() {
		byte[] data = new byte[4];
		data[0] = 0x48;
		data[1] = integerhelpers.toInt8(0xF3);
		data[2] = 0x47;
		data[3] = integerhelpers.toInt8(0xA6);
		int a = integerhelpers.toInt32(data, true);
		tools.println(a);
	}

	void json() {
		//String str1 = "{\"brand_no\":\"jycy,sy\",\"unit_rank\":\"2\",\"package\":\"2\"}";
		//String str2 = "{\"brand_no\":\"jycy,sy\",\"unit_rank\":\"2\",\"package\":\"2\"}";
		// JSONObject obj1 = jsonhelpers.toJSONObject(str1);
		// JSONObject obj2 = jsonhelpers.toJSONObject(str2);
		List<Object> a = new ArrayList<Object>();
		// a.add(obj1);
		// a.add(obj2);
		String json = jsonhelpers.javaBeanToJson(a);
		tools.println(json);
	}

	void random() {
		Random random = new Random();
		double a = random.nextDouble() * 160 + 220.00;
		tools.println("aaaaaaaaaaa:%s", String.format("%.2f", a));
	}

	void mscore_test() {
		String temp = "statecode=0000;\r\nsfsdfs\r\nsfsdf";
		// "statecode=0000;" + ""
		int start_index = 0;
		int head_length = 15;

		String code = temp.substring(0, head_length);
		tools.println("code:%s", code);
		start_index += head_length;
		String value = stringhelpers.toNumberString(temp.substring(start_index), "");
		tools.println("value:%s", value);
	}

	public static boolean login(String user, String password) throws Exception {
		String url = "http://192.168.0.254:8801/scmacstransfer/t.do";
		// url="http://192.168.4.254:5006";
		HttpURLClient client = new HttpURLClient(url);
		client.setMethod(Methods.POST);
		client.setContentType(ContentTypes.textHtml);
		client.add("id", "中国汉字");

		client.add("results[a]", "1");
		client.add("results[b]", "2");
		// i:1,
		// n:"测试1"
		client.add("names[0][i]", 1);
		client.add("names[0][n]", "测试1");

		client.add("names[1][i]", 2);
		client.add("names[1][n]", "测试2");

		client.setContentType(ContentTypes.applicationUrlencoded);
		String respose = client.send();
		// tools.println("respose:%s", respose);
		// String respose =
		// sendPost(url,client.getParameterPath(),ContentTypes.applicationUrlencoded);
		tools.println("respose:%s", respose);
		return false;
	}

	public static String sendPost(String url, String param, String content) {
		// PrintWriter out = null;
		BufferedReader in = null;
		String result = "";
		try {
			URL realUrl = new URL(url);
			// 打开和URL之间的连接
			HttpURLConnection conn = (HttpURLConnection) realUrl.openConnection();
			// 设置通用的请求属性
			conn.setRequestProperty("accept", "*/*");
			conn.setRequestProperty("connection", "Keep-Alive");
			conn.setRequestProperty("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
			conn.setRequestProperty("Content-Type", content + ";charset=utf-8");
			// 发送POST请求必须设置如下两行
			conn.setDoOutput(true);
			conn.setDoInput(true);
			// 获取URLConnection对象对应的输出流
			OutputStream out = conn.getOutputStream();

			tools.println("fffffffffffffffffffff:" + param);

			// 发送请求参数
			out.write(stringhelpers.toBytes(param, Encodings.UTF8));
			// flush输出流的缓冲
			out.flush();
			// 定义BufferedReader输入流来读取URL的响应
			in = new BufferedReader(new InputStreamReader(conn.getInputStream()));
			String line;
			while ((line = in.readLine()) != null) {
				result += line;
			}
		} catch (Exception e) {
			System.out.println("发送 POST 请求出现异常！" + e);
			e.printStackTrace();
		}
		// 使用finally块来关闭输出流、输入流
		finally {
			try {
				// if(out!=null){
				// out.close();
				// }
				if (in != null) {
					in.close();
				}
			} catch (IOException ex) {
				ex.printStackTrace();
			}
		}
		return result;
	}

	static void A() throws Exception {
	}

}
