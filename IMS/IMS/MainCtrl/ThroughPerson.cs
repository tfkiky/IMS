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
    public partial class ThroughPerson : UserControl
    {
        public ThroughPerson(string name,string photo)
        {
            InitializeComponent();

            lbName.Text = name;
            pbPhoto.ImageLocation = photo;
        }
    }
}
