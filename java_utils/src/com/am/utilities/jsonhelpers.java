package com.am.utilities;

import java.util.List;

import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.google.gson.reflect.TypeToken;

/**
 * json帮助类
 * 
 * @author am
 *
 */
public class jsonhelpers {

	/**
	 * 将对象转化为json字符串
	 * 
	 * @param obj
	 * @return json字符串
	 */
	public static String javaBeanToJson(Object obj) {
		Gson gson = new Gson();
		String json = gson.toJson(obj);
		// System.out.println(json);
		return json;
	}

	/**
	 * json to bean
	 * 
	 * @param className
	 *            类名称
	 * @param json
	 *            json字符串
	 * @return
	 */
	public static Object jsonToJavaBean(String className, String json) {
		Object result = null;
		try {
			result = jsonhelpers.jsonToJavaBean(Class.forName(className), json);
		} catch (ClassNotFoundException e) {
			e.printStackTrace();
		}
		return result;
	}

	/**
	 * json to bean
	 * 
	 * @param clazz
	 *            class
	 * @param json
	 *            json字符串
	 * @return
	 */
	public static <T> T jsonToJavaBean(Class<T> clazz, String json) {
		Gson gson = new Gson();
		return gson.fromJson(json, clazz);
	}

	/**
	 * json to 集合
	 * 
	 * @param className
	 *            类型名称
	 * @param jsons
	 *            json字符串
	 * @return
	 */
	public static Object jsonsToJavaBeans(String className, String jsons) {
		Class<?> t;
		try {
			t = Class.forName(className);
			return jsonsToJavaBeans(t, jsons);
		} catch (ClassNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		return null;
	}

	/**
	 * 封装将json对象转换为java集合对象
	 * 
	 * @param <T>
	 * @param clazz
	 * @param jsons
	 * @return
	 */
	public static <T> List<T> jsonsToJavaBeans(Class<T> clazz, String jsons) {
		Gson gson = new Gson();
		List<T> objs = gson.fromJson(jsons, new TypeToken<List<T>>() {
		}.getType());
		return objs;
	}

	/**
	 * json字符串转化为匿名对象
	 * 
	 * @param jsonStr
	 *            json字符串
	 * @return 匿名对象
	 */
	public static JsonObject toJsonObject(String jsonStr) {
		return jsonToJavaBean(JsonObject.class, jsonStr);
	}

}
