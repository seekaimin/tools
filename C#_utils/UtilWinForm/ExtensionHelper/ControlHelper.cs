using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;
using System.Drawing;

namespace Util.WinForm.ExtensionHelper
{
    /// <summary>
    /// ControlHelper
    /// </summary>
    public static class ControlHelper
    {

        /// <summary>
        /// 绑定DataGridViewComboBoxColumn
        /// </summary>
        /// <typeparam name="T1">Key的数据类型</typeparam>
        /// <typeparam name="T2">Value的数据类型</typeparam>
        /// <param name="lstc">下拉列表</param>
        /// <param name="resource">数据源</param>
        public static void Binding<T1, T2>(this DataGridViewComboBoxColumn lstc, IList<KeyValuePair<T1, T2>> resource)
        {
            SetMember(lstc, "Value", "Key");
            lstc.DataSource = resource;
        }
        /// <summary>
        /// 绑定DataGridViewComboBoxColumn
        /// </summary>
        /// <param name="listBox">DataGridViewComboBoxColumn</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="displayMember">显示属性名称</param>
        /// <param name="valueMember">值属性名称</param>
        public static void Binding<T>(this DataGridViewComboBoxColumn listBox,
            IEnumerable<T> dataSource,
            string displayMember = "Value",
            string valueMember = "Key")
        {
            SetMember(listBox, displayMember, valueMember);
            listBox.DataSource = dataSource;
        }

        /// <summary>
        /// 绑定ListControl
        /// </summary>
        /// <typeparam name="T1">Key的数据类型</typeparam>
        /// <typeparam name="T2">Value的数据类型</typeparam>
        /// <param name="lstc">下拉列表</param>
        /// <param name="resource">数据源</param>
        public static void Binding<T1, T2>(this ListControl lstc, IList<KeyValuePair<T1, T2>> resource)
        {
            SetMember(lstc, "Value", "Key");
            lstc.DataSource = resource;
        }
        /// <summary>
        /// 绑定ListControl
        /// </summary>
        /// <param name="listBox">ComBobox</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="displayMember">显示属性名称</param>
        /// <param name="valueMember">值属性名称</param>
        public static void Binding<T>(this ListControl listBox,
            IEnumerable<T> dataSource,
            string displayMember = "Value",
            string valueMember = "Key")
        {
            SetMember(listBox, displayMember, valueMember);
            listBox.DataSource = dataSource;
        }




