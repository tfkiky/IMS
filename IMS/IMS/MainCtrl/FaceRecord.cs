using DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IMS.MainCtrl
{
    public partial class FaceRecord : Office2007Form
    {

        Maticsoft.Model.IMS_PEOPLE_RECORD _peopleRecord;
        public FaceRecord(Maticsoft.Model.IMS_PEOPLE_RECORD peopleRecord)
        {
            InitializeComponent();
            _peopleRecord = peopleRecord;
        }

        private void LoadInfo()
        {

        }

    }
}
