package com.am.utilities.db.query;

import com.am.utilities.integerhelpers;
import com.am.utilities.stringhelpers;

/**
 * 查询基类
 * 
 * @author Administrator
 *
 */
public class QueryBase {
	int PageIndex = 0;
	long PageSize = 20;
	long TotalCount = 0;

	/**
	 * 查询基类构造
	 */
	public QueryBase() {
	}
	/**
	 * 查询基类构造
	 * 
	 * @param pageSize
	 *            每页显示记录数
	 */
	public QueryBase(long pageSize) {
		this.setPageSize(pageSize);
	}

	/**
	 * 查询基类构造
	 * 
	 * @param pageIndex
	 *            当前页下标
	 * @param pageSize
	 *            每页显示记录数
	 */
	public QueryBase(int pageIndex, long pageSize) {
		this.setPageSize(pageSize);
		this.setPageIndex(pageIndex);
	}

	/**
	 * 获取总页数
	 * 
	 * @return 总页数 最少1页
	 */
	public int getTotalPages() {
		int result = 1;
		if (getPageSize() > 0) {
			//分页
			result =integerhelpers.toInt32((getTotalCount() - 1) / getPageSize() + 1);
		}
		return result;
	}

	/**
	 * 当前页下标-页显示记录数-总记录数-总页数
	 * 
	 * @return toString()
	 */
	public String toString() {
		return stringhelpers.fmt("index:%d-size:%d-count:%d-pages:%d", getPageIndex(), getPageSize(), getTotalCount(),
				getTotalPages());
	}

	/**
	 * 当前页下标 从0开始
	 * 
	 * @return 页下标
	 */
	public int getPageIndex() {
		return PageIndex;
	}

	/**
	 * 
	 * 当前页下标 从0开始
	 * 
	 * @param pageIndex
	 *            页下标
	 */
	public void setPageIndex(int pageIndex) {
		if (pageIndex < 0) {
			pageIndex = 0;
		}
		PageIndex = pageIndex;
	}

	/**
	 * 没页显示记录数 0:表示不分页
	 * 
	 * @return 每页记录数
	 */
	public long getPageSize() {
		return PageSize;
	}

	/**
	 * 没页显示记录数 0:表示不分页
	 * 
	 * @param pageSize
	 *            每页记录数
	 */
	public void setPageSize(long pageSize) {
		if (pageSize < 0) {
			pageSize = 20;
		}
		PageSize = pageSize;
	}

	/**
	 * 总记录数
	 * 
	 * @return 总记录数
	 */
	public long getTotalCount() {
		return TotalCount;
	}

	/**
	 * 总记录数
	 * 
	 * @param totalCount
	 *            总记录数
	 */
	public void setTotalCount(long totalCount) {
		TotalCount = totalCount;
	}

}
