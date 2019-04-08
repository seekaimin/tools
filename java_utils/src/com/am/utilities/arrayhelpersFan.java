package com.am.utilities;

public class arrayhelpersFan<T> {

	public arrayhelpersFan() {
	}

	/*
	 * static arrayhelpersFan Instance = null;
	 * 
	 * public static arrayhelpersFan getInstance() { if(Instance ==null) {
	 * Instance = new arrayhelpersFan(); } return Instance; }
	 */
	/**
	 * 数组拷贝方法扩展
	 * 
	 * @param pool
	 *            目的数组
	 * @param source
	 *            源数组
	 * @param mode
	 *            传输对象
	 */
	public static void Copy(Class<?> pool, Class<?> source, CopyIndex mode) {
		// System.arraycopy(source, 0, pool, mode.getPoolIndex(),
		// source.length);
		// mode.AddIndex(source.length);
	}

}
