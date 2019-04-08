package com.am.utilities.settings;

import java.util.ArrayList;
import java.util.List;

import org.w3c.dom.Attr;
import org.w3c.dom.Element;
import org.w3c.dom.NamedNodeMap;

import com.am.utilities.stringhelpers;
import com.am.utilities.tools;
import com.am.utilities.xmlhelpers;

public class Property {

	/**
	 * 节点名称
	 */
	public final static String NodeTagName = "PROPERTY";
	/**
	 * key 名称 : Key
	 */
	public final static String KeyName = "Key";
	/**
	 * value 名称 : Value
	 */
	public final static String ValueName = "Value";
	/**
	 * comment 名称 : Comment
	 */
	public final static String CommentName = "Comment";

	/**
	 * 其他信息
	 */
	List<KeyValuePair<String, String>> attrs = new ArrayList<KeyValuePair<String, String>>();

	String key = "";
	String value = "";
	String comment = "";
	Element node;
	int level = 0;
	List<Property> childNodes = new ArrayList<Property>();

	public Property(String k, String v, String c, List<Property> properties) {
		this.setKey(k);
		this.setValue(v);
		this.setComment(c);
		this.setChildNodes(properties);
	}

	/**
	 * 构造
	 * 
	 * @param element
	 *            xml节点信息
	 */
	public Property(Element element, int lv, int loadlevel) {
		this.setNode(element);
		attrs.clear();
		this.setLevel(lv);
		if (element == null) {
			return;
		}

		// 获取 attributes
		NamedNodeMap map = element.getAttributes();
		for (int i = 0; i < map.getLength(); i++) {
			Attr att = (Attr) map.item(i);
			String arrtname = att.getName();
			String k = att.getName();
			String v = att.getValue();
			if (arrtname.equals(KeyName)) {
				this.setKey(v);
			} else if (arrtname.equals(ValueName)) {
				this.setValue(v);
			} else if (arrtname.equals(CommentName)) {
				this.setComment(v);
			} else {
				KeyValuePair<String, String> attr = new KeyValuePair<String, String>(k, v);
				this.getAttrs().add(attr);
			}
		}
		xmlhelpers.getChildren(this, loadlevel);
	}



	public void println() {

		tools.println(this.toString());
		for (Property p : this.getChildNodes()) {
			p.println();
		}
	}

	@Override
	public String toString() {
		StringBuffer sb = new StringBuffer();
		for (int i = 0; i < this.getLevel(); i++) {
			sb.append("  ");
		}
		return stringhelpers.fmt("%s %s---%s---%s", sb, this.getKey(), this.getValue(), this.getComment());
	}

	public List<KeyValuePair<String, String>> getAttrs() {
		return attrs;
	}

	public void setAttrs(List<KeyValuePair<String, String>> attrs) {
		this.attrs = attrs;
	}

	public String getKey() {
		return key;
	}

	public void setKey(String key) {
		this.key = key;
	}

	public String getValue() {
		return value;
	}

	public void setValue(String value) {
		this.value = value;
	}

	public String getComment() {
		return comment;
	}

	public void setComment(String comment) {
		this.comment = comment;
	}

	public Element getNode() {
		return node;
	}

	public void setNode(Element node) {
		this.node = node;
	}

	public List<Property> getChildNodes() {
		return childNodes;
	}

	public void setChildNodes(List<Property> childNodes) {
		this.childNodes = childNodes;
	}

	public int getLevel() {
		return level;
	}

	public void setLevel(int level) {
		this.level = level;
	}

}
