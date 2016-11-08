using DevComponents.DotNetBar;
using IMS.Collecter;
using log4net;
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
    public partial class CheckPerson : Office2007Form
    {
        private ILog mlog = log4net.LogManager.GetLogger("PersonRecord");
        Maticsoft.BLL.SMT_STAFF_INFO staffBll = new Maticsoft.BLL.SMT_STAFF_INFO();
        Maticsoft.BLL.SMT_ORG_INFO orgBll = new Maticsoft.BLL.SMT_ORG_INFO();
        Maticsoft.BLL.SMT_CARD_INFO cardBll = new Maticsoft.BLL.SMT_CARD_INFO();
        Maticsoft.BLL.SMT_STAFF_CARD staffCardBll = new Maticsoft.BLL.SMT_STAFF_CARD();
        Maticsoft.BLL.SMT_STAFF_DOOR doorBll = new Maticsoft.BLL.SMT_STAFF_DOOR();
        List<Maticsoft.Model.SMT_STAFF_INFO> staffList = new List<Maticsoft.Model.SMT_STAFF_INFO>();
        private int recordRowsCount = 0;
        StringBuilder strSql;

        public CheckPerson()
        {
            InitializeComponent();
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                strSql = new StringBuilder("1=1");
                
                dataGridView.Rows.Clear();
                if (!string.IsNullOrEmpty(tbName.Text))
                {
                    strSql.Append(" AND REAL_NAME LIKE '%" + tbName.Text + "%'");
                }
                recordRowsCount = staffBll.GetRecordCount(strSql.ToString());
                pageCtrlRecords.TotalRecords = recordRowsCount;
                pageCtrlRecords.CurrentPage = 1;
                if (recordRowsCount != 0)
                {
                    FillAlarmData(0);
                }
                else
                {
                    MessageBox.Show("查无数据");
                }
            }
            catch (System.Exception ex)
            {
                mlog.Error(ex);
            }
        }

        private void FillAlarmData(int startIndex)
        {
            try
            {
                if (recordRowsCount >= startIndex + pageCtrlRecords.RecordsPerPage)
                {
                    if (startIndex == 0)
                    {
                        staffList = staffBll.GetModelListByPageEx(strSql.ToString(), "ID", startIndex, startIndex + pageCtrlRecords.RecordsPerPage);
                    }
                    else
                    {
                        staffList = staffBll.GetModelListByPageEx(strSql.ToString(), "ID", startIndex + 1, startIndex + pageCtrlRecords.RecordsPerPage);
                    }
                }
                else
                {
                    if (startIndex == 0)
                    {
                        staffList = staffBll.GetModelListByPageEx(strSql.ToString(), "ID", startIndex, recordRowsCount);
                    }
                    else
                    {
                        staffList = staffBll.GetModelListByPageEx(strSql.ToString(), "ID", startIndex + 1, recordRowsCount);
                    }
                }
                for (int i = 0; i < staffList.Count; i++)
                {
                    DataGridViewRow newRow = new DataGridViewRow();

                    Maticsoft.Model.SMT_ORG_INFO orgInfo = orgBll.GetModel(staffList[i].ORG_ID.Value);
                    string orgname = "未知", isAllow = "禁止通行",cardid="",startTime="",endTime="";
                    if (orgInfo!=null)
                    {
                        orgname = orgInfo.ORG_NAME;
                    }
                    List<Maticsoft.Model.SMT_STAFF_DOOR> door = doorBll.GetModelList("STAFF_ID="+staffList[0].ID+" and DOOR_ID="+AccessCollect.Instance.FaceDoorID);
                    if (door!=null&&door.Count>0)
                    {
                        isAllow=(door[0].IS_UPLOAD==true)?"允许通行":"禁止通行";
                    }
                    List<Maticsoft.Model.SMT_STAFF_CARD> staffCard = staffCardBll.GetModelList("STAFF_ID=" + staffList[i].ID);
                    if (staffCard != null && staffCard.Count > 0)
                    {
                        Maticsoft.Model.SMT_CARD_INFO card=cardBll.GetModel(staffCard[0].CARD_ID);
                        if(card!=null)
                        {
                            cardid = card.CARD_NO;
                            startTime = staffCard[0].ACCESS_STARTTIME.ToString("yyyy-MM-dd HH:mm:ss");
                            endTime = staffCard[0].ACCESS_ENDTIME.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }

                    newRow.DefaultCellStyle.ForeColor = Color.Black;
                    newRow.CreateCells(dataGridView, new object[] { 
                    staffList[i].ID,
                    i+1,
                    staffList[i].REAL_NAME,
                    orgname,
                    cardid,
                    isAllow,
                    startTime,
                    endTime
                    });
                    newRow.Tag = staffList[i];
                    dataGridView.Rows.Add(newRow);
                }
                if (recordRowsCount == 0)
                {
                    pageCtrlRecords.TotalRecords = recordRowsCount;
                    MessageBox.Show("查无数据");
                }
            }
            catch (System.Exception ex)
            {
                mlog.Error(ex);
            }
        }


        private void pageCtrlRecords_PageChanged(object sender, SunCreate.DotNetBar.PageEventArgs args)
        {
            dataGridView.Rows.Clear();
            if (recordRowsCount != 0)
            {
                if (recordRowsCount >= args.StartIndex - 1)
                {
                    FillAlarmData(args.StartIndex - 1);
                }
            }
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView.SelectedRows[0].Tag is Maticsoft.Model.IMS_PEOPLE_RECORD)
            {
                Maticsoft.Model.SMT_STAFF_INFO record = dataGridView.SelectedRows[0].Tag as Maticsoft.Model.SMT_STAFF_INFO;

            }
        }

    }
}
