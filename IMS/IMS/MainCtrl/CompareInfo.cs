using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IMS.Collecter;

namespace IMS
{
    public partial class CompareInfo : UserControl
    {
        public CompareInfo()
        {
            InitializeComponent();
        }

        public void LoadValidateResult(ValidateResultEventArgs e)
        {
            this.Invoke(new Action(() =>
           {
               pbRealPhoto.ImageLocation = e.CaptruePic;
               pbLocalPhoto.ImageLocation = e.LocalPic;
               pbBlack.ImageLocation = e.BlackPic;
               lbValue.Text = e.ValidateValue.ToString();

           }));
        }

        public void LoadAccessInfo(Maticsoft.Model.SMT_STAFF_INFO staffInfo, string passTime)
        {
            this.Invoke(new Action(() =>
           {
               Maticsoft.BLL.SMT_ORG_INFO orgBll = new Maticsoft.BLL.SMT_ORG_INFO();
               List<Maticsoft.Model.SMT_ORG_INFO> orgList = orgBll.GetModelList("ID=" + staffInfo.ORG_ID);
               if (orgList != null && orgList.Count > 0)
               {
                   lbDepart.Text = orgList[0].ORG_NAME;
               }
               lbTime.Text = passTime;
           }));
        }

        public void LoadIDInfo(IDCardClass idcard)
        {
            this.Invoke(new Action(() =>
            {
                lbName.Text = idcard.Name;
                lbSex.Text = idcard.Sex;
                lbNation.Text = idcard.Nation;
                lbBirth.Text = idcard.Birth;
                lbAddress.Text = idcard.Address;
                lbIDCard.Text = idcard.Id;
            }));
        }
    }
}
