using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IMS.Common.Config;
using DevComponents.DotNetBar;
using IMS.Collecter;
using IMS.Common.Database;

namespace IMS
{
    public partial class MainForm : Office2007Form
    {
        #region 定义全局变量
        private int iFaceMode = 0;  //人脸识别验证模式 0- 1:1验证、1- 1：N验证、2- 1：n验证

        public int IFaceMode
        {
            get { return iFaceMode; }
            set { iFaceMode = value; }
        }
        private int iBlackMode = 0;    //黑名单模式 0-开启；1：关闭

        public int IBlackMode
        {
            get { return iBlackMode; }
            set { iBlackMode = value; }
        }
        private int iSwipeMode = 0;   //刷卡模式 0-门禁刷卡，1:-身份证刷卡

        public int ISwipeMode
        {
            get { return iSwipeMode; }
            set { iSwipeMode = value; }
        }
        private int iThreshold = 60;  //人脸识别阈值，默认60

        public int IThreshold
        {
            get { return iThreshold; }
            set { iThreshold = value; }
        }
        #endregion

        private static MainForm instance;

        public static MainForm Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        public MainForm()
        {
            InitializeComponent();
            Maticsoft.DBUtility.DbHelperSQL.connectionString = SysConfigClass.GetSqlServerConnectString();

            //DatabaseConfigClass configCls = new DatabaseConfigClass();
            //configCls.SaveConfig("SqlServerConnectString");

            StyleManager.Style = eStyle.Office2007Black;
            instance = this;
            FillDataGrid();
            LoadParams();
            FaceCollect.Start(iFaceMode, iSwipeMode, iThreshold, iBlackMode);
        }

        private void FillDataGrid()
        {
            dataGridViewX1.RowTemplate.Height = dataGridViewX1.Height / 3-2;
            dataGridViewX1.Rows.Add(3);
            dataGridViewX2.RowTemplate.Height = dataGridViewX1.Height / 3-2;
            dataGridViewX2.Rows.Add(3);
        }
        /// <summary>
        /// 加载参数
        /// </summary>
        private void LoadParams()
        {
            iFaceMode = int.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "FaceMode"));
            iBlackMode = int.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "IsBlackMode"));
            iSwipeMode = int.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "SwipeMode"));
        
            switch (iFaceMode)
            {
                case 0:
                    lbInspectMode.Text = "1:1模式";
                    iThreshold = int.Parse(SysConfigClass.GetIMSConfig("FACE_1_1", "Threshold"));
                    break;
                case 1:
                    lbInspectMode.Text = "1:N模式";
                    iThreshold = int.Parse(SysConfigClass.GetIMSConfig("FACE_1_N", "Threshold"));
                    break;
                case 2:
                    lbInspectMode.Text = "1:n模式";
                    iThreshold = int.Parse(SysConfigClass.GetIMSConfig("FACE_1_LN", "Threshold"));
                    break;
                default:
                    lbInspectMode.Text = "1:1模式";
                    iThreshold = int.Parse(SysConfigClass.GetIMSConfig("FACE_1_1", "Threshold"));
                    break;
            }
            switch (iBlackMode)
            {
                case 0:
                    lbBlackList.Text = "+黑名单";
                    break;
                case 1:
                    lbBlackList.Text = "";
                    break;
                default:
                    lbBlackList.Text = "";
                    break;
            }
            switch (iSwipeMode)
            {
                case 0:
                    lbSwipeMode.Text = "门禁刷卡";
                    break;
                case 1:
                    lbSwipeMode.Text = "身份证刷卡";
                    break;
                default:
                    lbSwipeMode.Text = "门禁刷卡";
                    break;
            }
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            CloseIMS();
        }

        private void CloseIMS()
        {
            if(ClientMainForm.Instance!=null)
            {
                ClientMainForm.Instance.CloseClient();
            }
            this.Close();
            FaceCollect.Stop();
        }

        private void tsmiSysConfig_Click(object sender, EventArgs e)
        {
            SysConfig sysCfg = new SysConfig();
            sysCfg.ShowDialog();
        }
    }
}
