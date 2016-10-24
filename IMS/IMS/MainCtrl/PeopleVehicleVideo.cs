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
        }

        public Panel VideoPanel
        {
            get { return splitContainer1.Panel1; }
        }
    }
}
