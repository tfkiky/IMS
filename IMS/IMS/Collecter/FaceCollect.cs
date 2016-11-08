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
        private static Maticsoft.BLL.SMT_DOOR_INFO doorBll = new Maticsoft.BLL.SMT_DOOR_INFO();
        private static Maticsoft.BLL.SMT_STAFF_INFO staffInfoBll = new Maticsoft.BLL.SMT_STAFF_INFO();
        private static Maticsoft.BLL.IMS_FACE_BLACKLIST blackListBll = new Maticsoft.BLL.IMS_FACE_BLACKLIST();
        private static Maticsoft.BLL.IMS_PEOPLE_RECORD recordBll = new Maticsoft.BLL.IMS_PEOPLE_RECORD();
        private static System.Threading.Timer timer;
        private static string staffFacePath, blackFacePath, tempFacePath;

        public static string StaffFacePath
        {
            get { return FaceCollect.staffFacePath; }
            set { FaceCollect.staffFacePath = value; }
        }

        private static Maticsoft.Model.SMT_STAFF_INFO staffInfo;

        public static Maticsoft.Model.SMT_STAFF_INFO StaffInfo
        {
            get { return FaceCollect.staffInfo; }
            set { FaceCollect.staffInfo = value; }
        }

        private static Maticsoft.Model.SMT_CARD_RECORDS cardRecord;

        public static Maticsoft.Model.SMT_CARD_RECORDS CardRecord
        {
            get { return FaceCollect.cardRecord; }
            set { FaceCollect.cardRecord = value; }
        }

        private static IDCardClass idCard;

        public static IDCardClass IdCard
        {
            get { return FaceCollect.idCard; }
            set { FaceCollect.idCard = value; }
        }

        private static int cardType;

        public static int CardType
        {
            get { return cardType; }
            set { cardType = value; }
        }

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
                mlog.InfoFormat("下载人员库白名单图片:人数{0}", staffList.Count);
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
                mlog.InfoFormat("加载人员库白名单:人数{0}", staffList.Count);
          
            }
            //加载黑名单
            blackList = blackListBll.GetModelList(" FacePic is not null");
            if (blackList != null && blackList.Count > 0)
            {
                foreach (Maticsoft.Model.IMS_FACE_BLACKLIST black in blackList)
                {
                    faceBlackList.Add(black.FacePic);
                }
                mlog.InfoFormat("加载黑名单:人数{0}", blackList.Count);
            }

        }
        
        public void Start(int faceMode, int swipeMode, int threshold,int isBlackList)
        {
            int iret=FaceService.face_init();
            if (iret == 0)
            {
                mlog.Info("人脸识别算法库初始化成功");
                InitFacePath();
                InitFaceStore();
                timer = new System.Threading.Timer(new TimerCallback(FaceValidate), null, 1000, 2000);
            }
            else
            {
                mlog.Info("人脸识别算法库初始化失败");
            }
        }

        public void Stop()
        {
            try
            {
                timer.Dispose();
                FaceService.face_exit();
            }
            catch (System.Exception ex)
            {
                mlog.Error("Stop Error", ex);
            }
        }
        int faceValue = 0;

        /// <summary>
        /// 人脸验证
        /// </summary>
        /// <param name="state"></param>
        private void FaceValidate(object state)
        {
            try
            {
                if (isFaceLoad&&!string.IsNullOrEmpty(currentFacePic))
                {
                    string capturePic = GetCameraPic();
                    string localPic = currentFacePic;
                    string blackPic = "";
                    byte[] feature1 = new byte[3000], feature2 = new byte[3000];
                    Maticsoft.Model.IMS_PEOPLE_RECORD record = new Maticsoft.Model.IMS_PEOPLE_RECORD();
                    record.Name = staffInfo.REAL_NAME;
                    if (MainForm.Instance.IBlackMode == 0)
                    {
                        if (faceBlackList != null && !string.IsNullOrEmpty(capturePic))
                        {
                            foreach (string pic in faceBlackList)
                            {
                                int f1 = FaceService.face_get_feature_from_image(pic, feature1);
                                int f2 = FaceService.face_get_feature_from_image(capturePic, feature2);
                                faceValue = FaceService.face_comp_feature(feature1, feature2);
                                if (faceValue > MainForm.Instance.IBlackThreshold)
                                {
                                    blackPic = pic;
                                    break;
                                }
                            }
                            if (!string.IsNullOrEmpty(blackPic))
                            {
                                mlog.InfoFormat("黑名单验证结果：{0}！", blackPic);

                                //Maticsoft.Model.IMS_PEOPLE_RECORD record = new Maticsoft.Model.IMS_PEOPLE_RECORD();
                                //record.Name = staffInfo.REAL_NAME;
                                record.Similarity = faceValue;
                                //record.ThroughResult = 2;
                                //record.OriginPic = "";
                                ////record.OriginPic = blackPic;
                                record.CapturePic = capturePic;
                                //record.CompareResult = 1;
                                //record.CardType = -1;
                                //record.Depart = staffInfo.ORG_ID.ToString();
                                //record.ThroughForward = -1;
                                //record.ThroughTime = DateTime.Now;
                                //record.CardNo = "";
                                //record.AccessChannel = AccessCollect.Instance.FaceControllerID.ToString();
                                //record.FacePosition = "";
                                //recordBll.Add(record);

                                if (ValidateEvent != null)
                                {
                                    ValidateEvent(this, new ValidateResultEventArgs(record, blackPic, ValidateResult.Black));
                                }
                            }
                        }
                    }

                    switch (MainForm.Instance.IFaceMode)
                    {
                        case 0: ///1:1验证
                            if (!string.IsNullOrEmpty(localPic) && !string.IsNullOrEmpty(capturePic))
                            {
                                int f1 = FaceService.face_get_feature_from_image(localPic, feature1);
                                int f2 = FaceService.face_get_feature_from_image(capturePic, feature2);
                                faceValue = FaceService.face_comp_feature(feature1, feature2);
                                mlog.InfoFormat("1:1验证得分：{0},验证目标{1},{2}", faceValue, staffInfo.REAL_NAME, localPic);
                            }
                            break;
                        case 1: ///1：N验证
                            if (faceWhiteList != null)
                            {
                                localPic = "";
                                foreach (string pic in faceWhiteList.Values)
                                {
                                    int f1 = FaceService.face_get_feature_from_image(pic, feature1);
                                    int f2 = FaceService.face_get_feature_from_image(capturePic, feature2);
                                    faceValue = FaceService.face_comp_feature(feature1, feature2);
                                    mlog.InfoFormat("1:N验证得分：{0}", faceValue);
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
                    if (faceValue > MainForm.Instance.IThreshold && !string.IsNullOrEmpty(localPic))
                    {
                        if (ValidateEvent != null)
                        {
                            
                            try
                            {
                                record.Similarity = faceValue;
                                record.ThroughResult = 1;
                                record.OriginPic = localPic;
                                record.CapturePic = capturePic;
                                record.CompareResult = 1;
                                record.CardType = cardType;
                                if (staffInfo.ORG_ID!=null)
                                {
                                    record.Depart = staffInfo.ORG_ID.ToString();
                                }
                                else record.Depart = "";
                                if (cardType == 0)
                                {
                                    record.ThroughForward = (cardRecord.IS_ENTER == true) ? 0 : 1;
                                    record.ThroughTime = cardRecord.RECORD_DATE.Value;
                                    record.CardNo = cardRecord.CARD_NO;
                                }
                                else
                                {
                                    record.ThroughForward = 2;
                                    record.ThroughTime = DateTime.Now;
                                    if (idCard!=null&&!string.IsNullOrEmpty(idCard.Id))
                                    {
                                        record.CardNo = idCard.Id;
                                    }
                                    else record.CardNo = "";

                                }
                                record.AccessChannel = AccessCollect.Instance.FaceControllerID.ToString();
                                record.FacePosition = "";
                                recordBll.Add(record);

                                record = recordBll.GetModelList("CardNo='" + record.CardNo + "' AND Name='" + record.Name+"'")[0];
                            }
                            catch (System.Exception ex)
                            {
                                mlog.ErrorFormat("添加人脸通行记录：{0}",ex);
                            }

                            mlog.InfoFormat("人脸验证结果：验证成功，员工{0},验证得分{1},阈值{2}", staffInfo.REAL_NAME, faceValue.ToString(), MainForm.Instance.IThreshold.ToString());
                            ValidateEvent(this, new ValidateResultEventArgs(record, blackPic, ValidateResult.Success));

                            using (IAccessCore access = new WGAccess())
                            {
                                ///控制开门
                                Maticsoft.Model.SMT_CONTROLLER_INFO _ctrlr = ctrlBll.GetModel(AccessCollect.Instance.FaceControllerID);
                                Controller c = ControllerHelper.ToController(_ctrlr);
                                Maticsoft.Model.SMT_DOOR_INFO _door = doorBll.GetModel(AccessCollect.Instance.FaceDoorID);

                                bool ret = access.OpenRemoteControllerDoor(c, _door.CTRL_DOOR_INDEX.Value);
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
                            mlog.InfoFormat("人脸验证结果：验证失败无此人");
                            ValidateEvent(this, new ValidateResultEventArgs(record,  blackPic, ValidateResult.NoPerson));
                            //if (File.Exists(capturePic))
                            //{
                            //    File.Delete(capturePic);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mlog.InfoFormat("验证异常:{0}", ex);
            }
        }

        private string GetCameraPic()
        {
            //return @"C:\查验系统\Code\IMS\IMS\IMS\bin\Debug\Faces\20161106194611546.jpg";

            string dir=AppDomain.CurrentDomain.BaseDirectory+"Faces\\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string sBmpPicFileName =dir+ DateTime.Now.ToString("yyyyMMddHHmmssfff")+".jpg";
            if (!string.IsNullOrEmpty(MainForm.Instance.PlayHandle))
            {
                MainForm.Instance.HikCam.CapturePicture(int.Parse(MainForm.Instance.PlayHandle), sBmpPicFileName);
                Thread.Sleep(100);
            }
            if (File.Exists(sBmpPicFileName))
            {
                if(FaceService.face_exist(sBmpPicFileName)==1)
                {
                    mlog.Info("当前图像中存在人脸！");
                    return sBmpPicFileName;
                }
                else
                {
                    do 
                    {
                        File.Delete(sBmpPicFileName);
                    } 
                    while (File.Exists(sBmpPicFileName));
                    return null;
                }
            }
            else  return null;
        }
    }
}
