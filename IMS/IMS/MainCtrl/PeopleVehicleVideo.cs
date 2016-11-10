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
            lbResult.Text = "人员/车辆查验结果显示区";
        }

        public Panel VideoPanel
        {
            get { return splitContainer1.Panel1; }
        }

        public void LoadValidateResult(IMS.Collecter.ValidateResult vr)
        {
            try
            {
                switch (vr)
                {
                    case IMS.Collecter.ValidateResult.Success:
                        lbResult.Text = "验证通过";
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
                        lbResult.Text = "人员/车辆查验结果显示区";
                        break;

                }
            }
            catch(Exception ex)
            {
                mlog.Error(ex);
            }
        }


        public void LoadAccessResult(Maticsoft.Model.SMT_STAFF_INFO staffInfo,Maticsoft.Model.SMT_CARD_RECORDS cardRecord)
        {
            if (staffInfo==null)
            {
                lbResult.Text = "此卡无效，无此用户！";
            }
            else
            {
                if (!cardRecord.IS_ALLOW)
                {
                    lbResult.Text = string.Format("{0}无权限,禁止通行！",staffInfo.REAL_NAME);
                }

            }
            
        }

        public void LoadIDCardResult(Maticsoft.Model.SMT_STAFF_INFO staffInfo, bool isAllow)
        {
            if (staffInfo == null)
            {
                lbResult.Text = "此卡无效，无此用户！";
            }
            else
            {
                if (!isAllow)
                {
                    lbResult.Text = string.Format("{0}无权限,禁止通行！", staffInfo.REAL_NAME);
                }

            }

        }
    }
}
