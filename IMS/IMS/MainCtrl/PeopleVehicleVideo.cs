using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using IMS.Common;

namespace IMS
{
    public partial class PeopleVehicleVideo : UserControl
    {
        private ILog mlog = LogManager.GetLogger("PeopleVehicleVideo");
        public PeopleVehicleVideo()
        {
            InitializeComponent();
            lbResult.Text = "";
        }

        public Panel VideoPanel
        {
            get { return splitContainer1.Panel1; }
        }

        public void LoadValidateResult(IMS.Collecter.ValidateResult vr,string similarity)
        {
            try
            {
                switch (vr)
                {
                    case IMS.Collecter.ValidateResult.Success:
                        lbResult.Text = "验证通过，相似度"+similarity;
                        //请进
                        MediaPlayer.PlaySound("./wav/请进.wav");
                        break;
                    case IMS.Collecter.ValidateResult.NoPerson:
                        lbResult.Text = "验证无此人";
                        //非法卡
                        MediaPlayer.PlaySound("./wav/非法卡.wav");
                        break;
                    case IMS.Collecter.ValidateResult.Black:
                        lbResult.Text = "验证为黑名单成员";
                        break;
                    case IMS.Collecter.ValidateResult.Error:
                        lbResult.Text = "验证错误";
                        break;
                    case IMS.Collecter.ValidateResult.BelowValue:
                        lbResult.Text = "人脸比对失败，相似度"+similarity;
                        MediaPlayer.PlaySound("./wav/比对失败.wav");
                        break;
                    default:
                        lbResult.Text = "";
                        break;

                }
            }
            catch(Exception ex)
            {
                mlog.Error(ex);
            }
        }

        public void Clear()
        {
            lbResult.Text = "";
            //labelX1.Text = "";
        }

        public void SetVideoText()
        {
            lbResult.Text = "抓拍中，请将面部对准相机镜头";
        }


        public void LoadResult(Maticsoft.Model.SMT_STAFF_INFO staffInfo, bool isAllow)
        {
            ClientMainForm.Instance.LoadResult(staffInfo, isAllow);
            this.Invoke(new Action(() =>
           {
               if (staffInfo == null)
               {
                   lbResult.Text = "此卡无效，无此用户！";
                   //非法卡
                   MediaPlayer.PlaySound("./wav/非法卡.wav");
               }
               else
               {
                   if (MainForm.Instance.IFaceMode == 3)
                   {
                       if (isAllow)
                       {
                           lbResult.Text = string.Format("{0}，请通行", staffInfo.REAL_NAME);
                           MediaPlayer.PlaySound("./wav/请进.wav");
                           //请进
                       }
                       else
                       {
                           lbResult.Text = string.Format("{0}无权限,禁止通行！", staffInfo.REAL_NAME);
                           MediaPlayer.PlaySound("./wav/无权限.wav");
                           //无权限
                       }
                   }
                   else
                   {
                       if (!isAllow)
                       {
                           lbResult.Text = string.Format("{0}无权限,禁止通行！", staffInfo.REAL_NAME);
                           MediaPlayer.PlaySound("./wav/无权限.wav");
                           //无权限
                       }
                   }

               }

           }));
        }
    }
}
