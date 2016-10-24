using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using IMS.Collecter.Common;
using IMS.Common.Config;
using FaceDll;
using Li.Access.Core;
using Li.Access.Core.WGAccesses;
using SmartAccess.Common.Datas;
using log4net;

namespace IMS.Collecter
{
    public class FaceCollect
    {
        #region 全局变量定义
        private ILog mlog = LogManager.GetLogger("FaceCollect");
        private static Maticsoft.BLL.IMS_DATA_CONFIG imsConfigBll = new Maticsoft.BLL.IMS_DATA_CONFIG();
        private static Maticsoft.BLL.SMT_CONTROLLER_INFO ctrlBll = new Maticsoft.BLL.SMT_CONTROLLER_INFO();
        private static Maticsoft.BLL.SMT_STAFF_INFO staffInfoBll = new Maticsoft.BLL.SMT_STAFF_INFO();
        private static Maticsoft.BLL.IMS_FACE_BLACKLIST blackListBll = new Maticsoft.BLL.IMS_FACE_BLACKLIST();
        private static System.Threading.Timer timer;
        private static string staffFacePath, blackFacePath, tempFacePath;
       
        private static List<Maticsoft.Model.SMT_STAFF_INFO> staffList;
        private static List<Maticsoft.Model.IMS_FACE_BLACKLIST> blackList;
        private static bool isFaceLoad = false;

        public static bool IsFaceLoad
        {
            get { return FaceCollect.isFaceLoad; }
            set { FaceCollect.isFaceLoad = value; }
        }
        private static string currentFacePic;

        public static string CurrentFacePic
        {
            get { return FaceCollect.currentFacePic; }
            set { FaceCollect.currentFacePic = value; }
        }
        private static Dictionary<int,string> faceWhiteList;

        public static Dictionary<int, string> FaceWhiteList
        {
            get { return FaceCollect.faceWhiteList; }
            set { FaceCollect.faceWhiteList = value; }
        }
        private static List<string> faceBlackList;

        public static List<string> FaceBlackList
        {
            get { return FaceCollect.faceBlackList; }
            set { FaceCollect.faceBlackList = value; }
        }
        private static List<string> faceTempList;

        public static List<string> FaceTempList
        {
            get { return FaceCollect.faceTempList; }
            set { FaceCollect.faceTempList = value; }
        }
        #endregion

        public event EventHandler<ValidateResultEventArgs> ValidateEvent;

        /// <summary>
        /// 初始化本地人脸库路径
        /// </summary>
        public void InitFacePath()
        {
            faceWhiteList = new Dictionary<int, string>();
            faceBlackList = new List<string>();
            faceTempList = new List<string>();
            staffFacePath = SysConfigClass.GetIMSConfig("STAFF_FACE", "FilePath");
            blackFacePath = SysConfigClass.GetIMSConfig("BLACK_FACE", "FilePath");
            tempFacePath = SysConfigClass.GetIMSConfig("TEMP_FACE", "FilePath");
            if (!Directory.Exists(staffFacePath))
            {
                Directory.CreateDirectory(staffFacePath);
            }
            if (!Directory.Exists(blackFacePath))
            {
                Directory.CreateDirectory(blackFacePath);
            }
            if (!Directory.Exists(tempFacePath))
            {
                Directory.CreateDirectory(tempFacePath);
            }
        }

        /// <summary>
        /// 加载人脸仓库
        /// </summary>
        public void InitFaceStore()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (p, q) =>
            {
                InitFaceFiles();
            };
            worker.RunWorkerCompleted += (p, q) =>
            {
                InitFaceList();
                isFaceLoad = true;
            };
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// 加载数据库中人脸图片至本地缓存
        /// </summary>
        public void InitFaceFiles()
        {
            //加载人员库白名单
            staffList = staffInfoBll.GetModelList(" PHOTO is not null");
            if (staffList!=null&&staffList.Count>0)
            {
                foreach (Maticsoft.Model.SMT_STAFF_INFO staff in staffList)
                {
                    ImageHelper.ImageSave(staffFacePath + staff.ID + ".jpg", staff.PHOTO);
                }
            }
            //加载黑名单
            
        }

