using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using IMS.Common.Config;

namespace IMS.MainCtrl
{
    public partial class SwipeMode : Office2007Form
    {
        public SwipeMode()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked )
            {
                SysConfigClass.SetIMSConfig("IMS_CONFIG", "SwipeMode", "0");
            }
            else
                SysConfigClass.SetIMSConfig("IMS_CONFIG", "SwipeMode", "1");

            this.Close();
        }

        private void SwipeMode_Load(object sender, EventArgs e)
        {
            if( int.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "SwipeMode"))==0)
            {
                radioButton1.Checked = true;
            }
            else
                radioButton2.Checked = true;
        }
    }
}