        /// <summary>
        /// 控制文本控件只能输入0-9
        /// </summary>
        /// <param name="txt">TextBoxBase</param>
        public static void OnlyInputNumber(this TextBoxBase txt)
        {
            txt.KeyPress += new KeyPressEventHandler(txt_KeyPress);
        }
        static void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            string txt = ((TextBoxBase)sender).Text;
#if DEBUG
            Console.WriteLine(txt);
#endif
            char key = e.KeyChar;
            if (key == 8 || (key > 47 && key < 58))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        /**Invoke***/
        /// <summary>
        /// 异步设置ListControl.SelectedValue
        /// </summary>
        /// <param name="control">ListControl</param>
        /// <param name="value">值</param>
        public static void SetValue(this ListControl control, object value)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new ControlInvokeHandler(delegate()
                {
                    control.SelectedValue = value;
                }));
            }
            else
            {
                control.SelectedValue = value;
            }
        }
        /// <summary>
        /// 异步获取ListControl.SelectedValue
        /// </summary>
        /// <param name="control">ListControl</param>
        public static object GetValue(this ListControl control)
        {
            object obj = new object();
            if (control.InvokeRequired)
            {
                control.Invoke(new ControlInvokeHandler(delegate()
                {
                    obj = control.SelectedValue;
                }));
            }
            else
            {
                obj = control.SelectedValue;
            }
            return obj;
        }
        /// <summary>
        /// 异步设置控件的Text值
        /// </summary>
        /// <param name="control">需要设置的控件</param>
        /// <param name="value">值</param>
        public static void SetText(this Control control, string value)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new ControlInvokeHandler(delegate()
                {
                    control.Text = value;
                }));
            }
            else
            {
                control.Text = value;
            }
        }
        /// <summary>
        /// 异步设置控件的Text值
        /// </summary>
        /// <param name="control">需要设置的控件</param>
        /// <param name="formatstring">需要格式化的字符串</param>
        /// <param name="parameters">参数</param>
        public static void SetText(this Control control, string formatstring, params object[] parameters)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new ControlInvokeHandler(delegate()
                {
                    control.Text = string.Format(formatstring, parameters);
                }));
            }
            else
            {
                control.Text = string.Format(formatstring, parameters);
            }
        }
        /// <summary>
        /// 异步设置控件的Text值
        /// </summary>
        /// <param name="control">需要设置的控件</param>
        public static string GetText(this Control control)
        {
            string result = string.Empty;
            if (control.InvokeRequired)
            {
                control.Invoke(new ControlInvokeHandler(delegate()
                {
                    result = control.Text;
                }));
            }
            else
            {
                result = control.Text;
            }
            return result;
        }
        /// <summary>
        /// 设置ProgressBar   value
        /// </summary>
        /// <param name="control">ProgressBar</param>
        /// <param name="value">int value</param>
        public static void SetProgressBarValue(this ProgressBar control, int value)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new ControlInvokeHandler(delegate()
                {
                    control.Value = value;
                }));
            }
            else
            {
                control.Value = value;
            }
        }
        /// <summary>
        /// 获取ProgressBar   value
        /// </summary>
        /// <param name="control">ProgressBar</param>
        public static int GetProgressBarValue(this ProgressBar control)
        {
            int result = 0;
            if (control.InvokeRequired)
            {
                control.Invoke(new ControlInvokeHandler(delegate()
                {
                    result = control.Value;
                }));
            }
            else
            {
                result = control.Value;
            }
            return result;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <typeparam name="T">属性值类型</typeparam>
        /// <param name="control">控件</param>
        /// <param name="name">属性名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T GetPropertry<T>(this Control control, string name, T defaultValue = default(T))
        {
            T result = defaultValue;
            PropertyInfo property = control.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(name));
            if (property != null)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(new ControlInvokeHandler(delegate()
                    {
                        result = (T)property.GetValue(control, null);
                    }));
                }
                else
                {
                    result = (T)property.GetValue(control, null);
                }
            }
            return result;
        }
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <typeparam name="T">属性值类型</typeparam>
        /// <param name="control">控件</param>
        /// <param name="name">属性名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void SetPropertry<T>(this Control control, string name, T value)
        {
            PropertyInfo property = control.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(name));
            if (property != null)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(new ControlInvokeHandler(delegate()
                    {
                        if (property != null)
                        {
                            property.SetValue(control, value, null);
                        }
                    }));
                }
                else
                {
                    property.SetValue(control, value, null);
                }
            }
        }

        /// <summary>
        /// 异步调用控件的方法 带返回值
        /// </summary>
        /// <typeparam name="T">属性值类型</typeparam>
        /// <param name="control">控件</param>
        /// <param name="name">属性名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static T DoMehtod<T>(this Control control, string name, params object[] value)
        {
            T result = default(T);
            MethodInfo method = control.GetType().GetMethods().FirstOrDefault(p => p.Name.Equals(name));
            if (method != null)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(new ControlInvokeHandler(delegate()
                    {
                        if (method != null)
                        {
                            result = (T)method.Invoke(control, value);
                        }
                    }));
                }
                else
                {
                    result = (T)method.Invoke(control, value);
                }
            }
            return result;
        }
        /// <summary>
        /// 异步调用控件的方法
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="name">属性名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void DoMehtod(this Control control, string name, params object[] value)
        {
            MethodInfo method = control.GetType().GetMethods().FirstOrDefault(p => p.Name.Equals(name));
            if (method != null)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(new ControlInvokeHandler(delegate()
                    {
                        method.Invoke(control, value);
                    }));
                }
                else
                {
                    method.Invoke(control, value);
                }
            }
        }
        /// <summary>
        /// 处理事件 带异步判断
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="action">处理方法</param>
        public static void DoAction(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// WebBrowser交互  调用javascript
        /// </summary>
        /// <param name="webbrowser">交互对象</param>
        /// <param name="scriptname">js名称</param>
        /// <param name="args">参数</param>
        /// <returns>object</returns>
        public static object WebBrowserInvokeScript(this WebBrowser webbrowser, string scriptname, params object[] args)
        {
            if (webbrowser.InvokeRequired)
                return webbrowser.Invoke(new ReturnAction<object>(() =>
                {
                    return webbrowser.Document.InvokeScript(scriptname, args);
                }));
            else
                return webbrowser.Document.InvokeScript(scriptname, args);
        }

        /// <summary>
        /// 获取控件相对form的位置
        /// </summary>
        /// <param name="c">控件</param>
        /// <returns></returns>
        public static Point GetPointWithForm(this Control c)
        {
            Point result = new Point(c.Location.X, c.Location.Y);
            if (c == null || c is Form)
            {
                result = new Point(0, 0);
            }
            else
            {
                Point temp = c.Parent.GetPointWithForm();
                result = new Point(result.X + temp.X, result.Y + temp.Y);
            }
            return result;
        }
        /// <summary>
        /// 设置选中节点样式
        /// </summary>
        /// <param name="tv">树形菜单</param>
        public static void SetSelectedNodeStyle(this TreeView tv)
        {
            tv.DrawMode = TreeViewDrawMode.OwnerDrawText;
            tv.HideSelection = false;
            tv.DrawNode += new DrawTreeNodeEventHandler(delegate(object sender, DrawTreeNodeEventArgs e)
            {
                e.Graphics.FillRectangle(Brushes.White, new Rectangle(e.Node.Bounds.X, e.Node.Bounds.Y, e.Node.Bounds.Width + 10, e.Node.Bounds.Height));
                if (e.State == TreeNodeStates.Selected) //选中借点失去焦点
                {
                    e.Graphics.FillRectangle(Brushes.Blue,
                        new Rectangle(e.Node.Bounds.Left,
                            e.Node.Bounds.Top,
                            e.Node.Bounds.Width + 10,
                            e.Node.Bounds.Height)); //设置背景为黑色
                    e.Graphics.DrawString(e.Node.Text, tv.Font, Brushes.White, e.Bounds);//设置字为白色

                }
                else
                {
                    e.DrawDefault = true;
                }
            });
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="c">需要提示的控件</param>
        /// <param name="message">提示信息</param>
        public static void SetToolTip(this Control c, string message)
        {
            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(c, message);
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="c">需要提示的控件</param>
        /// <param name="formatmessage">需要格式化的提示信息</param>
        /// <param name="parameters">参数</param>
        public static void SetToolTip(this Control c, string formatmessage, params object[] parameters)
        {
            c.SetToolTip(string.Format(formatmessage, parameters));
        }





        /// <summary>
        /// 设置Memeber
        /// </summary>
        /// <param name="control">ListControl</param>
        /// <param name="displayMember">displayMember</param>
        /// <param name="valueMember">valueMember</param>
        static void SetMember(ListControl control, string displayMember, string valueMember)
        {
            control.DisplayMember = displayMember;
            control.ValueMember = valueMember;
        }
        /// <summary>
        /// 设置Memeber
        /// </summary>
        /// <param name="control">DataGridViewComboBoxColumn</param>
        /// <param name="displayMember">displayMember</param>
        /// <param name="valueMember">valueMember</param>
        static void SetMember(DataGridViewComboBoxColumn control, string displayMember, string valueMember)
        {
            control.DisplayMember = displayMember;
            control.ValueMember = valueMember;
        }
    }
    /// <summary>
    /// 控件的异步操作
    /// </summary>
    delegate void ControlInvokeHandler();
    /// <summary>
    /// 带返回值的委托
    /// </summary>
    /// <returns>返回值</returns>
    //public delegate object ReturnAction();
    /// <summary>
    /// 带返回值的委托 强类型
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <returns>返回值</returns>
    public delegate T ReturnAction<T>();
}
