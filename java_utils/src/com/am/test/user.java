package com.am.test;

import com.am.utilities.stringhelpers;

public class user {
	public user(){}
	String name= "";
	int age = 10;
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public int getAge() {
		return age;
	}
	public void setAge(int age) {
		this.age = age;
	}
	
	public String toString()
	{
		return stringhelpers.fmt("%s--%d", getName(),getAge());
	}
}
