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
using DevComponents.DotNetBar.Controls;

namespace IMS
{
    public partial class MainForm : Office2007Form
    {
        #region 定义全局变量
        private ILog mlog = LogManager.GetLogger("MainForm");

        private bool isDBConn = true;

        public bool IsDBConn
        {
            get { return isDBConn; }
            set { isDBConn = value; }
        }
        private Maticsoft.BLL.IMS_FACE_CAMERA faceCameraBll = new Maticsoft.BLL.IMS_FACE_CAMERA();
        private Maticsoft.Model.IMS_FACE_CAMERA faceCamera;

        public Maticsoft.Model.IMS_FACE_CAMERA FaceCamera
        {
            get { return faceCamera; }
            set { faceCamera = value; }
        }
        private List<Maticsoft.Model.SMT_STAFF_INFO> staffList = new List<Maticsoft.Model.SMT_STAFF_INFO>();
        private List<Maticsoft.Model.IMS_VEHICLE_RECORD> vehicleList = new List<Maticsoft.Model.IMS_VEHICLE_RECORD>();

        private List<Maticsoft.Model.IMS_PEOPLE_RECORD> recordList = new List<Maticsoft.Model.IMS_PEOPLE_RECORD>();

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

        private int iFaceMode = 0;
        //人脸识别验证模式 0- 1:1验证、1- 1：N验证、2- 1：n验证
        public int IFaceMode
        {
            get { return iFaceMode; }
            set { iFaceMode = value; }
        }
        private int iBlackMode = 0;
        //黑名单模式 0-开启；1：关闭
        public int IBlackMode
        {
            get { return iBlackMode; }
            set { iBlackMode = value; }
        }
        private int iSwipeMode = 0;

