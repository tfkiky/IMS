using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.Collecter
{
    public class AccessCollect
    {
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

            //FaceCollect.CurrentFacePic=
        }

    }
}
