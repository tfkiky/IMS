using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IMS
{
    public partial class PeopleVehicleVideo : UserControl
    {
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

        public void LoadAccessResult(bool isAllow)
        {
            if(!isAllow)
            {
                lbResult.Text = "姓名，无权限禁止通行！";
            }
        }
    }
}
