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
    public partial class PeopleIDCard : UserControl
    {
        public PeopleIDCard()
        {
            InitializeComponent();
        }

        public void LoadIDCard(IDCardClass idcard)
        {
            try
            {
                lbName.Text = idcard.Name;
                lbSex.Text = idcard.Sex;
                lbNation.Text = idcard.Nation;
                lbBirth.Text = idcard.Birth;
                lbAddr.Text = idcard.Address;
                lbCardNo.Text = idcard.Id;
                pbPhoto.ImageLocation = idcard.PhotoFile;
            }
            catch { }

        }


        public void Clear()
        {
            try
            {
                lbName.Text = "";
                lbSex.Text = "";
                lbNation.Text = "";
                lbBirth.Text = "";
                lbAddr.Text = "";
                lbCardNo.Text = "";
                pbPhoto.Image = Properties.Resources.暂无图片;
            }
            catch { }
        }
    }
}
