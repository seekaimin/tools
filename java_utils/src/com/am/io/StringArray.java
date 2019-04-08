package com.am.io;

import com.am.utilities.tools;

/**
 * 字符串数据处理池
 * @author am
 *
 */
public class StringArray extends ObjectArray<String> {

	/**
	 * 构造
	 * @param size
	 */
	public StringArray(int size) {
		super(size);
	}


	@Override
	protected Class<String> type() {
		return String.class;
	}

	@Override
	protected String defvalue() {
		return null;
	}

	public static void main(String args[]) {
		
		String[] d = new String[10];

		for (int i = 0; i < d.length; i++) { 
			d[i] = i + "";
		}
		StringArray p = new StringArray(100);
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
			StringBuffer sb = new StringBuffer();
			for(String item : this.getActive())
			{
				sb.append(item);
			}
			return sb.toString();
		}
	}
	
}
