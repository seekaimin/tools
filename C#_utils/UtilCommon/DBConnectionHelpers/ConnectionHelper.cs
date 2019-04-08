using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using Util.Common.ExtensionHelper;
using System.Data.Common;
using System.Data.SqlClient;

namespace Util.Common.DBConnectionHelpers
{
    /// <summary>
    /// 为基于SQLServer的数据库访问提供所有的操作方法
    /// 并提供了命令参数的缓存处理
    /// </summary>
    public static class ConnectionHelper
    {
        /*
         * 其他辅助方法
         * */
        /// <summary>
        /// 映射指定属性
        /// </summary>
        /// <typeparam name="T">映射数据类型</typeparam>
        /// <param name="names">指定属性</param>
        /// <returns></returns>
        public static IDictionary<PropertyInfo, string> MapWith<T>(params string[] names)
        {
            //获取key-value
            IDictionary<PropertyInfo, string> maps = new Dictionary<PropertyInfo, string>();
            if (names == null || names.Length == 0)
                return maps;
            typeof(T).GetProperties().ToList().ForEach(delegate(PropertyInfo p)
            {
                if (p.CanWrite && names.Contains(p.Name))
                {
                    maps.Add(new KeyValuePair<PropertyInfo, string>(p, p.Name));
                }
            });
            return maps;
        }
        /// <summary>
        /// 映射属性(排除指定属性)
        /// </summary>
        /// <typeparam name="T">映射数据类型</typeparam>
        /// <param name="names">排除指定属性</param>
        /// <returns></returns>
        public static IDictionary<PropertyInfo, string> MapWithout<T>(params string[] names)
        {
            //获取key-value
            IDictionary<PropertyInfo, string> maps = new Dictionary<PropertyInfo, string>();
            typeof(T).GetProperties().ToList().ForEach(delegate(PropertyInfo p)
            {
                if (p.CanWrite && !names.Contains(p.Name))
                {
                    maps.Add(new KeyValuePair<PropertyInfo, string>(p, p.Name));
                }
            });
            return maps;
        }
        /// <summary>
        /// 封装实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="temp">实体对象</param>
        /// <param name="reader">System.Data.IDataReader</param>
        /// <param name="maps">映射集合</param>
        public static void EnEntity<T>(T temp, IDataReader reader, IDictionary<PropertyInfo, string> maps)
        {
            PropertyInfo[] ps = temp.GetType().GetProperties();
            maps.Keys.ToList().ForEach(delegate(PropertyInfo key)
            {
                if (ps.Contains(key))
                {
                    SetValue(temp, key, reader[maps[key]]);
                }
            });
        }
        static void SetValue<T>(T temp, PropertyInfo property, object value)
        {
            switch (property.PropertyType.Name)
            {
                case "Byte":
                    property.SetValue(temp, value.ToStr().ToInt8(), null);
                    break;
                case "Byte[]":
                    property.SetValue(temp, value.ToBytes(), null);
                    break;
                case "Int16":
                    property.SetValue(temp, value.ToStr().ToInt16(), null);
                    break;
                case "Int32":
                    property.SetValue(temp, value.ToStr().ToInt32(), null);
                    break;
                case "Int64":
                    property.SetValue(temp, value.ToStr().ToInt64(), null);
                    break;
                case "Boolean":
                    property.SetValue(temp, value.ToStr().ToBoolean(), null);
                    break;
                case "DateTime":
                    property.SetValue(temp, value.ToStr().ToDateTime(), null);
                    break;
                case "String":
                    property.SetValue(temp, value.ToStr(), null);
                    break;
                case "IPAddress":
                    property.SetValue(temp, value.ToStr().ToIPAdress(), null);
                    break;
                default:
                    property.SetValue(temp, value, null);
                    break;
            }
        }

        /// <summary>
        /// 转化为 DbParameter
        /// </summary>
        /// <param name="obj">需要转化的对象</param>
        /// <param name="name">参数对应的名称</param>
        /// <returns>DbParameter 结果</returns>
        public static T ToDbParameter<T>(this object obj, string name)
            where T : DbParameter, new()
        {
            T result = new T();
            result.ParameterName = name;
            result.Value = SetValue(obj);
            return result;
        }
        /// <summary>
        /// 转化为 DbParameter
        /// </summary>
        /// <param name="obj">需要转化的对象</param>
        /// <param name="name">参数对应的名称</param>
        /// <param name="dbtype">数据库数据类型</param>
        /// <returns>DbParameter 结果</returns>
        public static T ToDbParameter<T>(this object obj, string name, DbType dbtype)
            where T : DbParameter, new()
        {
            return new T()
            {
                ParameterName = name,
                Value = SetValue(obj),
                DbType = dbtype,
            };
        }
        /// <summary>
        /// 转化为 DbParameter
        /// </summary>
        /// <param name="obj">需要转化的对象</param>
        /// <param name="name">参数对应的名称</param>
        /// <param name="dbtype">数据库数据类型</param>
        /// <param name="direction">参数类型</param>
        /// <returns>DbParameter 结果</returns>
        public static T ToDbParameter<T>(this object obj, string name, DbType dbtype, ParameterDirection direction)
            where T : DbParameter, new()
        {
            return new T()
            {
                ParameterName = name,
                Value = SetValue(obj),
                DbType = dbtype,
                Direction = direction,
            };
        }

