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
        private bool bIsBlackMode = false;    //是否开启黑名单模式
        private int iSwipeMode = 0;   //刷卡模式 0-门禁刷卡，1:-身份证刷卡
        private int iThreshold = 60;  //人脸识别阈值，默认60
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
            LoadHeader();
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
            Maticsoft.BLL.IMS_DATA_CONFIG imsConfigBll=new Maticsoft.BLL.IMS_DATA_CONFIG();
            List<Maticsoft.Model.IMS_DATA_CONFIG> imsConfigModelList = new List<Maticsoft.Model.IMS_DATA_CONFIG>();
            //加载人脸识别验证模式
            imsConfigModelList = imsConfigBll.GetModelList("DataType='IMS_CONFIG' AND DataKey='FaceMode'");
            if (imsConfigModelList != null && imsConfigModelList.Count>0)
            {
                iFaceMode = int.Parse(imsConfigModelList[0].DataValue);
            }
            //加载是否开始黑名单
            imsConfigModelList = imsConfigBll.GetModelList("DataType='IMS_CONFIG' AND DataKey='IsBlackMode'");
            if (imsConfigModelList != null && imsConfigModelList.Count > 0)
            {
                bIsBlackMode = bool.Parse(imsConfigModelList[0].DataValue);
            }
            //加载刷卡验证模式
            imsConfigModelList = imsConfigBll.GetModelList("DataType='IMS_CONFIG' AND DataKey='SwipeMode'");
            if (imsConfigModelList != null && imsConfigModelList.Count > 0)
            {
                iSwipeMode = int.Parse(imsConfigModelList[0].DataValue);
            }
            //加载人脸验证阈值
            imsConfigModelList = imsConfigBll.GetModelList("DataType='IMS_CONFIG' AND DataKey='Threshold'");
            if (imsConfigModelList != null && imsConfigModelList.Count > 0)
            {
                iThreshold = int.Parse(imsConfigModelList[0].DataValue);
            }
            FaceCollect.Start(iFaceMode,iSwipeMode,iThreshold,bIsBlackMode);
        }

        private void LoadHeader()
        {
            switch (iFaceMode)
            {
                case 0:
                    lbInspectMode.Text = "1:1模式";
                    break;
                case 1:
                    lbInspectMode.Text = "1:N模式";
                    break;
                case 2:
                    lbInspectMode.Text = "1:n模式";
                    break;
                default:
                    lbInspectMode.Text = "1:1模式";
                    break;
            }
            switch (bIsBlackMode)
            {
                case true:
                    lbBlackList.Text = "+黑名单";
                    break;
                case false:
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
