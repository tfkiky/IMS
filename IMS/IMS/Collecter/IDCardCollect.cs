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
        private static int iLastErrorCode;
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
            }
        }

        public static void Stop()
        {
            iLastErrorCode = IDCardDll.IDCard.CloseComm();
            timer.Dispose();

        }
    }
}
