using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Common
{
    /// <summary>
    /// 无参数 无法回类型委托
    /// </summary>
    public delegate void ActionHandler();
    /// <summary>
    /// 带参数(单个) 无法回类型委托
    /// </summary>
    /// <typeparam name="T">输入参数类型</typeparam>
    /// <param name="parameter">输入参数</param>
    public delegate void ActionWithParameterHandler<T>(T parameter);
    /// <summary>
    /// 带参数(多个) 无法回类型委托
    /// </summary>
    /// <typeparam name="T">输入参数类型</typeparam>
    /// <param name="parameters">输入参数</param>
    public delegate void ActionWithParametersHandler<T>(params T[] parameters);


    /// <summary>
    /// 无参数 带回类型委托
    /// </summary>
    /// <typeparam name="T">返回参数类型</typeparam>
    /// <returns></returns>
    public delegate T ReturnActionHandler<T>();
    /// <summary>
    /// 带参数(单个) 带回类型委托
    /// </summary>
    /// <typeparam name="T1">输入参数类型</typeparam>
    /// <typeparam name="T">返回参数类型</typeparam>
    /// <param name="parameter">输入参数</param>
    /// <returns></returns>
    public delegate T ReturnActionWithParameterHandler<T1, T>(T1 parameter);
    /// <summary>
    /// 带参数(多个) 带回类型委托
    /// </summary>
    /// <typeparam name="T1">输入参数类型</typeparam>
    /// <typeparam name="T">返回参数类型</typeparam>
    /// <param name="parameters">输入参数</param>
    /// <returns></returns>
    public delegate T ReturnActionWithParametersHandler<T1, T>(params T1[] parameters);
}
