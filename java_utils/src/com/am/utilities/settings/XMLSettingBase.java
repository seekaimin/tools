package com.am.utilities.settings;

import java.util.ArrayList;
import java.util.List;

import org.w3c.dom.Document;
import org.w3c.dom.Element;

import com.am.utilities.xmlhelpers;

/**
 * XML配置工具基类
 * 
 * @author Administrator
 *
 */
public abstract class XMLSettingBase extends FileReadBase {

	Document doc = null;
	Element root = null;
	int loadlevel = -1;
	List<Property> properties = new ArrayList<Property>();

	public XMLSettingBase() {
	}

	/**
	 * 初始化
	 */
	public void Init() {
		String path = getPath();
		doc = xmlhelpers.loadXML(path);
		root = doc.getDocumentElement();
		this.getProperties().clear();
		if (root == null) {

		} else {
			List<Element> nodes = xmlhelpers.getChildren(root, Property.NodeTagName);
			for (int i = 0; i < nodes.size(); i++) {
				Element element = (Element) nodes.get(i);
				Property temp = new Property(element, 0, this.getLoadlevel());
				this.getProperties().add(temp);
			}
			this.Read();
		}
	}

	/**
	 * 保存
	 * 
	 */
	public void Save() {
		String path = getPath();
		Write();
		// for (Property item : this.getProperties()) {
		// tools.println(item.getValue());
		// xmlhelpers.updateXML(root, item);
		// }
		xmlhelpers.saveXML(path, this.doc);
	}

	/**
	 * 从集合在筛选相对应的属性信息
	 * 
	 * @param src
	 *            源
	 * @param key
	 *            key
	 * @return
	 */
	protected Property getProperty(String key) {
		return xmlhelpers.getProperty(this.getProperties(), key);
	}

	/**
	 * 从集合在筛选相对应的属性信息
	 * 
	 * @param src
	 *            源
	 * @param key
	 *            key
	 * @return
	 */
	protected List<Property> getProperties(String key) {
		return xmlhelpers.getProperties(this.getProperties(), key);
	}

	/**
	 * 更新节点信息
	 * 
	 * @param item
	 *            节点信息
	 */
	protected void update(String key, String value, String comment) {
		// Property item = new Property(key,value,comment,null);
		// xmlhelpers.updateXML(root, item);
		this.update(root, key, value, comment);
	}

	/**
	 * 更新节点信息
	 * 
	 * @param item
	 *            节点信息
	 */
	protected void update(Element p, String key, String value, String comment) {
		Property item = new Property(key, value, comment, null);
		xmlhelpers.updateXML(p, item);
	}

	/**
	 * 
	 * 允许加载的层级数量
	 * 
	 * @return -1：加载所有节点 0-n:加载层级数量
	 */
	public int getLoadlevel() {
		return loadlevel;
	}

	/**
	 * 允许加载的层级数量
	 * 
	 * @param loadlevel
	 *            -1：加载所有节点 0-n:加载层级数量
	 */
	public void setLoadlevel(int loadlevel) {
		this.loadlevel = loadlevel;
	}

	/**
	 * 根节点下的配置属性集合
	 * 
	 * @return 配置属性
	 */
	protected List<Property> getProperties() {
		return properties;
	}

}
