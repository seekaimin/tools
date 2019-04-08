package com.am.utilities;

import java.io.File;
import java.io.FileFilter;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.net.URL;
import java.net.URLDecoder;
import java.util.ArrayList;
import java.util.Enumeration;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import com.am.reflect.TBase;
import com.am.reflect.UriAttribute;
import com.am.reflect.UriMapping;


/**
 * class 帮助类
 * @author am
 *
 */
public class classhelpers {

	public static void main(String args[]) throws IllegalAccessException, IllegalArgumentException, InvocationTargetException, InstantiationException {
		String p = "com.dexin.reflect";
		//List<Class<?>> lst = classhelpers.getClass(p);
		//for (Class<?> item : lst) {
			//System.out.println(item.getName());
		//}
		Map<String, UriMapping> map = classhelpers.getUriMap(p);
		for(String k : map.keySet()){
			//System.out.println("Key :" + k);
			UriMapping m = map.get(k);
			//System.out.println("clazz :" + m.getClazz().getName());
			//System.out.println("method :" + m.getMethod().getName());
			//System.out.println("uri :" + m.getUriAttr().value());
			TBase t = (TBase)m.getClazz().newInstance();
			t.setT("ffffffffff");
			if("我是测试".equals(k)){
				Object o = m.getMethod().invoke(t,1,2);
				tools.println("result : " + o);
			}else{
				m.getMethod().invoke(t);
			}
			tools.println("t : " + t.getT());
		}
	}

	/**
	 * 获取包下所有的类
	 * 
	 * @param packageName
	 * @return
	 */
	public static List<Class<?>> getClass(String packageName) {
		List<Class<?>> classes = new ArrayList<Class<?>>();
		String packageDirName = packageName.replace('.', '/');
		try {
			// 定义一个枚举的集合 并进行循环来处理这个目录下的things
			Enumeration<URL> dirs = Thread.currentThread().getContextClassLoader().getResources(packageDirName);
			while (dirs.hasMoreElements()) {
				// 获取下一个元素
				URL url = dirs.nextElement();
				// 得到协议的名称
				String protocol = url.getProtocol();
				if ("file".equals(protocol)) {
					// 获取包的物理路径
					String filePath = URLDecoder.decode(url.getFile(), "UTF-8");
					boolean recursive = true;
					// 以文件的方式扫描整个包下的文件 并添加到集合中
					findAndAddClassesInPackageByFile(packageName, filePath, recursive, classes);
				}
			}
		} catch (Exception e) {

		}
		return classes;
	}

	/**
	 * findAndAddClassesInPackageByFile方法描述: 日期:2016年7月18日 下午5:41:12 异常对象:@param
	 * packageName 异常对象:@param packagePath 异常对象:@param recursive 异常对象:@param
	 * classes
	 */
	public static void findAndAddClassesInPackageByFile(String packageName, String packagePath, final boolean recursive,
			List<Class<?>> classes) {

		// 获取此包的目录 建立一个File
		File dir = new File(packagePath);

		// 如果不存在或者 也不是目录就直接返回
		if (!dir.exists() || !dir.isDirectory()) {
			return;
		}
		// 如果存在 就获取包下的所有文件 包括目录
		File[] dirfiles = dir.listFiles(new FileFilter() {
			// 自定义过滤规则 如果可以循环(包含子目录) 或则是以.class结尾的文件(编译好的java类文件)
			public boolean accept(File file) {

				return (recursive && file.isDirectory()) || (file.getName().endsWith(".class"));
			}
		});
		// 循环所有文件
		for (File file : dirfiles) {
			// 如果是目录 则继续扫描
			if (file.isDirectory()) {
				findAndAddClassesInPackageByFile(packageName + "." + file.getName(), file.getAbsolutePath(), recursive,
						classes);
			} else {
				// 如果是java类文件 去掉后面的.class 只留下类名
				String className = file.getName().substring(0, file.getName().length() - 6);
				try {
					// 添加到集合中去
					classes.add(
							Thread.currentThread().getContextClassLoader().loadClass(packageName + "." + className));
				} catch (ClassNotFoundException e) {
					e.printStackTrace();
				}
			}
		}
	}

	public static Map<String, UriMapping> getUriMap(String packageName) {
		Map<String, UriMapping> result = new HashMap<String, UriMapping>();
		List<Class<?>> classes = classhelpers.getClass(packageName);
		for (Class<?> clazz : classes) {
			// 循环获取所有的类
			Class<?> c = clazz;
			// 获取类的所有方法
			Method[] methods = c.getMethods();
			for (Method method : methods) {
				// 获取RequestMapping注解
				UriAttribute attr = method.getAnnotation(UriAttribute.class);
				if (attr != null) {
					// 获取注解的value值
					String value = attr.uri().toLowerCase();
					if (!result.containsKey(value)) {
						UriMapping m = new UriMapping();
						m.setClazz(clazz);
						m.setUriAttr(attr);
						m.setMethod(method);
						result.put(value, m);
					}
				}
			}
		}

		return result;
	}
}
