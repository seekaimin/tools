using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppBuilderRegister
{
    public partial class FormBuilderMain : Form
    {
        public FormBuilderMain()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            FormBuilder f = new FormBuilder(txtappname.Text, txtopno.Text);
            f.ShowDialog();
        }
    }
}
