using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IMS.Common.Config;

namespace IMS.Config
{
    public partial class ValidateModeConfig : UserControl
    {

        public ValidateModeConfig()
        {
            InitializeComponent();
            LoadMode();
        }

        private void LoadMode()
        {
            if (MainForm.Instance.IFaceMode==0)
            {
                radioButton1.Checked = true;
            }
            else if (MainForm.Instance.IFaceMode == 1)
            {
                radioButton2.Checked = true;
            }
            else if (MainForm.Instance.IFaceMode == 2)
            {
                radioButton3.Checked = true;
            }
            tbThreshold11.Text = SysConfigClass.GetIMSConfig("FACE_1_1", "Threshold");
            tbThreshold1N.Text = SysConfigClass.GetIMSConfig("FACE_1_N", "Threshold");
            tbThreshold1ln.Text = SysConfigClass.GetIMSConfig("FACE_1_LN", "Threshold");
            tbStoreLength.Text = SysConfigClass.GetIMSConfig("FACE_1_LN", "STORELENGTH");
            tbDeleteTick.Text = SysConfigClass.GetIMSConfig("FACE_1_LN", "DELETETICK");
            if (MainForm.Instance.IBlackMode==0)
            {
                cbBlackList.Checked = true;
            } 
            else
            {
                cbBlackList.Checked = false;
            }
            if (MainForm.Instance.ISwipeMode==0)
            {
                rbAccessMode.Checked = true;
            } 
            else
            {
                rbIDCardMode.Checked = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            LoadMode();
        }

    }
}
