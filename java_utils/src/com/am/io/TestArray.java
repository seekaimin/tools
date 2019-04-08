package com.am.io;

import com.am.utilities.stringhelpers;
import com.am.utilities.tools;

/**
 * 处理数据
 * @author am
 *
 */
public class TestArray extends ObjectArray<Byte> {

	public TestArray(int size) {
		super(size);
	}


	@Override
	protected Class<Byte> type() {
		return Byte.class;
	}

	@Override
	protected Byte defvalue() {
		return 0;
	}

	public static void main(String args[]) {
		
		Byte[] d = new Byte[10];

		for (int i = 0; i < d.length; i++) {
			d[i] = (byte) i;
		}
		TestArray p = new TestArray(100);
		p.add(d);
		tools.println(p.toString());
		p.add(d);
		tools.println(p.toString());
		p.movePos(19);
		p.reset();
		tools.println(p.toString());
		p.add(d);
		tools.println(p.toString());
		p.movePos(9);
		p.reset();
		tools.println(p.toString());
		p.add(d);
		tools.println(p.toString());
		p.add(d);
		tools.println(p.toString());

	}

	/**
	 * toString 显示position - end 以十六进制的形式显示
	 */
	@Override
	public String toString() {
		synchronized (this.getDataPool()) {
			return stringhelpers.bytesToHexString(this.getDataPool(),this.getLength());
		}
	}
	
}
