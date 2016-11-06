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
            this.Invoke(new Action(() =>
            {
                lbName.Text = idcard.Name;
                lbSex.Text = idcard.Sex;
                lbNation.Text = idcard.Nation;
                lbBirth.Text = idcard.Birth;
                lbAddr.Text = idcard.Address;
                lbCardNo.Text = idcard.Id;
                pbPhoto.ImageLocation = idcard.PhotoFile;
            }));

        }
    }
}
