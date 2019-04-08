using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util.WinForm;
using System.Xml.Linq;
using Util.Common;

namespace AppBuilderRegister
{
    public partial class FormBuilder : Form
    {
        public string AppName { get; private set; }
        public string OP_NO { get { return txtNo.Text.Trim(); } }
        public FormBuilder(string appname, string op_no)
        {
            InitializeComponent();
            AppName = appname;
            txtNo.Text = op_no;
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            XElement xelement = MyRegister.BuilderConfig(AppName, OP_NO);
            txtInfo.Text = xelement.ToString();
        }

        private void btnBuilder_Click(object sender, EventArgs e)
        {
            MyRegister.BuilderRegiste(AppName, OP_NO);
        }

        private void FormBuilder_Load(object sender, EventArgs e)
        {
            btnRead_Click(null, null);
        }
    }
}
