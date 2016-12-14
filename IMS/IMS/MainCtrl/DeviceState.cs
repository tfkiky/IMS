using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IMS.MainCtrl
{
    public partial class DeviceState : UserControl
    {
        public DeviceState()
        {
            InitializeComponent();
        }

        public void LoadState(bool isDBConn, bool isCamConn, bool isCtrlConn, bool isCarConn,bool isIDCardConn)
        {
            if(isDBConn)
            {
                pictureBox3.Image = Properties.Resources.LED绿;
            }
            else
                pictureBox3.Image = Properties.Resources.LED红;
            if (isCamConn)
            {
                pictureBox4.Image = Properties.Resources.LED绿;
            }
            else
                pictureBox4.Image = Properties.Resources.LED红;
            if (isCtrlConn)
            {
                pictureBox5.Image = Properties.Resources.LED绿;
            }
            else
                pictureBox5.Image = Properties.Resources.LED红;
            if (isCarConn)
            {
                pictureBox7.Image = Properties.Resources.LED绿;
            }
            else
                pictureBox7.Image = Properties.Resources.LED红;

            if (isIDCardConn)
            {
                pictureBox9.Image = Properties.Resources.LED绿;
            }
            else
                pictureBox9.Image = Properties.Resources.LED红;
        }
    }
}