        /// <summary>
        /// 获取当前列的列名
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <param name="index">当前列下标</param>
        /// <param name="defaultName">默认列名</param>
        /// <returns>列名 如果列名为空则返回 defaultName</returns>
        public static string GetColumnName(this IDataReader reader, int index = 0, string defaultName = "NULL")
        {
            string name = reader.GetName(index);
            return string.IsNullOrEmpty(name) ? defaultName : name;
        }


        static object SetValue(object obj)
        {
            return obj == null ? DBNull.Value : obj;
        }
    }


    /// <summary>
    /// 分页查询对象
    /// </summary>
    public class PageQuery<T>
        where T : DbParameter, new()
    {
        int _PageIndex = 0;
        int _PageSize = 20;
        int _TotalCount = 0;
        string _TableName = string.Empty;
        string _FieldNames = "*";
        string _Condiction = string.Empty;
        List<OrderByField> _OrderBy = new List<OrderByField>();
        List<T> _CmdParameters = new List<T>();

        /// <summary>
        /// 分页对象实体
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="orderby">排序列</param>
        /// <param name="fieldnames">需要查询的字段</param>
        public PageQuery(string tableName, List<OrderByField> orderby, string fieldnames = "*")
        {
            this.TableName = tableName;
            this.FieldNames = fieldnames;
            this.OrderBy = orderby;
        }
        /// <summary>
        /// 当前页下标
        /// </summary>
        public int PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; }
        }
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount
        {
            get { return _TotalCount; }
            set { _TotalCount = value; }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get { return (TotalCount - 1) / PageSize + 1; }
        }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }
        /// <summary>
        /// 显示字段名
        /// </summary>
        public string FieldNames
        {
            get { return _FieldNames; }
            set { _FieldNames = value; }
        }
        /// <summary>
        /// 条件
        /// </summary>
        public string Condiction
        {
            get { return _Condiction; }
            set { _Condiction = value; }
        }
        /// <summary>
        /// 排序类型
        /// </summary>
        public List<OrderByField> OrderBy
        {
            get { return _OrderBy; }
            set { _OrderBy = value; }
        }
        /// <summary>
        /// 参数
        /// </summary>
        public List<T> CmdParameters
        {
            get { return _CmdParameters; }
            set { _CmdParameters = value; }
        }
        /// <summary>
        /// 获取查询语句
        /// </summary>
        public string GetSql()
        {
            #region 准备sql语句
            string sql = @"SELECT *
            	                    FROM
            	                    (
            		                    SELECT TOP (@TopSize) *
            		                    FROM
            		                    (
            			                    SELECT TOP (@SkipSize + @TopSize)  {1} 
            			                    FROM {0}
                                            WHERE 1 = 1 {2}
            			                    ORDER BY {3}
            		                    ) t1
            		                    ORDER BY {4} 
            	                    ) t2
            	                    ORDER BY {3}
                                    ";
            #endregion
            #region 准备参数
            StringBuilder orderbyasc = new StringBuilder();
            StringBuilder orderbydesc = new StringBuilder();
            for (int i = 0; i < OrderBy.Count; i++)
            {
                OrderByField item = OrderBy[i];
                orderbyasc.Append(item.ToAsc());
                orderbydesc.Append(item.ToDesc());
                if (i < OrderBy.Count - 1)
                {
                    orderbyasc.Append(",");
                    orderbydesc.Append(",");
                }
            }
            #endregion
            return string.Format(sql, TableName, FieldNames, Condiction, orderbyasc, orderbydesc);
        }
        /// <summary>
        /// 获取参数
        /// </summary>
        public List<T> GetParameters()
        {
            #region 准备参数
            //计算当前显示第几页
            if (PageIndex < 0)
            {
                PageIndex = 0;
            }
            if (PageIndex > TotalPage - 1)
            {
                PageIndex = TotalPage - 1;
            }
            //计算当前显示的记录数
            int topSize = PageSize;
            //计算跳过记录数
            int skipSize = PageSize * PageIndex;
            if (TotalCount < (PageSize * (PageIndex + 1)))
            {
                topSize = TotalCount - (PageSize * PageIndex);
            }
            #endregion
            List<T> parameters = new List<T>();
            if (CmdParameters != null)
            {
                parameters.AddRange(CmdParameters);
            }
            parameters.Add(new T() { ParameterName = "@TopSize", Value = topSize, DbType = DbType.Int32 });
            parameters.Add(new T() { ParameterName = "@SkipSize", Value = skipSize, DbType = DbType.Int32 });
            return parameters;
        }
    }
    /// <summary>
    /// 排序字段
    /// </summary>
    public class OrderByField
    {
        string _FieldName;
        bool _IsAsc = true;
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="fieldName">排序字段</param>
        /// <param name="isAsc">排序类型</param>
        public OrderByField(string fieldName, bool isAsc = true)
        {
            FieldName = fieldName;
            IsAsc = isAsc;
        }
        /// <summary>
        /// 排序字段名称
        /// </summary>
        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }
        /// <summary>
        /// 排序类型
        /// </summary>
        public bool IsAsc
        {
            get { return _IsAsc; }
            set { _IsAsc = value; }
        }
        /// <summary>
        /// 重写ToSTring
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} {1}", FieldName, IsAsc);
        }
        /// <summary>
        /// 相对顺序
        /// </summary>
        public string ToAsc()
        {
            return string.Format("{0} {1}", FieldName,
                IsAsc ? "ASC" : "DESC");
        }
        /// <summary>
        /// 相对倒叙
        /// </summary>
        /// <returns></returns>
        public string ToDesc()
        {
            return string.Format("{0} {1}", FieldName,
                IsAsc ? "DESC" : "ASC");
        }
    }
}
