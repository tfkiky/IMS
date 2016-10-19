using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using IMS.Collecter.Common;
using IMS.Common.Config;

namespace IMS.Collecter
{
    public class FaceCollect
    {
        #region 全局变量定义
        private static Maticsoft.BLL.IMS_DATA_CONFIG imsConfigBll = new Maticsoft.BLL.IMS_DATA_CONFIG();
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

        /// <summary>
        /// 初始化本地人脸库路径
        /// </summary>
        public static void InitFacePath()
        {
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
        public static void InitFaceStore()
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
        public static void InitFaceFiles()
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
        public static void InitFaceList()
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
        
            
        public static void Start(int faceMode, int swipeMode, int threshold,int isBlackList)
        {
            InitFacePath();
            InitFaceStore();
            timer = new System.Threading.Timer(new TimerCallback(FaceValidate), null, 1000, 1000);

        }

        public static void Stop()
        {
            try
            {
                timer.Dispose();
            }
            catch (System.Exception ex)
            {
            }
        }

        private static void FaceValidate(object state)
        {
            if (isFaceLoad&&!string.IsNullOrEmpty(currentFacePic))
            {

            }
        }

    }
}
