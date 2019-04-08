package com.am.utilities.db.query;

import com.am.utilities.stringhelpers;

/**
 * 排序字段
 * 
 * @author Administrator
 *
 */
public class OrderByField {
	public OrderByField() {
	}

	public OrderByField(String name, OrderByTypes type) {
		setOrderName(name);
		setOrderByType(type);
	}

	/**
	 * 字段名称
	 */
	public String OrderName;
	/**
	 * 排序方式
	 */
	public OrderByTypes OrderByType = OrderByTypes.ASC;

	public String toAscString() {
		OrderByTypes type = getOrderByType();
		return stringhelpers.fmt(" %s %s", getOrderName(), type);
	}

	public String toDescString() {
		OrderByTypes type = OrderByTypes.ASC;
		if (getOrderByType() == OrderByTypes.ASC) {
			type = OrderByTypes.DESC;
		}
		return stringhelpers.fmt(" %s %s", getOrderName(), type);
	}

	/**
	 * 排序字段名称
	 * 
	 * @return 排序字段名称
	 */
	public String getOrderName() {
		return OrderName;
	}

	/**
	 * 排序字段名称
	 * 
	 * @param orderName
	 *            排序字段名称
	 */
	public void setOrderName(String orderName) {
		OrderName = orderName;
	}

	/**
	 * 排序方式
	 * 
	 * @return 排序方式
	 */
	public OrderByTypes getOrderByType() {
		return OrderByType;
	}

	/**
	 * 排序方式
	 * 
	 * @param orderByType
	 *            排序方式
	 */
	public void setOrderByType(OrderByTypes orderByType) {
		OrderByType = orderByType;
	}

}
