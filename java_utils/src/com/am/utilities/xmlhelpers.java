package com.am.utilities;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

import org.w3c.dom.Attr;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import com.am.utilities.settings.Property;

/**
 * xml帮助类
 * @author Administrator
 *
 */
public class xmlhelpers {

	/**
	 * 加载XML
	 * 
	 * @param path
	 *            文件路径
	 * @return Dom对象
	 */
	public static Document loadXML(String path) {
		Document result = null;
		try {
			// 1.创建解析工厂：
			DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
			// 2.指定DocumentBuilder
			DocumentBuilder db = factory.newDocumentBuilder();
			// 3.从文件构造一个Document,因为XML文件中已经指定了编码，所以这里不必了
			File file = new File(path);
			result = db.parse(file);
		} catch (ParserConfigurationException e) {
			tools.println(e.getMessage());
		} catch (SAXException e) {
			tools.println(e.getMessage());
		} catch (IOException e) {
			tools.println(e.getMessage());
		}
		return result;
	}

	/**
	 * 获取xml根节点
	 * 
	 * @param path
	 *            文件路径
	 * @return ROOT
	 */
	public static Element getRoot(String path) {
		Document dom = loadXML(path);
		return dom == null ? null : dom.getDocumentElement();
	}

	/**
	 * 保存xml
	 * 
	 * @param path
	 *            文件路径
	 * @param doc
	 *            doc对象
	 */
	public static void saveXML(String path, Document doc) {
		try {
			// 三.输出
			// 通过DOMSource和StreamResult完成
			// 首先创建转化工厂
			TransformerFactory transFactory = TransformerFactory.newInstance();
			// 创建Transformer，它能够将源树转换为结果树
			Transformer transformer = transFactory.newTransformer();
			// 接下来设置输出属性
			transformer.setOutputProperty("indent", "yes");
			DOMSource source = new DOMSource();
			source.setNode(doc);
			StreamResult result = new StreamResult();
			// 接下来有三种输出用途：
			// 1.将XML字符串输出到String字符串中
			ByteArrayOutputStream baos = new ByteArrayOutputStream();
			result.setOutputStream(baos);
			// 在执行完transformer.transform(source, result)后，
			// 加入String s = baos.toString();
			// 2.直接输出到控制台上
			// result.setOutputStream(System.out);
			// 3.保存到指定的文件里面
			result.setOutputStream(new FileOutputStream(path));

			// 开始执行将XML Source转换为 Result

			transformer.transform(source, result);
		} catch (TransformerException e) {
		} catch (FileNotFoundException e) {
			tools.println(e.getMessage());
		}
	}

	/**
	 * 获取下级节点
	 * 
	 * @param parent
	 *            父节点
	 * @param tagname
	 *            下级节点tagname
	 * @return 下级节点
	 */
	public static List<Element> getChildren(Element parent, String tagname) {
		List<Element> result = new ArrayList<Element>();
		if (parent != null) {
			NodeList nodes = parent.getChildNodes();
			for (int i = 0; i < nodes.getLength(); i++) {
				Node node = nodes.item(i);
				String tag = node.getNodeName();
				if (tag != null && tag.equals(tagname)) {
					Element element = (Element) node;
					result.add(element);
				}
			}
		}
		return result;
	}

	/**
	 * 将对象转化为节点信息
	 * 
	 * @param parent
	 *            父节点
	 */
	public static void updateXML(Element parent, Property src) {

		if (src.getNode() == null) {
			for (Element item : xmlhelpers.getChildren(parent, Property.NodeTagName)) {
				Attr att = xmlhelpers.getAttr(item, Property.KeyName, src.getKey());
				if (att != null) {
					src.setNode(item);
					break;
				}
			}
		}
		if (src.getNode() == null) {
			src.setNode(parent.getOwnerDocument().createElement(Property.NodeTagName));
			parent.appendChild(src.getNode());
			src.getNode().setAttribute(Property.KeyName, src.getKey());
		}
		src.getNode().setAttribute(Property.ValueName, src.getValue());
		if(src.getComment()!=null&&src.getComment().length()>0)
		{
			src.getNode().setAttribute(Property.CommentName, src.getComment());
		}
		
		if (src.getChildNodes() != null && src.getChildNodes().size() > 0) {
			for (Property item : src.getChildNodes()) {
				xmlhelpers.updateXML(src.getNode(), item);
			}
		}
	}

	/**
	 * 添加子节点
	 * 
	 * @param root
	 *            父节点
	 * @param item
	 *            需要添加的子节点
	 */
	public static void addChild(Element root, Element item) {
		root.appendChild(item);
	}

	/**
	 * 添加子节点
	 * 
	 * @param root
	 *            父节点
	 * @param items
	 *            需要添加的子节点
	 */
	public static void addChildren(Element root, List<Element> items) {
		for (int i = 0; i < items.size(); i++) {
			root.appendChild(items.get(i));
		}
	}

	/**
	 * 获取attr
	 * 
	 * @param node
	 *            节点
	 * @param kname
	 *            keyname
	 * @param kvalue
	 *            keyvalue
	 * @return
	 */
	public static Attr getAttr(Element node, String kname, String kvalue) {
		Attr att = node.getAttributeNode(kname);
		if (att != null && att.getValue().equals(kvalue)) {
			return att;
		}
		return null;
	}

	/**
	 * 获取下级节点
	 * 
	 * @return 下级节点
	 */
	public static void getChildren(Property parent, int loadlevel) {
		parent.getChildNodes().clear();
		if (loadlevel < 0 || loadlevel > parent.getLevel()) {
			int nextlevel = parent.getLevel() + 1;
			List<Element> nodes = xmlhelpers.getChildren(parent.getNode(), Property.NodeTagName);
			for (int i = 0; i < nodes.size(); i++) {
				Element element = (Element) nodes.get(i);
				Property temp = new Property(element, nextlevel, loadlevel);
				parent.getChildNodes().add(temp);
			}
		}
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
	public static Property getProperty(List<Property> src, String key) {
		for (Property item : src) {
			if (item.getKey().equals(key)) {
				return item;
			}
		}
		return null;
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
	public static List<Property> getProperties(List<Property> src, String key) {
		List<Property> result = new ArrayList<Property>();
		for (Property item : src) {
			if (item.equals(key)) {
				result.add(item);
			}
		}
		return result;
	}
}
