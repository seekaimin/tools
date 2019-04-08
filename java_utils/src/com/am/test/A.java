package com.am.test;

import java.util.LinkedList;
import java.util.List;

import com.am.utilities.CopyIndex;
import com.am.utilities.Encodings;
import com.am.utilities.arrayhelpers;
import com.am.utilities.jsonhelpers;
import com.am.utilities.stringhelpers;
import com.am.utilities.tools;

public class A {
	private int id = 0;
	private String a = "";

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public String getA() {
		return a;
	}

	public void setA(String a) {
		this.a = a;
	}

	public byte[] to() {
		// id(4) + len(2) + buff
		byte[] a_buff = stringhelpers.toBytes(a, Encodings.UTF8);
		int size = 4 + 2 + a_buff.length;
		byte[] data = new byte[size];
		CopyIndex index = new CopyIndex();
		arrayhelpers.Copy(data, id, index);
		arrayhelpers.Copy(data, (short) a_buff.length, index);
		arrayhelpers.Copy(data, a_buff, index);
		return data;
	}

	public String Show() {
		return "class A";
	}

	List<String> pool = new LinkedList<String>();

	public String getReaderKey() throws InterruptedException {
		synchronized (pool) {
			while (pool.isEmpty()) {
				pool.wait();
			}
			String key = pool.remove(0);
			return key;
		}
	}

	public static void main(String args[]) {
		
		B a = new B();
		a.setTestB(123);
		a.setA("aaaaaaaa");
		
		String json =  jsonhelpers.javaBeanToJson(a);
		tools.println("json:%s",json);
		B b =  jsonhelpers.jsonToJavaBean(B.class, json);
		tools.println(b.getA());
	}
	void a(){
		try {
			A a = new A();
			new Thread(() -> {
				int i = 0;
				while(i<2){
				tools.sleep(3000);
				tools.println("set");
				synchronized (a.pool) {
					a.pool.add("ffffffff");
					a.pool.notifyAll();
				}
				i++;
				}
			}).start();

			new Thread(() -> {
				try {
					tools.println("get");
					String key = a.getReaderKey();
					tools.println(key);
				} catch (InterruptedException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}).start();
			String key = a.getReaderKey();
			tools.println(key);

		} catch (InterruptedException e) {
			e.printStackTrace();
		}
	}
}
