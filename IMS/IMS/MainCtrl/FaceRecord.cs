using DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCardDll;
using IMS.Collecter;

namespace IMS.MainCtrl
{
    public partial class FaceRecord : Office2007Form
    {

        Maticsoft.Model.IMS_PEOPLE_RECORD _peopleRecord;
        public FaceRecord(Maticsoft.Model.IMS_PEOPLE_RECORD peopleRecord)
        {
            InitializeComponent();
            _peopleRecord = peopleRecord;
            LoadInfo();
        }

        private void LoadInfo()
        {
            lbName.Text = _peopleRecord.Name;
            lbTime.Text = _peopleRecord.ThroughTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            lbSwipe.Text = (_peopleRecord.CardType == 0) ? "门禁刷卡" : "身份证刷卡";
            lbValue.Text = _peopleRecord.Similarity.ToString();
            switch (_peopleRecord.ThroughForward)
            {
                case 0:
                    lbForward.Text = "出门";
                    break;
                case 1:
                    lbForward.Text = "进门";
                    break;
                case 2:
                    lbForward.Text = "未知";
                    break;
                default:
                    lbForward.Text = "未知";
                    break;
            }
            switch (_peopleRecord.ThroughResult)
            {
                case 0:
                    lbIsAllow.Text = "禁止通行";
                    break;
                case 1:
                    lbIsAllow.Text = "允许通行";
                    break;
                case 2:
                    lbIsAllow.Text = "未知";
                    break;
                default:
                    lbIsAllow.Text = "未知";
                    break;
            }
            switch(_peopleRecord.CompareResult)
            {
                case 0:
                    lbValidate.Text = "查无此人";
                    break;
                case 1:
                    lbValidate.Text = "验证通过";
                    break;
                default:
                    lbValidate.Text = "验证通过";
                    break;
            }

            pbCaptrue.ImageLocation = _peopleRecord.CapturePic;
            pbOrigin.ImageLocation = _peopleRecord.OriginPic;


        }

    }
}
