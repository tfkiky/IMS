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
    public partial class PersonRecord : Office2007Form
    {
        Maticsoft.BLL.IMS_PEOPLE_RECORD recordBll = new Maticsoft.BLL.IMS_PEOPLE_RECORD();
        List<Maticsoft.Model.IMS_PEOPLE_RECORD> recordList = new List<Maticsoft.Model.IMS_PEOPLE_RECORD>();
        private int recordRowsCount = 0;
        StringBuilder strSql;
        public PersonRecord()
        {
            InitializeComponent();
        }

        private void PersonRecord_Load(object sender, EventArgs e)
        {
            dtiFromDate.Value = DateTime.Now.AddDays(-1);
            dtiToDate.Value = DateTime.Now;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                strSql = new StringBuilder("1=1");
                if (dtiFromDate.Value >= dtiToDate.Value)
                {
                    MessageBox.Show("结束时间要大于开始时间");
                    return;
                }
                dataGridView.Rows.Clear();
                strSql.Append(" AND CONVERT(VARCHAR(24),ThroughTime,20) BETWEEN '" + dtiFromDate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + dtiToDate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' ORDER BY ThroughTime DESC");
                recordRowsCount = recordBll.GetRecordCount(strSql.ToString());
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
                        recordList = recordBll.GetModelListByPageEx(strSql.ToString(), "ThroughTime", startIndex, startIndex + pageCtrlRecords.RecordsPerPage);
                    }
                    else
                    {
                        recordList = recordBll.GetModelListByPageEx(strSql.ToString(), "ThroughTime", startIndex + 1, startIndex + pageCtrlRecords.RecordsPerPage);
                    }
                }
                else
                {
                    if (startIndex == 0)
                    {
                        recordList = recordBll.GetModelListByPageEx(strSql.ToString(), "ThroughTime", startIndex, recordRowsCount);
                    }
                    else
                    {
                        recordList = recordBll.GetModelListByPageEx(strSql.ToString(), "ThroughTime", startIndex + 1, recordRowsCount);
                    }
                }
                for (int i = 0; i < recordList.Count; i++)
                {
                    DataGridViewRow newRow = new DataGridViewRow();
                   
                    newRow.CreateCells(dataGridView, new object[] { 
                    recordList[i].ID,
                    i+1,
                    recordList[i].Name,
                    recordList[i].Depart,
                    recordList[i].ThroughTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    new Bitmap(recordList[i].CapturePic),
                    recordList[i].CompareResult,
                    
                    });
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

    }
}
