using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace IMS.Collecter
{
    public class IDCardCollect
    {
        private static System.Threading.Timer timer;
        private static IDCardClass currentIDCard;

        public static IDCardClass CurrentIDCard
        {
            get { return IDCardCollect.currentIDCard; }
            set { IDCardCollect.currentIDCard = value; }
        }
        private static int iLastErrorCode;

        public static int ILastErrorCode
        {
            get { return IDCardCollect.iLastErrorCode; }
            set { IDCardCollect.iLastErrorCode = value; }
        }
        private static string sLastErrorMsg;

        public static void Start()
        {
            iLastErrorCode = IDCardDll.IDCard.InitCommExt();
            timer = new System.Threading.Timer(new TimerCallback(CollectIDCard), null, 1000, 1000);
        }

        private static void CollectIDCard(object state)
        {
            iLastErrorCode = IDCardDll.IDCard.Authenticate();

            try
            {
                iLastErrorCode = IDCardDll.IDCard.Read_Content(1);

            }
            catch { }
            FileInfo fi = new FileInfo("./wz.txt");
            if (fi.Exists)
            {
                FileStream fsRead = fi.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                int fsLen = (int)fsRead.Length;
                byte[] heByte = new byte[fsLen];
                int r = fsRead.Read(heByte, 0, heByte.Length);
                string myStr = System.Text.Encoding.Unicode.GetString(heByte);

                if (string.IsNullOrEmpty(myStr))
                {
                    string[] idInfo = myStr.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    currentIDCard = new IDCardClass();
                    currentIDCard.Name = idInfo[0];
                    currentIDCard.Sex = idInfo[1];
                    currentIDCard.Nation = idInfo[2];
                    currentIDCard.Birth = DateTime.Parse(idInfo[3]);
                    currentIDCard.Address = idInfo[4];
                    currentIDCard.SignGov = idInfo[5];
                    currentIDCard.StartDate = DateTime.Parse(idInfo[6]);
                    currentIDCard.LimitDate = DateTime.Parse(idInfo[7]);
                    if (File.Exists("./zp.bmp"))
                    {
                        currentIDCard.PhotoFile = AppDomain.CurrentDomain.BaseDirectory+"zp.bmp";
                        FaceCollect.CurrentFacePic = currentIDCard.PhotoFile;
                    }
                }
            }
        }

        public static void Stop()
        {
            iLastErrorCode = IDCardDll.IDCard.CloseComm();
            timer.Dispose();

        }
    }
}
