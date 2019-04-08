package com.am.test;

import java.util.ArrayList;
import java.util.List;

import com.am.utilities.integerhelpers;
import com.am.utilities.tools;
import com.am.utilities.xmlhelpers;
import com.am.utilities.settings.Property;
import com.am.utilities.settings.XMLSettingBase;

public class XMLSettingTest extends XMLSettingBase {

	String ebm_signauture_enabled = "";
	String sn = "";
	public user u = null;
	public List<user> us = new ArrayList<user>();

	@Override
	public String getPropertyFileName() {
		// TODO Auto-generated method stub
		return "setting.xml";
	}

	@Override
	protected void Read() {
		Property p = getProperty("sn");
		setSn(p.getValue());
		p = getProperty("ebm_signauture_enabled");
		setEbm_signauture_enabled(p.getValue());
		p = getProperty("user");
		Property n = xmlhelpers.getProperty(p.getChildNodes(), "name");
		Property a = xmlhelpers.getProperty(p.getChildNodes(), "age");
		u = new user();
		u.setName(n.getValue());
		u.setAge(integerhelpers.toInt32(a.getValue()));
		us.clear();

		p = getProperty("users");
		for (Property aa : p.getChildNodes()) {
			n = xmlhelpers.getProperty(aa.getChildNodes(), "name");
			a = xmlhelpers.getProperty(aa.getChildNodes(), "age");
			user t = new user();
			t.setName(n.getValue());
			t.setAge(integerhelpers.toInt32(a.getValue()));
			us.add(t);
		}

		tools.println(u.toString());
		;

		for (user aa : us) {
			tools.println(aa.toString());
			;
		}
	}

	@Override
	protected void Write() {
		this.update("ebm_signauture_enabled", this.getEbm_signauture_enabled(), null);
		this.update("sn", this.getSn(), null);

		Property p = getProperty("user");
		this.update(p.getNode(), "name", u.getName(), null);
		this.update(p.getNode(), "age", u.getAge() + "", null);

	}

	public String getEbm_signauture_enabled() {
		return ebm_signauture_enabled;
	}

	public void setEbm_signauture_enabled(String ebm_signauture_enabled) {
		this.ebm_signauture_enabled = ebm_signauture_enabled;
	}

	public String getSn() {
		return sn;
	}

	public void setSn(String sn) {
		this.sn = sn;
	}

}
