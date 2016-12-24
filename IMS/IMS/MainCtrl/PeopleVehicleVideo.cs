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
                        break;
                    case IMS.Collecter.ValidateResult.NoPerson:
                        lbResult.Text = "验证无此人";
                        break;
                    case IMS.Collecter.ValidateResult.Black:
                        lbResult.Text = "验证为黑名单成员";
                        break;
                    case IMS.Collecter.ValidateResult.Error:
                        lbResult.Text = "验证错误";
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
        }


        public void LoadResult(Maticsoft.Model.SMT_STAFF_INFO staffInfo, bool isAllow)
        {
            ClientMainForm.Instance.LoadResult(staffInfo, isAllow);
            this.Invoke(new Action(() =>
   {
            if (staffInfo == null)
            {
                lbResult.Text = "此卡无效，无此用户！";
            }
            else
            {
                if (MainForm.Instance.IFaceMode == 3)
                {
                    lbResult.Text = string.Format("{0}，请通行", staffInfo.REAL_NAME);
                }
                if (MainForm.Instance.IFaceMode!=3&&!isAllow)
                {
                    lbResult.Text = string.Format("{0}无权限,禁止通行！", staffInfo.REAL_NAME);
                }

            }

   }));
        }
    }
}
