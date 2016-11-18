using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace IMS.MainCtrl
{
    public partial class CheckPwd : Office2007Form
    {
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public CheckPwd()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            password = textBoxX1.Text;
            this.Close();
        }
    }
}
