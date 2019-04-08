package com.am.test;

import com.am.utilities.settings.SettingBase;

public class settings extends SettingBase implements java.io.Serializable {

	/**
	 * 序列化ID
	 */
	private static final long serialVersionUID = 1L;
	
	String s = "";
	
	
	public String getS() {
		return s;
	}


	public void setS(String s) {
		this.s = s;
	}


	@Override
	public String getPropertyFileName() {
		return "ebsetting.properties";
	}

	
	@Override
	protected void Read() {
		setS(this.getPropertyByName("A"));
	}

	@Override
	protected void Write() {
		this.setProperty("A",getS());
	}

	@Override
	public String toString() {
		return getS();
	}
	
	
}
