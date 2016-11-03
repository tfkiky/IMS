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
using log4net;
using IMS.MainCtrl;

namespace IMS
{
    public partial class MainForm : Office2007Form
    {
        #region 定义全局变量
        private ILog mlog = LogManager.GetLogger("MainForm");
        private Maticsoft.BLL.IMS_FACE_CAMERA faceCameraBll = new Maticsoft.BLL.IMS_FACE_CAMERA();
        private Maticsoft.Model.IMS_FACE_CAMERA faceCamera;
        private HikSDK.HikonComDevice hikCam = new HikSDK.HikonComDevice();

        public HikSDK.HikonComDevice HikCam
        {
            get { return hikCam; }
            set { hikCam = value; }
        }
        private string playHandle;

        public string PlayHandle
        {
            get { return playHandle; }
            set { playHandle = value; }
        }

        private FaceCollect faceCollect = new FaceCollect();
        private AccessCollect accessCollect = new AccessCollect();
        private IDCardCollect idCardCollect = new IDCardCollect();

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

        private int iBlackThreshold = 60;  //人脸识别黑名单阈值，默认60

        public int IBlackThreshold
        {
            get { return iBlackThreshold; }
            set { iBlackThreshold = value; }
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
            instance = this;
            StyleManager.Style = eStyle.Office2007Black;
            FillDataGrid();
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Maticsoft.DBUtility.DbHelperSQL.connectionString = SysConfigClass.GetSqlServerConnectString();
            if (!SysConfigClass.TestDBConn())
            {
                MessageBox.Show("数据库连接失败，请检查网络或数据库配置");
                this.Close();
            }
            else
            {
                LoadParams();
                LoadCamera();
                accessCollect.Start();
                accessCollect.AccessEvent += accessCollect_AccessEvent;
                idCardCollect.Start();
                idCardCollect.IDCardEvent += idCardCollect_IDCardEvent;
                faceCollect.Start(iFaceMode, iSwipeMode, iThreshold, iBlackMode);
                if (FaceCollect.IsFaceLoad)
                {
                    faceCollect.ValidateEvent += faceCollect_ValidateEvent;
                }
            }
        }

        void idCardCollect_IDCardEvent(object sender, IDCardEventArgs e)
        {
            if(e.IDCard!=null)
            {
                compareInfo1.LoadIDInfo(e.IDCard);
            }
        }

        void accessCollect_AccessEvent(object sender, AccessEventArgs e)
        {
            if (e.StaffInfo != null)
            {
                compareInfo1.LoadAccessInfo(e.StaffInfo,e.PassTime);
                peopleVehicleVideo1.LoadAccessResult(e.IsAllow);
            }
        }

        void faceCollect_ValidateEvent(object sender, ValidateResultEventArgs e)
        {
            if (e.CaptruePic != null)
            {
                compareInfo1.LoadValidateResult(e);
                peopleVehicleVideo1.LoadValidateResult(e.ValidateResult);
                if (e.ValidateResult==IMS.Collecter.ValidateResult.Success)
                {
                    AddNewPerson(e.StaffName,e.LocalPic);
                }
            }
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
            try
            {
                iFaceMode = int.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "FaceMode"));
                iBlackMode = int.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "IsBlackMode"));
                iSwipeMode = int.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "SwipeMode"));
                iBlackThreshold = int.Parse(SysConfigClass.GetIMSConfig("FACE_BLACK", "Threshold"));

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

                int faceCameraID = int.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "FaceCamera"));
                faceCamera = faceCameraBll.GetModelList("ID=" + faceCameraID)[0];
            }
            catch(Exception ex)
            {
                mlog.Error("LoadParams", ex);
            }
        }

        private void LoadCamera()
        {
            if (faceCamera != null && !string.IsNullOrEmpty(faceCamera.CameraIP))
            {
                hikCam.Login(faceCamera.CameraIP, int.Parse(faceCamera.CameraPort), faceCamera.CameraUser, faceCamera.CameraPwd);
                playHandle = hikCam.RealPlay("0", (int)peopleVehicleVideo1.VideoPanel.Handle, 1, 1, 2);
            }
        }

        private void CloseCamera()
        {
            if (!string.IsNullOrEmpty(playHandle))
            {
                hikCam.RealStop(playHandle);
                hikCam.Logout();
            }
        }

        private void AddNewPerson(string name ,string photo)
        {

        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            CloseIMS();
            CloseCamera();
        }

        private void CloseIMS()
        {
            if(ClientMainForm.Instance!=null)
            {
                ClientMainForm.Instance.CloseClient();
            }
            this.Close();
            faceCollect.Stop();
            accessCollect.Stop();
            idCardCollect.Stop();
        }

        private void tsmiSysConfig_Click(object sender, EventArgs e)
        {
            SysConfig sysCfg = new SysConfig();
            sysCfg.ShowDialog();
        }

        private void btnCheckPerson_Click(object sender, EventArgs e)
        {
            CheckPerson cp = new CheckPerson();
            cp.ShowDialog();
        }

        private void btnPersonRecord_Click(object sender, EventArgs e)
        {
            PersonRecord pr = new PersonRecord();
            pr.ShowDialog();
        }

       
    }
}
