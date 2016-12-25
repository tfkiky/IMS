using DevComponents.DotNetBar;
using IMS.Collecter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IMS
{
    public partial class ClientMainForm : Office2007Form
    {
        private string playHandle;
        private static ClientMainForm instance;

        public static ClientMainForm Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        public ClientMainForm()
        {
            InitializeComponent();
            StyleManager.Style = eStyle.Office2007Black;
            instance = this;
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
           DialogResult dr = MessageBox.Show("确定退出系统？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
           if (dr == DialogResult.OK)
           {
               CloseClient();
           }
        }

        public void CloseClient()
        {
            timer1.Stop();
            CloseCamera();
            if (MainForm.Instance != null)
            {
                MainForm.Instance.CloseIMS();
            }
            else
            {
                this.Close();
            }
        }

        public void LoadCamera()
        {
            if (MainForm.Instance.FaceCamera != null && !string.IsNullOrEmpty(MainForm.Instance.FaceCamera.CameraIP))
            {
                MainForm.Instance.HikCam.Login(MainForm.Instance.FaceCamera.CameraIP, int.Parse(MainForm.Instance.FaceCamera.CameraPort), MainForm.Instance.FaceCamera.CameraUser, MainForm.Instance.FaceCamera.CameraPwd);
                playHandle = MainForm.Instance.HikCam.RealPlay("0", (int)splitContainer5.Panel1.Handle, 1, 1, 2);
            }
        }

        public void CloseCamera()
        {
            if (!string.IsNullOrEmpty(playHandle))
            {
                MainForm.Instance.HikCam.RealStop(playHandle);
                MainForm.Instance.HikCam.Logout();
            }
        }

        public void LoadValidateResult(ValidateResultEventArgs e)
        {
            //lbValue.Text = e.Record.Similarity.ToString();
            switch (e.ValidateResult)
            {
                case IMS.Collecter.ValidateResult.Success:
                    lbResult.Text = "验证通过";
                    break;
                case IMS.Collecter.ValidateResult.Black:
                    lbResult.Text = "黑名单人员，不予通过";
                    break;
                case IMS.Collecter.ValidateResult.NoPerson:
                    lbResult.Text = "查无此人，不予通过";
                    break;
                case IMS.Collecter.ValidateResult.Error:
                    lbResult.Text = "验证错误，请重试";
                    break;
            }
        }

        public void LoadResult(Maticsoft.Model.SMT_STAFF_INFO staffInfo, bool isAllow)
        {
            this.Invoke(new Action(() =>
            {
                if (staffInfo == null)
                {
                    lbResult.Text = "此卡无效，无此用户！";
                }
                else
                {
                    if (MainForm.Instance.IFaceMode == 3)
                    {
                        if (isAllow)
                        {
                            lbResult.Text = string.Format("{0}，请通行", staffInfo.REAL_NAME);
                        }
                        else
                        {
                            lbResult.Text = string.Format("{0}无权限,禁止通行！", staffInfo.REAL_NAME);
                        }
                    }
                    else
                    {
                        if (!isAllow)
                        {
                            lbResult.Text = string.Format("{0}无权限,禁止通行！", staffInfo.REAL_NAME);
                        }
                    }

                }

            }));
        }
        public void LoadIDCardInfo(IDCardClass idcard)
        {
            peopleIDCard1.LoadIDCard(idcard);
            //pbRealPhoto.Image = Properties.Resources.暂无图片;
        }

        public void LoadRealPic(string realPic)
        {
            pbRealPhoto.ImageLocation = realPic;
        }

        public void LoadBlackPic(string blackPic)
        {
            pbBlack.ImageLocation = blackPic;
        }

        public void LoadConnState(bool isConn)
        {
            //if(isConn)
            //{
            //    lbConn.Text = "数据库连接正常";
            //}
            //else{
            //    lbConn.Text = "数据库连接异常，请检查网络或数据库配置";
            //}
        }

        public void LoadCtrlState(bool isConn)
        {
            //if (isConn)
            //{
            //    lbConn.Text = "门禁控制器连接正常";
            //}
            //else
            //{
            //    lbConn.Text = "门禁控制器连接失败，请检查门禁控制器配置";
            //}
        }

        public void Clear()
        {
            peopleIDCard1.Clear();
            lbResult.Text = "";
            pbRealPhoto.Image = Properties.Resources.暂无图片;
            pbBlack.Image = Properties.Resources.暂无图片;
        }

        private void ClientMainForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
            LoadCamera();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
