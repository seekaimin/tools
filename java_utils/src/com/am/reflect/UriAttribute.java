package com.am.reflect;

import java.lang.annotation.Documented;
import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/**
 * 定义urilattribute 规范
 * @author am
 *
 */
@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.METHOD)
@Documented//说明该注解将被包含在javadoc中  
public @interface UriAttribute {

	/**
	 * uri
	 * @return uri
	 */
	String uri() default "";


}
