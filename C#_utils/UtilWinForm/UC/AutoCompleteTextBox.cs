using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using Util.WinForm.ExtensionHelper;

namespace Util.WinForm.UC
{
    /// <summary>
    /// 自动完成文本框
    /// </summary>
    public class AutoCompleteTextBox : TextBox
    {
        ListBox listBox;
        /// <summary>
        /// 选择的对象
        /// </summary>
        [
          Category("自定义属性"),
          Description("自动完成时显示的结果列表")
        ]
        public object SelectedItem
        {
            get;
            set;
        }
        /// <summary>
        /// 自动完成文本框构造
        /// </summary>
        public AutoCompleteTextBox()
            : base()
        {
            Init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            this.listBox = new System.Windows.Forms.ListBox();
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(this.Width, 96);
            this.listBox.Visible = false;
            this.listBox.KeyDown += new KeyEventHandler(listBox_KeyDown);
            this.listBox.MouseDoubleClick += new MouseEventHandler(listBox_MouseDoubleClick);

            AutoControlListBoxSize barcode = new AutoControlListBoxSize(listBox);
            //BarcodeControl barcode = new BarcodeControl(listBox);
        }

        /// <summary>
        /// 自定义事件
        /// </summary>
        [
          Category("自定义事件"),
          Description("实现自动补全")
        ]
        public AutoCompleteHandler AutoCompleteHandle;
        /// <summary>
        /// OnKeyUp
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            Keys key = e.KeyCode;
            if (key == Keys.Enter)
            {
                if (listBox.Visible)
                {
                    //选择结果
                    Selected();
                }
            }
            else if (key == Keys.Up) { }
            else if (key == Keys.Down)
            {
                listBox.Focus();
                if (listBox.Items.Count > 1)
                {
                    listBox.SelectedIndex = 1;
                }
            }
            else if (key == Keys.Left) { }
            else if (key == Keys.Right) { }
            else if (key == Keys.Back && this.Text.Length == 0)
            {
                this.listBox.Visible = false;
            }
            else
            {
                AutoCompleteTextBox_AutoComplateHandle(this, e);
            }
        }


        /// <summary>
        /// /实现自动补全
        /// </summary>
        public event EventHandler AutoComplateHandle;
        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        /// <param name="displayMember"></param>
        /// <param name="valueMember"></param>
        public void SetDataSource<T>(IEnumerable<T> dataSource, string displayMember = "", string valueMember = "")
        {
            if (this.listBox.Parent == null)
            {
                this.FindForm().Controls.Add(listBox);
                this.listBox.BringToFront();
            }
            Point point = this.GetPointWithForm();
            this.listBox.Location = new Point(point.X, point.Y + this.Height);
            listBox.Binding(dataSource, displayMember, valueMember);
            listBox.Visible = listBox.Items.Count > 0;
        }


        Point GetPointWithForm(Control c, Point p)
        {
            Point result = new Point(p.X, p.Y);

            if (c == null || c is Form)
            {
            }
            else
            {
                result = new Point(p.X + c.Location.X, p.Y + c.Location.Y);
                result = GetPointWithForm(c.Parent, result);
            }
            return result;
        }

