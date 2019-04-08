package com.am.utilities;

import java.util.HashMap;
import java.util.Map;

/**
 * 
 * @author am
 *
 * @param <K1>
 * @param <K2>
 * @param <T>
 */
public class DoubleKeyMap<K1, K2, T> {

	private Map<K1, K2> k1k2Map = new HashMap<>();
	private Map<K2, K1> k2k1Map = new HashMap<>();
	private Map<K1, T> K1Map = new HashMap<>();
	private Map<K2, T> K2Map = new HashMap<>();

	public K1 getK1ByK2(K2 k2) {
		return k2k1Map.get(k2);
	}

	public K2 getK2ByK1(K1 k1) {
		return k1k2Map.get(k1);
	}

	public T getByK1(K1 k1) {
		return K1Map.get(k1);
	}

	public T getByK2(K2 k2) {
		return K2Map.get(k2);
	}

	public void put(K1 k1, K2 k2, T value) throws Exception {
		if (k1k2Map.containsKey(k1)) {
			K2 temp = k1k2Map.get(k1);
			if (false == k2.equals(temp)) {
				throw new Exception(stringhelpers.fmt("与已存在相同的Key [%s]-[%s]", k1, temp));
			}
		}
		if (k2k1Map.containsKey(k2)) {
			K1 temp = k2k1Map.get(k2);
			if (false == k1.equals(temp)) {
				throw new Exception(stringhelpers.fmt("与已存在相同的Key [%s]-[%s]", temp, k2));
			}
		}

		k1k2Map.put(k1, k2);
		k2k1Map.put(k2, k1);
		K1Map.put(k1, value);
		K2Map.put(k2, value);
	}

	public void removeByK1(K1 k1) {
		if (k1k2Map.containsKey(k1)) {
			K2 k2 = getK2ByK1(k1);
			k1k2Map.remove(k2);
			k1k2Map.remove(k2);
			K1Map.remove(k1);
			K2Map.remove(k2);
		}
	}

	public void removeByK2(K2 k2) {
		if (k2k1Map.containsKey(k2)) {
			K1 k1 = getK1ByK2(k2);
			k1k2Map.remove(k1);
			k1k2Map.remove(k2);
			K1Map.remove(k1);
			K2Map.remove(k2);
		}
	}

	public void clear() {
		k1k2Map.clear();
		k2k1Map.clear();
		K1Map.clear();
		K2Map.clear();
	}

	public static void main(String args[]) {
		DoubleKeyMap<String, String, String> map = new DoubleKeyMap<>();
		String k1 = "a";
		String k2 = "b";
		String val = "ab";
		try {
			map.put(k1, k2, val);
			map.put(k1, k2, "cccccccc");
		} catch (Exception e) {
			e.printStackTrace();

		}

		String value = map.getByK1(k1);
		tools.println(value);
		map.removeByK1(k1);
		value = map.getByK2(k2);
		tools.println(value);
	}
}
