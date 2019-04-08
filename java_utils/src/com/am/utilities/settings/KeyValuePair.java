package com.am.utilities.settings;
import com.am.utilities.stringhelpers;

/**
 * key value键值对
 * 
 * @author Administrator
 *
 * @param <TKey>
 *            key 类型
 * @param <TValue>
 *            value 类型
 */
public class KeyValuePair<TKey, TValue> {
	private TKey key;
	private TValue value;

	public KeyValuePair(TKey k, TValue v) {
		this.setKey(k);
		this.setValue(v);
	}

	public TKey getKey() {
		return key;
	}

	public void setKey(TKey key) {
		this.key = key;
	}

	public TValue getValue() {
		return value;
	}

	public void setValue(TValue value) {
		this.value = value;
	}

	@Override
	public String toString() {
		return stringhelpers.fmt("key:%s;value:%s;", key, value);
	}
}
