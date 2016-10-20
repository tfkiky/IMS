using IMS.Common.Config;
using Li.Access.Core;
using Li.Access.Core.WGAccesses;
using SmartAccess.Common.Datas;
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
        private static AccessReader accessReader = null;
        private static int faceControllerID, faceDoorIndex;

        public static int FaceDoorIndex
        {
            get { return AccessCollect.faceDoorIndex; }
            set { AccessCollect.faceDoorIndex = value; }
        }

        public static int FaceControllerID
        {
            get { return AccessCollect.faceControllerID; }
            set { AccessCollect.faceControllerID = value; }
        }
        private static decimal lastAccessIndex;

        public static decimal LastAccessIndex
        {
            get { return lastAccessIndex; }
            set { lastAccessIndex = value; }
        }

        public static void Start()
        {
            SetControlType();
            accessReader = new AccessReader();
            accessReader.DataRead += new DataReadHandler(dataReader_DataRead);
            accessReader.start();
        }

        public static void Stop()
        {
            accessReader.stop();
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
                if (FaceCollect.FaceWhiteList.ContainsKey((int)record.STAFF_ID))
                {
                    FaceCollect.CurrentFacePic = FaceCollect.FaceWhiteList[(int)record.STAFF_ID];
                }
            }
        }
        /// <summary>
        /// 设置所有门禁为常关模式
        /// </summary>
        private static void SetControlType()
        {
            faceControllerID = int.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "Controller"));
            faceDoorIndex = int.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "Door"));

            Maticsoft.BLL.SMT_CONTROLLER_INFO ctrlBll=new Maticsoft.BLL.SMT_CONTROLLER_INFO();
            List<Maticsoft.Model.SMT_CONTROLLER_INFO> ctrlList = ctrlBll.GetModelList("ID=" + faceControllerID);
            Maticsoft.BLL.SMT_DOOR_INFO doorBll = new Maticsoft.BLL.SMT_DOOR_INFO();
            List<Maticsoft.Model.SMT_DOOR_INFO> doorList = new List<Maticsoft.Model.SMT_DOOR_INFO>();
            if (ctrlList != null && ctrlList.Count > 0)
            {
                foreach (Maticsoft.Model.SMT_CONTROLLER_INFO ctrl in ctrlList)
                {
                    Controller c = ControllerHelper.ToController(ctrl);
                    doorList = doorBll.GetModelList("CTRL_ID=" + ctrl.ID + "AND CTRL_DOOR_INDEX=" + faceDoorIndex);

                    if (doorList != null && doorList.Count > 0)
                    {
                        //设置门控制方式
                        using (IAccessCore access = new WGAccess())
                        {
                            bool ret = access.SetDoorControlStyle(c, (int)doorList[0].CTRL_DOOR_INDEX, DoorControlStyle.AlwaysClose, doorList[0].CTRL_DELAY_TIME);
                            if (!ret)
                            {
                                //WinInfoHelper.ShowInfoWindow(this, "上传门控制方式失败！");
                                return;
                            }
                        }
                    }
                }
            }
        }

    }
}
