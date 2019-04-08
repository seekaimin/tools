using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Util.WinForm.UC
{
    public partial class PCMBar : UserControl
    {
        private System.Windows.Forms.Timer timerSport;
        private System.Windows.Forms.Panel SportPanel;

        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private int _WarningValue = 70;
        private int _MaxValue = 100;
        private int _UnitWidth = 3;
        private int _UnitInterval = 1;
        private Orientation _Orientation = Orientation.Horizontal;
        private Color _VariablePanelColor = Color.Cyan;
        private Color _SportPanelColor = Color.Yellow;
        private Color _DefaultColor = Color.DarkGreen;
        private Color _WaringColor = Color.Orange;
        public int SportValue { get; set; }

        private int _SportPanelSide = 5;

        /// <summary>
        /// 滑块大小
        /// </summary>
        public int SportPanelSide
        {
            get { return _SportPanelSide; }
            set { _SportPanelSide = value; }
        }
        /// <summary>
        /// 获取当前下标的控件
        /// </summary>
        /// <param name="index">当前下标</param>
        /// <returns></returns>
        public Label this[int index]
        {
            get
            {
                if (this.Controls.Count == 0 || index < 0)
                    return null;
                int i = this.Controls.Count > index + 1 ? index : this.Controls.Count - 1;

                return this.Controls[i] as Label;
            }
        }
        int UnitSpan
        {
            get { return UnitWidth + UnitInterval; }
        }
        int UnitCount
        {
            get
            {
                if (UnitSpan == 0)
                    return 1;
                int count = Orientation == System.Windows.Forms.Orientation.Horizontal ? this.Width / UnitSpan : this.Height / UnitSpan;
                return count <= 0 ? 1 : count;
            }
        }

        /// <summary>
        /// 滑块背景颜色
        /// </summary>
        public Color VariablePanelColor
        {
            get { return _VariablePanelColor; }
            set { _VariablePanelColor = value; }
        }
        public Color SportPanelColor
        {
            get { return _SportPanelColor; }
            set { _SportPanelColor = value; }
        }
        /// <summary>
        /// 右边颜色
        /// </summary>
        public Color WaringColor
        {
            get { return _WaringColor; }
            set { _WaringColor = value; }
        }
        /// <summary>
        /// 左边颜色
        /// </summary>
        public Color DefaultColor
        {
            get { return _DefaultColor; }
            set { _DefaultColor = value; }
        }
        /// <summary>
        /// 刻度宽度
        /// </summary>
        public int UnitWidth
        {
            get { return _UnitWidth; }
            set { _UnitWidth = value; }
        }
        /// <summary>
        /// 刻度间隔
        /// </summary>
        public int UnitInterval
        {
            get { return _UnitInterval; }
            set { _UnitInterval = value; }
        }
        /// <summary>
        /// 警戒值
        /// </summary>
        public int WarningValue
        {
            get { return _WarningValue; }
            set { _WarningValue = value; }
        }
        /// <summary>
        /// 最大值
        /// </summary>
        public int MaxValue
        {
            get { return _MaxValue; }
            set
            {
                _MaxValue = value;
            }
        }
        /// <summary>
        /// 滑块时间间隔
        /// </summary>
        public int TimerInterval
        {
            get { return timerSport.Interval; }
            set { timerSport.Interval = value; }
        }
        /// <summary>
        /// 控件放置方向
        /// </summary>
        public Orientation Orientation
        {
            get { return _Orientation; }
            set { _Orientation = value; }
        }
        private void PCMBar_Load(object sender, EventArgs e)
        {
            //Load
            Init();
            timerSport.Start();
        }
        /// <summary>
        /// 构造
        /// </summary>
        public PCMBar()
            : base()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="orientation">控件放置方向</param>
        public PCMBar(Orientation orientation)
            : base()
        {
            InitializeComponent();
            Orientation = orientation;
        }
        void timerSport_Tick(object sender, EventArgs e)
        {
            SportValue -= MaxValue > UnitCount ? MaxValue / UnitCount : UnitCount / MaxValue;
            return;
            if (SportValue < -10) return;
            //获取当前运动下标
            int sportIndex = (SportValue * this.UnitCount) / MaxValue;
            Label sportlabel = this[sportIndex];
            if (sportlabel != null)
            {
                SportPanel.Location = new Point(sportlabel.Location.X, sportlabel.Location.Y);
                //sportlabel.BackColor = SportPanelColor;
            }
            SportValue -= MaxValue > UnitCount ? MaxValue / UnitCount : UnitCount / MaxValue;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            List<Rectangle> VariablePanelRectangles = new List<Rectangle>();
            List<Rectangle> LeftRectangles = new List<Rectangle>();
            List<Rectangle> RightRectangles = new List<Rectangle>();

            int valueitem = (Value * UnitCount) / MaxValue;
            int sportitem = (SportValue * this.Width) / MaxValue;
            int waringitem = (WarningValue * UnitCount) / MaxValue;

            for (int i = 0; i < UnitCount; i++)
            {
                Rectangle rect = new Rectangle(i * (UnitWidth + UnitSpan), 0, UnitWidth, this.Height - 2);
                if (i < valueitem)
                {
                    VariablePanelRectangles.Add(rect);
                }
                else if (i < waringitem)
                {
                    LeftRectangles.Add(rect);
                }
                else
                {
                    RightRectangles.Add(rect);
                }

            }
            if (VariablePanelRectangles.Count > 0)
                g.DrawRectangles(new Pen(VariablePanelColor), VariablePanelRectangles.ToArray());
            if (LeftRectangles.Count > 0)
                g.DrawRectangles(new Pen(DefaultColor), LeftRectangles.ToArray());
            if (RightRectangles.Count > 0)
                g.DrawRectangles(new Pen(WaringColor), RightRectangles.ToArray());
            g.DrawRectangle(new Pen(SportPanelColor), new Rectangle(sportitem, 0, 10, 10));

        }

        void Init()
        {

            SportPanel.Size = new Size(SportPanelSide, this.Height);
            SportPanel.Location = new Point(SportPanelSide * -1, 0);
            return;
            //this.Controls.Clear();
            if (Orientation == System.Windows.Forms.Orientation.Horizontal)
            {
                SportPanel.Size = new Size(SportPanelSide, this.Height);
                SportPanel.Location = new Point(SportPanelSide * -1, 0);
                //生成刻度
                for (int i = 0; i < UnitCount; i++)
                {
                    Label label = new Label()
                    {
                        Width = UnitWidth,
                        Height = this.Height,
                        Location = new Point(i * UnitSpan, 0),
                        Name = i.ToString(),
                    };
                    var v = (i * MaxValue) / UnitCount;
                    if (v < WarningValue)
                    {
                        label.BackColor = DefaultColor;
                    }
                    else
                    {
                        label.BackColor = WaringColor;
                    }
                    this.Controls.Add(label);
                }
            }
            else
            {
                SportPanel.Size = new Size(this.Width, SportPanelSide);
                SportPanel.Location = new Point(0, SportPanelSide * -1);
                //生成刻度
                for (int i = 0; i < UnitCount; i++)
                {
                    Label label = new Label()
                    {
                        Width = this.Height,
                        Height = UnitWidth,
                        Location = new Point(0, (UnitCount - 1 - i) * UnitSpan),
                        Name = i.ToString(),
                    };
                    var v = (i * MaxValue) / UnitCount;
                    if (v < WarningValue)
                    {
                        label.BackColor = DefaultColor;
                    }
                    else
                    {
                        label.BackColor = WaringColor;
                    }
                    this.Controls.Add(label);
                }
            }
        }
        private int _Value = 0;

        public int Value
        {
            get { return _Value; }
            set
            {
                if (value < 0)
                    _Value = 0;
                else
                    _Value = value;
                SetValue(value);
            }
        }
        public void SetValue(int value)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => { this.Refresh(); }));

            if (value > SportValue)
            {
                SportValue = value;
                timerSport_Tick(null, null);
            }

            if (Orientation == System.Windows.Forms.Orientation.Vertical)
            {
                int waringwidth = (WarningValue * this.Height) / MaxValue;
                int leftwidth = (value * this.Height) / MaxValue;
                foreach (Control label in this.Controls)
                {
                    if (label is Panel)
                    {
                        continue;
                    }
                    if ((this.Height - label.Location.Y) < leftwidth)
                    {
                        label.BackColor = VariablePanelColor;
                    }
                    else
                    {
                        if ((this.Height - label.Location.Y) < waringwidth)
                        {
                            label.BackColor = DefaultColor;
                        }
                        else
                        {
                            label.BackColor = WaringColor;
                        }
                    }
                }
            }
            else
            {
                int waringwidth = (WarningValue * this.Width) / MaxValue;
                int leftwidth = (value * this.Width) / MaxValue;
                foreach (Control label in this.Controls)
                {
                    if (label is Panel) continue;
                    if (label.Location.X < leftwidth)
                    {
                        label.BackColor = VariablePanelColor;
                    }
                    else
                    {
                        if (label.Location.X < waringwidth)
                        {
                            label.BackColor = DefaultColor;
                        }
                        else
                        {
                            label.BackColor = WaringColor;
                        }
                    }
                }
            }
        }


        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerSport = new System.Windows.Forms.Timer(this.components);
            this.SportPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // timerSport
            // 
            this.timerSport.Interval = 1000;
            this.timerSport.Tick += new System.EventHandler(this.timerSport_Tick);
            // 
            // SportPanel
            // 
            this.SportPanel.BackColor = System.Drawing.Color.Yellow;
            this.SportPanel.Location = new System.Drawing.Point(0, 0);
            this.SportPanel.Name = "SportPanel";
            this.SportPanel.Size = new System.Drawing.Size(5, 5);
            this.SportPanel.TabIndex = 0;
            // 
            // PCMBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SportPanel);
            this.Name = "PCMBar";
            this.Size = new System.Drawing.Size(669, 76);
            this.Load += new System.EventHandler(this.PCMBar_Load);
            this.ResumeLayout(false);

        }

        #endregion

    }
}