        /// <summary>
        ///  加载本地缓存路径至内存
        /// </summary>
        public void InitFaceList()
        {
            //加载人员库白名单
            staffList = staffInfoBll.GetModelList(" PHOTO is not null");
            if (staffList != null && staffList.Count > 0)
            {
                foreach (Maticsoft.Model.SMT_STAFF_INFO staff in staffList)
                {
                    faceWhiteList.Add((int)staff.ID, staffFacePath + staff.ID + ".jpg");
                }
            }
            //加载黑名单
            blackList = blackListBll.GetModelList(" FacePic is not null");
            if (blackList != null && blackList.Count > 0)
            {
                foreach (Maticsoft.Model.IMS_FACE_BLACKLIST black in blackList)
                {
                    faceBlackList.Add(black.FacePic);
                }
            }

        }
        
        public void Start(int faceMode, int swipeMode, int threshold,int isBlackList)
        {
            InitFacePath();
            InitFaceStore();
            timer = new System.Threading.Timer(new TimerCallback(FaceValidate), null, 1000, 1000);
        }

        public void Stop()
        {
            try
            {
                timer.Dispose();
            }
            catch (System.Exception ex)
            {
                mlog.Error("Stop Error", ex);
            }
        }
        /// <summary>
        /// 人脸验证
        /// </summary>
        /// <param name="state"></param>
        private void FaceValidate(object state)
        {
            string capturePic = GetCameraPic();
            string localPic=currentFacePic;
            byte[] feature1 = new byte[3000], feature2 = new byte[3000];
            int faceValue=0;
            if (isFaceLoad)
            {
                switch (MainForm.Instance.IFaceMode)
                {
                    case 0: ///1:1验证
                        if (!string.IsNullOrEmpty(localPic)&&!string.IsNullOrEmpty(capturePic))
                        {
                            int f1 = FaceService.face_get_feature_from_image(localPic, feature1);
                            int f2 = FaceService.face_get_feature_from_image(capturePic, feature2);
                            faceValue= FaceService.face_comp_feature(feature1, feature2);
                        }
                        break;
                    case 1: ///1：N验证
                        if (faceWhiteList!=null)
                        {
                            foreach(string pic in faceWhiteList.Values)
                            {
                                int f1 = FaceService.face_get_feature_from_image(pic, feature1);
                                int f2 = FaceService.face_get_feature_from_image(capturePic, feature2);
                                faceValue = FaceService.face_comp_feature(feature1, feature2);
                                if (faceValue > MainForm.Instance.IThreshold)
                                {
                                    localPic = pic;
                                    break;
                                }
                            }
                        }
                        break;
                    case 2:

                        break;
                }
                if (faceValue>MainForm.Instance.IThreshold)
                {
                    if (ValidateEvent!=null)
                    {
                        ValidateEvent(this, new ValidateResultEventArgs(capturePic, localPic, faceValue, ValidateResult.Success));
                        using (IAccessCore access = new WGAccess())
                        {
                            ///控制开门
                            Maticsoft.Model.SMT_CONTROLLER_INFO _ctrlr = ctrlBll.GetModel(AccessCollect.Instance.FaceControllerID);
                            Controller c = ControllerHelper.ToController(_ctrlr);
                            bool ret = access.OpenRemoteControllerDoor(c, AccessCollect.Instance.FaceDoorIndex);
                            if (!ret)
                            {
                                //WinInfoHelper.ShowInfoWindow(this, "上传门控制方式失败！");
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (ValidateEvent != null)
                    {
                        ValidateEvent(this, new ValidateResultEventArgs(capturePic, localPic, faceValue, ValidateResult.NoPerson));
                    }
                }
            }
        }

        private string GetCameraPic()
        {
            string picPath="";


            return picPath;
        }
    }
}
