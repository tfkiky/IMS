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
            if (MainForm.Instance.IsDBConn)
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
                if (MainForm.Instance.IBlackMode == 0)
                {
                    cbBlackList.Checked = true;
                }
                else
                {
                    cbBlackList.Checked = false;
                }
                if (MainForm.Instance.ISwipeMode == 0)
                {
                    //rbAccessMode.Checked = true;
                }
                else
                {
                    //rbIDCardMode.Checked = true;
                }
            }
            catch (System.Exception ex)
            {
            	
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

                //if (rbAccessMode.Checked)
                //{
                //    SysConfigClass.SetIMSConfig("IMS_CONFIG", "SwipeMode", "0");
                //}
                //else
                //    SysConfigClass.SetIMSConfig("IMS_CONFIG", "SwipeMode", "1");

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

                SysConfigClass.SetIMSConfig("FACE_1_1", "Threshold", tbThreshold11.Text);
                SysConfigClass.SetIMSConfig("FACE_1_N", "Threshold", tbThreshold1N.Text);
                SysConfigClass.SetIMSConfig("FACE_1_LN", "Threshold", tbThreshold1ln.Text);
                SysConfigClass.SetIMSConfig("FACE_1_LN", "Threshold", tbThreshold1ln.Text);
                SysConfigClass.SetIMSConfig("FACE_1_LN", "STORELENGTH", tbStoreLength.Text);
                SysConfigClass.SetIMSConfig("FACE_1_LN", "DELETETICK", tbDeleteTick.Text);

                MainForm.Instance.LoadParams();
                MainForm.Instance.LoadSwipeMode();
                MessageBox.Show("保存成功");
            }
            catch (System.Exception ex)
            {
            	
            }
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            LoadMode();
        }

        private void btnSwipe1_Click(object sender, EventArgs e)
        {

        }

    }
}
