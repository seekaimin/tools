package com.am.hibernate;

import java.io.Serializable;
import java.lang.reflect.ParameterizedType;

import org.hibernate.Criteria;
import org.hibernate.Session;
import org.hibernate.query.Query;

@SuppressWarnings("unchecked")
public class HibernateDao<T> implements AutoCloseable {

	public Session getSession() {
		return hibernatehelper.getSession();
	}

	@SuppressWarnings("deprecation")
	public Criteria getCriteria() {
		return this.getSession().createCriteria(getTClass());
	}

	public Class<T> getTClass() {
		return (Class<T>) ((ParameterizedType) getClass().getGenericSuperclass()).getActualTypeArguments()[0];
	}

	public Query<T> getHQuery(String hql) {
		return this.getSession().createQuery(hql);
	}

	public T get(Serializable id) {
		return this.getSession().get(getTClass(), id);
	}

	public T saveOrUpdate(T entity) {
		this.getSession().saveOrUpdate(entity);
		this.getSession().flush();
		return entity;
	}

	public void delete(T entity) {
		this.getSession().delete(entity);
	}

	public void delete(Serializable id) {
		T entity = this.get(id);
		if (entity != null) {
			this.delete(entity);
		}
	}

	public void commitTransaction() {
		this.getSession().getTransaction().commit();
	}

	public void rollbackTransaction() {
		this.getSession().getTransaction().rollback();
	}

	@Override
	public void close() throws Exception {
		hibernatehelper.closeSession();
	}
}