        //刷卡模式 0-门禁刷卡，1:-身份证刷卡
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
                ClientMainForm.Instance.LoadConnState(false);
                MessageBox.Show("数据库连接失败，请检查网络或数据库配置");
                isDBConn = false;
                SysConfig config = new SysConfig();
                config.ShowDialog();
            }
            else if (!SysConfigClass.TestController())
            {
                ClientMainForm.Instance.LoadCtrlState(false);
                MessageBox.Show("门禁控制器连接失败，请检查门禁控制器配置");
                SysConfig config = new SysConfig();
                config.ShowDialog();
            }
            else
            {
                ClientMainForm.Instance.LoadConnState(true);
                LoadParams();
                LoadCamera();
                LoadSwipeMode();
                accessCollect.Start();
                idCardCollect.Start();
                faceCollect.Start(iFaceMode, iSwipeMode, iThreshold, iBlackMode);
                //if (FaceCollect.IsFaceLoad)
                //{
                    faceCollect.ValidateEvent += faceCollect_ValidateEvent;
                //}
            }
        }

        public void LoadSwipeMode()
        {
            try
            {
                if (iSwipeMode == 0)
                {
                    idCardCollect.IDCardEvent -= idCardCollect_IDCardEvent;
                    accessCollect.AccessEvent += accessCollect_AccessEvent;
                }
                else
                {

                    accessCollect.AccessEvent -= accessCollect_AccessEvent;
                    idCardCollect.IDCardEvent += idCardCollect_IDCardEvent;
                }
            }
            catch { }
        }

        void idCardCollect_IDCardEvent(object sender, IDCardEventArgs e)
        {
            if(e.IDCard!=null)
            {
                compareInfo1.LoadIDInfo(e.IDCard);
                e.IDCard.PhotoFile = FaceCollect.FaceWhiteList[(int)e.StaffInfo.ID];
                ClientMainForm.Instance.LoadIDCardInfo(e.IDCard);
                peopleVehicleVideo1.LoadIDCardResult(e.StaffInfo, e.IsAllow);
            }
        }

        void accessCollect_AccessEvent(object sender, AccessEventArgs e)
        {
            if (e.StaffInfo != null)
            {
                compareInfo1.LoadAccessInfo(e.StaffInfo, e.CardRecord);
                IDCardClass idcard = new IDCardClass();
                idcard.Name = e.StaffInfo.REAL_NAME;
                switch (e.StaffInfo.SEX)
                {
                    case 0:
                        idcard.Sex = "未知";
                        break;
                    case 1:
                        idcard.Sex = "男";
                        break;
                    case 2:
                        idcard.Sex = "女";
                        break;
                    default:
                        idcard.Sex = "未知";
                        break;
                }

                idcard.Id = e.StaffInfo.CER_NO;
                idcard.Birth = e.StaffInfo.BIRTHDAY.Value.ToString("yyyy-MM-dd HH:mm:ss");
                idcard.Address = e.StaffInfo.ADDRESS;
                idcard.Nation = e.StaffInfo.NATION;
                idcard.PhotoFile = FaceCollect.FaceWhiteList[(int)e.StaffInfo.ID];

                ClientMainForm.Instance.LoadIDCardInfo(idcard);
                peopleVehicleVideo1.LoadAccessResult(e.StaffInfo, e.CardRecord);
            }
        }

        void faceCollect_ValidateEvent(object sender, ValidateResultEventArgs e)
        {
            try
            {
                if (e.Record.CapturePic != null)
                {
                    mlog.InfoFormat("抓拍图片：{0}", e.Record.CapturePic);
                    compareInfo1.LoadValidateResult(e);
                    ClientMainForm.Instance.LoadValidateResult(e);

                    peopleVehicleVideo1.LoadValidateResult(e.ValidateResult);
                    if (e.ValidateResult == IMS.Collecter.ValidateResult.Success)
                    {
                        AddNewPerson(e.Record);
                    }
                }
            }
            catch (Exception ex)
            {
                mlog.Error(ex);
            }
        }

        private void FillDataGrid()
        {
            dataGridViewX1.RowTemplate.Height = dataGridViewX1.Height / 3-2;
            dataGridViewX1.Rows.Add(3);
            dataGridViewX2.RowTemplate.Height = dataGridViewX1.Height / 3-2;
            dataGridViewX2.Rows.Add(3);
            column1.BeforeCellPaint += column_BeforeCellPaint;
            column2.BeforeCellPaint += column_BeforeCellPaint;
            column3.BeforeCellPaint += column_BeforeCellPaint;
            column4.BeforeCellPaint += column_BeforeCellPaint;
        }

        void column_BeforeCellPaint(object sender, BeforeCellPaintEventArgs e)
        {
            DataGridViewButtonXColumn bcx = sender as DataGridViewButtonXColumn;
            DataGridViewButtonCell cell = dataGridViewX2.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
            if (bcx != null)
            {
                if (cell.Tag is Maticsoft.Model.IMS_PEOPLE_RECORD)
                {
                    Maticsoft.Model.IMS_PEOPLE_RECORD record = cell.Tag as Maticsoft.Model.IMS_PEOPLE_RECORD;
                    Bitmap bmp = new Bitmap(recordList.Find(rec => rec.ID == record.ID).OriginPic);
                    bcx.Image = new Bitmap(bmp, 48, 48);
                    mlog.InfoFormat("加载图片：{0},行{1},列{2}", record.Name, e.RowIndex, e.ColumnIndex);
                    bmp.Dispose();
                }
                else
                    bcx.Image = null;
            }
        }
        /// <summary>
        /// 加载参数
        /// </summary>
        public void LoadParams()
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

        private void AddNewPerson(Maticsoft.Model.IMS_PEOPLE_RECORD record)
        {
            try
            {
                lock (recordList)
                {
                    if (recordList.Count > 11)
                    {
                        recordList.RemoveAt(0);
                    }
                    //if (recordList.Count==0||recordList.Last().ID != record.ID)
                    //{
                        recordList.Add(record);
                        mlog.InfoFormat("新进记录：{0}", record.Name);
                    //}
                    LoadPerson();

                }
            }
            catch(Exception ex){
                mlog.Error(ex);
            }
        }

        private void LoadPerson()
        {
            try
            {

                for (int i = 0; i < dataGridViewX2.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridViewX2.ColumnCount; j++)
                    {
                        DataGridViewButtonCell cell = dataGridViewX2.Rows[i].Cells[j] as DataGridViewButtonCell;

                        if (i * dataGridViewX2.ColumnCount + j < recordList.Count)
                        {
                            cell.Value = recordList[i * dataGridViewX2.ColumnCount + j].Name;
                            cell.Tag = recordList[i * dataGridViewX2.ColumnCount + j];
                            mlog.InfoFormat("加载记录：{0},行{1},列{2}", cell.Value,i,j);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                mlog.Error(ex);
            }
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
            if (CheckPwd())
            {
                CheckPerson cp = new CheckPerson();
                cp.ShowDialog();
            }
        }

        private void btnPersonRecord_Click(object sender, EventArgs e)
        {
            if (CheckPwd())
            {
                PersonRecord pr = new PersonRecord();
                pr.ShowDialog();
            }
        }

        private void dataGridViewX2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewButtonCell cell = dataGridViewX2.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
            if (cell.Tag is Maticsoft.Model.IMS_PEOPLE_RECORD)
            {
                Maticsoft.Model.IMS_PEOPLE_RECORD record = cell.Tag as Maticsoft.Model.IMS_PEOPLE_RECORD;
                FaceRecord fr = new FaceRecord(record);
                fr.ShowDialog();
            }
        }

        private bool CheckPwd()
        {
            bool bRet = false;
            string pwd = SysConfigClass.GetIMSConfig("IMS_CONFIG", "Password");
            CheckPwd check = new CheckPwd();
            check.ShowDialog();
            if (!string.IsNullOrEmpty(check.Password))
            {
                if (pwd==check.Password)
                {
                    bRet= true;
                }
                else
                    bRet = false;
            }
            else
                bRet = false;

            return bRet;
        }

       
    }
}
