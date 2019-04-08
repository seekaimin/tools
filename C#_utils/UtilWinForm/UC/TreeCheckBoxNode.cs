using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Util.WinForm.UC
{
    /// <summary>
    /// 自定义CheckBox节点
    /// </summary>
    [ToolboxBitmap(typeof(TreeNode))]
    public class TreeCheckBoxNode : TreeNode
    {
        private CheckedStates _CheckedState = CheckedStates.Nornal;
        /// <summary>
        /// 节点三种状态 分别对应TreeView图片集合的下表  0-1-2
        /// </summary>
        [Category("外观")]
        [Description("节点三种状态")]
        public CheckedStates CheckedState
        {
            get { return _CheckedState; }
            set
            {
                _CheckedState = value;
                this.ImageIndex = this.SelectedImageIndex = (int)value;
            }
        }
        public TreeCheckBoxNode()
            : base()
        {

        }
    }
    /// <summary>
    /// 选择状态
    /// </summary>
    public enum CheckedStates
    {
        /// <summary>
        /// 正常状态
        /// </summary>
        Nornal = 0,
        /// <summary>
        /// 选中状态
        /// </summary>
        Checked = 1,
        /// <summary>
        /// 第三种状态
        /// </summary>
        ThreadState = 2,
    }
    /// <summary>
    /// 带复选框的树形节点扩展类
    /// </summary>
    public static class TreeCheckBoxNodeHelper
    {
        /// <summary>
        /// 初始化带复选框的树形菜单
        /// </summary>
        /// <param name="treeview">树形菜单</param>
        public static void InitTreeViewWithTreeCheckBoxNode(this TreeView treeview)
        {
            treeview.NodeMouseClick += new TreeNodeMouseClickEventHandler(delegate(object sender, TreeNodeMouseClickEventArgs e)
            {
                int calcx = e.Node.Bounds.X - e.X;
                int calcy = e.Y - e.Node.Bounds.Y;
                if (calcx > 4 && calcx < 19 && calcy > 0 && calcy < 15)
                {
                    //检测只能点击图片才能选择或取消选择
                    TreeCheckBoxNode node = (TreeCheckBoxNode)e.Node;
                    switch (node.CheckedState)
                    {
                        case CheckedStates.Nornal:
                        case CheckedStates.ThreadState:
                            node.CheckedState = CheckedStates.Checked;
                            break;
                        case CheckedStates.Checked:
                            node.CheckedState = CheckedStates.Nornal;
                            break;
                    }
                    e.Node.CheckTreeCheckBoxNodeState();
                    ((TreeView)sender).SelectedNode = e.Node;
                }
            });
        }
        /// <summary>
        /// 检查节点状态
        /// </summary>
        /// <param name="node">当前节点</param>
        public static void CheckTreeCheckBoxNodeState(this TreeNode node)
        {
            node.SetChildrenCheckedState();
            node.SetParentCheckedState();
        }
        /// <summary>
        /// 设置所有子节点状态和当前节点状态一致
        /// </summary>
        /// <param name="node">传入节点</param>
        public static void SetChildrenCheckedState(this TreeNode node)
        {
            TreeCheckBoxNode parent = node as TreeCheckBoxNode;
            if (parent == null) return;
            foreach (TreeNode item in node.Nodes)
            {
                TreeCheckBoxNode temp = item as TreeCheckBoxNode;
                if (temp != null)
                {
                    temp.CheckedState = parent.CheckedState;
                    temp.SetChildrenCheckedState();
                }
            }
        }
        /// <summary>
        /// 设置所有父节点状态
        /// </summary>
        /// <param name="node"></param>
        public static void SetParentCheckedState(this TreeNode node)
        {
            TreeCheckBoxNode CurrentNode = node as TreeCheckBoxNode;
            if (CurrentNode == null) return;
            if (node.Parent == null) return;
            TreeCheckBoxNode parent = node.Parent as TreeCheckBoxNode;

            if (CurrentNode.CheckedState == CheckedStates.Checked)
            {
                foreach (TreeNode i in parent.Nodes)
                {
                    TreeCheckBoxNode item = i as TreeCheckBoxNode;
                    if (item.CheckedState != CheckedStates.Checked)
                    {
                        parent.CheckedState = CheckedStates.ThreadState;
                        SetParentCheckedState(parent);
                        return;
                    }
                }
                parent.CheckedState = CheckedStates.Checked;
            }
            else if (CurrentNode.CheckedState == CheckedStates.Nornal)
            {
                foreach (TreeNode i in parent.Nodes)
                {
                    TreeCheckBoxNode item = i as TreeCheckBoxNode;
                    if (item.CheckedState == CheckedStates.Checked || item.CheckedState == CheckedStates.ThreadState)
                    {
                        parent.CheckedState = CheckedStates.ThreadState;
                        SetParentCheckedState(parent);
                        return;
                    }
                }
                parent.CheckedState = CheckedStates.Nornal;
            }
            else if (CurrentNode.CheckedState == CheckedStates.ThreadState)
            {
                foreach (TreeNode i in parent.Nodes)
                {
                    TreeCheckBoxNode item = i as TreeCheckBoxNode;
                    if (item.CheckedState == CheckedStates.Checked || item.CheckedState == CheckedStates.ThreadState)
                    {
                        parent.CheckedState = CheckedStates.ThreadState;
                        SetParentCheckedState(parent);
                        return;
                    }
                }
                parent.CheckedState = CheckedStates.ThreadState;
            }
            SetParentCheckedState(parent);
        }

        /// <summary>
        /// 添加带复选框的节点
        /// </summary>
        /// <param name="node">当前节点</param>
        /// <param name="text">显示文本</param>
        public static void AddTreeCheckBoxNode(this TreeNode node, string text)
        {
            TreeCheckBoxNode temp = new TreeCheckBoxNode()
            {
                Text = text,
            };
            node.Nodes.Add(temp);
        }
        /// <summary>
        /// 添加带复选框的节点
        /// </summary>
        /// <typeparam name="T">当前节点的数据类型</typeparam>
        /// <param name="node">当前节点</param>
        /// <param name="text">显示文本</param>
        /// <param name="data">当前节点绑定的数据 设置或获取Tag</param>
        public static void AddTreeCheckBoxNode<T>(this TreeNode node, string text, T data)
        {
            TreeCheckBoxNode temp = new TreeCheckBoxNode()
            {
                Text = text,
                Tag = data,
            };
            node.Nodes.Add(temp);
        }
    }
}
