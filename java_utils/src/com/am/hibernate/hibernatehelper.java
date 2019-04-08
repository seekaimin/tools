package com.am.hibernate;

import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.Transaction;
import org.hibernate.cfg.Configuration;

import com.am.utilities.tools;

/**
 * hibernate工具类
 * 
 * @author Administrator
 *
 */
public class hibernatehelper {

	private static String configPath = "hibernate.cfg.xml";
	private static SessionFactory sessionFactory = null;

	/**
	 * 获取sesson工厂
	 * 
	 * @return
	 */
	public static SessionFactory getSessionFactory() {
		if (sessionFactory == null || (sessionFactory.isOpen() == false)) {
			Configuration config = new Configuration();
			config.configure(configPath);
			sessionFactory = config.buildSessionFactory();
		}
		return sessionFactory;
	}

	/**
	 * 获取当前sesson
	 * 
	 * @return
	 */
	public static Session getSession() {
		// _session = this.getSessionFactory().openSession();
		return getSessionFactory().getCurrentSession();
	}

	/**
	 * 开启事务
	 */
	public static void beginTransaction() {
		getSession().beginTransaction();
	}

	/**
	 * 完成事务
	 * 
	 * @param flag
	 *            提交是无标志 <br/>
	 *            true:提交事务;false:回滚事务;
	 */
	public static void complete(boolean flag) {
		Session session = null;
		try {
			session = getSession();
			if (session != null && session.isOpen() && session.isDirty()) {
				Transaction tran = session.getTransaction();
				if (tran != null && tran.isActive()) {
					if (flag) {
						tran.commit();
					} else {
						tran.rollback();
					}
				}
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	/**
	 * 关闭当前session
	 */
	public static void closeSession() {
		if (getSession() != null) {
			getSession().close();
		}
	}

	/**
	 * 关闭当前session
	 */
	public static void close() {
		tools.close(sessionFactory);
	}

}
