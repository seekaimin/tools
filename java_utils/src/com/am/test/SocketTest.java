package com.am.test;

import com.am.services.TCP.ITcpClientHandle;
import com.am.services.TCP.NewServer;
import com.am.services.TCP.TcpException;
import com.am.services.TCP.TcpSocket;

public class SocketTest {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		test();
	}

	static void test() {
		try {
			NewServer.ins().DEBUG = true;
			NewServer.ins().setServiceName("门禁");
			NewServer.ins().setPort(11001);
			NewServer.ins().setHandleSize(4);
			NewServer.ins().setMaxClientSize(0);
			NewServer.ins().setReceiveBufferSize(100);
			NewServer.ins().setHandle(new ITcpClientHandle() {
				@Override
				public void handle(TcpSocket socket) throws TcpException {
					// tools.sleep(1);
					// tools.println(stringhelpers.bytesToHexString(socket.getData()));
					// String s = "fe fd 01 00 00 00 00 00 02 00 00 32 06 06 10
					// 10 00 00 00 00 00 00 0106 06 20 16 00 00 00 01 00 30 00
					// 0b 00 00 04 59 dc f7 9c 00 00 00 006e 78 b2 cd fe fd 01
					// 00 00 00 00 00 01 00 00 38 06 06 10 10 00 00 0000 00 00
					// 01 06 06 10 10 00 00 00 00 00 12 00 11 01 f0 0e 05 ee ee
					// eeee ee ee aa aa aa aa aa aa aa df 37 86 67";
					// s = s.replace(" ", "");
					// s = "00000000";
					// byte[] d = stringhelpers.hexStringToBytes(s);
					socket.setWriteBuffer(socket.getData());
				}

				@Override
				public void exception(Exception e) {
				}
			});
			NewServer.ins().start();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
}
