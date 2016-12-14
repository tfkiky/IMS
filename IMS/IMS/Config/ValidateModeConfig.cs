using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IMS.Common.Config;
using IMS.MainCtrl;
using log4net;

namespace IMS.Config
{
    public partial class ValidateModeConfig : UserControl
    {
        private ILog mlog = LogManager.GetLogger("ValidateModeConfig");
        public ValidateModeConfig()
        {
            InitializeComponent();
            if (MainForm.Instance!=null&&MainForm.Instance.IsDBConn)
            {
                LoadMode();
            }
        }

        private void LoadMode()
        {
            try
            {
                if (MainForm.Instance.IFaceMode == 0)
                {
                    radioButton1.Checked = true;
                    btnSwipe1.Enabled = true;
                    btnSwipe2.Enabled = false;
                }
                else if (MainForm.Instance.IFaceMode == 1)
                {
                    radioButton2.Checked = true;
                    btnSwipe1.Enabled = false;
                    btnSwipe2.Enabled = false;
                }
                else if (MainForm.Instance.IFaceMode == 2)
                {
                    radioButton3.Checked = true;
                    btnSwipe2.Enabled = true;
                    btnSwipe1.Enabled = false;
                }
                else if (MainForm.Instance.IFaceMode == 3)
                {
                    radioButton4.Checked = true;
                    btnSwipe1.Enabled = false;
                    btnSwipe2.Enabled = false;
                }
                tbThreshold11.Text = SysConfigClass.GetIMSConfig("FACE_1_1", "Threshold");
                tbThreshold1N.Text = SysConfigClass.GetIMSConfig("FACE_1_N", "Threshold");
                tbThreshold1ln.Text = SysConfigClass.GetIMSConfig("FACE_1_LN", "Threshold");
                tbStoreLength.Text = SysConfigClass.GetIMSConfig("FACE_1_LN", "STORELENGTH");
                tbDeleteTick.Text = SysConfigClass.GetIMSConfig("FACE_1_LN", "DELETETICK");
                if (MainForm.Instance.IBlackMode == 0)
                {
                    cbBlackList.Checked = true;
                }
                else
                {
                    cbBlackList.Checked = false;
                }
            }
            catch (System.Exception ex)
            {
                mlog.Error(ex);
            }
           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbBlackList.Checked)
                {
                    SysConfigClass.SetIMSConfig("IMS_CONFIG", "IsBlackMode", "0");
                }
                else
                    SysConfigClass.SetIMSConfig("IMS_CONFIG", "IsBlackMode", "1");

                if (radioButton1.Checked)
                {
                    SysConfigClass.SetIMSConfig("IMS_CONFIG", "FaceMode", "0");
                }
                else if (radioButton2.Checked)
                {
                    SysConfigClass.SetIMSConfig("IMS_CONFIG", "FaceMode", "1");
                }
                else if (radioButton3.Checked)
                {
                    SysConfigClass.SetIMSConfig("IMS_CONFIG", "FaceMode", "2");
                }
                else if (radioButton4.Checked)
                {
                    SysConfigClass.SetIMSConfig("IMS_CONFIG", "FaceMode", "3");
                }


                SysConfigClass.SetIMSConfig("FACE_1_1", "Threshold", tbThreshold11.Text);
                SysConfigClass.SetIMSConfig("FACE_1_N", "Threshold", tbThreshold1N.Text);
                SysConfigClass.SetIMSConfig("FACE_1_LN", "Threshold", tbThreshold1ln.Text);
                SysConfigClass.SetIMSConfig("FACE_1_LN", "Threshold", tbThreshold1ln.Text);
                SysConfigClass.SetIMSConfig("FACE_1_LN", "STORELENGTH", tbStoreLength.Text);
                SysConfigClass.SetIMSConfig("FACE_1_LN", "DELETETICK", tbDeleteTick.Text);

                MainForm.Instance.LoadParams();
                
                MessageBox.Show("保存成功");
            }
            catch (System.Exception ex)
            {
                mlog.Error(ex);
            }
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            LoadMode();
        }

        private void btnSwipe1_Click(object sender, EventArgs e)
        {
            SwipeMode swipe = new SwipeMode();
            swipe.ShowDialog();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                btnSwipe1.Enabled = true;
                btnSwipe2.Enabled = false;
            }
            if (radioButton2.Checked)
            {
                btnSwipe1.Enabled = false;
                btnSwipe2.Enabled = false;
            }
            if (radioButton3.Checked)
            {
                btnSwipe2.Enabled = true;
                btnSwipe1.Enabled = false;
            }
            if (radioButton4.Checked)
            {
                btnSwipe1.Enabled = false;
                btnSwipe2.Enabled = false;
            }
        }

    }
}
