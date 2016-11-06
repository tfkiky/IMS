using IMS.Common.Data;
using log4net;
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
        private System.Threading.Timer timer;
        private ILog mlog = LogManager.GetLogger("IDCardCollect");

        public event EventHandler<IDCardEventArgs> IDCardEvent;
        private IDCardClass currentIDCard;

        public IDCardClass CurrentIDCard
        {
            get { return currentIDCard; }
            set { currentIDCard = value; }
        }
        private int iLastErrorCode;

        public int ILastErrorCode
        {
            get { return iLastErrorCode; }
            set { iLastErrorCode = value; }
        }

        public void Start()
        {
            iLastErrorCode = IDCardDll.IDCard.InitCommExt();
            timer = new System.Threading.Timer(new TimerCallback(CollectIDCard), null, 1000, 1000);
        }

        private void CollectIDCard(object state)
        {
            iLastErrorCode = IDCardDll.IDCard.Authenticate();
            if (iLastErrorCode!=1)
            {
               // mlog.Info("未放卡或卡片放置不正确");
            }
            try
            {
                iLastErrorCode = IDCardDll.IDCard.Read_Content(1);

            }
            catch(Exception ex)
            {
                mlog.ErrorFormat("身份证读取异常:",ex);
            }
            FileInfo fi = new FileInfo("./wz.txt");
            if (fi.Exists)
            {
                FileStream fsRead = fi.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                int fsLen = (int)fsRead.Length;
                byte[] heByte = new byte[fsLen];
                int r = fsRead.Read(heByte, 0, heByte.Length);
                string myStr = System.Text.Encoding.Unicode.GetString(heByte);
                //唐飞             10119890130安徽省合肥市蜀山区科学大道１９号华地紫园１５幢１００２室       340122198901300018合肥市公安局蜀山分局     2016032120360321          
                if (!string.IsNullOrEmpty(myStr))
                {
                    string[] idInfo = myStr.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    currentIDCard = new IDCardClass();
                    currentIDCard.Name = idInfo[0];
                    currentIDCard.Sex = SexHelper.GetSex(idInfo[1].Substring(0,1));
                    currentIDCard.Nation = NationHelper.GetNation(idInfo[1].Substring(1, 2));
                    currentIDCard.Birth = idInfo[1].Substring(3, 8);
                    currentIDCard.Address = idInfo[1].Substring(11);
                    currentIDCard.Id = idInfo[2].Substring(0,18);
                    currentIDCard.SignGov = idInfo[2].Substring(18);
                    currentIDCard.StartDate = idInfo[3].Substring(0,8);
                    currentIDCard.LimitDate = idInfo[3].Substring( 8);
                    if (File.Exists("./zp.bmp"))
                    {
                        currentIDCard.PhotoFile = AppDomain.CurrentDomain.BaseDirectory+"zp.bmp";
                        FaceCollect.CurrentFacePic = currentIDCard.PhotoFile;
                        FaceCollect.StaffName = currentIDCard.Name;
                        if (IDCardEvent != null)
                        {
                            IDCardEvent(this, new IDCardEventArgs(currentIDCard));
                        }
                    }
                }
            }
        }

        public void Stop()
        {
            iLastErrorCode = IDCardDll.IDCard.CloseComm();
            timer.Dispose();

        }
    }
}
