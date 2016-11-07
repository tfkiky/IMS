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
        private Maticsoft.BLL.SMT_STAFF_INFO staffBll = new Maticsoft.BLL.SMT_STAFF_INFO();
        private Maticsoft.BLL.SMT_STAFF_DOOR staffDorrBll = new Maticsoft.BLL.SMT_STAFF_DOOR();

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
            if (File.Exists("./wz.txt"))
            {
                File.Delete("./wz.txt");
            }
            if (File.Exists("./zp.bmp"))
            {
                File.Delete("./zp.bmp");
            }
            iLastErrorCode = IDCardDll.IDCard.InitCommExt();
            timer = new System.Threading.Timer(new TimerCallback(CollectIDCard), null, 1000, 1000);
        }

        private void CollectIDCard(object state)
        {
            try
            {
                iLastErrorCode = IDCardDll.IDCard.Authenticate();
                if (iLastErrorCode != 1)
                {
                    // mlog.Info("未放卡或卡片放置不正确");
                }
                try 
                {
                    iLastErrorCode = IDCardDll.IDCard.Read_Content(1);
                }
                catch (Exception ex)
                {
                    mlog.ErrorFormat("身份证读取异常:", ex);
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
                        currentIDCard.Sex = SexHelper.GetSex(idInfo[1].Substring(0, 1));
                        currentIDCard.Nation = NationHelper.GetNation(idInfo[1].Substring(1, 2));
                        currentIDCard.Birth = idInfo[1].Substring(3, 8);
                        currentIDCard.Address = idInfo[1].Substring(11);
                        currentIDCard.Id = idInfo[2].Substring(0, 18);
                        currentIDCard.SignGov = idInfo[2].Substring(18);
                        currentIDCard.StartDate = idInfo[3].Substring(0, 8);
                        currentIDCard.LimitDate = idInfo[3].Substring(8);
                        if (File.Exists("./zp.bmp"))
                        {
                            currentIDCard.PhotoFile = AppDomain.CurrentDomain.BaseDirectory + "zp.bmp";

                            if (AccessCollect.Instance.FaceDoorID!=0)
                            {
                                List<Maticsoft.Model.SMT_STAFF_INFO> staffList = new List<Maticsoft.Model.SMT_STAFF_INFO>();
                                //通过姓名和身份证号码验证库中是否有此员工
                                staffList = staffBll.GetModelList("REAL_NAME='" + currentIDCard.Name + "' AND CER_NO='" + currentIDCard.Id + "'");
                                if (staffList!=null&&staffList.Count>0)
                                {
                                    //验证此员工是否有此门同行权限
                                    List<Maticsoft.Model.SMT_STAFF_DOOR> staffDoorList=staffDorrBll.GetModelList("STAFF_ID="+staffList[0].ID+" AND DOOR_ID="+AccessCollect.Instance.FaceDoorID);
                                    bool isAllow = false;
                                    if (staffDoorList != null && staffDoorList.Count > 0)
                                    {
                                       if (staffDoorList[0].IS_UPLOAD)
                                       {
                                           FaceCollect.CardType = 1;
                                           FaceCollect.StaffInfo = staffList[0];
                                           FaceCollect.CurrentFacePic = currentIDCard.PhotoFile;
                                           mlog.Info("通过身份证识别人员：" + staffList[0].REAL_NAME);

                                           isAllow = true;
                                       }
                                       else
                                        isAllow = false;
                                    }
                                    else
                                    {
                                        isAllow = false;
                                    }
                                    if (IDCardEvent != null)
                                    {
                                        IDCardEvent(this, new IDCardEventArgs(currentIDCard, staffList[0], isAllow));
                                    }
                                }
                                else
                                {
                                    if (IDCardEvent != null)
                                    {
                                        IDCardEvent(this, new IDCardEventArgs(currentIDCard, null, false));
                                    }
                                }
                            }
                            
                        }
                    }
                }
            }
            catch(Exception  ex)
            {
                mlog.ErrorFormat("CollectIDCard异常：{0}", ex);
            }
        }

        public void Stop()
        {
            iLastErrorCode = IDCardDll.IDCard.CloseComm();
            timer.Dispose();

        }
    }
}
