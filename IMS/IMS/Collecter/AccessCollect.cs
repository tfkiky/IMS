using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.Collecter
{
    public class AccessCollect
    {
        private static Maticsoft.BLL.SMT_STAFF_INFO staffBll = new Maticsoft.BLL.SMT_STAFF_INFO();
        private static List<Maticsoft.Model.SMT_STAFF_INFO> staffList = new List<Maticsoft.Model.SMT_STAFF_INFO>();
        private static AlarmReader alarmReader = null; 

        private static decimal lastAccessIndex;

        public static decimal LastAccessIndex
        {
            get { return lastAccessIndex; }
            set { lastAccessIndex = value; }
        }

        public static void Start()
        {
            alarmReader = new AlarmReader();
            alarmReader.DataRead += new DataReadHandler(dataReader_DataRead);
            alarmReader.start();
        }

        /// <summary>
        /// 数据回调
        /// </summary>
        /// <param name="dataWapper">数据</param>
        private static void dataReader_DataRead(Maticsoft.Model.SMT_CARD_RECORDS record)
        {
            staffList = staffBll.GetModelList("ID='" + record.STAFF_ID + "'");
            if(staffList!=null&&staffList.Count>0)
            {

            }
            //FaceCollect.CurrentFacePic=
        }

    }
}
