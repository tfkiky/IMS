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
using log4net;

namespace IMS.MainCtrl
{
    public partial class FaceRecord : Office2007Form
    {
        private ILog mlog = LogManager.GetLogger("FaceRecord");
        Maticsoft.BLL.SMT_CARD_INFO cardBll = new Maticsoft.BLL.SMT_CARD_INFO();
        Maticsoft.BLL.SMT_STAFF_CARD scardBll = new Maticsoft.BLL.SMT_STAFF_CARD();
        Maticsoft.BLL.SMT_STAFF_INFO staffBll = new Maticsoft.BLL.SMT_STAFF_INFO();
        
        Maticsoft.Model.IMS_PEOPLE_RECORD _peopleRecord;
        public FaceRecord(Maticsoft.Model.IMS_PEOPLE_RECORD peopleRecord)
        {
            InitializeComponent();
            _peopleRecord = peopleRecord;
            LoadInfo();
        }

        private void LoadInfo()
        {
            try
            {
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
                switch (_peopleRecord.CompareResult)
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

                IDCardClass idcard = new IDCardClass();
                idcard.Name = _peopleRecord.Name;
                idcard.PhotoFile = _peopleRecord.OriginPic;
                if (_peopleRecord.CardType == 1)
                {
                    idcard.Id = _peopleRecord.CardNo;
                }
                else
                {
                    List<Maticsoft.Model.SMT_CARD_INFO> cardList = cardBll.GetModelList("CARD_NO='" + _peopleRecord.CardNo + "'");
                    if (cardList!=null&&cardList.Count>0)
                    {
                        List<Maticsoft.Model.SMT_STAFF_CARD> scardList = scardBll.GetModelList("CARD_ID=" + cardList[0].CARD_NO + "");
                        if (scardList != null && scardList.Count > 0)
                        {
                            Maticsoft.Model.SMT_STAFF_INFO staffInfo = staffBll.GetModel(scardList[0].STAFF_ID);
                            idcard.Address = staffInfo.ADDRESS;
                            idcard.Birth = staffInfo.BIRTHDAY.Value.ToString("yyyy-MM-dd HH:mm:ss");
                            idcard.Nation = staffInfo.NATION;
                            switch (staffInfo.SEX)
                            {
                                case 0:
                                    idcard.Sex = "未知";
                                    break;
                                case 1:
                                    idcard.Sex = "男";
                                    break;
                                case 2:
                                    idcard.Sex = "女";
                                    break;
                                default:
                                    idcard.Sex = "未知";
                                    break;
                            }
                            idcard.Address = staffInfo.ADDRESS;
                        }
                    }

                }

                peopleIDCard1.LoadIDCard(idcard);

                if (!string.IsNullOrEmpty(_peopleRecord.FacePosition))
                {
                    string[] faces = _peopleRecord.FacePosition.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach(string face in faces)
                    {
                        string[] position = face.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        Graphics g = this.CreateGraphics();
                        Pen p = new Pen(Color.Red, 4);
                        g.DrawRectangle(p, float.Parse(position[0]), float.Parse(position[1]), float.Parse(position[2]) - float.Parse(position[0]), float.Parse(position[3]) - float.Parse(position[1]));
                    }
                }


            }
            catch (System.Exception ex)
            {
                mlog.Error(ex);
            }
           
        }

    }
}
