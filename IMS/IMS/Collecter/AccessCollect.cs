using IMS.Common.Config;
using Li.Access.Core;
using Li.Access.Core.WGAccesses;
using log4net;
using SmartAccess.Common.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace IMS.Collecter
{
    public class AccessCollect
    {
        private ILog mlog = LogManager.GetLogger("AccessCollect");

        private System.Threading.Timer timer;
        Maticsoft.BLL.SMT_CARD_INFO cardBll = new Maticsoft.BLL.SMT_CARD_INFO();
        Maticsoft.BLL.SMT_STAFF_CARD scardBll = new Maticsoft.BLL.SMT_STAFF_CARD();
        private Maticsoft.BLL.SMT_STAFF_INFO staffBll = new Maticsoft.BLL.SMT_STAFF_INFO();
        private List<Maticsoft.Model.SMT_STAFF_INFO> staffList = new List<Maticsoft.Model.SMT_STAFF_INFO>();
        private static Maticsoft.BLL.SMT_CONTROLLER_INFO ctrlBll = new Maticsoft.BLL.SMT_CONTROLLER_INFO();
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
        private long lastAccessIndex=0;

        public long LastAccessIndex
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

            //recordList = recordBll.GetModelList(" DOOR_ID="+AccessCollect.Instance.FaceDoorID+" ORDER BY  ID DESC ");
            //if (recordList != null && recordList.Count > 0)
            //{
            //    lastAccessIndex = recordList[0].ID;
            //}
            timer = new System.Threading.Timer(new TimerCallback(CollectAccess), null, 1000, 1000);

            //accessReader = new AccessReader();
            //accessReader.DataRead += new DataReadHandler(dataReader_DataRead);
            //accessReader.start();
            bStarted = true;
        }

        private void CollectAccess(object state)
        {
           
                using (IAccessCore acc = new WGAccess())
                {
                    try
                    {
                        Maticsoft.Model.SMT_CONTROLLER_INFO _ctrlr = ctrlBll.GetModel(faceControllerID);
                        Maticsoft.Model.SMT_CARD_RECORDS modelRecord = new Maticsoft.Model.SMT_CARD_RECORDS();
                        Controller c = ControllerHelper.ToController(_ctrlr);
                        if (acc.BeginReadRecord(c))
                        {
                            while (true)
                            {
                                if (lastAccessIndex == 0)
                                {
                                    lastAccessIndex = acc.GetControllerReadedIndex(c);
                                    mlog.Info("标记初始化：" + lastAccessIndex);
                                }
                                ControllerState record = acc.GetControllerRecord(c,0xffffffff);
                                //ControllerState record = acc.ReadNextRecord();
                                if (record == null || record.recordType == RecordType.NoRecord||record.lastRecordIndex==lastAccessIndex)
                                {
                                    acc.EndReadRecord();
                                    //mlog.Info("记录读取完毕：" + c.sn);
                                    break;
                                }
                                modelRecord.IS_ALLOW = record.isAllowValid;
                                modelRecord.IS_ENTER = record.isEnterDoor;
                                modelRecord.RECORD_INDEX = record.lastRecordIndex;
                                modelRecord.RECORD_DATE = record.recordTime;
                                modelRecord.IS_ENTER = record.isEnterDoor;
                                modelRecord.CARD_NO = record.cardOrNoNumber;
                                    lastAccessIndex = record.lastRecordIndex;

                                mlog.Info("记录读取：" + record.lastRecordIndex);

                                if (MainForm.Instance.IFaceMode == 3 || (MainForm.Instance.ISwipeMode == 0 || MainForm.Instance.ISwipeMode == 2))
                                {
                                    List<Maticsoft.Model.SMT_CARD_INFO> cardList = cardBll.GetModelList("CARD_WG_NO='" + modelRecord.CARD_NO + "'");
                                    if (cardList != null && cardList.Count > 0)
                                    {
                                        List<Maticsoft.Model.SMT_STAFF_CARD> scardList = scardBll.GetModelList("CARD_ID=" + cardList[0].ID);
                                        if (scardList != null && scardList.Count > 0)
                                        {
                                            Maticsoft.Model.SMT_STAFF_INFO staffInfo = staffBll.GetModel(scardList[0].STAFF_ID);
                                            if (FaceCollect.FaceWhiteList != null && FaceCollect.FaceWhiteList.ContainsKey((int)staffInfo.ID))
                                            {
                                                if (MainForm.Instance.IFaceMode != 3)
                                                {
                                                    FaceCollect.CurrentFacePic = FaceCollect.FaceWhiteList[(int)staffInfo.ID];
                                                    FaceCollect.StaffInfo = staffInfo;
                                                    FaceCollect.CardRecord = modelRecord;
                                                    FaceCollect.CardType = 0;
                                                }
                                                else
                                                {

                                                }
                                            }
                                            if (AccessEvent != null)
                                            {
                                                AccessEvent(this, new AccessEventArgs(staffInfo, modelRecord));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        mlog.Error(ex);
                    }
                }
        }

        public void Stop()
        {
            if (accessReader != null)
            {
                accessReader.stop();
            }
            if (timer!=null)
            {
                timer.Dispose();
            }
            bStarted = false;
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
                //lastAccessIndex = recordList[0].ID;

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