        /// <summary>
        /// 实现自动补全
        /// </summary>
        void AutoCompleteTextBox_AutoComplateHandle(object sender, EventArgs e)
        {
            listBox.DataSource = null;
            if (this.AutoComplateHandle != null)
            {
                this.AutoComplateHandle(this, e);
                if (AutoCompleteHandle != null)
                {
                    AutoCompleteHandle(this);
                }
            }
        }
        void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;
            if (key == Keys.Enter)
            {
                Selected();
            }
            else if (key == Keys.Up)
            {
                if (listBox.SelectedIndex == 0)
                {
                    this.Focus();
                }
            }
        }
        void listBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Selected();
        }
        void Selected()
        {
            SelectedItem = this.listBox.Visible ? this.listBox.SelectedItem : null;
            this.Text = listBox.SelectedValue == null ? string.Empty : listBox.SelectedValue.ToString();
            this.listBox.Visible = false;
            this.Focus();
        }

    }
    /// <summary>
    /// 自动完成句柄
    /// </summary>
    /// <param name="txt">文本框</param>
    public delegate void AutoCompleteHandler(TextBox txt);
    /// <summary>
    /// AutoControlListBoxSize
    /// </summary>
    public class AutoControlListBoxSize
    {
        #region private

        private const int MIN_SIZE = 100; //对控件缩放的最小值   
        private const int HANDLE_SIZE = 8;  //调整大小触模柄方框大小   
        private Control _moveControl = null;
        private Color HANDLE_COLOR = Color.White;
        private Label _lbl = new Label();
        private Cursor _cursor = Cursors.SizeNWSE;
        Size size;
        Point point;
        #endregion

        /// <summary>   
        /// 构造控件拖动对象   
        /// </summary>   
        /// <param name="moveControl">需要拖动的控件 </param>   
        public AutoControlListBoxSize(Control moveControl)
        {
            //   
            // TODO: 在此处添加构造函数逻辑   
            //   
            _moveControl = moveControl;
            _moveControl.MouseDown += new MouseEventHandler(this.Control_MouseDown);
            _moveControl.VisibleChanged += new EventHandler(_MControl_VisibleChanged);


            //构造触模柄   
            _lbl = new Label();
            _lbl.TabIndex = 0;
            _lbl.FlatStyle = 0;
            _lbl.BorderStyle = BorderStyle.FixedSingle;
            _lbl.Cursor = _cursor;
            _lbl.Text = "";
            _lbl.Size = new Size(HANDLE_SIZE, HANDLE_SIZE);
            _lbl.BackColor = HANDLE_COLOR;
            _lbl.MouseDown += new MouseEventHandler(this.handle_MouseDown);
            _lbl.MouseMove += new MouseEventHandler(this.handle_MouseMove);
            _lbl.MouseUp += new MouseEventHandler(_lbl_MouseUp);
        }
        void Control_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            size = _moveControl.Size;
            ShowHandles();
        }
        void handle_MouseDown(object sender, MouseEventArgs e)
        {
            point = Cursor.Position;
            _lbl.Visible = false;
        }
        //调整控件大小   
        Point temp;
        void handle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (temp.X != e.X || temp.Y != e.Y)
                {
                    temp = new Point(e.X, e.Y);
                    int w = (temp.X + size.Width) > MIN_SIZE ? (temp.X + size.Width) : MIN_SIZE;
                    int h = (temp.Y + size.Height) > MIN_SIZE ? (temp.Y + size.Height) : MIN_SIZE;
                    _moveControl.SetBounds(_moveControl.Left, _moveControl.Top, w, h);
                }
            }
        }
        void HideHandles()
        {
            _lbl.Visible = false;
            _moveControl.Parent.Controls.Remove(_lbl);
        }
        void ShowHandles()
        {
            if (_moveControl != null && _moveControl.Visible)
            {
                int sW = size.Width + _moveControl.Location.X - HANDLE_SIZE / 2;
                int sH = size.Height + _moveControl.Location.Y - HANDLE_SIZE / 2;
                _moveControl.Parent.Controls.Add(_lbl);
                _lbl.Visible = true;
                _lbl.SetBounds(sW, sH, HANDLE_SIZE, HANDLE_SIZE);
                _lbl.BringToFront();
            }
        }
        void _MControl_VisibleChanged(object sender, EventArgs e)
        {
            if ((!_moveControl.Visible) && _moveControl.Parent.Controls.Contains(_lbl))
            {
                HideHandles();
            }
        }
        void _lbl_MouseUp(object sender, MouseEventArgs e)
        {
            HideHandles();
        }
    }

}
