using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Util.WinForm.UC
{
    public class UCTextBox : TextBox
    {
        public UCTextBox()
            : base()
        {

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}
