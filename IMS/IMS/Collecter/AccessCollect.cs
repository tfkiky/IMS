using IMS.Common.Config;
using Li.Access.Core;
using Li.Access.Core.WGAccesses;
using log4net;
using SmartAccess.Common.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.Collecter
{
    public class AccessCollect
    {
        private ILog mlog = LogManager.GetLogger("AccessCollect");

        private Maticsoft.BLL.SMT_STAFF_INFO staffBll = new Maticsoft.BLL.SMT_STAFF_INFO();
        private List<Maticsoft.Model.SMT_STAFF_INFO> staffList = new List<Maticsoft.Model.SMT_STAFF_INFO>();
        Maticsoft.BLL.SMT_CARD_RECORDS recordBll = new Maticsoft.BLL.SMT_CARD_RECORDS();
        List<Maticsoft.Model.SMT_CARD_RECORDS> recordList ;
        private AccessReader accessReader = null;
        private int faceControllerID, faceDoorID;
        public event EventHandler<AccessEventArgs> AccessEvent;

        private static AccessCollect instance;

        public static AccessCollect Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        private bool bStarted = false;

        public bool BStarted
        {
            get { return bStarted; }
            set { bStarted = value; }
        }

        public int FaceDoorID
        {
            get { return faceDoorID; }
            set { faceDoorID = value; }
        }

        public int FaceControllerID
        {
            get { return faceControllerID; }
            set { faceControllerID = value; }
        }
        private decimal lastAccessIndex=0;

        public decimal LastAccessIndex
        {
            get { return lastAccessIndex; }
            set { lastAccessIndex = value; }
        }

        public AccessCollect()
        {
            instance = this;

        }

        public void Start()
        {
            faceDoorID = int.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "Door"));
            faceControllerID = int.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "Controller"));

            recordList = recordBll.GetModelList(" DOOR_ID="+AccessCollect.Instance.FaceDoorID+" ORDER BY  ID DESC ");
            if (recordList != null && recordList.Count > 0)
            {
                lastAccessIndex = recordList[0].ID;
            }
            accessReader = new AccessReader();
            accessReader.DataRead += new DataReadHandler(dataReader_DataRead);
            accessReader.start();
            bStarted = true;
        }

        public void Stop()
        {
            if (accessReader != null)
            {
                accessReader.stop();
                bStarted = false;
            }
        }

        /// <summary>
        /// 数据回调
        /// </summary>
        /// <param name="dataWapper">数据</param>
        private void dataReader_DataRead(Maticsoft.Model.SMT_CARD_RECORDS record)
        {
            try
            {
                recordList = recordBll.GetModelList(" DOOR_ID="+AccessCollect.Instance.FaceDoorID+" ORDER BY  ID DESC ");
                lastAccessIndex = recordList[0].ID;

                if (record.STAFF_ID != null)
                {
                    staffList = staffBll.GetModelList("ID='" + record.STAFF_ID + "'");
                    if (staffList != null && staffList.Count > 0)
                    {
                        mlog.InfoFormat("获得新门禁记录：用户编号{0}", record.STAFF_ID);
                        if (FaceCollect.FaceWhiteList != null && FaceCollect.FaceWhiteList.ContainsKey((int)record.STAFF_ID))
                        {
                            FaceCollect.CurrentFacePic = FaceCollect.FaceWhiteList[(int)record.STAFF_ID];
                            FaceCollect.StaffInfo = staffList[0];
                            FaceCollect.CardRecord = record;
                            FaceCollect.CardType = 0;
                            if (AccessEvent != null)
                            {
                                AccessEvent(this, new AccessEventArgs(staffList[0], record));
                            }
                        }
                    }
                    else
                    {
                        if (AccessEvent != null)
                        {
                            AccessEvent(this, new AccessEventArgs(null, record));
                        }
                    }
                }
                else
                {
                    if (AccessEvent != null)
                    {
                        AccessEvent(this, new AccessEventArgs(null, record));
                    }
                }
            }
            catch(Exception ex)
            {
                mlog.ErrorFormat("dataReader_DataRead :{0}", ex);
            }
        }
        
    }
}
