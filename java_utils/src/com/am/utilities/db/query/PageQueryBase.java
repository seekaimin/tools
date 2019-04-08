package com.am.utilities.db.query;

import java.util.ArrayList;
import java.util.List;

import com.am.utilities.integerhelpers;

/**
 * 分页查询数据
 * 
 * @author Administrator
 *
 */
public class PageQueryBase extends QueryBase {
	String Columns;
	String TableView;
	String Condition;
	long TopSize;
	long SkipSize;
	List<OrderByField> OrderByFields = new ArrayList<OrderByField>();

	/**
	 * 分页查询参数
	 */
	public PageQueryBase() {}

	/**
	 * 分页查询参数
	 * 
	 * @param columns
	 *            显示的列
	 * @param table
	 *            表或视图
	 * @param conditions
	 *            查询条件
	 */
	public PageQueryBase(String columns, String table, String conditions) {
		this.Columns = columns;
		this.TableView = table;
		this.Condition = conditions;
	}

	/**
	 * 排序条件
	 * 
	 * @param fieldname
	 *            排序列名
	 * @param orderbytype
	 *            排序方式
	 */
	public void addOrderByField(String fieldname, OrderByTypes orderbytype) {
		OrderByField field = new OrderByField(fieldname, orderbytype);
		OrderByFields.add(field);
	}
	/**
	 * 设置需要显示的列
	 * @param columns
	 */
	public void setColumns(String columns)
	{
		this.Columns = columns;
	}
	/**
	 * 设置表或视图
	 * @param tableview
	 */
	public void setTableView(String tableview)
	{
		this.TableView = tableview;
	}
	/**
	 * 设置条件
	 * @param condition
	 */
	public void setCondition(String condition)
	{
		this.Condition = condition;
	}
	

	/**
	 * 升序字符串
	 * 
	 * @return
	 */
	String getAscString() {
		StringBuffer orderby = new StringBuffer();
		for (int i = 0; i < OrderByFields.size(); i++) {
			OrderByField field = OrderByFields.get(i);
			orderby.append(field.toAscString());
			if (i < OrderByFields.size() - 1) {
				orderby.append(",");
			}
		}
		return orderby.toString();
	}

	/**
	 * 降序字符串
	 * 
	 * @return
	 */
	String getDescString() {
		StringBuffer orderby = new StringBuffer();
		for (int i = 0; i < OrderByFields.size(); i++) {
			OrderByField field = OrderByFields.get(i);
			orderby.append(field.toDescString());
			if (i < OrderByFields.size() - 1) {
				orderby.append(",");
			}
		}
		return orderby.toString();
	}

	/**
	 * 检核 查询数据前调用
	 */
	void check() {
		// 计算当前显示第几页
		if (getPageIndex() < 0) {
			setPageIndex(0);
		}
		if (getPageIndex() > getTotalPages() - 1) {
			setPageIndex(getTotalPages() - 1);
		}
		// 计算当前显示的记录数
		TopSize = this.getPageSize();
		// 计算跳过记录数
		SkipSize = PageSize * PageIndex;
		if (getTotalCount() < (getPageSize() * (getPageIndex() + 1))) {
			TopSize = TotalCount - (PageSize * PageIndex);
		}
	}

	/**
	 * 获取查询总记录数的语句
	 * 
	 * @return 查询总记录数的语句
	 */
	public String queryCountSql() {
		String sql = "SELECT COUNT(1) as TotalCount FROM @TableView WHERE 1 = 1 @Condition";
		return sql.replace("@TableView", this.TableView).replace("@Condition", this.Condition);
	}

	/**
	 * 获取查询数据语句
	 * 
	 * @return 查询数据语句
	 */
	public String querySql() {
		check();
		StringBuffer sql = new StringBuffer();
		sql.append("SELECT * FROM (SELECT TOP (@TopSize) * FROM (SELECT TOP (@SkipSize + @TopSize) @Columns ");
		sql.append(" FROM @TableView WHERE 1 = 1 @Condition ORDER BY @Asc ) t1 ORDER BY @Desc ) t2 ORDER BY @Asc ");
		return sql.toString().replace("@TopSize", integerhelpers.toString(TopSize))
				.replace("@SkipSize", integerhelpers.toString(this.SkipSize)).replace("@Columns", this.Columns)
				.replace("@TableView", this.TableView).replace("@Condition", this.Condition).replace("@Asc", this.getAscString())
				.replace("@Desc", this.getDescString());
	}

}
