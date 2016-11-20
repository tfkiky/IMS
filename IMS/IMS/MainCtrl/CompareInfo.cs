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
using log4net;

namespace IMS
{
    public partial class CompareInfo : UserControl
    {
        private ILog mlog = LogManager.GetLogger("CompareInfo");
        public CompareInfo()
        {
            InitializeComponent();
        }

        public void LoadValidateResult(ValidateResultEventArgs e)
        {
            try
            {
                this.Invoke(new Action(() =>
               {
                   pbRealPhoto.ImageLocation = e.Record.CapturePic;
                   pbLocalPhoto.ImageLocation = e.Record.OriginPic;
                   pbBlack.ImageLocation = e.BlackPic;
                   //lbValue.Text = e.Record.Similarity.ToString();

                   Maticsoft.BLL.SMT_ORG_INFO orgBll = new Maticsoft.BLL.SMT_ORG_INFO();
                   List<Maticsoft.Model.SMT_ORG_INFO> orgList = orgBll.GetModelList("ID=" + e.Record.Depart);
                   if (orgList != null && orgList.Count > 0)
                   {
                       tbDepart.Text = orgList[0].ORG_NAME;
                   }
                   else
                       tbDepart.Text = "";
               }));
            }
            catch (Exception ex)
            {
                mlog.Error(ex);
            }
        }

        public void LoadStaffInfo(string staffPic)
        {
            pbLocalPhoto.ImageLocation = staffPic;

        }

        public void LoadAccessInfo(Maticsoft.Model.SMT_STAFF_INFO staffInfo, Maticsoft.Model.SMT_CARD_RECORDS cardRecord)
        {
            this.Invoke(new Action(() =>
           {
               Maticsoft.BLL.SMT_ORG_INFO orgBll = new Maticsoft.BLL.SMT_ORG_INFO();
               List<Maticsoft.Model.SMT_ORG_INFO> orgList = orgBll.GetModelList("ID=" + staffInfo.ORG_ID);
               if (orgList != null && orgList.Count > 0)
               {
                   tbDepart.Text = orgList[0].ORG_NAME;
               }
               else
                   tbDepart.Text = "";
               tbPassTime.Text = cardRecord.RECORD_DATE.Value.ToString("yyyy-MM-dd HH:mm:ss");
               tbName.Text = staffInfo.REAL_NAME;

               switch (staffInfo.SEX)
               {
                   case 0:
                       tbSex.Text = "未知";
                       break;
                   case 1:
                       tbSex.Text = "男";
                       break;
                   case 2:
                       tbSex.Text = "女";
                       break;
                   default:
                       tbSex.Text = "未知";
                       break;
               }
               tbIDCard.Text = staffInfo.CER_NO;
               tbBirth.Text = staffInfo.BIRTHDAY.Value.ToString("yyyy-MM-dd HH:mm:ss");
               tbAddress.Text = staffInfo.ADDRESS;
               tbNation.Text = staffInfo.NATION;
           }));
        }

        public void LoadIDInfo(IDCardClass idcard)
        {
            this.Invoke(new Action(() =>
            {
                tbName.Text = idcard.Name;
                tbSex.Text = idcard.Sex;
                tbNation.Text = idcard.Nation;
                tbBirth.Text = idcard.Birth;
                tbAddress.Text = idcard.Address;
                tbIDCard.Text = idcard.Id;
                tbPassTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            }));
        }
    }
}
