package com.am.test;

import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.MulticastSocket;
import java.nio.ByteBuffer;
import java.nio.channels.DatagramChannel;
import java.nio.channels.SelectionKey;
import java.nio.channels.Selector;
import java.util.ArrayList;
import java.util.Date;
import java.util.Iterator;
import java.util.List;

import com.am.utilities.CopyIndex;
import com.am.utilities.arrayhelpers;
import com.am.utilities.stringhelpers;
import com.am.utilities.tools;

public class CDRReceiveDemo {
	public static void main(String args[]) {
		try {
			CDRReceiveDemo cdr = new CDRReceiveDemo();

			// String str
			// ="86:010000000000000068:04000000000000:8900000047:5f:fe:15:72:2d:6e:65:74:77:6f:72:6b00db:4e:3b:11:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:fc:01:02:03:1f:01:8200100023:02:01000c:01:10:01:04:0100c80000:b3:b1:49:98:02001f:01:10:43:48:4e000000020001008d:e8:20:0b:63:64:72:2d:6e:65:74:77:6f:72:6b00db:4e:3b:11:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:ff:fc:01:02:03:20:01:8200100023:02:01:47:0f:fe:17:e8:5c:1b:b7:ef:15:f0:91:2a:46:a8:3a:5d:d9:01:5a:82:09:af:2b:5d:53:b7:f7:ff:1b:01:b1:d0:25:0800c5:6a:f0:c3:b0:cb:79:dc:ad:31:98:bb:f7:cc:e8:c6:7a00000000:55:d0:aa:da0001:7f:ff:04:5a:84:1e:a5:cd:2e:45:b4:b6:f8:52:b4:9d:dd:ec:7e:fb:13:ba:57:fd:47:d7:f7:c7:b1:21:c0:40:1b:33:7f:f3:f1:9f:a4:7b:c9:9c:f5:c0:20:14:25:59:e5:a2:b2:58:7f:c4:cc:25:df:55:27:74:2e:cd:24:82:4b:a9:e9:ce:2f:4d:ed:36:07:5d:9a:ce:a9:58:95:9b:15:ab:46:8c:5a:62:40:81:d1:6a:ad:0b:6f:74:9a:34:dd:c0:16:78:9d:d2:c6:4f:ca:5d:d2:97:d5:9c:d9:b5:e2:73:a4002f:06:c5:1d:34:7b:62:43:1c:47:0f:fe:18:dc:7c:30:35:d3:cf:0a:95:52:c9:3b:1f:01:f1:d1:a4:c9:6a:5b:4a:51:cc:ce:ea:40:e8:3e:01:09:59:d9:cb:36:90:05:5f:bb:ef:6f:9b:6c:c40055:d0:aa:da0001:7f:ff:04:7a:84:1c:5b:77:e0:df:ae:e5:bf:99:9d:8f:66:b7:71:be:3f:21:96:1c008f:fa:4c:17:3e:19:44:41:59:17:0f:d3:f1:aa:fe:de:ae:58:03:01:2a:ca:ab:5b:cc:f8:2f:88:41:f8:e5:7e:30:26:72:bf:63:bf:c5:49:49:59:26:52:d1:7d:11:c0:5c:40:1c:a3:37:fa:20:91:11:08:27:ce:8e:80:76:18:24:02:be:63:c1:3b:01:80:da:56:2d:4f:b7:30:9b:07:03:0e:ab:d5:ba:36:90:a0:0d:ab:6a:32:d8:92:be:e8:5d:9b:81:a3:37:33:73:2d:7b:3b:6f:83:52:f3:47:0f:fe:19:1d:3e:05:2b:ff:1b:01:b1:d2:98:ce:a5:c8:54:94:25:6a:80:37:0c:5e:ad:d6:60:04:7f:7e:0b:6000000000:55:d0:aa:da0001:7f:ff:04:3a:84:1e:75:5b:a5:77:a0:51:c4:64:b2:c5:ec:68:1f:44:13:78:45:cd:fc:51:13:dd:8e:fe:e5:d6:93:c5:c2:f8:54:09:50:8e:a1:cc:05:7d:59:90:b5:f5:0d:95:2e:71:27:ad:3c:0b:8a:27:df:26:82:f1:28:8c:b3:4d:b0:72:b3:0f:8c:c9:3b:04:33:35:04:5f:95:ec:d0:14:d8:ef:0e:44:c1:20:d4:9f:70:50:98:d3:3c:60:fa:68:bf:47:9b:8a:9e:dd:9a:82:6e:9e:bf:f6:f7:32:73:77:23:c0:ae:1b:60:48:6e:80:24002b:a4:54:a8:95:ab:ff:ff:23:02:31:de:a40021:49:f8:fb:67:df:70:47:0f:fe:1a:89:d3:e0:36:64:52:6a:77:26:cb:02:83:9b:fb:f4:6e:de:4b:d000000000:55:d0:aa:da0001:7f:ff:04:7a:84:1c:6a:a1:de:bd:f1:4a:c9:6a:8e:62:1c:0f:ba:e8:89:3c:24:af:b5:67:54:0b:99:ab:800055:5d:c5:54:26:030091:64:98:f8:9f:cd:62:82:35:fb:5f:b9:f5:c8:84:24:bd:8a:e2:c9:1a:be:a8:38:a1:28:0e:15:02:81:0f:f1:0b:78:46:ea:c3:85:71:40:a8:6e:42:e2:b4:90:4d:1c:35:44:0b:19:aa:36:de:5f:18:2c:ea:c1:8e:51:10:aa:28:1b:85:cf:5f:fd:7f:f1:98:c1:57:a2:ee:7b:67:b7:b0:be:0e:06:0f:e8:22:dc:c8:cc:df:22:66:b7:07:58:54:aa:9f:ff:ff:1b:01:b1:d0:29:49:25:cb:dc:28:d6:3b:0e:83:ba:47:0f:fe:1b:7b:89:b6:d8:cc:5c:7f:7d:54:8000000055:d0:aa:da0001:7f:ff:04:9a:84:1e:b7:0c:b5:10:91:c9:de:7f:1d:4c:0f:5d:77:76:f6:f8:9e:ff:e9:7e:b7:79:bd:d3:cb:03:6f:b6:db:7f:89:e6:fd:19:39:b7:1f:de:e5:dd:0f:6e:db:3b:4d:de:b1:86:65:ed:43:93:b1:1a:d1:43:bc:f8:44:fb:40:9b:62:9a:2e:6e:51:85:fa:30:12:cd:ae:95:17:e3:0d:e3:1b:82:3d:34:9a:04:36:28:27:ee:aa:87:60:49:90:f5:11:af:29:7d:6b:54:f9:f4:12:29:ab:c8:0d:e9:06:e1:bc:32:60:61:56:18:b0:06:0e:70:04:70:07:f3:0a:ec:7b:99:6c:04:51:28:a9:81:aa:a4:aa:a9:17:01:71:d0:39:56:42:4b:61:68:6e:19:3e:c6:7d:89:0e:09:b1:65:46";
			// str = str.replace(":", "");
			// byte[] d = stringhelpers.hexStringToBytes(str);
			// cdr.doParseData(d);
			// byte[] src = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			// byte[] flags = new byte[] { 3, 4, 5, 6, 7, 8 };
			// int index = cdr.arrayhelpers.indexOf(0, 10, src, flags);
			// tools.println("index : %d", index);
			cdr.doWork();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	private String ip = "192.168.7.9";
	private int port = 10001;
	private int size = 1024 * 2;

	/**
	 * multicastClient方法描述:组播客户端实现，用于向加入的组播其它用户发送组播消息
	 * 
	 * @author : Ricky
	 * @createTime : Apr 13, 2016 8:44:58 AM
	 * @throws Exception
	 */
	public void multicastClient() throws Exception {
		InetAddress group = InetAddress.getByName(ip);// 组播地址
		MulticastSocket mss = null;// 创建组播套接字
		try {
			mss = new MulticastSocket(port);
			mss.joinGroup(group);
			System.out.println("发送数据包启动！（启动时间" + new Date() + ")");

			String message = "Box_ " + new Date();
			byte[] buffer = message.getBytes();
			DatagramPacket dp = new DatagramPacket(buffer, buffer.length, group, port);
			mss.send(dp);
			System.out.println("发送数据包给 " + group + ":" + port);
			Thread.sleep(1000);
		} catch (Exception e) {
			e.printStackTrace();
		} finally {
			try {
				if (mss != null) {
					mss.leaveGroup(group);
					mss.close();
				}
			} catch (Exception e2) {
				e2.printStackTrace();
			}
		}
	}

	byte[] flag1 = new byte[] { 1, 2, 3 };
	byte[] flag2 = new byte[] { 0, 1, 2, 3 };

	/**
	 * multicastServer方法描述:组播服务端实现，用于监听组播中的消息，在接收到消息以后再向组播中的其它成员反馈消息
	 * 
	 * @author : Ricky
	 * @createTime : Apr 13, 2016 8:45:25 AM
	 * @throws Exception
	 */
	public void multicastServer() throws Exception {

		//InetAddress group = InetAddress.getByName(ip);// 组播地址
		// MulticastSocket msr = null;// 创建组播套接字
		DatagramSocket socket = null;
		try {
			socket = new DatagramSocket(port);
			// msr = new MulticastSocket(port);
			// msr.joinGroup(group);// 加入连接
			byte[] buffer = new byte[size];
			System.out.println("接收数据包启动！(启动时间: " + new Date() + ")");

			boolean flag = true;
			List<byte[]> sections = new ArrayList<>();

			while (true) {
				// 建立一个指定缓冲区大小的数据包
				DatagramPacket dp = new DatagramPacket(buffer, buffer.length);
				socket.receive(dp);
				// 解码组播数据包
				// System.out.println("接收到的组播数据包是：" +
				// stringhelpers.bytesToHexString(dp.getData()));

				List<byte[]> temp = doParseData(dp.getData());
				CopyIndex index = new CopyIndex();
				for (byte[] d : temp) {
					if (flag) {
						int pos = arrayhelpers.indexOf(0, d.length, d, flag1);
						if (pos >= 0) {
							pos = arrayhelpers.indexOf(pos, d.length, d, flag2);
							if (pos >= 0) {
								index.Reset();
								sections.clear();
								flag = false;
								index.AddIndex(pos);
								// tools.println("包数据 :%s--%s",d.length,
								// stringhelpers.bytesToHexString(d));
								byte[] t = arrayhelpers.GetBytes(d, package_length - 4 - index.getIndex(), index);
								sections.add(t);
								// sections.add(d);
							}
						}
					} else {
						sections.add(d);
					}
					// tools.println("包数据 :%s--%s",d.length,
					// stringhelpers.bytesToHexString(d));
				}

				if (sections.size() >= 6) {
					int c = 0;
					for (byte[] d : sections) {
						tools.println("包数据 :%d-%s", c++, stringhelpers.bytesToHexString(d));
					}
					flag = true;
				}

				// multicastClient();
			}
		} catch (Exception e) {
			e.printStackTrace();
		} finally {
			if (socket != null) {
				try {
					// socket.leaveGroup(group);
					socket.close();
				} catch (Exception e2) {
					e2.printStackTrace();
				}
			}
		}
	}
	byte tableid = (byte) 0x86;
	byte sysn = (byte) 0x47;
	boolean isFirst = true;

	int package_length = 188;
	int head_length = 21;

	int pos_tablid = 0;

	int pos_sysn = 0;
	int pos_con = 3;

	/**
	 * 解析数据的方法，过滤 0x86
	 *
	 * @param buffer
	 *            收到的字节数组
	 */
	private List<byte[]> doParseData(byte[] buffer) {
		List<byte[]> sections = new ArrayList<byte[]>();
		CopyIndex index = new CopyIndex();
		if (isFirst) {
			if (buffer[pos_tablid] != tableid) {
				sections.clear();
				return sections;
			}
			index.AddIndex(21);
			for (; sections.size() < 6;) {
				// 同步字节错误
				byte current_sysn = arrayhelpers.GetInt8(buffer, index);
				if (current_sysn != sysn) {
					sections.clear();
					return sections;
				}
				index.AddIndex(2);
				//int current_con = (arrayhelpers.GetInt8(buffer, index) & 0xF);
				// tools.println("连续计数器: %s", current_con);

				byte[] p = arrayhelpers.GetBytes(buffer, package_length - 4, index);
				// tools.println("包数据 : %s", stringhelpers.bytesToHexString(p));
				sections.add(p);
			}
		}
		return sections;
	}

	public void doParseData_old(byte[] buffer) {

		int pos = 0;
		int length = buffer.length;
		CopyIndex copyIndex = new CopyIndex();
		//List<byte[]> sections = new ArrayList<>();
		while (pos < length) {
			if (isFirst) {
				if (buffer[pos + pos_tablid] != tableid) {
					pos = arrayhelpers.indexOf(pos, buffer, tableid);
					if (pos < 0) {
						break;
					}
				}

				if (pos + package_length + head_length > length) {
					break;
				}

				// 同步字节错误
				if (buffer[pos + head_length] != sysn) {
					pos++;
					continue;
				}
				pos += head_length;
				isFirst = false;
				continue;
			} else {
				if (pos + package_length > length) {
					break;
				}

				if (buffer[pos] != sysn) {
					pos = arrayhelpers.indexOf(pos, buffer, sysn);
					if (pos < 0) {
						break;
					}
				}
				int current_con = (buffer[pos + pos_con] & 0xF);
				tools.println("连续计数器: %s", current_con);

				copyIndex.Reset();
				copyIndex.AddIndex(pos);

				//byte[] p = arrayhelpers.GetBytes(buffer, package_length, copyIndex);
				arrayhelpers.GetBytes(buffer, package_length, copyIndex);
				// tools.println("包数据 : %s", stringhelpers.bytesToHexString(p));
				pos += package_length;
			}

			// 寻找首包
			byte[] flag = new byte[] { 1, 2, 3 };
			int temp_index = arrayhelpers.indexOf(pos, pos + package_length + head_length, buffer, flag);
			if (temp_index < 0) {
				// pos++;
				// continue;
			}
		}
	}

	DatagramChannel channel;

	Selector selector;

	void doWork() {
		try {
			// 打开一个UDP Channel
			channel = DatagramChannel.open();
			// 设定为非阻塞通道
			channel.configureBlocking(false);
			// 绑定端口
			channel.socket().bind(new InetSocketAddress(port));

			// 打开一个选择器
			selector = Selector.open();
			channel.register(selector, SelectionKey.OP_READ);
		} catch (Exception e) {
			e.printStackTrace();
		}
		ByteBuffer byteBuffer = ByteBuffer.allocate(size);
		while (true) {
			try {
				int selectCount = selector.select();
				if (selectCount > 0) {
					Iterator<SelectionKey> iterator = selector.selectedKeys().iterator();
					while (iterator.hasNext()) {
						SelectionKey key =  iterator.next();
						iterator.remove();
						if (key.isReadable()) {
							//DatagramChannel channel = (DatagramChannel) key.channel();
							byteBuffer.clear();
							// 读取
							//InetSocketAddress address = (InetSocketAddress) channel.receive(byteBuffer);
							System.out.println(stringhelpers.bytesToHexString(byteBuffer.array()));
							// 删除缓冲区中的数据
							//byteBuffer.clear();
							//String message = "data come from server";
							//byteBuffer.put(message.getBytes());
							//byteBuffer.flip();
							// 发送数据
							//channel.send(byteBuffer, address);
							//tools.sleep(1000);
						}
					}
				}else
				{
					tools.println("sssssssssssssssss  0");
				}
			} catch (Exception e) {
				e.printStackTrace();
			}
		}
	}

}
