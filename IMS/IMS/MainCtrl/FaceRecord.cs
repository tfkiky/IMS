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
            lbName.Text = _peopleRecord.Name;
            lbTime.Text = _peopleRecord.ThroughTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            lbSwipe.Text = (_peopleRecord.CardType == 0) ? "门禁刷卡" : "身份证刷卡";
            lbValue.Text = _peopleRecord.Similarity.ToString();
            pbCaptrue.ImageLocation = _peopleRecord.CapturePic;
            //peopleIDCard1.LoadIDCard();
        }

    }
}
