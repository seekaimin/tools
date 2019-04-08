using System.Windows.Forms;

namespace Util.WinForm.ExtensionHelper
{
    /// <summary>
    /// 提示信息
    /// </summary>
    public static class MessageBoxHelper
    {
        /// <summary>
        /// 提示文本信息
        /// MessageBox.Show()
        /// </summary>
        /// <param name="obj"></param>
        public static void Show(this string obj)
        {
            MessageBox.Show(obj);
        }
    }
}
