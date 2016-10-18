using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace IMS.Collecter
{
    public class FaceCollect
    {
        Maticsoft.BLL.IMS_DATA_CONFIG imsConfigBll = new Maticsoft.BLL.IMS_DATA_CONFIG();
        Maticsoft.BLL.SMT_STAFF_INFO staffInfoBll = new Maticsoft.BLL.SMT_STAFF_INFO();
        Maticsoft.BLL.IMS_FACE_BLACKLIST blackListBll = new Maticsoft.BLL.IMS_FACE_BLACKLIST();
        private static Thread faceThread;
        private static bool isFaceLoad;

        public static bool IsFaceLoad
        {
            get { return FaceCollect.isFaceLoad; }
            set { FaceCollect.isFaceLoad = value; }
        }
        private static string currentFacePic;
        private static string localFaceDir;

        public static string CurrentFacePic
        {
            get { return FaceCollect.currentFacePic; }
            set { FaceCollect.currentFacePic = value; }
        }
        private static List<string> faceWhiteList;

        public static List<string> FaceWhiteList
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
            };
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// 加载数据库中人脸图片至本地缓存
        /// </summary>
        public static void InitFaceFiles()
        {
            //加载人员库白名单

            //加载黑名单
            
        }

        /// <summary>
        ///  加载本地缓存路径至内存
        /// </summary>
        public static void InitFaceList()
        {
            
        }
        
            
        public static void Start(int faceMode, int swipeMode, int threshold,bool isBlackList)
        {
            faceThread = new Thread(new ThreadStart(FaceValidate));
            faceThread.Start();
            faceThread.IsBackground = true;
        }

        public static void Stop()
        {
            try
            {
                faceThread.Abort();
            }
            catch (System.Exception ex)
            {
            }
        }

        private static void FaceValidate()
        {

        }

    }
}
