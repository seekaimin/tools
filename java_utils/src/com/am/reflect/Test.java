package com.am.reflect;

public class Test extends TBase {
	public Test() {
	}

	public void test0() {

	}

	@UriAttribute(uri = "我是测试")
	public int test1(int a, int b) {
		return a + b;
	}
}
