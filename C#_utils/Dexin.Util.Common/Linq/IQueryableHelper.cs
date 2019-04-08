using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Dexin.Util.Common.Linq
{
    /// <summary>
    /// IQueryable帮助类
    /// </summary>
    public static class IQueryableHelper
    {
        #region Private expression tree helpers
        private static LambdaExpression GenerateSelector<TEntity>(String propertyName, out Type resultType)
        {
            PropertyInfo property;
            Expression propertyAccess;
            var parameter = Expression.Parameter(typeof(TEntity), "Entity");

            if (propertyName.Contains('.'))
            {
                String[] childProperties = propertyName.Split('.');
                property = typeof(TEntity).GetProperty(childProperties[0]);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
                for (int i = 1; i < childProperties.Length; i++)
                {
                    property = property.PropertyType.GetProperty(childProperties[i]);
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = typeof(TEntity).GetProperty(propertyName);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }

            resultType = property.PropertyType;

            return Expression.Lambda(propertyAccess, parameter);
        }

        private static MethodCallExpression GenerateMethodCall<TEntity>(IQueryable<TEntity> source, string methodName, String fieldName) where TEntity : class
        {
            Type type = typeof(TEntity);
            Type selectorResultType;
            LambdaExpression selector = GenerateSelector<TEntity>(fieldName, out selectorResultType);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName,
            new Type[] { type, selectorResultType },
            source.Expression, Expression.Quote(selector));
            return resultExp;
        }
        #endregion

        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall<TEntity>(source, "OrderBy", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        public static IOrderedQueryable<TEntity> OrderByDescending<TEntity>(this IQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall<TEntity>(source, "OrderByDescending", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        public static IOrderedQueryable<TEntity> ThenBy<TEntity>(this IOrderedQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall<TEntity>(source, "ThenBy", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        public static IOrderedQueryable<TEntity> ThenByDescending<TEntity>(this IOrderedQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall<TEntity>(source, "ThenByDescending", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }
        public static IOrderedQueryable<TEntity> OrderUsingSortExpression<TEntity>(this IQueryable<TEntity> source, string sortExpression) where TEntity : class
        {
            String[] orderFields = sortExpression.Split(',');
            IOrderedQueryable<TEntity> result = null;
            for (int currentFieldIndex = 0; currentFieldIndex < orderFields.Length; currentFieldIndex++)
            {
                String[] expressionPart = orderFields[currentFieldIndex].Trim().Split(' ');
                String sortField = expressionPart[0];
                Boolean sortDescending = (expressionPart.Length == 2) && (expressionPart[1].Equals("DESC", StringComparison.OrdinalIgnoreCase));
                if (sortDescending)
                {
                    result = currentFieldIndex == 0 ? source.OrderByDescending(sortField) : result.ThenByDescending(sortField);
                }
                else
                {
                    result = currentFieldIndex == 0 ? source.OrderBy(sortField) : result.ThenBy(sortField);
                }
            }
            return result;
        }


        /// <summary>
        /// 扩展排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortExpression"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortExpression, bool ascending)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), sortExpression);
            Type selectorResultType;
            LambdaExpression selector = GenerateSelector<T>(sortExpression, out selectorResultType);

            Type[] types = new Type[2];
            types[0] = typeof(T);
            types[1] = selectorResultType;
            Expression expr = Expression.Call(typeof(Queryable)
                , ascending ? "OrderBy" : "OrderByDescending"
                , types
                , source.Expression
                , Expression.Quote(selector));

            return source.Provider.CreateQuery<T>(expr);
        }

        /// <summary>
        /// 处理分页显示
        /// </summary>
        /// <typeparam name="T">需要分页的对象</typeparam>
        /// <param name="paging">分页信息[为null时返回所有记录]</param>
        /// <param name="skipOrder">是否跳过排序逻辑</param>
        /// <param name="total">查询结果</param>
        /// <returns></returns>
        public static IList<T> Query<T>(this IQueryable<T> total, PagingInfo paging, bool skipOrder = false)
        {
            if (paging == null)
                return total.ToList();

            paging.TotalItems = total.Count();
            //检核paging对象
            paging.ValidatePaging();

            //排序
            if (!skipOrder && !string.IsNullOrEmpty(paging.OrderField))
            {
                total = total.OrderBy(paging.OrderField, paging.OrderDirection == OrderDirections.asc);
            }

            //分页
            if (paging.NumPerPage > 0)
            {
                total = total
                    .Skip((paging.PageNum - 1) * paging.NumPerPage)
                    .Take(paging.NumPerPage);
            }
            return total.ToList<T>();
        }
    }

    /// <summary>
    /// 分页对象
    /// </summary>
    public class PagingInfo
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PagingInfo()
        {
            TotalItems = 0;
            NumPerPage = 20;
            PageNum = 1;
            OrderField = string.Empty;
            OrderDirection = OrderDirections.asc;
            numPerPageArray = new int[] { 1, 20, 50, 100, 200 };
        }

        private int _TotalItems;
        /// <summary>
        /// 获取或设置总记录数
        /// </summary>
        public int TotalItems
        {
            get { return _TotalItems; }
            set
            {
                _TotalItems = value;

                if (PageNum > TotalPages)
                    PageNum = TotalPages;
            }
        }

        /// <summary>
        /// 获取或设置每页显示的记录数
        /// 默认20
        /// </summary>
        public int NumPerPage { get; set; }

        /// <summary>
        /// 获取或设置当前页数
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 获取总页数
        /// </summary>
        public int TotalPages
        {
            get
            {
                //return (int)Math.Ceiling((decimal)TotalItems / NumPerPage);
                //AM 2013 10 31 当每页显示记录数为0时不分页显示
                return NumPerPage == 0 ? 1 : (int)Math.Ceiling((decimal)TotalItems / NumPerPage);
            }
        }

        string _OrderField = string.Empty;
        /// <summary>
        /// 获取或设置排序字段
        /// </summary>
        public string OrderField
        {
            get { return _OrderField; }
            set
            {
                _OrderField = value == null ? string.Empty : value;
            }
        }

        /// <summary>
        /// 获取或设置排序方式
        /// </summary>
        public OrderDirections OrderDirection { get; set; }

        /// <summary>
        /// 获取或设置默认的每页可显示数据的列表
        /// </summary>
        public int[] numPerPageArray { get; set; }

        public virtual void ValidatePaging()
        {
            if (PageNum > TotalPages)
                PageNum = TotalPages;
            if (OrderField == null)
                OrderField = string.Empty;
        }
    }

    public enum OrderDirections
    {
        /// <summary>
        /// 升序
        /// </summary>
        asc,
        /// <summary>
        /// 降序
        /// </summary>
        desc
    }
}
