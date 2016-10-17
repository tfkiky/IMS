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
        private static string currentFacePic;
        private static string localFaceDir;

        public static string CurrentFacePic
        {
            get { return FaceCollect.currentFacePic; }
            set { FaceCollect.currentFacePic = value; }
        }
        private static List<string> faceWhiteList;
        private static List<string> faceBlackList;
        private static List<string> faceTempList;

        public static void InitFaceList()
        {
            
        }

        public static void InitFaceStore()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (p, q) =>
            {
               
            };
            worker.RunWorkerCompleted += (p, q) =>
            {
                InitFaceList();
            };
            worker.RunWorkerAsync();
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
