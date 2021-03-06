﻿using System;
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
using IMS.Common.Data;

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
        public static List<Maticsoft.Model.SMT_STAFF_INFO_EX> staffExList = new List<Maticsoft.Model.SMT_STAFF_INFO_EX>();
        private static System.Threading.Timer timer;
        private bool bStarted;

        public bool BStarted
        {
            get { return bStarted; }
            set { bStarted = value; }
        }
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

        private static int cardType=2;

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
                    try
                    {
                        if (!string.IsNullOrEmpty(staff.REAL_NAME))
                        {
                            staffExList.Add(new Maticsoft.Model.SMT_STAFF_INFO_EX()
                            {
                                ID = staff.ID,
                                REAL_NAME = staff.REAL_NAME,
                                Pinyi = (PinyiHelper.GetPinyin(staff.REAL_NAME) + "," + PinyiHelper.GetFirstPinyin(staff.REAL_NAME)).ToLower()
                            });
                            mlog.Info(staff.REAL_NAME + "：" + (PinyiHelper.GetPinyin(staff.REAL_NAME) + "," + PinyiHelper.GetFirstPinyin(staff.REAL_NAME)).ToLower());
                        }
                    }
                    catch (System.Exception ex)
                    {
                        mlog.ErrorFormat("获取人员姓名：{0}拼音异常，{1}",staff.REAL_NAME ,ex);
                    }
                    try
                    {
                        ImageHelper.ImageSave(staffFacePath + staff.ID + ".jpg", staff.PHOTO);
                    }
                    catch (System.Exception ex)
                    {
                        mlog.ErrorFormat("保存图片失败：{0}，{1}", staff.REAL_NAME, ex);
                    }
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

        public FaceCollect()
        {
            InitFacePath();
            InitFaceStore();
        }
        
        public bool Start(int faceMode, int swipeMode, int threshold,int isBlackList)
        {
            try
            {
                int iret = FaceService.face_init();
                if (iret == 0)
                {
                    mlog.Info("人脸识别算法库初始化成功，开启抓取比对线程——");
                    bStarted = true;
                    timer = new System.Threading.Timer(new TimerCallback(FaceValidate), null, 1000, 1000);
                    //BackgroundWorker worker = new BackgroundWorker();
                    //worker.DoWork += (p, q) =>
                    //{
                    //    FaceValidate();
                    //};
                    //worker.RunWorkerAsync();
                    return true;
                }
                else
                {
                    mlog.Info("人脸识别算法库初始化失败");
                    bStarted = false;
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public void Stop()
        {
            try
            {
                timer.Dispose();
                sw.Stop();
                bStarted = false;
                FaceService.face_exit();
            }
            catch (System.Exception ex)
            {
                bStarted = false;
                mlog.Error("Stop Error", ex);
            }
        }
        int faceValue = 0;
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// 人脸验证
        /// </summary>
        /// <param name="state"></param>
        private void FaceValidate(object state)
        {
            try
            {
                //while (bStarted)
                {
                    //timer.Change(Timeout.Infinite,0);
                    //Thread.Sleep(200);
                    if (isFaceLoad && !string.IsNullOrEmpty(currentFacePic))
                    {
                        mlog.InfoFormat("建立比对任务：{0}，开始抓图！", currentFacePic);
                        sw.Restart();
                        string capturePic = GetCameraPic();
                        if(string.IsNullOrEmpty(capturePic))
                        {
                            mlog.InfoFormat("截图耗时：{0}毫秒,当前画面无人脸", sw.ElapsedMilliseconds);
                            //timer.Change(Timeout.Infinite, 500);
                            return;
                            //continue;
                        }
                        mlog.InfoFormat("截图耗时：{0}毫秒,路径{1}", sw.ElapsedMilliseconds, capturePic);
                        string localPic = currentFacePic;
                        string blackPic = "";
                        byte[] feature1 = new byte[3000], feature2 = new byte[3000];
                        Maticsoft.Model.IMS_PEOPLE_RECORD record = new Maticsoft.Model.IMS_PEOPLE_RECORD();
                        if (staffInfo != null)
                        {
                            record.Name = staffInfo.REAL_NAME;
                        }
                        else
                            record.Name = "";
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
                                    currentFacePic = "";

                                }
                            }
                        }

                        switch (MainForm.Instance.IFaceMode)
                        {
                            case 0: ///1:1验证
                                if (!string.IsNullOrEmpty(localPic) && !string.IsNullOrEmpty(capturePic))
                                {
                                    mlog.InfoFormat("1:1验证，库内图片：{0}，实时截图：{1}", localPic, capturePic);
                                    int f1 = FaceService.face_get_feature_from_image(localPic, feature1);
                                    int f2 = FaceService.face_get_feature_from_image(capturePic, feature2);
                                    faceValue = FaceService.face_comp_feature(feature1, feature2);
                                    mlog.InfoFormat("1:1验证得分：{0},验证目标{1},{2},{3},验证耗时{4}毫秒", faceValue, staffInfo.REAL_NAME, localPic, capturePic, sw.ElapsedMilliseconds);
                                }
                                else
                                {
                                    //timer.Change(Timeout.Infinite, 500);
                                    return;
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
                                            staffInfo = staffInfoBll.GetModel(faceWhiteList.FirstOrDefault(q => q.Value == pic).Key);
                                            record.Name = staffInfo.REAL_NAME;
                                            break;
                                        }
                                    }
                                }
                                break;
                            case 2:

                                break;
                            case 3:

                                break;
                        }

                        FacePos[] position = new FacePos[15];
                        int iret = FaceService.face_get_pos(capturePic, ref position);
                        //mlog.InfoFormat("图像中有{0}个人脸", iret);
                        for (int i = 0; i < iret; i++)
                        {
                            //mlog.InfoFormat("图像中人脸坐标{0}，{1}，{2}，{3}", position[i].x1, position[i].x2, position[i].y1, position[i].y2);
                            //record.FacePosition += ";" + position[i].x1 + "," + position[i].x2 + "," + position[i].y1 + "," + position[i].y2 + ";";
                        }
                        if (!string.IsNullOrEmpty(localPic) && !string.IsNullOrEmpty(capturePic))
                        {

                            try
                            {
                                record.Similarity = faceValue;
                                record.ThroughResult = 1;
                                record.OriginPic = localPic;
                                record.CapturePic = capturePic;
                                record.CompareResult = 1;
                                record.CardType = cardType;
                                if (staffInfo != null && staffInfo.ORG_ID != null)
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
                                else if (cardType == 1)
                                {
                                    record.ThroughForward = 2;
                                    record.ThroughTime = DateTime.Now;
                                    if (idCard != null && !string.IsNullOrEmpty(idCard.Id))
                                    {
                                        record.CardNo = idCard.Id;
                                    }
                                    else record.CardNo = "";

                                }
                                else
                                {
                                    record.ThroughForward = 2;
                                    record.ThroughTime = DateTime.Now;
                                    record.CardNo = "";
                                }
                                record.AccessChannel = AccessCollect.Instance.FaceControllerID.ToString();
                                record.FacePosition = "";
                                recordBll.Add(record);

                                record = recordBll.GetModelList("CardNo='" + record.CardNo + "' AND Name='" + record.Name + "' AND CONVERT(VARCHAR(24),ThroughTime,20) ='" + record.ThroughTime.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'")[0];
                            }
                            catch (System.Exception ex)
                            {
                                mlog.ErrorFormat("添加人脸通行记录：{0}", ex);
                            }

                            if (faceValue > MainForm.Instance.IThreshold)
                            {
                                if (ValidateEvent != null)
                                {
                                    sw.Stop();
                                    mlog.InfoFormat("人脸验证结果：验证成功，员工{0},验证得分{1},阈值{2},验证耗时{3}毫秒", record.Name, faceValue.ToString(), MainForm.Instance.IThreshold.ToString(),sw.ElapsedMilliseconds);
                                    ValidateEvent(this, new ValidateResultEventArgs(record, blackPic, ValidateResult.Success));
                                    currentFacePic = "";

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
                                            //timer.Change(Timeout.Infinite, 500);
                                            return;
                                        }
                                    }
                                    //timer.Change(Timeout.Infinite, 500);
                                }
                            }
                            else
                            {
                                if (ValidateEvent != null)
                                {
                                    //mlog.InfoFormat("人脸验证结果：验证失败无此人");
                                    sw.Stop();
                                    mlog.InfoFormat("人脸验证结果：验证成功但低于阈值，员工{0},验证得分{1},阈值{2},验证耗时{3}毫秒", record.Name, faceValue.ToString(), MainForm.Instance.IThreshold.ToString(), sw.ElapsedMilliseconds);
                                    ValidateEvent(this, new ValidateResultEventArgs(record, blackPic, ValidateResult.BelowValue));
                                    currentFacePic = "";
                                    //timer.Change(Timeout.Infinite, 500);
                                    //if (File.Exists(capturePic))
                                    //{
                                    //    File.Delete(capturePic);
                                    //}
                                }
                            }
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
            try
            {
                string dir = AppDomain.CurrentDomain.BaseDirectory + "Faces\\";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string sBmpPicFileName = dir + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                MainForm.Instance.HikCam.CapturePicture(sBmpPicFileName);
                if (File.Exists(sBmpPicFileName))
                {
                    if (FaceService.face_exist(sBmpPicFileName) == 1)
                    {
                        mlog.Info("当前图像中存在人脸！");
                        return sBmpPicFileName;
                    }
                    else
                    {
                        mlog.Info("当前图像中无人脸！");
                        do
                        {
                            File.Delete(sBmpPicFileName);
                        }
                        while (File.Exists(sBmpPicFileName));
                        return null;
                    }
                }
                else return null;
            }
            catch (Exception ex)
            {
                mlog.Error(ex);
                return null;
            }
        }
    }
}
